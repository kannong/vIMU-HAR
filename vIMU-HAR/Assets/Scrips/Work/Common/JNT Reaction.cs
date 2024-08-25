using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JNT_Reaction
{

    enum avatar
    {
        hips_JNT=1,
        l_upleg_JNT, l_leg_JNT,l_foot_JNT,l_toebase_JNT,
        r_upleg_JNT, r_leg_JNT,r_foot_JNT,r_toebase_JNT,
        spine_JNT, spine1_JNT,spine2_JNT,
        l_shoulder_JNT, l_arm_JNT, l_forearm_JNT, l_hand_JNT,
        l_handIndex1_JNT,  l_handIndex2_JNT, l_handIndex3_JNT,
        l_handMiddle1_JNT, l_handMiddle2_JNT,l_handMiddle3_JNT,
        l_handPinky1_JNT,  l_handPinky2_JNT, l_handPinky3_JNT,
        l_handRing1_JNT,   l_handRing2_JNT,  l_handRing3_JNT,
        l_handThumb1_JNT,  l_handThumb2_JNT, l_handThumb3_JNT,
        neck_JNT, head_JNT,
        r_shoulder_JNT, r_arm_JNT, r_forearm_JNT, r_hand_JNT,
        r_handIndex1_JNT,  r_handIndex2_JNT, r_handIndex3_JNT,
        r_handMiddle1_JNT, r_handMiddle2_JNT,r_handMiddle3_JNT,
        r_handPinky1_JNT,  r_handPinky2_JNT, r_handPinky3_JNT,
        r_handRing1_JNT,   r_handRing2_JNT,  r_handRing3_JNT,
        r_handThumb1_JNT,  r_handThumb2_JNT, r_handThumb3_JNT
    };



    public class JNTReaction : MonoBehaviour
    {
        private Transform[] body_JNT;
        private Transform[] left_arm_JNT;
        private Transform[] right_arm_JNT;
        private Transform[] left_leg_JNT;
        private Transform[] right_leg_JNT;
        private Transform[] head_JNT;
        private Transform[] left_hand_JNT;
        private Transform[] right_hand_JNT;

        public Transform[] Body_JNT
        {
            get
            {
                return body_JNT;
            }
        }
        public Transform[] Left_arm_JNT
        {
            get
            {
                return left_arm_JNT;
            }
        }
        public Transform[] Right_arm_JNT
        {
            get
            {
                return right_arm_JNT;
            }
        }
        public Transform[] Left_leg_JNT
        {
            get 
            { 
                return left_leg_JNT;
            }
        }
        public Transform[] Right_leg_JNT
        {
            get
            {
                return right_leg_JNT;
            }
        }
        public Transform[] Head_JNT
        {
            get
            {
                return head_JNT;
            }
        }
        public Transform[] Left_hand_JNT
        {
            get
            {
                return left_hand_JNT;
            }
        }
        public Transform[] Right_hand_JNT
        {
            get 
            { 
                return right_hand_JNT;
            }
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Get_allJNT(Transform[] JNTTransforms)
        {
            Transform[] body = { JNTTransforms[(int)avatar.hips_JNT],    // Hips:  hips_JNT
                                 JNTTransforms[(int)avatar.spine_JNT],   // Spine: spine_JNT
                                 JNTTransforms[(int)avatar.spine1_JNT],   // Chest: spine1_JNT
                                 JNTTransforms[(int)avatar.spine2_JNT] }; // Upper Chest: spine2_JNT

            Transform[] left_arm = { JNTTransforms[(int)avatar.l_shoulder_JNT],   // Shouder: l_shoulder_JNT
                                     JNTTransforms[(int)avatar.l_arm_JNT],        // Upper Arm: l_arm_JNT
                                     JNTTransforms[(int)avatar.l_forearm_JNT],    // Lower Arm: l_forearm_JNT
                                     JNTTransforms[(int)avatar.l_hand_JNT] };     // Hand: l_hand_JNT 

            Transform[] right_arm = { JNTTransforms[(int)avatar.r_shoulder_JNT],   // Shouder: r_shoulder_JNT
                                      JNTTransforms[(int)avatar.r_arm_JNT],        // Upper Arm: r_arm_JNT
                                      JNTTransforms[(int)avatar.r_forearm_JNT],    // Lower Arm: r_forearm_JNT
                                      JNTTransforms[(int)avatar.r_hand_JNT] };     // Hand: r_hand_JNT 

            Transform[] left_leg = { JNTTransforms[(int)avatar.l_upleg_JNT],        // Upper Leg: l_upleg_JNT
                                     JNTTransforms[(int)avatar.l_leg_JNT],          // Lower Leg: l_leg_JNT
                                     JNTTransforms[(int)avatar.l_foot_JNT],         // Foot: l_foot_JNT
                                     JNTTransforms[(int)avatar.l_toebase_JNT] };    // Toes: l_toebase_JNT

            Transform[] right_leg = { JNTTransforms[(int)avatar.r_upleg_JNT],       // Upper Leg: r_upleg_JNT
                                     JNTTransforms[(int)avatar.r_leg_JNT],          // Lower Leg: r_leg_JNT
                                     JNTTransforms[(int)avatar.r_foot_JNT],         // Foot: r_foot_JNT
                                     JNTTransforms[(int)avatar.r_toebase_JNT] };    // Toes: r_toebase_JNT

            Transform[] head = { JNTTransforms[(int)avatar.neck_JNT],    // Neck: neck_JNT
                                 JNTTransforms[(int)avatar.head_JNT] };  // Head: head_JNT

            Transform[] left_hand = {   JNTTransforms[(int)avatar.l_handThumb1_JNT],    // Thumb Proximal:     l_handThumb1_JNT
                                        JNTTransforms[(int)avatar.l_handThumb2_JNT],    // Thumb Intermediate: l_handThumb2_JNT
                                        JNTTransforms[(int)avatar.l_handThumb3_JNT],    // Thumb Distal:       l_handThumb3_JNT
                                        JNTTransforms[(int)avatar.l_handIndex1_JNT],    // Index Proximal:     l_handIndex1_JNT
                                        JNTTransforms[(int)avatar.l_handIndex2_JNT],    // Index Intermediate: l_handIndex2_JNT
                                        JNTTransforms[(int)avatar.l_handIndex3_JNT],    // Index Distal:       l_handIndex3_JNT  
                                        JNTTransforms[(int)avatar.l_handMiddle1_JNT],   // Middle Proximal:    l_handMiddle1_JNT
                                        JNTTransforms[(int)avatar.l_handMiddle2_JNT],   // Middle Intermediate:l_handMiddle2_JNT
                                        JNTTransforms[(int)avatar.l_handMiddle3_JNT],   // Middle Distal:      l_handMiddle3_JNT
                                        JNTTransforms[(int)avatar.l_handRing1_JNT],     // Ring Proximal:      l_handRing1_JNT
                                        JNTTransforms[(int)avatar.l_handRing2_JNT],     // Ring Intermediate:  l_handRing2_JNT
                                        JNTTransforms[(int)avatar.l_handRing3_JNT],     // Ring Distal:        l_handRing3_JNT
                                        JNTTransforms[(int)avatar.l_handPinky1_JNT],    // Little Proximal:    l_handPinky1_JNT
                                        JNTTransforms[(int)avatar.l_handPinky2_JNT],    // Little Intermediate:l_handPinky2_JNT
                                        JNTTransforms[(int)avatar.l_handPinky3_JNT], }; // Little Distal:      l_handPinky3_JNT

            Transform[] right_hand = {  JNTTransforms[(int)avatar.r_handThumb1_JNT],    // Thumb Proximal:     r_handThumb1_JNT
                                        JNTTransforms[(int)avatar.r_handThumb2_JNT],    // Thumb Intermediate: r_handThumb2_JNT
                                        JNTTransforms[(int)avatar.r_handThumb3_JNT],    // Thumb Distal:       r_handThumb3_JNT
                                        JNTTransforms[(int)avatar.r_handIndex1_JNT],    // Index Proximal:     r_handIndex1_JNT
                                        JNTTransforms[(int)avatar.r_handIndex2_JNT],    // Index Intermediate: r_handIndex2_JNT
                                        JNTTransforms[(int)avatar.r_handIndex3_JNT],    // Index Distal:       r_handIndex3_JNT  
                                        JNTTransforms[(int)avatar.r_handMiddle1_JNT],   // Middle Proximal:    r_handMiddle1_JNT
                                        JNTTransforms[(int)avatar.r_handMiddle2_JNT],   // Middle Intermediate:r_handMiddle2_JNT
                                        JNTTransforms[(int)avatar.r_handMiddle3_JNT],   // Middle Distal:      r_handMiddle3_JNT
                                        JNTTransforms[(int)avatar.r_handRing1_JNT],     // Ring Proximal:      r_handRing1_JNT
                                        JNTTransforms[(int)avatar.r_handRing2_JNT],     // Ring Intermediate:  r_handRing2_JNT
                                        JNTTransforms[(int)avatar.r_handRing3_JNT],     // Ring Distal:        r_handRing3_JNT
                                        JNTTransforms[(int)avatar.r_handPinky1_JNT],    // Little Proximal:    r_handPinky1_JNT
                                        JNTTransforms[(int)avatar.r_handPinky2_JNT],    // Little Intermediate:r_handPinky2_JNT
                                        JNTTransforms[(int)avatar.r_handPinky3_JNT], }; // Little Distal:      r_handPinky3_JNT

            body_JNT = body;
            left_arm_JNT = left_arm;
            right_arm_JNT = right_arm;
            left_leg_JNT = left_leg;
            right_leg_JNT = right_leg;
            head_JNT = head;
            left_hand_JNT = left_hand;
            right_hand_JNT = right_hand;


        }

        public void Print_allenuangles()
        {
            foreach (var child in body_JNT)
            {
                Debug.Log("Body_JNT: " + child.name + child.transform.localEulerAngles);
            }
            foreach (var child in left_arm_JNT)
            {
                Debug.Log("Left_arm_JNT JNT: " + child.name + child.transform.localEulerAngles);
            }
            foreach (var child in right_arm_JNT)
            {
                Debug.Log("Right_arm_JNT JNT: " + child.name + child.transform.localEulerAngles);
            }
            foreach (var child in left_leg_JNT)
            {
                Debug.Log("Left_leg_JNT JNT: " + child.name + child.transform.localEulerAngles);
            }
            foreach (var child in right_leg_JNT)
            {
                Debug.Log("Right_leg_JNT JNT: " + child.name + child.transform.localEulerAngles);
            }
            foreach (var child in head_JNT)
            {
                Debug.Log("Head_JNT JNT: " + child.name + child.transform.localEulerAngles);
            }
            foreach (var child in left_hand_JNT)
            {
                Debug.Log("Left_hand JNT: " + child.name + child.transform.localEulerAngles);
            }
            foreach (var child in right_hand_JNT)
            {
                Debug.Log("Right_hand JNT: " + child.name + child.transform.localEulerAngles);
            }
        }


    }
    //    public void Get_allJNT(Transform[] JNTTransforms)
    //    {
    //        foreach (var child in JNTTransforms)
    //        {
    //            Debug.Log(child.name);
    //        }
    //        Transform[] body = { JNTTransforms[1],    // Hips:  hips_JNT
    //                             JNTTransforms[10],   // Spine: spine_JNT
    //                             JNTTransforms[11],   // Chest: spine1_JNT
    //                             JNTTransforms[12] }; // Upper Chest: spine2_JNT

    //        Transform[] left_arm = { JNTTransforms[13],   // Shouder: l_shoulder_JNT
    //                                 JNTTransforms[14],   // Upper Arm: l_arm_JNT
    //                                 JNTTransforms[15],   // Lower Arm: l_forearm_JNT
    //                                 JNTTransforms[16] }; // Hand: l_hand_JNT 

    //        Transform[] right_arm = { JNTTransforms[34],   // Shouder: r_shoulder_JNT
    //                                  JNTTransforms[35],   // Upper Arm: r_arm_JNT
    //                                  JNTTransforms[36],   // Lower Arm: r_forearm_JNT
    //                                  JNTTransforms[37] }; // Hand: r_hand_JNT 

    //        Transform[] left_leg = { JNTTransforms[2],   // Upper Leg: l_upleg_JNT
    //                                 JNTTransforms[3],   // Lower Leg: l_leg_JNT
    //                                 JNTTransforms[4],   // Foot: l_foot_JNT
    //                                 JNTTransforms[5] }; // Toes: l_toebase_JNT

    //        Transform[] right_leg = { JNTTransforms[6],   // Upper Leg: r_upleg_JNT
    //                                 JNTTransforms[7],    // Lower Leg: r_leg_JNT
    //                                 JNTTransforms[8],    // Foot: r_foot_JNT
    //                                 JNTTransforms[9] };  // Toes: r_toebase_JNT

    //        Transform[] head = { JNTTransforms[32],    // Neck: neck_JNT
    //                             JNTTransforms[33] };  // Head: head_JNT

    //        Transform[] left_hand = {   JNTTransforms[29],    // Thumb Proximal:     l_handThumb1_JNT
    //                                    JNTTransforms[30],    // Thumb Intermediate: l_handThumb2_JNT
    //                                    JNTTransforms[31],    // Thumb Distal:       l_handThumb3_JNT
    //                                    JNTTransforms[17],    // Index Proximal:     l_handIndex1_JNT
    //                                    JNTTransforms[18],    // Index Intermediate: l_handIndex2_JNT
    //                                    JNTTransforms[19],    // Index Distal:       l_handIndex3_JNT  
    //                                    JNTTransforms[20],    // Middle Proximal:    l_handMiddle1_JNT
    //                                    JNTTransforms[21],    // Middle Intermediate:l_handMiddle2_JNT
    //                                    JNTTransforms[22],    // Middle Distal:      l_handMiddle3_JNT
    //                                    JNTTransforms[26],    // Ring Proximal:      l_handRing1_JNT
    //                                    JNTTransforms[27],    // Ring Intermediate:  l_handRing2_JNT
    //                                    JNTTransforms[28],    // Ring Distal:        l_handRing3_JNT
    //                                    JNTTransforms[23],    // Little Proximal:    l_handPinky1_JNT
    //                                    JNTTransforms[24],    // Little Intermediate:l_handPinky2_JNT
    //                                    JNTTransforms[25], }; // Little Distal:      l_handPinky3_JNT

    //        Transform[] right_hand = {  JNTTransforms[50],    // Thumb Proximal:     r_handThumb1_JNT
    //                                    JNTTransforms[51],    // Thumb Intermediate: r_handThumb2_JNT
    //                                    JNTTransforms[52],    // Thumb Distal:       r_handThumb3_JNT
    //                                    JNTTransforms[38],    // Index Proximal:     r_handIndex1_JNT
    //                                    JNTTransforms[39],    // Index Intermediate: r_handIndex2_JNT
    //                                    JNTTransforms[40],    // Index Distal:       r_handIndex3_JNT  
    //                                    JNTTransforms[41],    // Middle Proximal:    r_handMiddle1_JNT
    //                                    JNTTransforms[42],    // Middle Intermediate:r_handMiddle2_JNT
    //                                    JNTTransforms[43],    // Middle Distal:      r_handMiddle3_JNT
    //                                    JNTTransforms[47],    // Ring Proximal:      r_handRing1_JNT
    //                                    JNTTransforms[48],    // Ring Intermediate:  r_handRing2_JNT
    //                                    JNTTransforms[49],    // Ring Distal:        r_handRing3_JNT
    //                                    JNTTransforms[44],    // Little Proximal:    r_handPinky1_JNT
    //                                    JNTTransforms[45],    // Little Intermediate:r_handPinky2_JNT
    //                                    JNTTransforms[46], }; // Little Distal:      r_handPinky3_JNT

    //        body_JNT = body;
    //        left_arm_JNT = left_arm;
    //        right_arm_JNT = right_arm;
    //        left_leg_JNT = left_leg;
    //        right_leg_JNT = right_leg;
    //        head_JNT = head;
    //        left_hand_JNT = left_hand;
    //        right_hand_JNT = right_hand;


    //    }

    //}
}
