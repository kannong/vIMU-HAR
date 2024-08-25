using LayerPara;
using NN_;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class _NN_Dense_layer_0_Sub_Control : MonoBehaviour
{
    public Button dense_layer_0_b;
    public InputField dense_layer_0_neurons_num_if;
    public Dropdown dense_layer_0_activation_dp;

    private int activation_val = 0;

    private void OnEnable()
    {
        dense_layer_0_neurons_num_if.text = NN.Dense_Layer_0.neurons_num;
        dense_layer_0_activation_dp.value = activation_val;
    }
    // Start is called before the first frame update
    void Start()
    {
        dense_layer_0_neurons_num_if.text = NN.Dense_Layer_0.neurons_num;
        dense_layer_0_activation_dp.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Para_Set()
    {
        NN.Dense_Layer_0.neurons_num = dense_layer_0_neurons_num_if.text;
        NN.Dense_Layer_0.activation = dense_layer_0_activation_dp.options[dense_layer_0_activation_dp.value].text;

        activation_val = dense_layer_0_activation_dp.value;

        Text title = dense_layer_0_b.transform.Find("Text (Legacy)").GetComponent<Text>();
        title.text = NN.Dense_Layer_0.name + " (" + NN.Dense_Layer_0.neurons_num + " neurons)";

        //Debug.Log("NN.Dense_Layer_0.name= " + NN.Dense_Layer_0.name);
        //Debug.Log("NN.Dense_Layer_0.type= " + NN.Dense_Layer_0.type);
        //Debug.Log("NN.Dense_Layer_0.neurons_num= " + NN.Dense_Layer_0.neurons_num);
        //Debug.Log("NN.Dense_Layer_0.activation= " + NN.Dense_Layer_0.activation);
    }
}
