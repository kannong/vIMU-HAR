using JNT_Reaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMUSim;
using Csv_Function;

public class IMUSim_Test : MonoBehaviour
{
    // IMUSim测试类，得到一个关节的IMU数据并保存到csv文件中
    private string[] avatar_prefabs = { "Knee_Kick_Adult_Female", "ReverseLunge_Adult_Female" };
    JNTReaction jntrea = new JNTReaction();
    IMU_Sim imusim;
    CsvFunction csvf = new CsvFunction("IMUSim");
    // Start is called before the first frame update
    void Start()
    {// get avatar prefabs gameobject
        GameObject avatar_obj = GameObject.Find(avatar_prefabs[0]);
        // get all children Transform and EulerAngles
        Transform[] JNTTransforms = avatar_obj.GetComponentsInChildren<Transform>();
        jntrea.Get_allJNT(JNTTransforms);
        //IMUSim init
        imusim = new IMU_Sim(jntrea.Body_JNT[0], avatar_prefabs[0], Main_Canvas_Control.zoom);

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        imusim.IMU_Calculate(); // 开启虚拟IMU
        csvf.Saveimudata_tocsv(imusim); // 保存数据到csv文件中
    }
}
