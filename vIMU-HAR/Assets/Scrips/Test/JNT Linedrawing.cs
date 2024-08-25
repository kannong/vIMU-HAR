using Csv_Function;
using JNT_Reaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if INPUT_SYSTEM_ENABLED
using Input = XCharts.Runtime.InputHelper;
#endif
using XCharts.Runtime;
using static UnityEditor.Progress;

namespace XCharts.Example
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class JNTLinedrawing : MonoBehaviour
    {
        private Dropdown avatar_action_dp;
        private Dropdown target_bone_dp;
        private Dropdown target_JNT_dp;
        private Toggle x_tg;
        private Toggle y_tg;
        private Toggle z_tg;
        private Toggle a_tg;
        private string avatar_action;
        private string target_bone;
        private string target_JNT;
        private JNTReaction jntrea = new JNTReaction();
        private Transform[] target_bone_tf;
        private Transform target_JNT_tf;
        private LineChart chart;
        private Serie serie_x,serie_y,serie_z;
        private string[] avatar_prefabs = { "Knee_Kick_Adult_Female", "ReverseLunge_Adult_Female" };
        private bool is_drawing = false;

        // Start is called before the first frame update: set default value
        void Start()
        {
            // get objects
            avatar_action_dp = GameObject.Find("Avatar Action").GetComponent<Dropdown>();
            target_bone_dp = GameObject.Find("Target Bone").GetComponent<Dropdown>();
            target_JNT_dp = GameObject.Find("Target JNT").GetComponent<Dropdown>();
            x_tg = GameObject.Find("x").GetComponent<Toggle>();
            y_tg = GameObject.Find("y").GetComponent<Toggle>();
            z_tg = GameObject.Find("z").GetComponent<Toggle>();
            a_tg = GameObject.Find("Series Active").GetComponent<Toggle>();
            // get current value
            avatar_action = avatar_action_dp.options[avatar_action_dp.value].text;
            target_bone = target_bone_dp.options[target_bone_dp.value].text;
            // set target JNT dropdown according to the target bone
            Set_targetJNTdp();
        }

        int i = 0;
        // Update is called once per frame
        void Update()
        {
            if (is_drawing)
            {
                Linechar_Adddata(i);
                i++;
            }
        }
        public void BeginButtonClick()
        {
            Debug.Log("Begin");
            // get target JNT
            GetTarget_JNT();
            //init linechar
            Linechar_Init();
            is_drawing = true;
            i = 0;
        }
        public void PauseButtonClick()
        {
            Debug.Log("Pause");
            is_drawing = false;
        }
        public void ActiveTG_Change()
        {

            if (!a_tg.isOn)
            {
                serie_x.show = false;
                serie_y.show = false;
                serie_z.show = false;
            }
            else
            {
                serie_x.show = x_tg.isOn;
                serie_y.show = y_tg.isOn;
                serie_z.show = z_tg.isOn;
            }
        }
        public void SeriexTG_Change()
        {
            //Debug.Log("x is on"+ x_tg.isOn);
            if(a_tg.isOn)
                serie_x.show = x_tg.isOn;
        }

        public void SerieyTG_Change()
        {
            //Debug.Log("y is on" + y_tg.isOn);
            if(a_tg.isOn)
                serie_y.show = y_tg.isOn;
        }

        public void SeriezTG_Change()
        {
            //Debug.Log("z is on" + z_tg.isOn);
            if(a_tg.isOn)
                serie_z.show = z_tg.isOn;
        }

        public void AvataractionDP_Change()
        {
            avatar_action = avatar_action_dp.options[avatar_action_dp.value].text;
        }
        public void TargetboneDP_Change()
        {
            target_bone = target_bone_dp.options[target_bone_dp.value].text;
            Set_targetJNTdp();
        }
        public void TargetJNTDP_Change()
        {
            target_JNT = target_JNT_dp.options[target_JNT_dp.value].text;
        }

        public void Linechar_Init()
        {
            chart = gameObject.GetComponent<LineChart>();
            if (chart == null)
            {
                chart = gameObject.AddComponent<LineChart>();
                chart.Init();
                chart.SetSize(580, 300);
            }

            //设置标题
            chart.EnsureChartComponent<Title>().show = true;
            chart.EnsureChartComponent<Title>().text = avatar_action + "-" +
                                                       target_bone + "-" +
                                                       target_JNT;

            //设置提示框和图例是否显示
            chart.EnsureChartComponent<Tooltip>().show = true;
            chart.EnsureChartComponent<Legend>().show = true;

            //设置坐标轴
            var xAxis = chart.EnsureChartComponent<XAxis>();
            var yAxis = chart.EnsureChartComponent<YAxis>();
            xAxis.show = true;
            yAxis.show = true;
            xAxis.type = Axis.AxisType.Category;
            yAxis.type = Axis.AxisType.Value;

            xAxis.splitNumber = 10;
            xAxis.boundaryGap = true;

            //清空默认数据，添加Line类型的Serie用于接收数据
            chart.RemoveData();
            serie_x = chart.AddSerie<Line>(target_JNT_tf.name + "_x");
            serie_y = chart.AddSerie<Line>(target_JNT_tf.name + "_y");
            serie_z = chart.AddSerie<Line>(target_JNT_tf.name + "_z");
            serie_x.symbol.show = false;
            serie_y.symbol.show = false;
            serie_z.symbol.show = false;
            serie_x.lineType = LineType.Smooth;
            serie_y.lineType = LineType.Smooth;
            serie_z.lineType = LineType.Smooth;
            if(!a_tg.isOn)
            {
                serie_x.show = false;
                serie_y.show = false;
                serie_z.show = false;
            }
            else
            {
                serie_x.show = x_tg.isOn;
                serie_y.show = y_tg.isOn;
                serie_z.show = z_tg.isOn;
            }
            
            //在下一帧刷新整个图表
            chart.RefreshChart();

        }
        public void Linechar_Adddata(int i)
        {
            chart.AddXAxisData(i.ToString());
            chart.AddData(0, target_JNT_tf.transform.localEulerAngles.x);
            chart.AddData(1, target_JNT_tf.transform.localEulerAngles.y);
            chart.AddData(2, target_JNT_tf.transform.localEulerAngles.z);
        }


        public void GetTarget_JNT()
        {
            // get avatar prefabs gameobject
            GameObject avatar_obj = GameObject.Find(avatar_prefabs[avatar_action_dp.value]);
            // get all children Transform and EulerAngles
            Transform[] JNTTransforms = avatar_obj.GetComponentsInChildren<Transform>();
            jntrea.Get_allJNT(JNTTransforms);
            Debug.Log("avatar action = "+ avatar_action_dp.value+avatar_action);
            // get target JNT
            Debug.Log("target bone = "+ target_bone);
            if (target_bone == "Body")
            {
                target_bone_tf = jntrea.Body_JNT;
            }
            else if (target_bone == "Left Arm")
            {
                target_bone_tf = jntrea.Left_arm_JNT;
            }
            else if (target_bone == "Right Arm")
            {
                target_bone_tf = jntrea.Right_arm_JNT;
            }
            else if (target_bone == "Left Leg")
            {
                target_bone_tf = jntrea.Left_leg_JNT;
            }
            else if (target_bone == "Right Leg")
            {
                target_bone_tf = jntrea.Right_leg_JNT;
            }
            else if (target_bone == "Head")
            {
                target_bone_tf = jntrea.Head_JNT;
            }
            else if (target_bone == "Left Hand")
            {
                target_bone_tf = jntrea.Left_hand_JNT;
            }
            else if (target_bone == "Right Hand")
            {
                target_bone_tf = jntrea.Right_hand_JNT;
            }
            
            target_JNT_tf = target_bone_tf[target_JNT_dp.value];
            Debug.Log("target JNT = " + target_JNT_tf.name);

        }

        public void Set_targetJNTdp()
        {
            // get options 
            List<Dropdown.OptionData> options = target_JNT_dp.options;
            options.Clear();
            // modify options
            if (target_bone == "Body")
            {
                options.Add(new Dropdown.OptionData("hips JNT"));
                options.Add(new Dropdown.OptionData("spine JNT"));
                options.Add(new Dropdown.OptionData("spine1 JNT"));
                options.Add(new Dropdown.OptionData("spine2 JNT"));
            }
            else if (target_bone == "Left Arm")
            {
                options.Add(new Dropdown.OptionData("l shoulder JNT"));
                options.Add(new Dropdown.OptionData("l arm JNT"));
                options.Add(new Dropdown.OptionData("l forearm JNT"));
                options.Add(new Dropdown.OptionData("l hand JNT"));
            }
            else if (target_bone == "Right Arm")
            {
                options.Add(new Dropdown.OptionData("r shoulder JNT"));
                options.Add(new Dropdown.OptionData("r arm JNT"));
                options.Add(new Dropdown.OptionData("r forearm JNT"));
                options.Add(new Dropdown.OptionData("r hand JNT"));
            }
            else if (target_bone == "Left Leg")
            {
                options.Add(new Dropdown.OptionData("l upleg JNT"));
                options.Add(new Dropdown.OptionData("l leg JNT"));
                options.Add(new Dropdown.OptionData("l foot JNT"));
                options.Add(new Dropdown.OptionData("l toebase JNT"));
            }
            else if (target_bone == "Right Leg")
            {
                options.Add(new Dropdown.OptionData("r upleg JNT"));
                options.Add(new Dropdown.OptionData("r leg JNT"));
                options.Add(new Dropdown.OptionData("r foot JNT"));
                options.Add(new Dropdown.OptionData("r toebase JNT"));
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
            else if (target_bone == "Right Hand")
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
            // get target JNT
            //Debug.Log(target_JNT_dp.value); defalut target JNT index = 0
            target_JNT = target_JNT_dp.options[target_JNT_dp.value].text;
            //Debug.Log("dp target JNT = " + target_JNT + Target_JNT);
        }

    }
}
