using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CNN_;
using NN_;

public class CNN_Sub_Control : MonoBehaviour
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
        training_cycles_if.text = CNN.training_cycles;
        optimizer_dp.value = optimizer_val;
        learning_rate_if.text = CNN.learning_rate;
        loss_dp.value = loss_val;

        Debug.Log("reflesh features_num and classed_num");
        CNN.Input_Layer.features_num = (data_source_dp.value == 0) ?
                                       Main_Canvas_Control.raw_features_num.ToString() : Main_Canvas_Control.extracted_features_num.ToString();
        CNN.Output_Layer.classes_num = Main_Canvas_Control.class_num.ToString();

        Text title1 = input_layer_b.transform.Find("Text (Legacy)").GetComponent<Text>();
        title1.text = CNN.Input_Layer.name + " (" + CNN.Input_Layer.features_num + " features)";

        Text title2 = output_layer_b.transform.Find("Text (Legacy)").GetComponent<Text>();
        title2.text = CNN.Output_Layer.name + " (" + CNN.Output_Layer.classes_num + " classes)";

    }

    // Start is called before the first frame update
    void Start()
    {
        data_source_dp.value = 0;
        training_cycles_if.text = CNN.training_cycles;
        optimizer_dp.value = 0;
        learning_rate_if.text = CNN.learning_rate;
        loss_dp.value = 0;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Data_Source_Changed()
    {
        Debug.Log("Data source changed");
        CNN.Input_Layer.features_num = (data_source_dp.value == 0) ?
                                       Main_Canvas_Control.raw_features_num.ToString() : Main_Canvas_Control.extracted_features_num.ToString();

        Text title = input_layer_b.transform.Find("Text (Legacy)").GetComponent<Text>();
        title.text = CNN.Input_Layer.name + " (" + CNN.Input_Layer.features_num + " features)";
    }
    public void Para_Set()
    {
        CNN.data_source = (data_source_dp.value == 0) ? "r" : "e";
        CNN.training_cycles = training_cycles_if.text;
        CNN.optimizer = optimizer_dp.options[optimizer_dp.value].text;
        CNN.learning_rate = learning_rate_if.text;
        CNN.loss = loss_dp.options[loss_dp.value].text;

        optimizer_val = optimizer_dp.value;
        loss_val = loss_dp.value;

        // print information
        //Debug.Log("CNN.data_source= " + CNN.data_source);
        //Debug.Log("CNN.training_cycles= " + CNN.training_cycles);
        //Debug.Log("CNN.optimizer= " + CNN.optimizer);
        //Debug.Log("CNN.learning_rate= " + CNN.learning_rate);
        //Debug.Log("CNN.loss= " + CNN.loss);

        //Debug.Log("CNN.Input_Layer.name= " + CNN.Input_Layer.name);
        //Debug.Log("CNN.Input_Layer.type= " + CNN.Input_Layer.type);
        //Debug.Log("CNN.Input_Layer.features_num= " + CNN.Input_Layer.features_num);

        //Debug.Log("CNN.Output_Layer.name= " + CNN.Output_Layer.name);
        //Debug.Log("CNN.Output_Layer.type= " + CNN.Output_Layer.type);
        //Debug.Log("CNN.Output_Layer.classes_num= " + CNN.Output_Layer.classes_num);


    }

}
