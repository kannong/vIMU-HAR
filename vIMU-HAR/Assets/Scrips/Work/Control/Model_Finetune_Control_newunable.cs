using ConnectPy;
using Csv_Function;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Uduino;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using XUGL;
using System;
using System.Drawing;
using Unity.VisualScripting;
using System.Linq;
using UnityEditor;
using System.Runtime.Remoting.Messaging;
using UnityEngine.Analytics;
using RandomForest;
using CNN_;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

public class Model_Finetune_Sub_Control_newunable : MonoBehaviour
{

    public Text message_t;
    public GameObject confirm_box;
    public Dropdown avatar_action_dp;
    public Dropdown target_bone_dp;
    public Dropdown target_JNT_dp;
    public InputField labels_if;
    public GameObject perfab;
    public GameObject canvas;
    public Text total_num_t;
    public Text train_num_t;
    public Text test_num_t;
    public Text model_t;
    public Text data_source_t;
    public InputField traing_cycle_if;
    public InputField train_spilt_if;
    public InputField test_spilt_if;
    private List<string> objects_list = new List<string>();
    private List<string> labels_list = new List<string>();  // 标签
    private List<string> labels_list_object = new List<string>();  // 标签对应的对象
    private List<int> second_list = new List<int>();
    private List<UnityEngine.UI.Toggle> toggles_list = new List<UnityEngine.UI.Toggle>();  // 标签对应的toggle
    private bool is_sampling = false;
    private string imuName = "r"; // You should ignore this if there is one IMU.
    private CsvFunction csvf = new CsvFunction("IMUReal/raw_data");
    private CsvFunction new_csvf = new CsvFunction("Data_Process");
    private CsvFunction csvf_obj = new CsvFunction("IMUReal/object_data");
    private string csv_path;
    private List<string[]> windows_datas = new List<string[]>();
    private string data_sr = "r";

    private int starttime = 0;
    private int endtime = 0;
    private long total_second = 0;


    // Start is called before the first frame update
    void Start()
    {
        traing_cycle_if.text = 50.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (is_sampling)
        {
            //time
            DateTime now = DateTime.Now;
            //Debug.Log("结束北京时间：" + now);
            //Debug.Log("结束北京时间：时 分 秒：" + now.Hour+now.Minute+now.Second);
            endtime = now.Hour * 3600 + now.Minute * 60 + now.Second;
            //Debug.Log("经过的时间：" + (endtime - starttime));
            GetTimeLongAgo(endtime - starttime);
        }

    }

    public void Target_Bone_Changed()
    {
        Set_targetJNTdp();
    }
    public void Start_Sample()
    {
        // csv init
        string header = "";
        header += "a_x" + "," + "a_y" + "," + "a_z" + "," +
                  "g_x" + "," + "g_y" + "," + "g_z" + "," +
                  "label" + ",";
        string object_name = avatar_action_dp.options[avatar_action_dp.value].text + "_" +
                      target_bone_dp.options[target_bone_dp.value].text + "_" +
                      target_JNT_dp.options[target_JNT_dp.value].text;
        string label_name = labels_if.text;
        // 新增：是否列表中已经存在对象
        bool exists = objects_list.Where(w => w.Contains(object_name)).Any();
        Debug.Log("exists=" + exists);
        if(!exists)
        {
            // add info
            objects_list.Add(object_name);
        }
        csv_path = csvf.BinSourcesFolder + object_name + "_" + label_name + "_" + "imureal_data.csv";
        csvf.Csv_Init(header, csv_path);
        labels_list_object.Add(object_name);
        labels_list.Add(label_name);

        //time
        DateTime now = DateTime.Now;
        Debug.Log("开始北京时间：" + now);
        Debug.Log("开始北京时间：时 分 秒：" + now.Hour + now.Minute + now.Second);
        starttime = now.Hour * 3600 + now.Minute * 60 + now.Second;


        // print message
        message_t.text = "";
        message_t.text += "start samping \n";

        is_sampling = true;

    }
    public void Stop_Sample()
    {
        is_sampling = false;

        // close file
        csvf.FileClose(csv_path);

        // add object
        GameObject object_tg = Instantiate(perfab);
        object_tg.name = "Sample_object_" + (objects_list.Count - 1).ToString();
        object_tg.GetComponentInChildren<Text>().text = labels_if.text;
        object_tg.transform.SetParent(canvas.transform, false);
        object_tg.SetActive(true);
        toggles_list.Add(object_tg.GetComponentInChildren<UnityEngine.UI.Toggle>());

        //time
        int delta = endtime - starttime;
        second_list.Add(delta);

        // print message
        message_t.text += "samping finish \n";

    }
    public void ReadIMU(string data, UduinoDevice device)
    {

        string[] values = data.Split('/');
        if (values.Length == 12 && values[0] == imuName) // Rotation of the first one 
        {
            float ax = float.Parse(values[1]);
            float ay = float.Parse(values[2]);
            float az = float.Parse(values[3]);
            float gx = float.Parse(values[4]);
            float gy = float.Parse(values[5]);
            float gz = float.Parse(values[6]);
            float temp = float.Parse(values[7]);
            float w = float.Parse(values[8]);
            float x = float.Parse(values[9]);
            float y = float.Parse(values[10]);
            float z = float.Parse(values[11]);

            // if is_samping write to csv
            if (is_sampling)
            {
                Debug.Log("ax,ay,az= " + ax + "\t" + ay + "\t" + az + "\t" +
                          " gx,gy,gz= " + gx + "\t" + gy + "\t" + gz + "\t" +
                          "tempature= " + temp);

                // write to csv
                string one_row = ax + "," + ay + "," + az + "," + gx + "," + gy + "," + gz + ",";
                if (avatar_action_dp.options[avatar_action_dp.value].text == "Knee_Kick")
                {
                    one_row += Data_annotation_Control.labels[0].ToString() + ",";
                }
                else if (avatar_action_dp.options[avatar_action_dp.value].text == "Reverse_Lunge")
                {
                    one_row += Data_annotation_Control.labels[1].ToString() + ",";
                }
                else if (avatar_action_dp.options[avatar_action_dp.value].text == "Ankle")
                {
                    one_row += Data_annotation_Control.labels[2].ToString() + ",";
                }
                else if (avatar_action_dp.options[avatar_action_dp.value].text == "Walking")
                {
                    one_row += Data_annotation_Control.labels[3].ToString() + ",";
                }
                else if (avatar_action_dp.options[avatar_action_dp.value].text == "Sidetoside")
                {
                    one_row += Data_annotation_Control.labels[4].ToString() + ",";
                }
                else if (avatar_action_dp.options[avatar_action_dp.value].text == "SideCrunch")
                {
                    one_row += Data_annotation_Control.labels[5].ToString() + ",";
                }
                else if (avatar_action_dp.options[avatar_action_dp.value].text == "HighKnee")
                {
                    one_row += Data_annotation_Control.labels[6].ToString() + ",";
                }

                csvf.WriteCsvnew(one_row, csv_path);

            }
        }
        else
        {
            Debug.LogWarning(data);
        }
        //  Log.Debug("The new rotation is : " + transform.Find("IMU_Object").eulerAngles);

    }
    public void Sample_Create()
    {
        // Get features
        if(Model_Processing_Control.object_model == "NN" || Model_Processing_Control.object_model == "CNN")
        {
            data_sr = (Model_Processing_Control.object_model == "NN") ? NN_.NN.data_source : CNN_.CNN.data_source;
            if (data_sr == "e")
            {
                Feature_Extract();
            }
        }

        // 将所选标签对应的数据合并到对应对象文件中
        Label_To_Object();

        //Get each train and test data
        Get_Train_Test_Data();

        // Get all train and test data according to the objects_list, Updated parameter display
        Data_Synthesise();

    }
    public void Model_Fine_tune()
    {
        // if object is not CNN
        if (Model_Processing_Control.object_model != "CNN")
        {
            confirm_box.SetActive(true);
            return;
        }

        // pre-train has finished
        if (Model_Processing_Control.is_pretrain)
        {
            // Use Python
            string[] para = new string[5];
            para[0] = 1.ToString(); // object model
            para[1] = CNN_.CNN.data_source; // data source
            para[2] = CNN_.CNN.loss;
            para[3] = (traing_cycle_if.text == "") ? "0" : traing_cycle_if.text;
            //foreach (var item in para)
            //{
            //    Debug.Log(item);
            //}
            Connect_Python_func.RunPythonScript(Application.dataPath + "/Scrips/Work/py/model_fine_tune.py", para);

            message_t.text = "model fine-tune finish" + '\n' +
                             Connect_Python_func.output.ToString() + '\n';
        }
        else 
        {
            List<string> para = new List<string>();

            para.Insert(0, 2.ToString());
            para.Insert(1, (CNN.data_source == "") ? "0" : CNN.data_source);
            para.Insert(2, (CNN.training_cycles == "") ? "0" : CNN.training_cycles);
            para.Insert(3, (CNN.optimizer == "") ? "0" : CNN.optimizer);
            para.Insert(4, (CNN.learning_rate == "") ? "0" : CNN.learning_rate);
            para.Insert(5, (CNN.loss == "") ? "0" : CNN.loss);
            para.Insert(6, (CNN.vaildation_spilt == "") ? "0" : CNN.vaildation_spilt); // %
            para.Insert(7, (CNN.batch_size == "") ? "0" : CNN.batch_size);
            para.Insert(8, CNN.fit_generator);
            para.Insert(9, (CNN.Input_Layer.features_num == "") ? "0" : CNN.Input_Layer.features_num);
            para.Insert(10, (CNN.CONV_Pool_Layer_0.filiters_num == "") ? "0" : CNN.CONV_Pool_Layer_0.filiters_num);
            para.Insert(11, (CNN.CONV_Pool_Layer_0.kernel_size == "") ? "0" : CNN.CONV_Pool_Layer_0.kernel_size);
            para.Insert(12, (CNN.CONV_Pool_Layer_0.activation == "") ? "0" : CNN.CONV_Pool_Layer_0.activation);
            para.Insert(13, (CNN.CONV_Pool_Layer_0.pooling_size == "") ? "0" : CNN.CONV_Pool_Layer_0.pooling_size);
            para.Insert(14, (CNN.CONV_Pool_Layer_1.filiters_num == "") ? "0" : CNN.CONV_Pool_Layer_1.filiters_num);
            para.Insert(15, (CNN.CONV_Pool_Layer_1.kernel_size == "") ? "0" : CNN.CONV_Pool_Layer_1.kernel_size);
            para.Insert(16, (CNN.CONV_Pool_Layer_1.activation == "") ? "0" : CNN.CONV_Pool_Layer_1.activation);
            para.Insert(17, (CNN.CONV_Pool_Layer_1.pooling_size == "") ? "0" : CNN.CONV_Pool_Layer_1.pooling_size);
            para.Insert(18, (CNN.CONV_Pool_Layer_2.filiters_num == "") ? "0" : CNN.CONV_Pool_Layer_2.filiters_num);
            para.Insert(19, (CNN.CONV_Pool_Layer_2.kernel_size == "") ? "0" : CNN.CONV_Pool_Layer_2.kernel_size);
            para.Insert(20, (CNN.CONV_Pool_Layer_2.activation == "") ? "0" : CNN.CONV_Pool_Layer_2.activation);
            para.Insert(21, (CNN.CONV_Pool_Layer_2.pooling_size == "") ? "0" : CNN.CONV_Pool_Layer_2.pooling_size);
            para.Insert(22, objects_list.Count.ToString());

            //int t = 0;
            //foreach (var item in para)
            //{
            //    Debug.Log(t + " " + item + " " + '\n');
            //    t++;
            //}
            Connect_Python_func.RunPythonScript(Application.dataPath + "/Scrips/Work/py/model_fine_tune.py", para.ToArray());

            message_t.text = "model train finish" + '\n' +
                             Connect_Python_func.output.ToString() + '\n';

        }
    }

    public void Model_Deploy()
    {
        string[] para = new string[5];
        para[0] = Get_Savepath();
        Connect_Python_func.RunPythonScript(Application.dataPath + "/Scrips/Work/py/model_deploy.py", para);

    }
    private void Data_Synthesise()
    {
        List<string> object_train_datas = new List<string>();
        List<string> object_test_datas = new List<string>();
        string all_train_data_path = new_csvf.BinSourcesFolder + "imureal_all_train_data.csv";
        string all_test_data_path = new_csvf.BinSourcesFolder + "imureal_all_test_data.csv";
        string header;
        bool first_init = false;
        List<string> object_train_datas_fea = new List<string>();
        List<string> object_test_datas_fea = new List<string>();
        string all_train_data_path_fea = new_csvf.BinSourcesFolder + "imureal_all_train_data_e.csv";
        string all_test_data_path_fea = new_csvf.BinSourcesFolder + "imureal_all_test_data_e.csv";
        string header_fea;
        bool first_init_fea = false;

        bool is_error = false;

        // Get all object datas
        for (int i = 0; i < objects_list.Count; i++)
        {
            // raw data process
            // read csv, get corresponding object datas
            Get_Object_Datas(i, "_raw_train_data.csv", out header, out string[] train_datas);
            Get_Object_Datas(i, "_raw_test_data.csv", out header, out string[] test_datas);
            if (train_datas == null || test_datas == null)
            {
                is_error = true;
                return;
            }
            object_train_datas.AddRange(train_datas);
            object_test_datas.AddRange(test_datas);

            // first csv init
            if (!first_init)
            {
                //Debug.Log("header=" + header);
                new_csvf.Csv_Init(header, all_train_data_path);
                new_csvf.Csv_Init(header, all_test_data_path);

                first_init = true;
            }

            // feas data process
            if (data_sr == "e")
            {
                // read csv, get corresponding object datas
                Get_Object_Datas(i, "_train_data.csv", out header_fea, out string[] train_datas_fea);
                Get_Object_Datas(i, "_test_data.csv", out header_fea, out string[] test_datas_fea);
                if (train_datas_fea == null || test_datas_fea == null)
                {
                    is_error = true;
                    return;
                }
                object_train_datas_fea.AddRange(train_datas_fea);
                object_test_datas_fea.AddRange(test_datas_fea);

                // first csv init
                if (!first_init_fea)
                {
                    new_csvf.Csv_Init(header_fea, all_train_data_path_fea);
                    new_csvf.Csv_Init(header_fea, all_test_data_path_fea);

                    first_init_fea = true;
                }
            }
        }

        if (is_error)
            return;

        // raw data process
        // Write to csv
        new_csvf.WriteCsvDatasnew(object_train_datas, all_train_data_path);
        new_csvf.WriteCsvDatasnew(object_test_datas, all_test_data_path);

        // Close file
        new_csvf.FileClose(all_train_data_path);
        new_csvf.FileClose(all_test_data_path);

        // feas data process
        if(data_sr == "e")
        {
            // Write to csv
            new_csvf.WriteCsvDatasnew(object_train_datas_fea, all_train_data_path_fea);
            new_csvf.WriteCsvDatasnew(object_test_datas_fea, all_test_data_path_fea);

            // Close file
            new_csvf.FileClose(all_train_data_path_fea);
            new_csvf.FileClose(all_test_data_path_fea);

        }

        // Reflesh paras
        model_t.text = Model_Processing_Control.object_model;
        data_source_t.text = data_sr;
        if (data_source_t.text == "r")
        {
            total_num_t.text = (object_train_datas.Count + object_test_datas.Count).ToString();
            train_num_t.text = object_train_datas.Count.ToString();
            test_num_t.text = object_test_datas.Count.ToString();
        }else if (data_source_t.text == "e")
        {
            total_num_t.text = (object_train_datas_fea.Count + object_test_datas_fea.Count).ToString();
            train_num_t.text = object_train_datas_fea.Count.ToString();
            test_num_t.text = object_test_datas_fea.Count.ToString();
        }

        // print message
        GetTimeLongAgo(total_second);
        message_t.text += "total num=" + total_num_t.text + '\n' +
                         "train num=" + train_num_t.text + '\n' +
                         "test num=" + test_num_t.text + '\n';
        message_t.text += "Sample generation finish" + '\n';

    }
    private void Get_Object_Datas(int index,
                                  string name,
                                  out string header,
                                  out string[] object_datas)
    {
        // read csv, get corresponding windows datas
        string raw_data_path = new_csvf.BinSourcesFolder +
                               objects_list[index]+
                               "_" + "imureal" + 
                               name;

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

    private void Feature_Extract()
    {
        // csv
        for (int i = 0; i < labels_list_object.Count; i++)
        {
            // read csv, get corresponding windows datas
            string raw_data_path = csvf.BinSourcesFolder + labels_list_object[i] + "_" + labels_list[i] + "_" + "imureal_data.csv";
            string new_data_path = csvf.BinSourcesFolder + labels_list_object[i] + "_" + labels_list[i] + "_" + "imureal_data_feas.csv";

            // read csv
            Encoding utf = Encoding.GetEncoding("UTF-8");
            string InfoConfig = File.ReadAllText(raw_data_path, utf);
            InfoConfig = InfoConfig.Replace("\r", "");
            InfoConfig = InfoConfig.Replace("\"", "");
            string[] CSVDatas = InfoConfig.Split('\n');
            //Debug.Log("CSVDatas length=" + CSVDatas.Length);

            // first csv init
            string header = Feature_extraction_Control.features_header;
            //Debug.Log("header=" + header);
            csvf.Csv_Init(header, new_data_path);

            //divide datas and get windows datas
            windows_datas.AddRange(Get_Windows_Datas(CSVDatas, Data_interception_Control.window_size, Data_interception_Control.window_step));
            //Debug.Log("window datas len=" + windows_datas.Count);

            // calculate feacture, write new csv
            for (int j = 0; j < windows_datas.Count; j++)
            {
                // calculate feacture
                string newrow = Calculate_Feature(j,windows_datas[j]);
                // write datas
                csvf.WriteCsvnew(newrow, new_data_path);

            }

            // clear list
            windows_datas.Clear();

        }

    }

    private void Get_Train_Test_Data()
    {
        // Get each train and test data
        for (int i = 0; i < objects_list.Count; i++)
        {
            // Use train_test_spilt_func.py,...... to get each train and test data
            if (data_sr == "e")
            {
                string[] para2 = new string[5];
                para2[0] = csvf_obj.BinSourcesFolder + objects_list[i] + "_" + "imureal_data_feas.csv";
                para2[1] = train_spilt_if.text;
                para2[2] = test_spilt_if.text;
                para2[3] = objects_list[i] + "_" + "imureal";
                Connect_Python_func.RunPythonScript(Application.dataPath + "/Scrips/Work/py/train_test_spilt_func.py", para2);
            }

            // Use train_test_spilt_rawdata.py,...... to get window datas and write to csv get each train and test data
            string[] para = new string[7];
            para[0] = csvf_obj.BinSourcesFolder + objects_list[i] + "_" + "imureal_data.csv";
            para[1] = train_spilt_if.text;
            para[2] = test_spilt_if.text;
            para[3] = objects_list[i] + "_" + "imureal";
            para[4] = Data_interception_Control.window_size.ToString();
            para[5] = Data_interception_Control.window_step.ToString();
            Connect_Python_func.RunPythonScript(Application.dataPath + "/Scrips/Work/py/train_test_spilt_rawdata.py", para);

            //Debug.Log("训练次数" + traing_cycle_if.text);
            //Debug.Log("训练集比例"+train_spilt_if.text);
            //Debug.Log("测试集比例"+test_spilt_if.text);

        }

    }

    private void Label_To_Object()
    {
        // 将所选标签对应的数据合并到对应对象文件中
        bool[] is_init = new bool[objects_list.Count];
        bool[] is_init_feas = new bool[objects_list.Count];
        bool is_error = false;
        total_second = 0;

        for (int i = 0; i < toggles_list.Count; i++)
        {
            if (toggles_list[i].isOn)  // if toggle is on
            {
                string object_name = labels_list_object[i];
                int object_index = Find_Object_Index(objects_list, object_name);  // fet object index
                List<string> object_raw_datas = new List<string>();
                string raw_data_path = csvf_obj.BinSourcesFolder + object_name + "_" + "imureal_data.csv";
                string raw_header;

                //time
                total_second += second_list[i];

                // raw data process
                // read csv, get corresponding object datas
                Get_Object_Datas_2(i, "imureal_data.csv", out raw_header, out string[] raw_datas);
                if (raw_datas == null)
                {
                    is_error = true;
                    break;
                }
                object_raw_datas.AddRange(raw_datas);

                // first csv init
                if (!is_init[object_index])
                {
                    //Debug.Log("header=" + header);
                    csvf_obj.Csv_Init(raw_header, raw_data_path);
                    is_init[object_index] = true;
                }

                //Write to csv
                csvf_obj.WriteCsvDatasnew(object_raw_datas, raw_data_path);

                // Close file
                csvf_obj.FileClose(raw_data_path);

                // feas data process
                if (data_sr == "e")
                {
                    List<string> object_feas_datas = new List<string>();
                    string feas_data_path = csvf_obj.BinSourcesFolder + object_name + "_" + "imureal_data_feas.csv";
                    string feas_header;

                    // read csv, get corresponding object datas
                    Get_Object_Datas_2(i, "imureal_data_feas.csv", out feas_header, out string[] feas_datas);
                    if (feas_datas == null)
                    {
                        is_error = true;
                        break;
                    }
                    object_feas_datas.AddRange(feas_datas);

                    // first csv init
                    if (!is_init_feas[object_index])
                    {
                        csvf_obj.Csv_Init(feas_header, feas_data_path);
                        is_init_feas[object_index] = true;
                    }

                    //Write to csv
                    csvf_obj.WriteCsvDatasnew(object_feas_datas, feas_data_path);

                    // Close file
                    csvf_obj.FileClose(feas_data_path);
                }

            }
        }

        if (is_error)
        {
            return;
        }



    }
    private void Get_Object_Datas_2(int index,
                                    string name,
                                    out string header,
                                    out string[] object_datas)
    {
        // read csv, get corresponding windows datas
        string raw_data_path = csvf.BinSourcesFolder + labels_list_object[index] + "_" + labels_list[index] + "_" + name;

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

    private int Find_Object_Index(List<string>mylist, string object_string)
    {
        // find object index
        int res_index = 0;
        for (int j = 0; j < mylist.Count; j++)
        {
            if (mylist[j].Contains(object_string)) // (you use the word"contains". either equals or indexof might be appropriate)
            {
                res_index = j;
                break;
            }
        }
        return res_index;
    }

    public List<string[]> Get_Windows_Datas(string[] CSVDatas, int data_window_size, int data_window_step)
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
            for (int i = 0, j = 0; i < row_len - data_window_size; i += data_window_step, j++) //j = window_index
            {
                one_window_data = next_window(raw_datas, i, data_window_size);
                all_windows_datas.Add(one_window_data);
            }
        }
        else
        {
            //Debug.Log("当前数据未有一个数据窗口长度，数据窗口长度为：" + row_len);
            message_t.text += "当前数据未有一个数据窗口长度+ \n" +
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
            one_window[j] = data[j + i];
        }

        return one_window;
    }

    private string Calculate_Feature(int window_index,string[] datas)
    {
        float[] acc_x_datas = new float[Feature_extraction_Control.three_axis_acc_feas_t_Length];
        float[] acc_y_datas = new float[Feature_extraction_Control.three_axis_acc_feas_t_Length];
        float[] acc_z_datas = new float[Feature_extraction_Control.three_axis_acc_feas_t_Length];
        float[] syn_acc_datas = new float[Feature_extraction_Control.synthetic_acc_feas_t_Length];

        // add info to the first row
        int begin = 0;
        int end = Data_interception_Control.window_size - 1;
        int len = Data_interception_Control.window_size;  
        bool action_exist = true;
        string frow = datas[0];
        frow += window_index.ToString() + "," +
                action_exist.ToString() + "," +
                begin.ToString() + "," +
                end.ToString() + "," +
                len.ToString() + ",";
        datas[0] = frow;

        // calculate feacture
        Calculate_func.Get_Average_Energy(datas, out float average_energy);
        Calculate_func.Get_Acc_Mean(datas, out acc_x_datas[0], out acc_y_datas[0], out acc_z_datas[0]);
        Calculate_func.Get_Acc_Std(datas, out acc_x_datas[1], out acc_y_datas[1], out acc_z_datas[1]);
        Calculate_func.Get_Acc_Max(datas, out acc_x_datas[2], out acc_y_datas[2], out acc_z_datas[2]);
        Calculate_func.Get_Acc_Min(datas, out acc_x_datas[3], out acc_y_datas[3], out acc_z_datas[3]);
        Calculate_func.Get_Acc_Pv(datas, out acc_x_datas[4], out acc_y_datas[4], out acc_z_datas[4]);
        Calculate_func.Get_Syn_Acc_Mean(datas, out syn_acc_datas[0]);
        Calculate_func.Get_Syn_Acc_Std(datas, out syn_acc_datas[1]);

        // get new_row
        string new_row = "";
        string[] first_row = datas[0].Split(',');
        string label = first_row[6]; // Label
        new_row += label + ",";
        if (Feature_extraction_Control.features[0] != null)
        {
            new_row += average_energy + ",";
        }
        for (int i = 0; i < Feature_extraction_Control.three_axis_acc_feas_t_Length; i++)
        {
            if (Feature_extraction_Control.features[1 + 3 * i + i] != null)
            {
                if (Feature_extraction_Control.features[2 + 3 * i + i] != null)
                    new_row += acc_x_datas[i] + ",";
                if (Feature_extraction_Control.features[3 + 3 * i + i] != null)
                    new_row += acc_y_datas[i] + ",";
                if (Feature_extraction_Control.features[4 + 3 * i + i] != null)
                    new_row += acc_z_datas[i] + ",";

            }
        }
        for (int i = 0; i < Feature_extraction_Control.synthetic_acc_feas_t_Length; i++)
        {
            if (Feature_extraction_Control.features[4 * Feature_extraction_Control.three_axis_acc_feas_t_Length + 1 + i] != null)
                new_row += syn_acc_datas[i] + ",";
        }

        return new_row;
    }

    public void Set_targetJNTdp()
    {
        // get options 
        List<Dropdown.OptionData> options = target_JNT_dp.options;
        options.Clear();
        string target_bone = target_bone_dp.options[target_bone_dp.value].text;
        // modify options
        if (target_bone == "Body")
        {
            options.Add(new Dropdown.OptionData("hips_JNT"));
            options.Add(new Dropdown.OptionData("spine_JNT"));
            options.Add(new Dropdown.OptionData("spine1_JNT"));
            options.Add(new Dropdown.OptionData("spine2_JNT"));
        }
        else if (target_bone == "Left_Arm")
        {
            options.Add(new Dropdown.OptionData("l_shoulder_JNT"));
            options.Add(new Dropdown.OptionData("l_arm_JNT"));
            options.Add(new Dropdown.OptionData("l_forearm_JNT"));
            options.Add(new Dropdown.OptionData("l_hand_JNT"));
        }
        else if (target_bone == "Right_Arm")
        {
            options.Add(new Dropdown.OptionData("r_shoulder_JNT"));
            options.Add(new Dropdown.OptionData("r_arm_JNT"));
            options.Add(new Dropdown.OptionData("r_forearm_JNT"));
            options.Add(new Dropdown.OptionData("r_hand_JNT"));
        }
        else if (target_bone == "Left_Leg")
        {
            options.Add(new Dropdown.OptionData("l_upleg_JNT"));
            options.Add(new Dropdown.OptionData("l_leg_JNT"));
            options.Add(new Dropdown.OptionData("l_foot_JNT"));
            options.Add(new Dropdown.OptionData("l_toebase_JNT"));
        }
        else if (target_bone == "Right_Leg")
        {
            options.Add(new Dropdown.OptionData("r_upleg_JNT"));
            options.Add(new Dropdown.OptionData("r_leg_JNT"));
            options.Add(new Dropdown.OptionData("r_foot_JNT"));
            options.Add(new Dropdown.OptionData("r_toebase_JNT"));
        }
        else if (target_bone == "Head")
        {
            options.Add(new Dropdown.OptionData("neck_JNT"));
            options.Add(new Dropdown.OptionData("head_JNT"));
        }
        else if (target_bone == "Left Hand")
        {
            options.Add(new Dropdown.OptionData("l_handThumb1_JNT"));
            options.Add(new Dropdown.OptionData("l_handThumb2_JNT"));
            options.Add(new Dropdown.OptionData("l_handThumb3_JNT"));
            options.Add(new Dropdown.OptionData("l_handIndex1_JNT"));
            options.Add(new Dropdown.OptionData("l_handIndex2_JNT"));
            options.Add(new Dropdown.OptionData("l_handIndex3_JNT"));
            options.Add(new Dropdown.OptionData("l_handMiddle1_JNT"));
            options.Add(new Dropdown.OptionData("l_handMiddle2_JNT"));
            options.Add(new Dropdown.OptionData("l_handMiddle3_JNT"));
            options.Add(new Dropdown.OptionData("l_handRing1_JNT"));
            options.Add(new Dropdown.OptionData("l_handRing2_JNT"));
            options.Add(new Dropdown.OptionData("l_handRing3_JNT"));
            options.Add(new Dropdown.OptionData("l_handPinky1_JNT"));
            options.Add(new Dropdown.OptionData("l_handPinky2_JNT"));
            options.Add(new Dropdown.OptionData("l_handPinky3_JNT"));
        }
        else if (target_bone == "Right_Hand")
        {
            options.Add(new Dropdown.OptionData("r_handThumb1_JNT"));
            options.Add(new Dropdown.OptionData("r_handThumb2_JNT"));
            options.Add(new Dropdown.OptionData("r_handThumb3_JNT"));
            options.Add(new Dropdown.OptionData("r_handIndex1_JNT"));
            options.Add(new Dropdown.OptionData("r_handIndex2_JNT"));
            options.Add(new Dropdown.OptionData("r_handIndex3_JNT"));
            options.Add(new Dropdown.OptionData("r_handMiddle1_JNT"));
            options.Add(new Dropdown.OptionData("r_handMiddle2_JNT"));
            options.Add(new Dropdown.OptionData("r_handMiddle3_JNT"));
            options.Add(new Dropdown.OptionData("r_handRing1_JNT"));
            options.Add(new Dropdown.OptionData("r_handRing2_JNT"));
            options.Add(new Dropdown.OptionData("r_handRing3_JNT"));
            options.Add(new Dropdown.OptionData("r_handPinky1_JNT"));
            options.Add(new Dropdown.OptionData("r_handPinky2_JNT"));
            options.Add(new Dropdown.OptionData("r_handPinky3_JNT"));
        }

        // set target JNT dropdown options
        target_JNT_dp.options = options;
        //Debug.Log("dp target JNT = " + target_JNT + Target_JNT);
    }

    private string Get_Savepath()
    {
        //初始化
        OpenFileName ofn = new OpenFileName();
        ofn.structSize = Marshal.SizeOf(ofn);
        ofn.filter = "tflite (*.tflite)";
        ofn.file = new string(new char[1024]);
        ofn.maxFile = ofn.file.Length;
        ofn.fileTitle = new string(new char[64]);
        ofn.maxFileTitle = ofn.fileTitle.Length;
        string path = Application.streamingAssetsPath;
        path = path.Replace('/', '\\');
        ofn.initialDir = path;  //默认路径
        ofn.title = "Save model";
        ofn.defExt = "tflite";//显示文件的类型  
        //注意 一下项目不一定要全选 但是0x00000008项不要缺少  
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR  

        if (SaveDll.GetSaveFileName(ofn))
        {

        }

        string[] Splitstr = { "\0" };
        string[] strs = ofn.file.Split(Splitstr, StringSplitOptions.RemoveEmptyEntries);

        //Debug.Log(strs[0]);

        return strs[0];
    }

    public void GetTimeLongAgo(double t)
    {
        double num;
        int second;
        int minute;
        if (t < 60)
        {
            //Debug.Log("00:00:"+ (int)t);
            message_t.text = "00:00:" + (int)t + '\n';
        }
        else if (t >= 60 && t < 3600)
        {
            num = Math.Floor(t / 60);
            second = (int)t - 60 * (int)num;
            //Debug.Log("00:"+(int)num + ":"+ second);
            message_t.text = "00:" + (int)num + ":" + second + '\n';
        }
        else if (t >= 3600 && t < 86400)
        {
            num = Math.Floor(t / 3600);
            minute = (int)Math.Floor((t - 3600 * num) / 60);
            second = (int)t - 3600 * (int)num - 60 * (int)minute;
            //Debug.Log("{0}小时"+ num);
            message_t.text = num + ":" + minute + ":" + second + '\n';
        }

    }

}
