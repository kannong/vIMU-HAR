using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DropdownContral : MonoBehaviour
{
    private Dropdown avatar_action_dp;
    private Dropdown target_bone_dp;
    private Dropdown target_JNT_dp;
    private string avatar_action;
    private string target_bone;
    private string target_JNT;
    private int avatar_action_index;
    private int target_bone_index;
    private int target_JNT_index;
    public string Avatar_action {  get { return avatar_action; }  }
    public string Target_bone { get { return target_bone;  } }
    public string Target_JNT { get { return target_JNT; } }
    public int Avatar_action_index { get { return avatar_action_index; } }
    public int Target_bone_index { get { return target_bone_index; } }
    public int Target_JNT_index { get {  return target_JNT_index; } }
    public Dropdown Avatar_action_dp { get {  return avatar_action_dp; } set { avatar_action_dp = value; } }
    public Dropdown Target_bone_dp { get { return target_bone_dp; } set {  target_bone_dp = value; } }
    public Dropdown Target_JNT_dp {  get { return target_JNT_dp;} set { target_JNT_dp = value;} }

    // Start is called before the first frame update
    void Start()
    {
        avatar_action_dp = GameObject.Find("Avatar Action").GetComponent<Dropdown>();
        target_bone_dp = GameObject.Find("Target Bone").GetComponent<Dropdown>();
        target_JNT_dp = GameObject.Find("Target JNT").GetComponent<Dropdown>();
        // get current value
        avatar_action_index = avatar_action_dp.value;
        target_bone_index = target_bone_dp.value;
        avatar_action = avatar_action_dp.options[avatar_action_dp.value].text;
        target_bone = target_bone_dp.options[target_bone_dp.value].text;
        // set target JNT dropdown according to the target bone
        Set_targetJNTdp();
        Debug.Log("dp avatar_action  = " + avatar_action + Avatar_action);
        Debug.Log("dp target bone = " + target_bone + Target_bone);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AvataractionDP_Change()
    {
        avatar_action_index = avatar_action_dp.value;
        avatar_action = avatar_action_dp.options[avatar_action_dp.value].text;
    }
    public void TargetboneDP_Change()
    {
        target_bone_index = target_bone_dp.value;
        target_bone = target_bone_dp.options[target_bone_dp.value].text;
        Set_targetJNTdp();
    }
    public void TargetJNTDP_Change()
    {
        target_JNT_index = target_JNT_dp.value;
        target_JNT = target_JNT_dp.options[target_JNT_dp.value].text;
    }
    public void Set_targetJNTdp()
    {
        // get options 
        List<Dropdown.OptionData> options = target_JNT_dp.options;
        options.Clear();
        // modify options
        if(target_bone == "Body")
        {
            options.Add(new Dropdown.OptionData("hips JNT"));
            options.Add(new Dropdown.OptionData("spine JNT"));
            options.Add(new Dropdown.OptionData("spine1 JNT"));
            options.Add(new Dropdown.OptionData("spine2 JNT"));
        }
        else if(target_bone == "Left Arm")
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
        target_JNT_index = target_JNT_dp.value;
        target_JNT = target_JNT_dp.options[target_JNT_dp.value].text;
        //Debug.Log("dp target JNT = " + target_JNT + Target_JNT);
    }
}
