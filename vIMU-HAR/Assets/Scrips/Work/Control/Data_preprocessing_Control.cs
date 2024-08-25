using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConnectPy;
using UnityEngine.UI;
using System.Text;
using System.IO;
using Csv_Function;
using System;
using System.Reflection;
using System.CodeDom.Compiler;


public class Data_preprocessing_Control : MonoBehaviour
{
    public InputField[] train_test_if;
    public Text total_num_t;
    public Text train_t;
    public Text test_t;
    public static string train_spilt;
    public static string test_spilt;
    private CsvFunction csvf = new CsvFunction("IMUSim");
    private string raw_data_path;
    private string object_name_for_path;
    private List<string> object_datas = new List<string>();
    public GameObject confirm_box;
    public Text message_t;

    // Start is called before the first frame update
    void Start()
    {
        // set default para
        float spilt_train = 4.0f;
        float spilt_test = 1.0f;
        train_test_if[0].text = spilt_train.ToString();
        train_test_if[1].text = spilt_test.ToString();
        train_spilt = train_test_if[0].text;
        test_spilt = train_test_if[1].text;
        total_num_t.text = 0.ToString();
        train_t.text = 0.ToString();
        test_t.text = 0.ToString();

    }

    public void Data_Process()
    {
        // clear message
        message_t.text = "";
        int total_sample_num = 0;
        int total_train_num = 0;
        int total_test_num = 0;

        // data segmentation
        for (int object_index = 0; object_index < Main_Canvas_Control.object_num; object_index++)
        {

            // read csv, get object window data
            object_datas.Clear();
            object_datas.AddRange(Get_Object_Datas(object_index));
            if (object_datas.Count == 0)
            {
                return;
            }

            // empty-value handling
            if (train_test_if[0].text == "")
                train_test_if[0].text = 4.ToString();
            if (train_test_if[1].text == "")
                train_test_if[1].text = 1.ToString();
            train_spilt = train_test_if[0].text;
            test_spilt = train_test_if[1].text;

            //data process
            string[] para = new string[5];
            para[0] = raw_data_path;
            para[1] = train_test_if[0].text;
            para[2] = train_test_if[1].text;
            para[3] = object_name_for_path;
            //Connect_Python_func.RunPythonScript(@"E:\unity_pro\My_workV0.0\Assets\Scrips\Work\py\train_test_spilt_func.py", para);
            Connect_Python_func.RunPythonScript(Application.dataPath + "/Scrips/Work/py/train_test_spilt_func.py", para);

            // calculate num 
            string[] datas = Connect_Python_func.output.ToString().Split(' ');
            int object_train_num = Convert.ToInt32(datas[0]);
            int object_test_num = Convert.ToInt32(datas[1]);
            int object_sample_num = object_train_num + object_test_num;
            total_train_num += object_train_num;
            total_test_num += object_test_num;
            total_sample_num += object_sample_num;

            // print object message
            message_t.text += "object" + object_index.ToString() + ":" + '\n' +
                              Main_Canvas_Control.avatar_name[object_index] + "_" +
                              Main_Canvas_Control.joint_name[object_index] + '\n' +
                              "sample num= " + object_sample_num.ToString() + "\n" +
                              "train num= " + object_train_num.ToString() + '\n' +
                              "test num= " + object_test_num.ToString() + '\n';

            //data process to raw data
            string[] para2 = new string[7];
            para2[0] = csvf.BinSourcesFolder +
                       Main_Canvas_Control.avatar_name[object_index] + "_" +
                       Main_Canvas_Control.joint_name[object_index] + "_label_data.csv";
            para2[1] = train_test_if[0].text;
            para2[2] = train_test_if[1].text;
            para2[3] = object_name_for_path;
            para2[4] = Data_interception_Control.window_size.ToString();
            para2[5] = Data_interception_Control.window_step.ToString();
            //Connect_Python_func.RunPythonScript(@"E:\unity_pro\My_workV0.0\Assets\Scrips\Work\py\train_test_spilt_rawdata.py", para2);
            Connect_Python_func.RunPythonScript(Application.dataPath + "/Scrips/Work/py/train_test_spilt_rawdata.py", para2);
        }

        // reflesh para show
        total_num_t.text = total_sample_num.ToString();
        train_t.text = total_train_num.ToString();
        test_t.text = total_test_num.ToString();

        // print message
        message_t.text += '\n' + "data preprocess finish" + '\n';
        message_t.text += "total sample num=" + total_sample_num + '\n' + 
                          "total train num= " + total_train_num + '\n' +
                          "total test num= " + total_test_num + '\n';
    }

    void Update()
    {
        
    }

    public string[] Get_Object_Datas(int index)
    {
        // read csv, get corresponding windows datas
        raw_data_path = csvf.BinSourcesFolder +
                        Main_Canvas_Control.avatar_name[index] + "_" +
                        Main_Canvas_Control.joint_name[index] + "_feature_data.csv";
        object_name_for_path = Main_Canvas_Control.avatar_name[index] + "_" +
                               Main_Canvas_Control.joint_name[index];

        // if raw data is not exist
        if (!File.Exists(raw_data_path))
        {
            confirm_box.SetActive(true);
            string[] error= { };
            return error;
        }

        // read csv
        Encoding utf = Encoding.GetEncoding("UTF-8");
        string InfoConfig = File.ReadAllText(raw_data_path, utf);
        InfoConfig = InfoConfig.Replace("\r", "");
        InfoConfig = InfoConfig.Replace("\"", "");
        string[] CSVDatas = InfoConfig.Split('\n');
        //Debug.Log("CSVDatas length=" + CSVDatas.Length);

        string[] object_datas= new string[CSVDatas.Length-1];
        for (int j = 0; j < CSVDatas.Length - 1; j++)
        {
            object_datas[j] = CSVDatas[j + 1];
        }

        return object_datas;
    }

}
