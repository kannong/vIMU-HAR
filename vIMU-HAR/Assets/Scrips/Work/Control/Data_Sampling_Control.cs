using Csv_Function;
using IMUSim;
using JNT_Reaction;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEditor;
using UnityEngine.Pool;
using TMPro;

public class Data_Sampling_Control : MonoBehaviour
{
    private CsvFunction csvf = new CsvFunction("IMUSim");
    private List<bool> first_csv = new List<bool>();

    private List<Dropdown> avatar_action_dps = new List<Dropdown>();
    private List<Dropdown> target_bone_dps = new List<Dropdown>();
    private List<Dropdown> target_JNT_dps = new List<Dropdown>();
    private List<Toggle> active_tgs = new List<Toggle>();

    public GameObject[] object_selection_bars; // 页面上初始的对象选择栏
    public Animator[] animators; // 动画控制器

    private JNTReaction jntrea = new JNTReaction();
    private Transform[] target_bone_tf;
    private List<Transform> target_JNT_tfs = new List<Transform>();
    private List<string> avatar_actions = new List<string>();
    private List<string> target_bones = new List<string>();
    private List<string> target_JNTs = new List<string>();
    private List<bool> is_active = new List<bool>();
    private IMU_Sim[] imusim;
    private bool is_sampling = false;
    private bool[] has_object = new bool[Main_Canvas_Control.all_class_num];

    private long starttime =  0;
    private long endtime = 0;

    public Text message_t;
    public GameObject confirm_box;
    public GameObject confirm_box_1;
    public GameObject perfab;
    public GameObject canvas;
    private int object_selection_bar_num = 3;


    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("数据采样面板");
        //Debug.Log("object num= " + Main_Canvas_Control.object_num + 
        //          " avater name= " + Main_Canvas_Control.avatar_name[0] +
        //          " bone name= " + Main_Canvas_Control.bone_name[0] +
        //          " joint name= " + Main_Canvas_Control.joint_name[0] + 
        //          " avater prefabs name= " + Main_Canvas_Control.avatar_prefabs[0]);

        // 设置原有的3个对象选择栏数据 添加子对象到对应的组件集合中
        for (int i = 0; i < object_selection_bars.Length; i++)
        {
            avatar_action_dps.Add(object_selection_bars[i].transform.GetChild(0).GetChild(0).GetComponentInChildren<UnityEngine.UI.Dropdown>());
            target_bone_dps.Add(object_selection_bars[i].transform.GetChild(0).GetChild(1).GetComponentInChildren<UnityEngine.UI.Dropdown>());
            target_JNT_dps.Add(object_selection_bars[i].transform.GetChild(0).GetChild(2).GetComponentInChildren<UnityEngine.UI.Dropdown>());
            active_tgs.Add(object_selection_bars[i].transform.GetChild(1).GetComponentInChildren<UnityEngine.UI.Toggle>());
            //Debug.Log("avatar_actions dp" + avatar_action_dps[i].options[avatar_action_dps[i].value].text);
            //Debug.Log("target_bones dp" + target_bone_dps[i].options[target_bone_dps[i].value].text);
            //Debug.Log("target_JNT dp" + target_JNT_dps[i].options[target_JNT_dps[i].value].text);
            //Debug.Log("active tg" + active_tgs[i].isOn);

        }


        // get current value
        for (int i = 0; i < object_selection_bar_num; i++)
        {
            avatar_actions.Add(avatar_action_dps[i].options[avatar_action_dps[i].value].text);
            target_bones.Add(target_bone_dps[i].options[target_bone_dps[i].value].text);
            target_JNTs.Add(target_JNT_dps[i].options[target_JNT_dps[i].value].text);
            is_active.Add(active_tgs[i].isOn);
            // set target JNT dropdown according to the target bone
            Set_targetJNTdp(i);
        }

        // print message
        Print_Object_Message();

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


    private void FixedUpdate()
    {
        if(is_sampling)
        {
            for (int i = 0;i<Main_Canvas_Control.object_num;i++)
            {
                imusim[i].IMU_Calculate(); // 开启虚拟IMU
                csvf.Saveimudata_tocsvnew(imusim[i], first_csv[i]); // 保存数据到对应csv文件中

                // print message
                Print_IMUData_Message(i, first_csv[i]);

                first_csv[i] = false;// change flag

            }

        }
    }
    
    public void Add_Object()
    {
        // if object selection bar more than 10
        if (object_selection_bar_num >= Main_Canvas_Control.max_object_num)
        {
            confirm_box_1.SetActive(true);
            message_t.text = "Failed to add object selection bar";
            return;
        }

        // add object selection bar
        GameObject object_bar = Instantiate(perfab);
        object_bar.name = "Object_select_" + (object_selection_bar_num).ToString();
        object_bar.transform.GetChild(1).GetComponentInChildren<UnityEngine.UI.Toggle>().GetComponentInChildren<Text>().text =
            "Object" + object_selection_bar_num + " Active";
        object_bar.transform.SetParent(canvas.transform, false);
        object_bar.SetActive(true);

        // 添加子对象到对应的组件集合中
        avatar_action_dps.Add(object_bar.transform.GetChild(0).GetChild(0).GetComponentInChildren<UnityEngine.UI.Dropdown>());
        target_bone_dps.Add(object_bar.transform.GetChild(0).GetChild(1).GetComponentInChildren<UnityEngine.UI.Dropdown>());
        target_JNT_dps.Add(object_bar.transform.GetChild(0).GetChild(2).GetComponentInChildren<UnityEngine.UI.Dropdown>());
        active_tgs.Add(object_bar.transform.GetChild(1).GetComponentInChildren<UnityEngine.UI.Toggle>());
        
        // 添加组件值到对应集合中
        avatar_actions.Add(avatar_action_dps[object_selection_bar_num].options[avatar_action_dps[object_selection_bar_num].value].text);
        target_bones.Add(target_bone_dps[object_selection_bar_num].options[target_bone_dps[object_selection_bar_num].value].text);
        target_JNTs.Add(target_JNT_dps[object_selection_bar_num].options[target_JNT_dps[object_selection_bar_num].value].text);
        is_active.Add(active_tgs[object_selection_bar_num].isOn);

        // 添加事件
        avatar_action_dps[object_selection_bar_num].onValueChanged.AddListener((int index) => AvataractionDP_Change());
        target_bone_dps[object_selection_bar_num].onValueChanged.AddListener((int index) => TargetboneDP_Change());
        target_JNT_dps[object_selection_bar_num].onValueChanged.AddListener((int index) => TargetJNTDP_Change());
        active_tgs[object_selection_bar_num].onValueChanged.AddListener((bool bo) => ActiveTG_Change());

        // set target JNT dropdown according to the target bone
        Set_targetJNTdp(object_selection_bar_num);

        object_selection_bar_num++;
    }
    public void Start_Sample()
    {
        for(int i = 0;i<animators.Length;i++)
        {
            if (has_object[i] == true)
            {
                animators[i].SetBool("start", true);
            }
        }
        for (int i = 0; i < Main_Canvas_Control.object_num; i++) // first csv
        {
            first_csv[i] = true;
        }
        is_sampling = true;

        // time
        DateTime now = DateTime.Now;
        Debug.Log("开始北京时间：" + now);
        Debug.Log("开始北京时间：时 分 秒：" + now.Hour + now.Minute + now.Second);
        starttime = now.Hour*3600+now.Minute*60+now.Second;

    }

    public void Stop_Sample()
    {
        for (int i = 0; i < animators.Length; i++)
        {
            if (has_object[i] == true)
            {
                animators[i].SetBool("start", false);
            }
        }
        is_sampling = false;

        // print message
        Print_End_Message();
    }
    
    public void Set_Object()
    {
        // Contains duplicate elements or not
        List<string> actions_temp = new List<string>();
        int count = 0;
        for (int i = 0; i < object_selection_bar_num; i++)
        {
            if (is_active[i])
            {
                actions_temp.Add(avatar_actions[i]);
                count++;
            }
        }
        foreach (var item in actions_temp)
        {
            Debug.Log(item);
        }
        bool repeatable = actions_temp.GroupBy(n => n).Any(c => c.Count() > 1);
        if(count == 1) repeatable = false;
        Debug.Log("是否含有重复元素" + repeatable);
        // if contains duplicate elements
        if (repeatable)
        {
            confirm_box.SetActive(true);
            message_t.text = "object set failed";
            return;
        }

        //Debug.Log("set object");
        // get target JNT
        GetTarget_JNT();
        // set target JNT to the globe
        SetTarget_JNT();
        // add IMU to target JNT
        AddIMUto_Object();

        // print message
        if (!repeatable)
        {
            Print_Object_Message();
            message_t.text += "object set finish";
        }

        // 新增：是否列表中已经存在对象
        Check_object_exist();

        // csv num
        first_csv.Clear();
        for (int i = 0; i < Main_Canvas_Control.object_num; i++) // first csv
        {
            first_csv.Add(true);
        }
    }

    public void ActiveTG_Change()
    {
        for (int i = 0; i < object_selection_bar_num; i++)
            is_active[i] = active_tgs[i].isOn;

        //Debug.Log("active change" + is_active[object_selection_bar_num-1]);

    }
    public void AvataractionDP_Change()
    {
        for (int i = 0; i < object_selection_bar_num; i++)
        {
            avatar_actions[i] = avatar_action_dps[i].options[avatar_action_dps[i].value].text;

            //Debug.Log("avatar actions change" + i + "=" + avatar_action_dps[i].value);
        }

    }
    public void TargetboneDP_Change()
    {
        // get current value
        for (int i = 0; i < object_selection_bar_num; i++)
        {
            target_bones[i] = target_bone_dps[i].options[target_bone_dps[i].value].text;
            // set target JNT dropdown according to the target bone
            Set_targetJNTdp(i);
        }
        //Debug.Log("target_bones change" + target_bone_dps[object_selection_bar_num - 1].value);

    }
    public void TargetJNTDP_Change()
    {
        for (int i = 0; i < object_selection_bar_num; i++)
            target_JNTs[i] = target_JNT_dps[i].options[target_JNT_dps[i].value].text;
        
        //Debug.Log("target_JNTs change" + target_JNT_dps[object_selection_bar_num - 1].value);

    }

    public void GetTarget_JNT()
    {
        target_JNT_tfs.Clear();
        for (int i = 0; i < object_selection_bar_num; i++)
        {
            // get avatar prefabs gameobject
            GameObject avatar_obj = GameObject.Find(Main_Canvas_Control.avatar_prefabs[avatar_action_dps[i].value]);
            // get all children Transform and EulerAngles
            Transform[] JNTTransforms = avatar_obj.GetComponentsInChildren<Transform>();
            jntrea.Get_allJNT(JNTTransforms);
            //Debug.Log("avatar_action_dps value = " + avatar_action_dps[i].value + "avatar action = "  + avatar_actions[i]);
            // get target JNT
            //Debug.Log("target bone = " + target_bones[i]);
            if (target_bones[i] == "Body")
            {
                target_bone_tf = jntrea.Body_JNT;
            }
            else if (target_bones[i] == "Left Arm")
            {
                target_bone_tf = jntrea.Left_arm_JNT;
            }
            else if (target_bones[i] == "Right Arm")
            {
                target_bone_tf = jntrea.Right_arm_JNT;
            }
            else if (target_bones[i] == "Left Leg")
            {
                target_bone_tf = jntrea.Left_leg_JNT;
            }
            else if (target_bones[i] == "Right Leg")
            {
                target_bone_tf = jntrea.Right_leg_JNT;
            }
            else if (target_bones[i] == "Head")
            {
                target_bone_tf = jntrea.Head_JNT;
            }
            else if (target_bones[i] == "Left Hand")
            {
                target_bone_tf = jntrea.Left_hand_JNT;
            }
            else if (target_bones[i] == "Right Hand")
            {
                target_bone_tf = jntrea.Right_hand_JNT;
            }

            target_JNT_tfs.Add(target_bone_tf[target_JNT_dps[i].value]);
            //Debug.Log("target JNT = " + target_JNT_tfs[i].name);

        }

    }
    public void SetTarget_JNT()
    {
        int count = 0;
        for (int i=0;i<object_selection_bar_num;i++)
        {
            if (is_active[i])
            {
                Main_Canvas_Control.avatar_name[count] = avatar_actions[i];
                Main_Canvas_Control.bone_name[count] = target_bones[i];
                Main_Canvas_Control.joint_name[count] = target_JNTs[i];
                Main_Canvas_Control.target_JNT_tfs[count] = target_JNT_tfs[i];
                count++;
            }
        }
        Main_Canvas_Control.object_num = count; // actual object number
        ////mes print
        //string mes = "";
        //for (int i = 0; i < Main_Canvas_Control.object_num;i++)
        //{
        //    mes += " avater name= " + Main_Canvas_Control.avatar_name[i] +
        //           " bone name= "   + Main_Canvas_Control.bone_name[i] +
        //           " joint name= "  + Main_Canvas_Control.joint_name[i] + 
        //           " target JNT name=" + Main_Canvas_Control.target_JNT_tfs[i].name + '\n';
        //}
        //Debug.Log("object num= " + Main_Canvas_Control.object_num + '\n' + mes);
    }
    public void AddIMUto_Object()
    {
        imusim = new IMU_Sim[Main_Canvas_Control.object_num];

        for (int i = 0; i < Main_Canvas_Control.object_num; i++)
        {
            IMU_Sim temp = new IMU_Sim(Main_Canvas_Control.target_JNT_tfs[i], Main_Canvas_Control.avatar_name[i], Main_Canvas_Control.zoom);
            imusim[i] = temp;
            Debug.Log("imusim" + i + imusim[i].Avater_name + imusim[i].Obj.name + imusim[i].Zoom);
        }
    }
    public void Set_targetJNTdp(int i)
    {
        // get options 
        List<Dropdown.OptionData> options = target_JNT_dps[i].options;
        options.Clear();
        // modify options
        if (target_bones[i] == "Body")
        {
            options.Add(new Dropdown.OptionData("hips_JNT"));
            options.Add(new Dropdown.OptionData("spine_JNT"));
            options.Add(new Dropdown.OptionData("spine1_JNT"));
            options.Add(new Dropdown.OptionData("spine2_JNT"));
        }
        else if (target_bones[i] == "Left Arm")
        {
            options.Add(new Dropdown.OptionData("l_shoulder_JNT"));
            options.Add(new Dropdown.OptionData("l_arm_JNT"));
            options.Add(new Dropdown.OptionData("l_forearm_JNT"));
            options.Add(new Dropdown.OptionData("l_hand_JNT"));
        }
        else if (target_bones[i] == "Right Arm")
        {
            options.Add(new Dropdown.OptionData("r_shoulder_JNT"));
            options.Add(new Dropdown.OptionData("r_arm_JNT"));
            options.Add(new Dropdown.OptionData("r_forearm_JNT"));
            options.Add(new Dropdown.OptionData("r_hand_JNT"));
        }
        else if (target_bones[i] == "Left Leg")
        {
            options.Add(new Dropdown.OptionData("l_upleg_JNT"));
            options.Add(new Dropdown.OptionData("l_leg_JNT"));
            options.Add(new Dropdown.OptionData("l_foot_JNT"));
            options.Add(new Dropdown.OptionData("l_toebase_JNT"));
        }
        else if (target_bones[i] == "Right Leg")
        {
            options.Add(new Dropdown.OptionData("r_upleg_JNT"));
            options.Add(new Dropdown.OptionData("r_leg_JNT"));
            options.Add(new Dropdown.OptionData("r_foot_JNT"));
            options.Add(new Dropdown.OptionData("r_toebase_JNT"));
        }
        else if (target_bones[i] == "Head")
        {
            options.Add(new Dropdown.OptionData("neck_JNT"));
            options.Add(new Dropdown.OptionData("head_JNT"));
        }
        else if (target_bones[i] == "Left Hand")
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
        else if (target_bones[i] == "Right Hand")
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
        target_JNT_dps[i].options = options;
        // get target JNT
        target_JNTs[i] = target_JNT_dps[i].options[target_JNT_dps[i].value].text;
        //Debug.Log("dp target JNT = " + target_JNT + Target_JNT);
    }
    public void Print_Object_Message()
    {
        string mes = "";
        mes += "object num= " + Main_Canvas_Control.object_num.ToString() + '\n';
        for (int i = 0; i < Main_Canvas_Control.object_num; i++)
        {
            mes += " object " + i + '\n' + ':' + 
                   " avater name= " + Main_Canvas_Control.avatar_name[i] + '\n' + ';' +
                   " bone name= " + Main_Canvas_Control.bone_name[i] + '\n' + ';' +
                   " joint name= " + Main_Canvas_Control.joint_name[i] + '\n';
        }
        message_t.text = mes;
    }
    public void Print_IMUData_Message(int index,bool first_csv)
    {

        if (first_csv)
        {
            message_t.text = "";
            message_t.text += "start samping \n";
            //message_t.text += "object num= " + Main_Canvas_Control.object_num.ToString() + '\n';
        }

        //message_t.text += "imusim " + index + ": " + imusim[index].Obj.name + '\n'+
        //       " ax=" + imusim[index].A_x + '\n' + 
        //       " ay=" + imusim[index].A_y + '\n' +
        //       " az=" + imusim[index].A_z + '\n' +
        //       " wx=" + imusim[index].W_x + '\n' + 
        //       " wy=" + imusim[index].W_y + '\n' + 
        //       " wz=" + imusim[index].W_z + '\n';
    }

    public void Print_End_Message()
    {       
        message_t.text += "samping finish \n";
    }

    private void Check_object_exist()
    {
        string[] actual_act = new string[object_selection_bar_num];
        for (int i = 0; i < object_selection_bar_num; i++)
        {
            if (is_active[i])
            {
                actual_act[i] = avatar_actions[i];
            }
            else
            {
                actual_act[i] = "None";
            }
        }
        for (int i = 0; i < Main_Canvas_Control.all_class_num; i++)
        {
            bool exists = actual_act.Where(w => w.Contains(Main_Canvas_Control.all_actions_name[i])).Any();
            has_object[i] = exists;
        }
        // get actual class_num
        int actual_class_num = 0;
        for (int i = 0; i < Main_Canvas_Control.all_class_num; i++)
        {
            if (has_object[i] == true)
                actual_class_num++;
        }
        Main_Canvas_Control.class_num = actual_class_num;
        //// debug test
        //for(int i = 0;i < Main_Canvas_Control.all_class_num; i++)
        //{
        //    Debug.Log("has object= "+i+" " + has_object[i]);
        //}
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
