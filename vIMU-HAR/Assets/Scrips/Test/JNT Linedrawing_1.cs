using Csv_Function;
using JNT_Reaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

#if INPUT_SYSTEM_ENABLED
using Input = XCharts.Runtime.InputHelper;
#endif
using XCharts.Runtime;
using static UnityEditor.Progress;
namespace XCharts.Example
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    // Compared to the previous version, multiple JNTs can be displayed at the same time
    public class JNTLinedrawing_1 : MonoBehaviour
    {
        static int serie_num = 3;
        private string[] avatar_prefabs = { "Knee_Kick_Adult_Female", "ReverseLunge_Adult_Female" };
        public Dropdown[] avatar_action_dps;
        public Dropdown[] target_bone_dps;
        public Dropdown[] target_JNT_dps;
        public UnityEngine.UI.Toggle[] x_tgs;
        public UnityEngine.UI.Toggle[] y_tgs;
        public UnityEngine.UI.Toggle[] z_tgs;
        public UnityEngine.UI.Toggle[] a_tgs;
        private string[] avatar_actions = new string[serie_num];
        private string[] target_bones = new string[serie_num];
        private string[] target_JNTs = new string[serie_num];
        private JNTReaction jntrea = new JNTReaction();
        private Transform[] target_bone_tf;
        private Transform[] target_JNT_tfs = new Transform[serie_num];
        private LineChart chart;
        private Serie[] series_x = new Serie[serie_num];
        private Serie[] series_y = new Serie[serie_num];
        private Serie[] series_z = new Serie[serie_num];
        private bool is_drawing = false;

        // Start is called before the first frame update: set default value
        void Start()
        {
            // get current value
            for (int i = 0; i < serie_num; i++)
            {
                avatar_actions[i] = avatar_action_dps[i].options[avatar_action_dps[i].value].text;
                target_bones[i] = target_bone_dps[i].options[target_bone_dps[i].value].text;
                // set target JNT dropdown according to the target bone
                Set_targetJNTdp(i);
            }

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
            for(int i = 0;i < serie_num; i++)
            {
                series_x[i].ClearData();
                series_y[i].ClearData();
                series_z[i].ClearData();
            }
        }
        public void ActiveTG_Change()
        {
            for(int i = 0; i < serie_num;i++)
            {
                if (!a_tgs[i].isOn)
                {
                    series_x[i].show = false;
                    series_y[i].show = false;
                    series_z[i].show = false;
                }
                else
                {
                    series_x[i].show = x_tgs[i].isOn;
                    series_y[i].show = y_tgs[i].isOn;
                    series_z[i].show = z_tgs[i].isOn;
                }
            }
        }
        public void SeriexTG_Change()
        {
            //Debug.Log("x is on"+ x_tg.isOn);
            for(int i = 0;i < serie_num;i++) 
            {
                if (a_tgs[i].isOn)
                    series_x[i].show = x_tgs[i].isOn;

            }
        }

        public void SerieyTG_Change()
        {
            //Debug.Log("y is on" + y_tg.isOn);
            for (int i = 0; i < serie_num; i++)
            {
                if (a_tgs[i].isOn)
                    series_y[i].show = y_tgs[i].isOn;

            }
        }

        public void SeriezTG_Change()
        {
            //Debug.Log("z is on" + z_tg.isOn);
            for (int i = 0; i < serie_num; i++)
            {
                if (a_tgs[i].isOn)
                    series_z[i].show = z_tgs[i].isOn;

            }
        }

        public void AvataractionDP_Change()
        {
            for (int i = 0; i < serie_num; i++)
                avatar_actions[i] = avatar_action_dps[i].options[avatar_action_dps[i].value].text;
        }
        public void TargetboneDP_Change()
        {
            // get current value
            for (int i = 0; i < serie_num; i++)
            {
                target_bones[i] = target_bone_dps[i].options[target_bone_dps[i].value].text;
                // set target JNT dropdown according to the target bone
                Set_targetJNTdp(i);
            }
        }
        public void TargetJNTDP_Change()
        {
            for (int i = 0; i < serie_num; i++)
                target_JNTs[i] = target_JNT_dps[i].options[target_JNT_dps[i].value].text;
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
            string title = "";
            for (int i = 0; i < serie_num; i++)
                title += avatar_actions[i] + " - " + target_bones[i] + " - " + target_JNTs[i] + "\n";
            chart.EnsureChartComponent<Title>().text = title;
            chart.EnsureChartComponent<Title>().labelStyle.textStyle.fontSize = 12;
            //设置提示框和图例是否显示
            chart.EnsureChartComponent<Tooltip>().show = true;
            chart.EnsureChartComponent<Legend>().show = true;
            chart.EnsureChartComponent<Legend>().labelStyle.textStyle.fontSize = 12;
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
            for(int i = 0; i < serie_num; i++)
            {
                series_x[i] = chart.AddSerie<Line>(avatar_actions[i].Substring(0, 3) + "_" + target_JNT_tfs[i].name + "_x");
                series_y[i] = chart.AddSerie<Line>(avatar_actions[i].Substring(0, 3) + "_" + target_JNT_tfs[i].name + "_y");
                series_z[i] = chart.AddSerie<Line>(avatar_actions[i].Substring(0, 3) + "_" + target_JNT_tfs[i].name + "_z");
                series_x[i].symbol.show = false;
                series_y[i].symbol.show = false;
                series_z[i].symbol.show = false;
                series_x[i].lineType = LineType.Smooth;
                series_y[i].lineType = LineType.Smooth;
                series_z[i].lineType = LineType.Smooth;
                if (!a_tgs[i].isOn)
                {
                    series_x[i].show = false;
                    series_y[i].show = false;
                    series_z[i].show = false;
                }
                else
                {
                    series_x[i].show = x_tgs[i].isOn;
                    series_y[i].show = y_tgs[i].isOn;
                    series_z[i].show = z_tgs[i].isOn;
                }
            }
            
            //在下一帧刷新整个图表
            chart.RefreshChart();

        }
        public void Linechar_Adddata(int i)
        {
            chart.AddXAxisData(i.ToString());
            for (int j = 0;j<serie_num;j++)
            {
                chart.AddData(0 + 3 * j, target_JNT_tfs[j].transform.localEulerAngles.x);
                chart.AddData(1 + 3 * j, target_JNT_tfs[j].transform.localEulerAngles.y);
                chart.AddData(2 + 3 * j, target_JNT_tfs[j].transform.localEulerAngles.z);
            }
        }


        public void GetTarget_JNT()
        {
            for(int i = 0;i<serie_num;i++)
            {
                // get avatar prefabs gameobject
                GameObject avatar_obj = GameObject.Find(avatar_prefabs[avatar_action_dps[i].value]);
                // get all children Transform and EulerAngles
                Transform[] JNTTransforms = avatar_obj.GetComponentsInChildren<Transform>();
                jntrea.Get_allJNT(JNTTransforms);
                Debug.Log("avatar action = " + avatar_action_dps[i].value + avatar_actions[i]);
                // get target JNT
                Debug.Log("target bone = " + target_bones[i]);
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

                target_JNT_tfs[i] = target_bone_tf[target_JNT_dps[i].value];
                Debug.Log("target JNT = " + target_JNT_tfs[i].name);

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
                options.Add(new Dropdown.OptionData("hips JNT"));
                options.Add(new Dropdown.OptionData("spine JNT"));
                options.Add(new Dropdown.OptionData("spine1 JNT"));
                options.Add(new Dropdown.OptionData("spine2 JNT"));
            }
            else if (target_bones[i] == "Left Arm")
            {
                options.Add(new Dropdown.OptionData("l shoulder JNT"));
                options.Add(new Dropdown.OptionData("l arm JNT"));
                options.Add(new Dropdown.OptionData("l forearm JNT"));
                options.Add(new Dropdown.OptionData("l hand JNT"));
            }
            else if (target_bones[i] == "Right Arm")
            {
                options.Add(new Dropdown.OptionData("r shoulder JNT"));
                options.Add(new Dropdown.OptionData("r arm JNT"));
                options.Add(new Dropdown.OptionData("r forearm JNT"));
                options.Add(new Dropdown.OptionData("r hand JNT"));
            }
            else if (target_bones[i] == "Left Leg")
            {
                options.Add(new Dropdown.OptionData("l upleg JNT"));
                options.Add(new Dropdown.OptionData("l leg JNT"));
                options.Add(new Dropdown.OptionData("l foot JNT"));
                options.Add(new Dropdown.OptionData("l toebase JNT"));
            }
            else if (target_bones[i] == "Right Leg")
            {
                options.Add(new Dropdown.OptionData("r upleg JNT"));
                options.Add(new Dropdown.OptionData("r leg JNT"));
                options.Add(new Dropdown.OptionData("r foot JNT"));
                options.Add(new Dropdown.OptionData("r toebase JNT"));
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
            //Debug.Log(target_JNT_dp.value); defalut target JNT index = 0
            target_JNTs[i] = target_JNT_dps[i].options[target_JNT_dps[i].value].text;
            //Debug.Log("dp target JNT = " + target_JNT + Target_JNT);
        }

    }
}
