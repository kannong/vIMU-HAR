using Csv_Function;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;
using XCharts.Runtime;

public class Data_interception_Control : MonoBehaviour
{
    public InputField window_size_if;
    public InputField window_step_if;
    public static int window_size;
    public static int window_step;
    private CsvFunction csvf = new CsvFunction("IMUSim");
    private List<string[]> windows_datas = new List<string[]>();
    public GameObject confirm_box;
    public GameObject confirm_box_1;
    public GameObject data_clear_sub;
    public Text message_t;

    // Start is called before the first frame update
    void Start()
    {
        //Set default value
        window_size = 40;
        window_step = 20;
        window_size_if.text = window_size.ToString();
        window_step_if.text = window_step.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Data_Intercepte()
    {
        // empty-value handling 
        if (window_size_if.text == "")
        {
            window_size_if.text = 40.ToString();
        }
        if(window_step_if.text == "")
        {
            window_step_if.text = 20.ToString();
        }
        // get value
        window_size = Convert.ToInt32(window_size_if.text);
        window_step = Convert.ToInt32(window_step_if.text);

        // clear message text
        message_t.text = "";

        //read raw data from csv files ,add value ,write to csv files
        for (int j = 0; j < Main_Canvas_Control.object_num; j++)
        {
            string raw_data_path = csvf.BinSourcesFolder +
                                   Main_Canvas_Control.avatar_name[j] + "_" +
                                   Main_Canvas_Control.joint_name[j] + "_label_data.csv";
            string new_data_path = csvf.BinSourcesFolder +
                                   Main_Canvas_Control.avatar_name[j] + "_" +
                                   Main_Canvas_Control.joint_name[j] + "_cleared_data.csv";
            // if raw data is not exist
            if (!File.Exists(raw_data_path))
            {
                confirm_box.SetActive(true);
                return;
            }

            //read csv
            Encoding utf = Encoding.GetEncoding("UTF-8");
            string InfoConfig = File.ReadAllText(raw_data_path, utf);
            InfoConfig = InfoConfig.Replace("\r", "");
            InfoConfig = InfoConfig.Replace("\"", "");
            string[] CSVDatas = InfoConfig.Split('\n');
            //Debug.Log("CSVDatas length=" + CSVDatas.Length);
            message_t.text += "object" + j + " length=" + (CSVDatas.Length - 2) + '\n';

            // first csv init
            CSVDatas[0] += "window_index" + "," + 
                           "action_exist" + "," +
                           "begin_index" + "," + 
                           "end_index" + "," + 
                           "len" + ",";
            //Debug.Log("header=" + CSVDatas[0]);
            csvf.Csv_Init(CSVDatas[0], new_data_path);

            //divide datas and get windows datas
            windows_datas.AddRange(Get_Windows_Datas(CSVDatas, window_size, window_step));

            // get begin index and end index, add info, write datas
            for (int window_index = 0; window_index < windows_datas.Count; window_index++)
            {
                // get begin index and end index
                int begin = 0;
                int end = window_size-1;
                int len = window_size;
                bool action_exist = true;

                // add info, write datas
                for (int k = 0; k < window_size; k++)
                {
                    string one_row = windows_datas[window_index][k];
                    if (k == 0)
                    {
                        one_row += window_index.ToString() + "," +
                                   action_exist.ToString() + "," +
                                   begin.ToString() + "," +
                                   end.ToString() + "," +
                                   len.ToString() + ",";
                    }
                    //Debug.Log("csvdata intercepted then=" + window_index.ToString() + ' ' + k.ToString() + '\n' + one_row);
                    // write datas
                    csvf.WriteCsvnew(one_row, new_data_path);
                }
            }

            // print message
            message_t.text += "window size=" + window_size + '\n' +
                              "total windows=" + windows_datas.Count + '\n';

            // clear datas
            windows_datas.Clear();

        }

        message_t.text += "data interception finish" + '\n';
    }

    public void Data_Clear()
    {
        // if new data is not exist
        for (int j = 0; j < Main_Canvas_Control.object_num; j++)
        {
            string new_data_path = csvf.BinSourcesFolder +
                                   Main_Canvas_Control.avatar_name[j] + "_" +
                                   Main_Canvas_Control.joint_name[j] + "_cleared_data.csv";
            if (!File.Exists(new_data_path))
            {
                confirm_box_1.SetActive(true);
                return;
            }
        }

        // open data clear sub
        data_clear_sub.SetActive(true);

    }
    public List<string[]> Get_Windows_Datas(string[] CSVDatas,int data_window_size,int data_window_step)
    {
        int row_len = CSVDatas.Length;
        string[] raw_datas = new string[row_len - 1];
        string[] one_window_data = new string[data_window_size];
        List<string[]> all_windows_datas = new List<string[]>();

        for (int i = 0; i < row_len - 1; i++)
        {
            raw_datas[i] = CSVDatas[i + 1];
        }

        if (row_len >= data_window_size)
        {
            for (int i = 0,j = 0; i < row_len - data_window_size; i += data_window_step,j++) //j = window_index
            {
                one_window_data = next_window(raw_datas, i, data_window_size);
                all_windows_datas.Add(one_window_data);
            }
        }
        else
        {
            //Debug.Log("当前数据未有一个数据窗口长度，数据窗口长度为：" + row_len);
            message_t.text += "当前数据未有一个数据窗口长度+ \n"+
                              "当前数据窗口长度为：" + data_window_size + '\n' + 
                              "当前数据总长度为:" + (row_len - 2) + '\n';
        }
        return all_windows_datas;
    }

    public string[] next_window(string[] data, int i, int data_window_size)
    {
        //'''
        //函数功能：读取下一个窗口的数据
        //: param data: 从csv中读取的数据
        //: param i: 当前行的索引
        //: param data_window_size: 一个窗口的采样点数量
        //: return: 一个窗口的数据列表
        //'''
        string[] one_window = new string[data_window_size];
        for (int j = 0; j < data_window_size; j++)
        {
            one_window[j] = data[j+i];
        }

        return one_window;
    }

    public void Print_Object_Message()
    {
        string mes = "";
        mes += "object num= " + Main_Canvas_Control.object_num.ToString() + '\n';
        for (int i = 0; i < Main_Canvas_Control.object_num; i++)
        {
            mes += " object " + i + '\n' + ':' +
                   Main_Canvas_Control.avatar_name[i] + "_" +
                   Main_Canvas_Control.joint_name[i] + '\n';
        }
        message_t.text = mes;
    }

}
