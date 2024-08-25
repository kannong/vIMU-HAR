using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LayerPara
{
    public enum Layer_Type
    {
        Input_Layer=1,
        Dense_Layer,
        CONV_Pool_Layer,
        Flatten_Layer,
        Output_Layer
    }
    public struct Input_Layer // �����
    {
        public Layer_Type type;
        public string name;
        public string features_num;
    }
    public struct Dense_Layer // ȫ���Ӳ�
    {
        public Layer_Type type;
        public string name;
        public string neurons_num; 
        public string activation;
    }
    public struct CONV_Pool_Layer // �����_�ػ���(Ĭ�����ػ�)
    {
        public Layer_Type type;
        public string name;
        public string conv_type; // CONV1D CONV2D 
        public string filiters_num;
        public string kernel_size;
        public string activation;
        public string pooling_size;
    }
    public struct Flatten_Layer // ����Ϊһά������
    {
        public Layer_Type type;
        public string name;
    }
    public struct Output_Layer // �����
    {
        public Layer_Type type;
        public string name;
        public string classes_num;
    }

    public class Layer_Para_Struction : MonoBehaviour
    {

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
