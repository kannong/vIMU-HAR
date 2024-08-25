using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class Calculate_func : MonoBehaviour
{
    public static float ax0 = 0;
    public static float ay0 = 0;
    public static float az0 = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static float[] Get_Acc_Abs_Sum(string[] one_window)
    {
        float[] res_datas = new float[one_window.Length];
        for (int i = 0; i < one_window.Length; i++)
        {
            string[] one_row = one_window[i].Split(',');
            float acc_x = 0;
            float acc_y = 0;
            float acc_z = 0;
            try
            {
                // Convert the strings to floats
                acc_x = float.Parse(one_row[0]);
                acc_y = float.Parse(one_row[1]);
                acc_z = float.Parse(one_row[2]);
            }
            catch { }
            float acc_x_abs = Math.Abs(acc_x);
            float acc_y_abs = Math.Abs(acc_y);
            float acc_z_abs = Math.Abs(acc_z);
            float res = acc_x_abs + acc_y_abs + acc_z_abs;
            res_datas[i] = res;
        }
        return res_datas;
    }

    public static void Get_Average_Energy(string[] one_window,
                                          out float average_energy)
    {
        float res = 0;
        float sum = 0;
        string[] first_row = one_window[0].Split(',');
        bool exist = Convert.ToBoolean(first_row[8]);
        int begin = Convert.ToInt16(first_row[9]);
        int end = Convert.ToInt16(first_row[10]);
        int len = Convert.ToInt16(first_row[11]);

        // 存在动作则返回范围内均值，不存在动作则返回所有数据的均值
        if (!exist)
        {
            begin = 0;
            end = one_window.Length - 1;
            len = one_window.Length;
        }

        for (int i = begin; i < end + 1; i++)
        {
            string[] one_row = one_window[i].Split(',');
            float acc_x = 0;
            float acc_y = 0;
            float acc_z = 0;
            try
            {
                // Convert the strings to floats
                acc_x = float.Parse(one_row[0]);
                acc_y = float.Parse(one_row[1]);
                acc_z = float.Parse(one_row[2]);
            }
            catch { }
            float one_energy = Math.Abs(acc_x - ax0) + Math.Abs(acc_y - ay0) + Math.Abs(acc_z - az0);
            sum += one_energy;
        }

        if ( exist )
        {
            res = sum / len;
        }
        else
        {
            res = 0;
        }

        average_energy = res;
    }

    public static void Get_Acc_Mean(string[] one_window, 
                                    out float acc_x_mean, out float acc_y_mean, out float acc_z_mean)
    {
        float acc_x_sum = 0;
        float acc_y_sum = 0;
        float acc_z_sum = 0;
        string[] first_row = one_window[0].Split(',');
        bool exist = Convert.ToBoolean(first_row[8]);
        int begin = Convert.ToInt16(first_row[9]);
        int end = Convert.ToInt16(first_row[10]);
        int len = Convert.ToInt16(first_row[11]);

        // 存在动作则返回范围内均值，不存在动作则返回所有数据的均值
        if ( !exist )
        {
            begin = 0;
            end = one_window.Length-1;
            len = one_window.Length;
        }

        for (int i = begin; i < end+1; i++)
        {
            string[] one_row = one_window[i].Split(',');
            float acc_x = 0;
            float acc_y = 0;
            float acc_z = 0;
            try
            {
                // Convert the strings to floats
                acc_x = float.Parse(one_row[0]);
                acc_y = float.Parse(one_row[1]);
                acc_z = float.Parse(one_row[2]);
            }
            catch { }
            acc_x_sum += acc_x;
            acc_y_sum+= acc_y;
            acc_z_sum+= acc_z;
        }
        acc_x_mean = acc_x_sum/len;
        acc_y_mean = acc_y_sum/len;
        acc_z_mean = acc_z_sum/len;
    }

    public static void Get_Acc_Std(string[] one_window,
                                   out float acc_x_std, out float acc_y_std, out float acc_z_std)
    {
        float acc_x_sum = 0;
        float acc_y_sum = 0;
        float acc_z_sum = 0;
        string[] first_row = one_window[0].Split(',');
        bool exist = Convert.ToBoolean(first_row[8]);
        int begin = Convert.ToInt16(first_row[9]);
        int end = Convert.ToInt16(first_row[10]);
        int len = Convert.ToInt16(first_row[11]);

        Get_Acc_Mean(one_window, out float acc_x_mean, out float acc_y_mean, out float acc_z_mean);

        // 存在动作则返回范围内标准差，不存在动作则返回所有数据的标准差
        if (!exist)
        {
            begin = 0;
            end = one_window.Length - 1;
            len = one_window.Length;
        }

        for (int i = begin; i < end + 1; i++)
        {
            string[] one_row = one_window[i].Split(',');
            float acc_x = 0;
            float acc_y = 0;
            float acc_z = 0;
            try
            {
                // Convert the strings to floats
                acc_x = float.Parse(one_row[0]);
                acc_y = float.Parse(one_row[1]);
                acc_z = float.Parse(one_row[2]);
            }
            catch { }
            acc_x_sum += (float)Math.Pow((acc_x - acc_x_mean), 2);
            acc_y_sum += (float)Math.Pow((acc_y - acc_y_mean), 2);
            acc_z_sum += (float)Math.Pow((acc_z - acc_z_mean), 2);
        }
        acc_x_std = (float)Math.Sqrt(acc_x_sum / len);
        acc_y_std = (float)Math.Sqrt(acc_y_sum / len);
        acc_z_std = (float)Math.Sqrt(acc_z_sum / len);
    }

    public static void Get_Acc_Max(string[] one_window,
                                   out float acc_x_max, out float acc_y_max, out float acc_z_max)
    {
        string[] first_row = one_window[0].Split(',');
        bool exist = Convert.ToBoolean(first_row[8]);
        int begin = Convert.ToInt16(first_row[9]);
        int end = Convert.ToInt16(first_row[10]);
        int len = Convert.ToInt16(first_row[11]);
        acc_x_max = 0;
        acc_y_max = 0;
        acc_z_max = 0;

        // 存在动作则返回范围内标准差，不存在动作则返回所有数据的标准差
        if (!exist)
        {
            begin = 0;
            end = one_window.Length - 1;
            len = one_window.Length;
        }

        for (int i = begin; i < end + 1; i++)
        {
            string[] one_row = one_window[i].Split(',');
            float acc_x = 0;
            float acc_y = 0;
            float acc_z = 0;
            try
            {
                // Convert the strings to floats
                acc_x = float.Parse(one_row[0]);
                acc_y = float.Parse(one_row[1]);
                acc_z = float.Parse(one_row[2]);
            }
            catch { }
            if (acc_x > acc_x_max)
                acc_x_max = acc_x;
            if(acc_y > acc_y_max)
                acc_y_max = acc_y;
            if( acc_z > acc_z_max)
                acc_z_max = acc_z;
        }
    }

    public static void Get_Acc_Min(string[] one_window,
                                   out float acc_x_min, out float acc_y_min, out float acc_z_min)
    {
        string[] first_row = one_window[0].Split(',');
        bool exist = Convert.ToBoolean(first_row[8]);
        int begin = Convert.ToInt16(first_row[9]);
        int end = Convert.ToInt16(first_row[10]);
        int len = Convert.ToInt16(first_row[11]);
        acc_x_min = 0;
        acc_y_min = 0;
        acc_z_min = 0;

        // 存在动作则返回范围内MIN，不存在动作则返回所有数据的MIN
        if (!exist)
        {
            begin = 0;
            end = one_window.Length - 1;
            len = one_window.Length;
        }

        for (int i = begin; i < end + 1; i++)
        {
            string[] one_row = one_window[i].Split(',');
            float acc_x = 0;
            float acc_y = 0;
            float acc_z = 0;
            try
            {
                // Convert the strings to floats
                acc_x = float.Parse(one_row[0]);
                acc_y = float.Parse(one_row[1]);
                acc_z = float.Parse(one_row[2]);
            }
            catch { }
            if (acc_x < acc_x_min)
                acc_x_min = acc_x;
            if (acc_y < acc_y_min)
                acc_y_min = acc_y;
            if (acc_z < acc_z_min)
                acc_z_min = acc_z;
        }
    }

    public static void Get_Acc_Pv(string[] one_window,
                                  out float acc_x_pv, out float acc_y_pv, out float acc_z_pv)
    {
        Get_Acc_Max(one_window, out float acc_x_max, out float acc_y_max, out float acc_z_max);
        Get_Acc_Min(one_window, out float acc_x_min, out float acc_y_min, out float acc_z_min);
        acc_x_pv = acc_x_max - acc_x_min;
        acc_y_pv = acc_y_max - acc_y_min;
        acc_z_pv = acc_z_max - acc_z_min;
    }
    public static void Get_Acc_Fre_Gravity_Center(string one_window_fre_feas,
                                                  out float acc_x_fre_gra, out float acc_y_fre_gra, out float acc_z_fre_gra)
    {
        string[] one_row = one_window_fre_feas.Split(',');
        acc_x_fre_gra = 0;
        acc_y_fre_gra = 0;
        acc_z_fre_gra = 0;

        try
        {
            // Convert the strings to floats
            acc_x_fre_gra = float.Parse(one_row[0]);
            acc_y_fre_gra = float.Parse(one_row[5]);
            acc_z_fre_gra = float.Parse(one_row[10]);
        }
        catch { }
    }
    public static void Get_Acc_Fre_Average(string one_window_fre_feas,
                                           out float acc_x_fre_aver, out float acc_y_fre_aver, out float acc_z_fre_aver)
    {
        string[] one_row = one_window_fre_feas.Split(',');
        acc_x_fre_aver = 0;
        acc_y_fre_aver = 0;
        acc_z_fre_aver = 0;

        try
        {
            // Convert the strings to floats
            acc_x_fre_aver = float.Parse(one_row[1]);
            acc_y_fre_aver = float.Parse(one_row[6]);
            acc_z_fre_aver = float.Parse(one_row[11]);
        }
        catch { }
    }
    public static void Get_Acc_Fre_RMS(string one_window_fre_feas,
                                       out float acc_x_fre_rms, out float acc_y_fre_rms, out float acc_z_fre_rms)
    {
        string[] one_row = one_window_fre_feas.Split(',');
        acc_x_fre_rms = 0;
        acc_y_fre_rms = 0;
        acc_z_fre_rms = 0;

        try
        {
            // Convert the strings to floats
            acc_x_fre_rms = float.Parse(one_row[2]);
            acc_y_fre_rms = float.Parse(one_row[7]);
            acc_z_fre_rms = float.Parse(one_row[12]);
        }
        catch { }
    }
    public static void Get_Acc_Fre_Variance(string one_window_fre_feas,
                                            out float acc_x_fre_var, out float acc_y_fre_var, out float acc_z_fre_var)
    {
        string[] one_row = one_window_fre_feas.Split(',');
        acc_x_fre_var = 0;
        acc_y_fre_var = 0;
        acc_z_fre_var = 0;

        try
        {
            // Convert the strings to floats
            acc_x_fre_var = float.Parse(one_row[3]);
            acc_y_fre_var = float.Parse(one_row[8]);
            acc_z_fre_var = float.Parse(one_row[13]);
        }
        catch { }
    }
    public static void Get_Acc_Fre_Std(string one_window_fre_feas,
                                       out float acc_x_fre_std, out float acc_y_fre_std, out float acc_z_fre_std)
    {
        string[] one_row = one_window_fre_feas.Split(',');
        acc_x_fre_std = 0;
        acc_y_fre_std = 0;
        acc_z_fre_std = 0;

        try
        {
            // Convert the strings to floats
            acc_x_fre_std = float.Parse(one_row[4]);
            acc_y_fre_std = float.Parse(one_row[9]);
            acc_z_fre_std = float.Parse(one_row[14]);
        }
        catch { }
    }


    public static float[] Get_Syn_Acc(string[] one_window)
    {
        float[] res_datas = new float[one_window.Length];
        for (int i = 0; i < one_window.Length; i++)
        {
            string[] one_row = one_window[i].Split(',');
            float acc_x = 0;
            float acc_y = 0;
            float acc_z = 0;
            try
            {
                // Convert the strings to floats
                acc_x = float.Parse(one_row[0]);
                acc_y = float.Parse(one_row[1]);
                acc_z = float.Parse(one_row[2]);
            }
            catch { }
            float acc_x2 = (float)Math.Pow(acc_x, 2);
            float acc_y2 = (float)Math.Pow(acc_y, 2);
            float acc_z2 = (float)Math.Pow(acc_z, 2);
            float res = (float)Math.Sqrt(acc_x2+ acc_y2 + acc_z2);
            res_datas[i] = res;
        }
        return res_datas;
    }

    public static void Get_Syn_Acc_Mean(string[] one_window,
                                        out float syn_acc_mean)
    {
        float syn_acc_sum = 0;
        string[] first_row = one_window[0].Split(',');
        bool exist = Convert.ToBoolean(first_row[8]);
        int begin = Convert.ToInt16(first_row[9]);
        int end = Convert.ToInt16(first_row[10]);
        int len = Convert.ToInt16(first_row[11]);

        float[] syn_acc = Get_Syn_Acc(one_window);

        // 存在动作则返回范围内均值，不存在动作则返回所有数据的均值
        if (!exist)
        {
            begin = 0;
            end = one_window.Length - 1;
            len = one_window.Length;
        }

        for (int i = begin; i < end + 1; i++)
        {
            syn_acc_sum += syn_acc[i];
        }

        syn_acc_mean = syn_acc_sum / len;
    }

    public static void Get_Syn_Acc_Std(string[] one_window,
                                       out float syn_acc_std)
    {
        float syn_acc_sum = 0;
        string[] first_row = one_window[0].Split(',');
        bool exist = Convert.ToBoolean(first_row[8]);
        int begin = Convert.ToInt16(first_row[9]);
        int end = Convert.ToInt16(first_row[10]);
        int len = Convert.ToInt16(first_row[11]);

        float[] syn_acc = Get_Syn_Acc(one_window);
        Get_Syn_Acc_Mean(one_window, out float syn_acc_mean);

        // 存在动作则返回范围内标准差，不存在动作则返回所有数据的标准差
        if (!exist)
        {
            begin = 0;
            end = one_window.Length - 1;
            len = one_window.Length;
        }

        for (int i = begin; i < end + 1; i++)
        {
            syn_acc_sum += (float)Math.Pow((syn_acc[i] - syn_acc_mean), 2);
        }

        syn_acc_std = (float)Math.Sqrt(syn_acc_sum / len);
    }



}
