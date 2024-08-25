using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JNT_Reaction;
using Csv_Function;
using System.IO;
using System.Text;
using UnityEngine.Analytics;


public class ReadJNTData_Knee_Kick : MonoBehaviour
{
    // get Knee_Kick JNTS all EulerAngles
    JNTReaction jntrea = new JNTReaction();
    CsvFunction csvf = new CsvFunction("Knee_Kick");

    // Start is called before the first frame update
    void Start()
    {
        // get all children transform
        Transform[] JNTTransforms = GetComponentsInChildren<Transform>();
        jntrea.Get_allJNT(JNTTransforms);

    }

    // Update is called once per frame
    void Update()
    {
        Safe_alleua_tocsv();

    }

    public void Safe_alleua_tocsv()
    {
        csvf.Saveeua_tocsv(jntrea.Body_JNT, "body");
        csvf.Saveeua_tocsv(jntrea.Left_arm_JNT, "left_arm");
        csvf.Saveeua_tocsv(jntrea.Right_arm_JNT, "right_arm");
        csvf.Saveeua_tocsv(jntrea.Left_leg_JNT, "left_leg");
        csvf.Saveeua_tocsv(jntrea.Right_leg_JNT, "right_leg");
        csvf.Saveeua_tocsv(jntrea.Head_JNT, "head");
        csvf.Saveeua_tocsv(jntrea.Left_hand_JNT, "left_hand");
        csvf.Saveeua_tocsv(jntrea.Right_hand_JNT, "right_hand");
    }

    public void Print_allenuangles()
    {
        foreach (var child in jntrea.Body_JNT)
        {
            Debug.Log("Body_JNT: " + child.name + child.transform.localEulerAngles);
        }
        foreach (var child in jntrea.Left_arm_JNT)
        {
            Debug.Log("Left_arm_JNT JNT: " + child.name + child.transform.localEulerAngles);
        }
        foreach (var child in jntrea.Right_arm_JNT)
        {
            Debug.Log("Right_arm_JNT JNT: " + child.name + child.transform.localEulerAngles);
        }
        foreach (var child in jntrea.Left_leg_JNT)
        {
            Debug.Log("Left_leg_JNT JNT: " + child.name + child.transform.localEulerAngles);
        }
        foreach (var child in jntrea.Right_leg_JNT)
        {
            Debug.Log("Right_leg_JNT JNT: " + child.name + child.transform.localEulerAngles);
        }
        foreach (var child in jntrea.Head_JNT)
        {
            Debug.Log("Head_JNT JNT: " + child.name + child.transform.localEulerAngles);
        }
        foreach (var child in jntrea.Left_hand_JNT)
        {
            Debug.Log("Left_hand JNT: " + child.name + child.transform.localEulerAngles);
        }
        foreach (var child in jntrea.Right_hand_JNT)
        {
            Debug.Log("Right_hand JNT: " + child.name + child.transform.localEulerAngles);
        }
    }


}
