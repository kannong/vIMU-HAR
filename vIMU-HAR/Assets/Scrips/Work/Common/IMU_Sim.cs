using JNT_Reaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace IMUSim
{
    public class IMU_Sim : MonoBehaviour
    {
        // 虚拟IMU类
        public static float dt = 0.1f; //0.02f
        private Transform obj;
        private string avater_name;
        private float zoom; // 缩放比例（因为数字人有缩放）
        private float a_x, a_y, a_z = 0; // m/s^2
        private float w_x, w_y, w_z = 0; // m/s
        private float last_x, last_y, last_z = 0;
        private float last_vx, last_vy, last_vz = 0;
        private float last_euax, last_euay, last_euaz = 0;
        public Transform Obj {  get { return obj; } set { obj = value; } }
        public string Avater_name { get { return avater_name; } set { avater_name = value; } }
        public float Zoom { get { return zoom; } set { zoom = value; } }
        public float A_x { get { return a_x; } }
        public float A_y { get { return a_y; } }
        public float A_z { get { return a_z; } }
        public float W_x { get { return w_x; } }
        public float W_y { get { return w_y; } }
        public float W_z { get { return w_z; } }

        public IMU_Sim() { }
        public IMU_Sim(Transform transform,string avater, float zoom)
        {
            IMU_Init(transform, avater, zoom);
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void IMU_Init(Transform transform, string avater, float zoom)
        {
            this.obj = transform;
            this.avater_name = avater;
            this.zoom = zoom;
            // Get initial position
            last_x = this.obj.position.x /this.zoom;
            last_y = this.obj.position.y / this.zoom;
            last_z = this.obj.position.z / this.zoom;
            last_euax = this.obj.eulerAngles.x / this.zoom;
            last_euay = this.obj.eulerAngles.y / this.zoom;
            last_euaz = this.obj.eulerAngles.z / this.zoom;
            //// 得到机体坐标系下的位置数据
            //last_x = last_x * (Cos(last_euay) * Cos(last_euaz) + Sin(last_euax) * Sin(last_euay) * Sin(last_euaz)) -
            //         last_y * (Cos(last_euay) * Sin(last_euaz) - Cos(last_euaz) * Sin(last_euax) * Sin(last_euay)) +
            //         last_z * Cos(last_euax) * Sin(last_euay);
            //last_y = last_y * Cos(last_euax) * Cos(last_euaz) -
            //         last_z * Sin(last_euax) +
            //         last_x * Cos(last_euax) * Sin(last_euaz);
            //last_z = last_y * (Sin(last_euay) * Sin(last_euaz) + Cos(last_euay) * Cos(last_euaz) * Sin(last_euax)) -
            //         last_x * (Cos(last_euaz) * Sin(last_euay) - Cos(last_euay) * Sin(last_euax) * Sin(last_euaz)) +
            //         last_z * Cos(last_euax) * Cos(last_euay);

            Debug.Log("imusim init");
            Debug.Log("imusim init " + this.obj.name +
                      " x=" + last_x + " y=" + last_y + " z=" + last_z +
                      " ax=" + A_x + " ay=" + A_y + " az=" + A_z +
                      " wx=" + W_x + " wy=" + W_y + " wz=" + W_z);
        }
        public float Sin(float x) {
            return Mathf.Sin(x * Mathf.Deg2Rad);
        }
        public float Cos(float x)
        {
            return Mathf.Cos(x * Mathf.Deg2Rad);
        }

        public void IMU_Calculate()
        {
            if (this.obj != null)
            {
                float cur_x = this.obj.position.x;
                float cur_y = this.obj.position.y;
                float cur_z = this.obj.position.z;
                float cur_euax = this.obj.eulerAngles.x;
                float cur_euay = this.obj.eulerAngles.y;
                float cur_euaz = this.obj.eulerAngles.z;
                //// 得到机体坐标系下的位置数据
                //cur_x = cur_x * (Cos(cur_euay) * Cos(cur_euaz) + Sin(cur_euax) * Sin(cur_euay) * Sin(cur_euaz)) - 
                //        cur_y * (Cos(cur_euay) * Sin(cur_euaz) - Cos(cur_euaz) * Sin(cur_euax) * Sin(cur_euay)) + 
                //        cur_z * Cos(cur_euax) * Sin(cur_euay);
                //cur_y = cur_y * Cos(cur_euax) * Cos(cur_euaz) - 
                //        cur_z * Sin(cur_euax) + 
                //        cur_x * Cos(cur_euax) * Sin(cur_euaz);
                //cur_z = cur_y * (Sin(cur_euay) * Sin(cur_euaz) + Cos(cur_euay) * Cos(cur_euaz) * Sin(cur_euax)) - 
                //        cur_x * (Cos(cur_euaz) * Sin(cur_euay) - Cos(cur_euay) * Sin(cur_euax) * Sin(cur_euaz)) + 
                //        cur_z * Cos(cur_euax) * Cos(cur_euay);
                // 计算加速度与角速度
                float dx = cur_x - last_x;
                float dy = cur_y - last_y;
                float dz = cur_z - last_z;
                float v_x = dx / dt;
                float v_y = dy / dt;
                float v_z = dz / dt;
                a_x = (v_x - last_vx) / dt;
                a_y = (v_y - last_vy) / dt;
                a_z = (v_z - last_vz) / dt;
                w_x = (cur_euax - last_euax) / dt;
                w_y = (cur_euay - last_euay) / dt;
                w_z = (cur_euaz - last_euaz) / dt;

                last_x = cur_x; last_y = cur_y; last_z = cur_z;
                last_vx = v_x; last_vy = v_y; last_vz = v_z;
                last_euax = cur_euax; last_euay = cur_euay; last_euaz = cur_euaz;
                //Debug.Log("imusim " + this.obj.name +
                //          " ax=" + A_x + " ay=" + A_y + " az=" + A_z +
                //          " wx=" + W_x + " wy=" + W_y + " wz=" + W_z);


            }
        }


    }
}
