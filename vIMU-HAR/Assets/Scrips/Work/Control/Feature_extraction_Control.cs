using ConnectPy;
using Csv_Function;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Feature_extraction_Control : MonoBehaviour
{
    public static int max_features; // default
    public Toggle average_energy_t;
    public Toggle[] three_axis_acc_feas_t; //mean, std, max, min, pv //fc, aver, rms, var, std
    public Toggle[] axis_select_t; // x, y, z
    public Toggle[] synthetic_acc_feas_t; // syacc, syacc_mean, syacc_std
    private CsvFunction csvf = new CsvFunction("IMUSim");
    private List<string[]> windows_datas = new List<string[]>();
    public GameObject confirm_box;
    public Text message_t;
    public static string[] features;
    public static string features_header;
    public static int three_axis_acc_feas_t_Length;
    public static int synthetic_acc_feas_t_Length;

    // Start is called before the first frame update
    void Start()
    {
        three_axis_acc_feas_t_Length = three_axis_acc_feas_t.Length;
        synthetic_acc_feas_t_Length = synthetic_acc_feas_t.Length;
        max_features = 1 + three_axis_acc_feas_t_Length * axis_select_t.Length + synthetic_acc_feas_t_Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Feature_Extract()
    {
        // csv
        for (int i = 0; i < Main_Canvas_Control.object_num; i++)
        {
            // read csv, get corresponding windows datas
            string raw_data_path = csvf.BinSourcesFolder +
                                   Main_Canvas_Control.avatar_name[i] + "_" +
                                   Main_Canvas_Control.joint_name[i] + "_cleared_data.csv";
            string new_data_path = csvf.BinSourcesFolder +
                                   Main_Canvas_Control.avatar_name[i] + "_" +
                                   Main_Canvas_Control.joint_name[i] + "_feature_data.csv";
            string object_name = Main_Canvas_Control.avatar_name[i] + "_" +
                                 Main_Canvas_Control.joint_name[i];

            // if raw data is not exist
            if (!File.Exists(raw_data_path))
            {
                confirm_box.SetActive(true);
                return;
            }

            // read csv
            Encoding utf = Encoding.GetEncoding("UTF-8");
            string InfoConfig = File.ReadAllText(raw_data_path, utf);
            InfoConfig = InfoConfig.Replace("\r", "");
            InfoConfig = InfoConfig.Replace("\"", "");
            string[] CSVDatas = InfoConfig.Split('\n');
            //Debug.Log("CSVDatas length=" + CSVDatas.Length);

            // first csv init
            string header = Get_Header();
            features_header = header;
            //Debug.Log("header=" + header);
            csvf.Csv_Init(header, new_data_path);

            // get windows datas
            windows_datas.AddRange(Get_Windows_Datas(CSVDatas));
            //Debug.Log("window datas len=" + windows_datas.Count);

            // get frequency features
            string[] frequency_datas = Get_All_Frequency_Features(object_name);

            // calculate feacture, write new csv
            for (int j = 0; j < windows_datas.Count; j++)
            {
                // calculate feacture
                string newrow = Calculate_Feature(windows_datas[j], frequency_datas[j]);
                // write datas
                csvf.WriteCsvnew(newrow, new_data_path);

            }

            // clear list
            windows_datas.Clear();

        }

        message_t.text = "feature extraction finish" + '\n';
    }

    private List<string[]> Get_Windows_Datas(string[] datas)
    {
        string[] one_window_data = new string[Data_interception_Control.window_size];
        List<string[]> all_windows_datas = new List<string[]>();

        for (int i = 0; i < (datas.Length - 1) / Data_interception_Control.window_size; i++)
        {
            one_window_data = Get_One_Window(datas, i, Data_interception_Control.window_size);
            //Debug.Log("object window data first row = " + i + "  " + one_window_data[0]);
            all_windows_datas.Add(one_window_data);
            //Debug.Log("add window data " + i + "  " + all_windows_datas[i][0]);
        }

        return all_windows_datas;
    }
    private string[] Get_One_Window(string[] all_datas,int window_index,int window_size)
    {
        string[] one_window = new string[window_size];
        for (int i = 0; i < window_size; i++)
        {
            one_window[i] = all_datas[window_size*window_index + i + 1];
        }

        return one_window;
    }
    public string Get_Header()
    {
        int actual_fea_num = 0;
        string header = "";
        features = new string[max_features + three_axis_acc_feas_t_Length];
        header += "label" + ",";
        if (average_energy_t.isOn)
        {
            header += average_energy_t.name + ",";
            actual_fea_num++;
            features[0] = (average_energy_t.name);
        }
        int i = 0;
        foreach (var item in three_axis_acc_feas_t)
        {
            if (item.isOn)
            {
                features[1 + 3 * i + i] = (item.name);
                if (axis_select_t[0].isOn)
                {
                    header += item.name + "_x" + ",";
                    actual_fea_num++;
                    features[2 + 3 * i + i] = (item.name + "_x");
                }
                if (axis_select_t[1].isOn)
                {
                    header += item.name + "_y" + ",";
                    actual_fea_num++;
                    features[3 + 3 * i + i] = (item.name + "_y");
                }
                if (axis_select_t[2].isOn)
                {
                    header += item.name + "_z" + ",";
                    actual_fea_num++;
                    features[4 + 3 * i + i] = (item.name + "_z");
                }
            }
            i++;

        }
        i = 0;
        foreach (var item in synthetic_acc_feas_t)
        {
            if (item.isOn)
            {
                header += item.name + ",";
                actual_fea_num++;
                features[4 * three_axis_acc_feas_t_Length + 1 + i] = (item.name);
            }
        }
        Main_Canvas_Control.extracted_features_num = actual_fea_num;
        return header;
    }

    private string Calculate_Feature(string[] datas, string frequency_datas)
    {
        string new_row = "";
        float[] acc_x_datas = new float[three_axis_acc_feas_t.Length];
        float[] acc_y_datas = new float[three_axis_acc_feas_t.Length];
        float[] acc_z_datas = new float[three_axis_acc_feas_t.Length];
        float[] syn_acc_datas = new float[synthetic_acc_feas_t.Length];

        // calculate feacture
        Calculate_func.Get_Average_Energy(datas, out float average_energy);
        Calculate_func.Get_Acc_Mean(datas, out acc_x_datas[0], out acc_y_datas[0], out acc_z_datas[0]);
        Calculate_func.Get_Acc_Std(datas,  out acc_x_datas[1], out acc_y_datas[1], out acc_z_datas[1]);
        Calculate_func.Get_Acc_Max(datas,  out acc_x_datas[2], out acc_y_datas[2], out acc_z_datas[2]);
        Calculate_func.Get_Acc_Min(datas,  out acc_x_datas[3], out acc_y_datas[3], out acc_z_datas[3]);
        Calculate_func.Get_Acc_Pv(datas,   out acc_x_datas[4], out acc_y_datas[4], out acc_z_datas[4]);
        Calculate_func.Get_Acc_Fre_Gravity_Center(frequency_datas, out acc_x_datas[5], out acc_y_datas[5], out acc_z_datas[5]);
        Calculate_func.Get_Acc_Fre_Average(frequency_datas, out acc_x_datas[6], out acc_y_datas[6], out acc_z_datas[6]);
        Calculate_func.Get_Acc_Fre_RMS(frequency_datas, out acc_x_datas[7], out acc_y_datas[7], out acc_z_datas[7]);
        Calculate_func.Get_Acc_Fre_Variance(frequency_datas, out acc_x_datas[8], out acc_y_datas[8], out acc_z_datas[8]);
        Calculate_func.Get_Acc_Fre_Std(frequency_datas, out acc_x_datas[9], out acc_y_datas[9], out acc_z_datas[9]);
        Calculate_func.Get_Syn_Acc_Mean(datas, out syn_acc_datas[0]);
        Calculate_func.Get_Syn_Acc_Std(datas,  out syn_acc_datas[1]);

        // get new_row
        string[] first_row = datas[0].Split(',');
        string label = first_row[6]; // Label
        new_row += label + ",";
        if (average_energy_t.isOn)
        {
            new_row += average_energy + ",";
        }
        for (int i = 0; i < three_axis_acc_feas_t.Length; i++)
        {
            if (three_axis_acc_feas_t[i].isOn)
            {
                if (axis_select_t[0].isOn)
                    new_row += acc_x_datas[i] + ",";
                if (axis_select_t[1].isOn)
                    new_row += acc_y_datas[i] + ",";
                if (axis_select_t[2].isOn)
                    new_row += acc_z_datas[i] + ",";

            }
        }
        for (int i = 0; i < synthetic_acc_feas_t.Length; i++)
        {
            if (synthetic_acc_feas_t[i].isOn)
                new_row += syn_acc_datas[i] + ",";
        }

        return new_row;
    }
    private string[] Get_All_Frequency_Features(string object_name)
    {
        string raw_file_path = csvf.BinSourcesFolder + object_name + "_label_data.csv";
        string frequency_file_path = csvf.BinSourcesFolder + object_name + "_frequency_feature_data.csv";
        // get all frequency features by Python, save to csv
        string[] para = new string[5];
        para[0] = raw_file_path;
        para[1] = frequency_file_path;
        para[2] = Data_interception_Control.window_size.ToString();
        para[3] = Data_interception_Control.window_step.ToString();
        Connect_Python_func.RunPythonScript(Application.dataPath + "/Scrips/Work/py/get_all_fequency_features.py", para);

        // read csv, get all frequency features
        Encoding utf = Encoding.GetEncoding("UTF-8");
        string InfoConfig = File.ReadAllText(frequency_file_path, utf);
        InfoConfig = InfoConfig.Replace("\r", "");
        InfoConfig = InfoConfig.Replace("\"", "");
        string[] CSVDatas = InfoConfig.Split('\n');

        return CSVDatas;
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
