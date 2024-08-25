using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System;
using System.Linq;
using UnityEditor;
using System.Runtime.InteropServices.ComTypes;
using Csv_Function;
using System.Text;
using System.IO;

public class File_Import_Control : MonoBehaviour
{
    public Text message_t;
    public GameObject perfab;
    public GameObject canvas;
    public InputField action_name_if;
    private List<UnityEngine.UI.Toggle> objects_list_tg = new List<UnityEngine.UI.Toggle>();  // 文件路径对应的toggle
    private List<string> objects_list = new List<string>();
    private CsvFunction new_csvf = new CsvFunction("Animation_files");

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Add_Files()
    {
        //初始化
        OpenFileName ofn = new OpenFileName();
        ofn.structSize = Marshal.SizeOf(ofn);
        ofn.filter = "All Files\0*.*\0\0";
        ofn.file = new string(new char[1024]);
        ofn.maxFile = ofn.file.Length;
        ofn.fileTitle = new string(new char[64]);
        ofn.maxFileTitle = ofn.fileTitle.Length;
        string path = Application.streamingAssetsPath;
        path = path.Replace('/', '\\');
        ofn.initialDir = path;  //默认路径
        ofn.title = "Open Project";
        ofn.defExt = "csv";//显示文件的类型  
        //注意 一下项目不一定要全选 但是0x00000008项不要缺少  
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR  

        //判断是否打开文件
        if (WindowDll.GetOpenFileName(ofn))
        {
            message_t.text = "";
            int add_files_num = 0;
            //多选文件
            string[] Splitstr = { "\0" };
            string[] strs = ofn.file.Split(Splitstr, StringSplitOptions.RemoveEmptyEntries);
            if (strs.Length > 1)
            {
                for (int i = 1; i < strs.Length; i++)
                {
                    string file_path = strs[0] + "\\" + strs[i];

                    // 新增：是否列表中已经存在对象
                    bool exists = objects_list.Where(w => w.Contains(file_path)).Any();
                    if (!exists)
                    {
                        // add info
                        GameObject object_tg = Instantiate(perfab);
                        object_tg.name = "File_path_" + objects_list_tg.Count.ToString();
                        object_tg.GetComponentInChildren<Text>().text = file_path;
                        object_tg.transform.SetParent(canvas.transform, false);
                        object_tg.SetActive(true);
                        objects_list.Add(file_path);
                        objects_list_tg.Add(object_tg.GetComponentInChildren<UnityEngine.UI.Toggle>());
                        add_files_num++;
                    }
                    else
                    {
                        message_t.text += file_path + " has been added \n";
                    }

                    //Debug.Log(file_path);
                }
                message_t.text += "Successfully added "+ add_files_num.ToString() +" documents \n";
            }
            else
            {
                string file_path = strs[0];

                // 新增：是否列表中已经存在对象
                bool exists = objects_list.Where(w => w.Contains(file_path)).Any();
                if (!exists)
                {
                    // add info
                    GameObject object_tg = Instantiate(perfab);
                    object_tg.name = "File_path_" + objects_list_tg.Count.ToString();
                    object_tg.GetComponentInChildren<Text>().text = file_path;
                    object_tg.transform.SetParent(canvas.transform, false);
                    object_tg.SetActive(true);
                    objects_list.Add(file_path);
                    objects_list_tg.Add(object_tg.GetComponentInChildren<UnityEngine.UI.Toggle>());
                    add_files_num=1;
                }
                else
                {
                    message_t.text += file_path + " has been added \n";
                }

                //Debug.Log(file_path);

                message_t.text += "Successfully added " + add_files_num.ToString() + " documents \n";
            }
        }
        else
        {
            //Debug.LogFormat("Path is empty");
            message_t.text += "Path is empty \n";
        }

    }

    public void Clear_Files()
    {
        if (objects_list.Count > 0)
        {
            objects_list.Clear();
            objects_list_tg.Clear();

            foreach (Transform child in canvas.transform)
            {
                //Debug.Log(child.gameObject.name);
                GameObject.Destroy(child.gameObject);
            }

            message_t.text = "Successfully cleared all files \n";
        }
        else
        {
            message_t.text = "Current file list is empty \n";
        }
    }

    public void Set_Action()
    {
        string action = action_name_if.text;
        // document merge
        Document_Merge(action);
    }

    private void Document_Merge(string action_name)
    {
        string new_data_path = new_csvf.BinSourcesFolder + action_name + "_animation" + ".csv";
        bool is_init = false;
        bool is_error = false;

        Debug.Log(objects_list.Count);
        for (int i = 0; i < objects_list.Count; i++)
        {
            if (objects_list_tg[i].isOn)  // if toggle is on
            {
                string fiel_path = objects_list[i];
                List<string> object_datas = new List<string>();

                // read csv, get corresponding object datas
                Get_Object_Datas(fiel_path, out string header, out string[] raw_datas);
                if (raw_datas == null)
                {
                    is_error = true;
                    break;
                }
                object_datas.AddRange(raw_datas);

                // first csv init
                if (!is_init)
                {
                    //Debug.Log("header=" + header);
                    new_csvf.Csv_Init(header, new_data_path);
                    is_init = true;
                }

                //Write to csv
                new_csvf.WriteCsvDatasnew(object_datas, new_data_path);

                // Close file
                new_csvf.FileClose(new_data_path);

            }
        }

        if (is_error)
        {
            return;
        }

    }

    private void Get_Object_Datas(string path,
                                  out string header,
                                  out string[] object_datas)
    {
        // read csv, get corresponding windows datas
        string raw_data_path = path;

        // read csv
        Encoding utf = Encoding.GetEncoding("UTF-8");
        string InfoConfig = File.ReadAllText(raw_data_path, utf);
        InfoConfig = InfoConfig.Replace("\r", "");
        InfoConfig = InfoConfig.Replace("\"", "");
        string[] CSVDatas = InfoConfig.Split('\n');
        //Debug.Log("CSVDatas length=" + CSVDatas.Length);
        header = CSVDatas[0];

        object_datas = new string[CSVDatas.Length - 2];
        for (int j = 0; j < CSVDatas.Length - 2; j++) // except header and the empty row(the last row)
        {
            object_datas[j] = CSVDatas[j + 1];
        }

    }
}
