using LayerPara;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CNN_
{
    public class CNN : MonoBehaviour
    {
        public static string data_source;
        public static string training_cycles;
        public static string optimizer;
        public static string learning_rate;
        public static string loss;

        public static string vaildation_spilt; //(%)
        public static string batch_size;
        public static string fit_generator;
        public static string strps_per_epoch; // only read
        public static string vaildation_steps; // only read

        public static LayerPara.Input_Layer Input_Layer;
        public static LayerPara.CONV_Pool_Layer CONV_Pool_Layer_0;
        public static LayerPara.CONV_Pool_Layer CONV_Pool_Layer_1;
        public static LayerPara.CONV_Pool_Layer CONV_Pool_Layer_2;
        public static LayerPara.Flatten_Layer Flatten_Layer;
        public static LayerPara.Output_Layer Output_Layer;

        // Start is called before the first frame update
        void Start()
        {
            data_source = "r";
            training_cycles = "10";
            optimizer = "Adam";
            learning_rate = "0.001";
            loss = "SparseCategoricalCrossentropy";
            vaildation_spilt = "20";
            batch_size = "10";
            fit_generator = "True";
            strps_per_epoch = "0";
            vaildation_steps = "0";

            Input_Layer.name = "Input layer";
            Input_Layer.type = Layer_Type.Input_Layer;
            Input_Layer.features_num = "6";

            CONV_Pool_Layer_0.name = "CONV Pool layer 0";
            CONV_Pool_Layer_0.type = Layer_Type.CONV_Pool_Layer;
            CONV_Pool_Layer_0.conv_type = "CONV1D";
            CONV_Pool_Layer_0.filiters_num = "16";
            CONV_Pool_Layer_0.kernel_size = "3";
            CONV_Pool_Layer_0.activation = "relu";
            CONV_Pool_Layer_0.pooling_size = "2";

            CONV_Pool_Layer_1.name = "CONV Pool layer 1";
            CONV_Pool_Layer_1.type = Layer_Type.CONV_Pool_Layer;
            CONV_Pool_Layer_1.conv_type = "CONV1D";
            CONV_Pool_Layer_1.filiters_num = "32";
            CONV_Pool_Layer_1.kernel_size = "3";
            CONV_Pool_Layer_1.activation = "relu";
            CONV_Pool_Layer_1.pooling_size = "2";

            CONV_Pool_Layer_2.name = "CONV Pool layer 2";
            CONV_Pool_Layer_2.type = Layer_Type.CONV_Pool_Layer;
            CONV_Pool_Layer_2.conv_type = "CONV1D";
            CONV_Pool_Layer_2.filiters_num = "64";
            CONV_Pool_Layer_2.kernel_size = "3";
            CONV_Pool_Layer_2.activation = "relu";
            CONV_Pool_Layer_2.pooling_size = "2";

            Flatten_Layer.name = "Flatten layer";
            Flatten_Layer.type = Layer_Type.Flatten_Layer;

            Output_Layer.name = "Output layer";
            Output_Layer.type = Layer_Type.Output_Layer;
            Output_Layer.classes_num = "1";
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
