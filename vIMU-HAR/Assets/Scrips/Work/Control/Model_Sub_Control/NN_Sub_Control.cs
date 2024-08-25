using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NN_;
using UnityEngine.UI;
using RandomForest;


public class NN_Sub_Control : MonoBehaviour
{
    public Dropdown data_source_dp;
    public InputField training_cycles_if;
    public Dropdown optimizer_dp;
    public InputField learning_rate_if; 
    public Dropdown loss_dp;

    public Button input_layer_b;
    public Button output_layer_b;

    private int optimizer_val = 0;
    private int loss_val = 0;

    private void OnEnable() // reflesh features_num and classed_num
    {
        // reflesh
        training_cycles_if.text = NN.training_cycles;
        optimizer_dp.value = optimizer_val;
        learning_rate_if.text = NN.learning_rate;
        loss_dp.value = loss_val;

        Debug.Log("reflesh features_num and classed_num");
        NN.Input_Layer.features_num = (data_source_dp.value == 0) ?
                                       Main_Canvas_Control.raw_features_num.ToString() : Main_Canvas_Control.extracted_features_num.ToString();
        NN.Output_Layer.classes_num = Main_Canvas_Control.class_num.ToString();

        Text title1 = input_layer_b.transform.Find("Text (Legacy)").GetComponent<Text>();
        title1.text = NN.Input_Layer.name + " (" + NN.Input_Layer.features_num + " features)";

        Text title2 = output_layer_b.transform.Find("Text (Legacy)").GetComponent<Text>();
        title2.text = NN.Output_Layer.name + " (" + NN.Output_Layer.classes_num + " classes)";

    }

    // Start is called before the first frame update
    void Start()
    {
        data_source_dp.value = 0;
        training_cycles_if.text = NN.training_cycles;
        optimizer_dp.value = 0;
        learning_rate_if.text= NN.learning_rate;
        loss_dp.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Data_Source_Changed()
    {
        Debug.Log("Data source changed");
        NN.Input_Layer.features_num = (data_source_dp.value == 0) ? 
                                       Main_Canvas_Control.raw_features_num.ToString() : Main_Canvas_Control.extracted_features_num.ToString();

        Text title = input_layer_b.transform.Find("Text (Legacy)").GetComponent<Text>();
        title.text = NN.Input_Layer.name + " (" + NN.Input_Layer.features_num + " features)";
    }
    public void Para_Set()
    {
        NN.data_source = (data_source_dp.value == 0) ? "r" : "e";
        NN.training_cycles = training_cycles_if.text;
        NN.optimizer = optimizer_dp.options[optimizer_dp.value].text;
        NN.learning_rate = learning_rate_if.text;
        NN.loss = loss_dp.options[loss_dp.value].text;

        optimizer_val = optimizer_dp.value;
        loss_val = loss_dp.value;

        // print information
        //Debug.Log("NN.data_source= " + NN.data_source);
        //Debug.Log("NN.training_cycles= " + NN.training_cycles);
        //Debug.Log("NN.optimizer= " + NN.optimizer);
        //Debug.Log("NN.learning_rate= " + NN.learning_rate);
        //Debug.Log("NN.loss= " + NN.loss);

        //Debug.Log("NN.Input_Layer.name= " + NN.Input_Layer.name);
        //Debug.Log("NN.Input_Layer.type= " + NN.Input_Layer.type);
        //Debug.Log("NN.Input_Layer.features_num= " + NN.Input_Layer.features_num);

        //Debug.Log("NN.Output_Layer.name= " + NN.Output_Layer.name);
        //Debug.Log("NN.Output_Layer.type= " + NN.Output_Layer.type);
        //Debug.Log("NN.Output_Layer.classes_num= " + NN.Output_Layer.classes_num);


    }
}
