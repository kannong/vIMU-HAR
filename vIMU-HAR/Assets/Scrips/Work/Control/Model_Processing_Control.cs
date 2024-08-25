using Csv_Function;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using ConnectPy;
using DecideTree;
using Unity.VisualScripting;
using RandomForest;
using NN_;
using CNN_;

public class Model_Processing_Control : MonoBehaviour
{
    public Toggle[] object_select_tgs;
    public Toggle is_normalize_tg;
    public Text total_num_t;
    public Text train_num_t;
    public Text test_num_t;
    public Dropdown model_select_dp;
    public Image[] model_para_set_igs;
    public static string object_model;
    private CsvFunction csvf = new CsvFunction("Data_Process");
    public GameObject confirm_box;
    public GameObject confirm_box_1;
    private bool is_synthesise = false;
    public Text message_t;

    public static bool is_pretrain = false;

    // Start is called before the first frame update
    void Start()
    {
        // Create object select toggles
        Object_Toggles_Creates();
        // Set defalut value
        object_model = model_select_dp.options[model_select_dp.value].text;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Data_Synthesise()
    {
        List<string> object_train_datas = new List<string>();
        List<string> object_test_datas = new List<string>();
        string all_train_data_path = csvf.BinSourcesFolder + "all_train_data.csv";
        string all_test_data_path = csvf.BinSourcesFolder + "all_test_data.csv";
        string header;
        List<string> object_train_datas_r = new List<string>();
        List<string> object_test_datas_r = new List<string>();
        string all_train_data_path_r = csvf.BinSourcesFolder + "raw_all_train_data.csv";
        string all_test_data_path_r = csvf.BinSourcesFolder + "raw_all_test_data.csv";
        string header_r;
        bool first_init = false;
        bool is_error = false;

        // Get all object datas
        for (int i = 0; i < Main_Canvas_Control.object_num; i++)
        {
            // Get object select, get all object datas
            if (object_select_tgs[i].isOn)
            {

                // read csv, get corresponding object datas
                Get_Object_Datas(i, "_train_data.csv", out header, out string[] train_datas);
                Get_Object_Datas(i, "_test_data.csv", out header, out string[] test_datas);
                Get_Object_Datas(i, "_raw_train_data.csv", out header_r, out string[] train_datas_r);
                Get_Object_Datas(i, "_raw_test_data.csv", out header_r, out string[] test_datas_r);
                if (train_datas == null || test_datas == null)
                {
                    is_error = true;
                    break;
                }
                if (train_datas_r == null || test_datas_r == null)
                {
                    is_error = true;
                    break;
                }
                object_train_datas.AddRange(train_datas);
                object_test_datas.AddRange(test_datas);
                object_train_datas_r.AddRange(train_datas_r);
                object_test_datas_r.AddRange(test_datas_r);

                // first csv init
                if (!first_init)
                {
                    //Debug.Log("header=" + header);
                    csvf.Csv_Init(header, all_train_data_path);
                    csvf.Csv_Init(header, all_test_data_path);
                    csvf.Csv_Init(header_r, all_train_data_path_r);
                    csvf.Csv_Init(header_r, all_test_data_path_r);
                    first_init = true;
                }

            }
        }

        if (is_error)
            return;

        //Write to csv
        csvf.WriteCsvDatasnew(object_train_datas, all_train_data_path);
        csvf.WriteCsvDatasnew(object_test_datas, all_test_data_path);
        csvf.WriteCsvDatasnew(object_train_datas_r, all_train_data_path_r);
        csvf.WriteCsvDatasnew(object_test_datas_r, all_test_data_path_r);

        // Close file
        csvf.FileClose(all_train_data_path);
        csvf.FileClose(all_test_data_path);
        csvf.FileClose(all_train_data_path_r);
        csvf.FileClose(all_test_data_path_r);

        //Reflesh paras
        total_num_t.text = (object_train_datas.Count + object_test_datas.Count).ToString();
        train_num_t.text = object_train_datas.Count.ToString();
        test_num_t.text = object_test_datas.Count.ToString();

        // is systheise
        is_synthesise = true;

        // print message
        message_t.text = "data synthesise finish" + '\n' +
                         "total num=" + total_num_t.text + '\n' +
                         "train num=" + train_num_t.text + '\n' +
                         "test num=" + test_num_t.text + '\n';
    }
    public void Model_Change()
    {
        object_model = model_select_dp.options[model_select_dp.value].text;
    }
    public void Model_Para_Settings()
    {
        //set object model
        object_model = model_select_dp.options[model_select_dp.value].text;
        model_para_set_igs[model_select_dp.value].gameObject.SetActive(true);
    }
    public void Model_Train()
    {
        List<string> para = new List<string>();
        
        // if is not systhesise
        if(!is_synthesise)
        {
            confirm_box_1.SetActive(true);
            return;
        }

        switch (model_select_dp.value)
        {
            case 0:
                para.Insert(0, object_model);
                para.Insert(1, (Decide_Tree.max_depth == "")         ? "0" : Decide_Tree.max_depth);
                para.Insert(2, (Decide_Tree.min_samples_split == "") ? "2" : Decide_Tree.min_samples_split);
                para.Insert(3, (Decide_Tree.min_samples_leaf == "")  ? "1" : Decide_Tree.min_samples_leaf);
                para.Insert(4, (Decide_Tree.max_features == "")      ? "0" : Decide_Tree.max_features);
                para.Insert(5, (Decide_Tree.max_leaf_nodes == "")    ? "0" : Decide_Tree.max_leaf_nodes);
                para.Insert(6, (is_normalize_tg.isOn == true) ? "True" : "False");
                break;
            case 1:
                para.Insert(0, object_model);
                para.Insert(1, (Random_Forest.max_depth == "")         ? "0" : Random_Forest.max_depth);
                para.Insert(2, (Random_Forest.min_samples_split == "") ? "2" : Random_Forest.min_samples_split);
                para.Insert(3, (Random_Forest.min_samples_leaf == "")  ? "1" : Random_Forest.min_samples_leaf);
                para.Insert(4, (Random_Forest.max_features == "")      ? "0" : Random_Forest.max_features);
                para.Insert(5, (Random_Forest.max_leaf_nodes == "")    ? "0" : Random_Forest.max_leaf_nodes);
                para.Insert(6, (Random_Forest.n_estimators == "")      ? "0" : Random_Forest.n_estimators);
                para.Insert(7, (Random_Forest.n_jobs == "")            ? "0" : Random_Forest.n_jobs);
                para.Insert(8, (Random_Forest.max_samples == "")       ? "0" : Random_Forest.max_samples);
                para.Insert(9,  Random_Forest.bootstrap);
                para.Insert(10, Random_Forest.oob_score);
                para.Insert(11, Random_Forest.warm_start);
                para.Insert(12, (is_normalize_tg.isOn == true) ? "True" : "False");
                break;
            case 2:
                para.Insert(0,  object_model);
                para.Insert(1,  (NN.data_source == "") ? "0" : NN.data_source);
                para.Insert(2,  (NN.training_cycles == "") ? "0" : NN.training_cycles);
                para.Insert(3,  (NN.optimizer == "") ? "0" : NN.optimizer);
                para.Insert(4,  (NN.learning_rate == "") ? "0" : NN.learning_rate);
                para.Insert(5,  (NN.loss == "") ? "0" : NN.loss);
                para.Insert(6,  (NN.vaildation_spilt == "") ? "0" : NN.vaildation_spilt); // %
                para.Insert(7,  (NN.batch_size == "") ? "0" : NN.batch_size);
                para.Insert(8,   NN.fit_generator);
                para.Insert(9,  (NN.Input_Layer.features_num == "") ? "0" : NN.Input_Layer.features_num); 
                para.Insert(10, (NN.Dense_Layer_0.neurons_num == "") ? "0" : NN.Dense_Layer_0.neurons_num);
                para.Insert(11, (NN.Dense_Layer_0.activation == "") ? "0" : NN.Dense_Layer_0.activation);
                para.Insert(12, (NN.Dense_Layer_1.neurons_num == "") ? "0" : NN.Dense_Layer_1.neurons_num);
                para.Insert(13, (NN.Dense_Layer_1.activation == "") ? "0" : NN.Dense_Layer_1.activation);
                para.Insert(14, (NN.Output_Layer.classes_num == "") ? "0" : NN.Output_Layer.classes_num);
                break;
            case 3:
                para.Insert(0, object_model);
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
                para.Insert(22, (CNN.Output_Layer.classes_num == "") ? "0" : CNN.Output_Layer.classes_num);
                break;
            default:
                break;
        }
        //int t = 0;
        //foreach (var item in para)
        //{
        //    Debug.Log(t + " " + item + " " + '\n');
        //    t++;
        //}
        Connect_Python_func.RunPythonScript(Application.dataPath + "/Scrips/Work/py/model_train.py", para.ToArray());

        //string[] datas = Connect_Python_func.output.ToString().Split(' ');
        //for (int i = 0; i < datas.Length; i++)
        //{
        //    Debug.Log("output datas= " + i + " " + datas[i]);
        //}
        message_t.text = "model train finish" + '\n' +
                         Connect_Python_func.output.ToString() + '\n';

        // pre-train has finished
        is_pretrain = true;
    }
    public void Model_Predict()
    {

    }
    private void Get_Object_Datas(int index,
                                  string name,
                                  out string header, 
                                  out string[] object_datas)
    {
        // read csv, get corresponding windows datas
        string raw_data_path = csvf.BinSourcesFolder +
                               Main_Canvas_Control.avatar_name[index] + "_" +
                               Main_Canvas_Control.joint_name[index] + name;

        // if raw data is not exist
        if (!File.Exists(raw_data_path))
        {
            confirm_box.SetActive(true);
            header = string.Empty;
            object_datas = null;
            return;
        }

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
    public void Object_Toggles_Creates()
    {
        for (int i = 0; i < Main_Canvas_Control.object_num; i++)
        {
            object_select_tgs[i].gameObject.SetActive(true);
        }
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
