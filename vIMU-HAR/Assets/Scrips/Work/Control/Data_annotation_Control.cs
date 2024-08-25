using Csv_Function;
using IMUSim;
using RandomForest;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class Data_annotation_Control : MonoBehaviour
{
    public static int label_num = 7; //default
    public InputField[] inputFields;
    public static string[] labels = { "knee" , "reverse" , "ankle" , "walk" , "sidetoside", "sidecrunch", "highknee" };
    private CsvFunction csvf = new CsvFunction("IMUSim");
    public GameObject confirm_box;
    public Text message_t;

    // Start is called before the first frame update
    void Start()
    {
        label_num = inputFields.Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Label_Set()
    {
        //read raw data from csv files ,add label value ,write to csv files
        for (int j = 0; j < Main_Canvas_Control.object_num; j++)
        {

            string raw_data_path = csvf.BinSourcesFolder +
                                   Main_Canvas_Control.avatar_name[j] + "_" +
                                   Main_Canvas_Control.joint_name[j] + "_data.csv";
            string new_data_path = csvf.BinSourcesFolder +
                                   Main_Canvas_Control.avatar_name[j] + "_" +
                                   Main_Canvas_Control.joint_name[j] + "_label_data.csv";

            // if raw data is not exist
            if (!File.Exists(raw_data_path))
            {
                confirm_box.SetActive(true);
                return;
            }

            //read csv
            Encoding utf = Encoding.GetEncoding("UTF-8");
            string InfoConfig =  File.ReadAllText(raw_data_path, utf);
            InfoConfig = InfoConfig.Replace("\r", "");
            InfoConfig = InfoConfig.Replace("\"", "");
            string[] CSVDatas = InfoConfig.Split('\n');
            //Debug.Log("CSVDatas length=" + CSVDatas.Length);

            // first csv init
            CSVDatas[0] += "label" + ",";
            //Debug.Log("header=" + CSVDatas[0]);
            csvf.Csv_Init(CSVDatas[0], new_data_path); 

            // write datas
            for (int i = 1; i < CSVDatas.Length-1; i++)
            {
                if (CSVDatas[i] != "")
                {
                    if (Main_Canvas_Control.avatar_name[j] == "Knee_Kick")
                    {
                        if (inputFields[0].text != "")
                        {
                            labels[0] = inputFields[0].text;
                        }
                        else
                        {
                            labels[0] = "knee";
                            inputFields[0].text = labels[0];
                        }
                        CSVDatas[i] += labels[0].ToString() + ",";

                    }
                    else if (Main_Canvas_Control.avatar_name[j] == "Reverse_Lunge")
                    {
                        if (inputFields[1].text != "")
                        {
                            labels[1] = inputFields[1].text;
                        }
                        else
                        {
                            labels[1] = "reverse";
                            inputFields[1].text = labels[1];
                        }
                        CSVDatas[i] += labels[1].ToString() + ",";
                    }
                    else if (Main_Canvas_Control.avatar_name[j] == "Ankle")
                    {
                        if (inputFields[2].text != "")
                        {
                            labels[2] = inputFields[2].text;
                        }
                        else
                        {
                            labels[2] = "ankle";
                            inputFields[2].text = labels[2];
                        }
                        CSVDatas[i] += labels[2].ToString() + ",";
                    }
                    else if (Main_Canvas_Control.avatar_name[j] == "Walking")
                    {
                        if (inputFields[3].text != "")
                        {
                            labels[3] = inputFields[3].text;
                        }
                        else
                        {
                            labels[3] = "walk";
                            inputFields[3].text = labels[3];
                        }
                        CSVDatas[i] += labels[3].ToString() + ",";
                    }
                    else if (Main_Canvas_Control.avatar_name[j] == "Sidetoside")
                    {
                        if (inputFields[4].text != "")
                        {
                            labels[4] = inputFields[4].text;
                        }
                        else
                        {
                            labels[4] = "sidetoside";
                            inputFields[4].text = labels[4];
                        }
                        CSVDatas[i] += labels[4].ToString() + ",";
                    }
                    else if (Main_Canvas_Control.avatar_name[j] == "SideCrunch")
                    {
                        if (inputFields[5].text != "")
                        {
                            labels[5] = inputFields[5].text;
                        }
                        else
                        {
                            labels[5] = "sidecrunch";
                            inputFields[5].text = labels[5];
                        }
                        CSVDatas[i] += labels[5].ToString() + ",";
                    }
                    else if (Main_Canvas_Control.avatar_name[j] == "HighKnee")
                    {
                        if (inputFields[6].text != "")
                        {
                            labels[6] = inputFields[6].text;
                        }
                        else
                        {
                            labels[6] = "highknee";
                            inputFields[6].text = labels[6];
                        }
                        CSVDatas[i] += labels[6].ToString() + ",";
                    }

                    //Debug.Log("avatar name=" + Main_Canvas_Control.avatar_name[j]);
                }

                //Debug.Log("csvdata add label then=" + i + ' ' + CSVDatas[i]);
                // write datas
                csvf.WriteCsvnew(CSVDatas[i], new_data_path);

            }
            
        }

        // print message
        message_t.text = "label annotation finish" + '\n';
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
