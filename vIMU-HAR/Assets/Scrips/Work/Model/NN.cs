using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LayerPara;
using static UnityEditor.Timeline.TimelinePlaybackControls;
using static UnityEngine.UIElements.UxmlAttributeDescription;

namespace NN_
{
    public class NN : MonoBehaviour
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
        public static LayerPara.Dense_Layer Dense_Layer_0;
        public static LayerPara.Dense_Layer Dense_Layer_1;
        public static LayerPara.Output_Layer Output_Layer;

        // Start is called before the first frame update
        void Start()
        {
            data_source = "r";
            training_cycles = "10";
            optimizer = "Adam";
            learning_rate = "0.001";
            loss = "SparseCategoricalCrossentropy";
            vaildation_spilt = "25";
            batch_size = "64";
            fit_generator = "True";
            strps_per_epoch = "0";
            vaildation_steps = "0";

            Input_Layer.name = "Input layer";
            Input_Layer.type = Layer_Type.Input_Layer;
            Input_Layer.features_num = "6";

            Dense_Layer_0.name = "Dense layer";
            Dense_Layer_0.type = Layer_Type.Dense_Layer;
            Dense_Layer_0.neurons_num = "32";
            Dense_Layer_0.activation = "relu";

            Dense_Layer_1.name = "Dense layer";
            Dense_Layer_1.type = Layer_Type.Dense_Layer;
            Dense_Layer_1.neurons_num = "32";
            Dense_Layer_1.activation = "relu";

            Output_Layer.name = "Output layer";
            Output_Layer.type = Layer_Type.Output_Layer;
            Output_Layer.classes_num = "1";

            //// print information
            //Debug.Log("NN Init:");
            //Debug.Log("NN.data_source= " + data_source);
            //Debug.Log("NN.training_cycles= " + training_cycles);
            //Debug.Log("NN.optimizer= " + optimizer);
            //Debug.Log("NN.learning_rate= " + learning_rate);
            //Debug.Log("NN.loss= " + loss);

            //Debug.Log("NN.vaildation_spilt= " + vaildation_spilt);
            //Debug.Log("NN.batch_size= " + batch_size);
            //Debug.Log("NN.fit_generator= " + fit_generator);
            //Debug.Log("NN.strps_per_epoch= " + strps_per_epoch);
            //Debug.Log("NN.vaildation_steps= " + vaildation_steps);

            //Debug.Log("NN.Input_Layer.name= " + Input_Layer.name);
            //Debug.Log("NN.Input_Layer.type= " + Input_Layer.type);
            //Debug.Log("NN.Input_Layer.features_num= " + Input_Layer.features_num);

            //Debug.Log("NN.Dense_Layer_0.name= " + Dense_Layer_0.name);
            //Debug.Log("NN.Dense_Layer_0.type= " + Dense_Layer_0.type);
            //Debug.Log("NN.Dense_Layer_0.neurons_num= " + Dense_Layer_0.neurons_num);
            //Debug.Log("NN.Dense_Layer_0.activation= " + Dense_Layer_0.activation);

            //Debug.Log("NN.Dense_Layer_1.name= " + Dense_Layer_1.name);
            //Debug.Log("NN.Dense_Layer_1.type= " + Dense_Layer_1.type);
            //Debug.Log("NN.Dense_Layer_1.neurons_num= " + Dense_Layer_1.neurons_num);
            //Debug.Log("NN.Dense_Layer_1.activation= " + Dense_Layer_1.activation);

            //Debug.Log("NN.Output_Layer.name= " + Output_Layer.name);
            //Debug.Log("NN.Output_Layer.type= " + Output_Layer.type);
            //Debug.Log("NN.Output_Layer.classes_num= " + Output_Layer.classes_num);

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
