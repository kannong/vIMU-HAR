using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Canvas_Control : MonoBehaviour
{
    // Start is called before the first frame update
    public static string[] avatar_prefabs = { "Knee_Kick_Adult_Female", "ReverseLunge_Adult_Female", "Ankle_Adult_Female" , 
        "Walking_Adult_Female", "SidetoSide_Adult_Female", "SideCrunch_Adult_Female", "HighKnee_Adult_Female" };
    public static string[] all_actions_name = { "Knee_Kick", "Reverse_Lunge", "Ankle", "Walking", "Sidetoside" , "SideCrunch", "HighKnee" };
    public static int all_class_num = 7; // 取决于当前导入的动作数
    public static int class_num = 1;  // actual class number
    public static int object_num; // actual object number
    public static int max_object_num = 10; // max object number(取决于界面上最多设置的对象数量)
    public static string[] avatar_name = new string[max_object_num];
    public static string[] bone_name = new string[max_object_num];
    public static string[] joint_name = new string[max_object_num];
    public static Transform[] target_JNT_tfs = new Transform[max_object_num];
    public static int extracted_features_num = 18;
    public static int raw_features_num = 3; // 6 axis
    public static float zoom = 140; // 数字人缩放的比例

    private void Awake()
    {
        //Debug.Log("主控制面板");
        //Set default value
        object_num = 1; 
        avatar_name[0] = "Knee_Kick";
        bone_name[0] = "Body";
        joint_name[0] = "hips_JNT";
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
