using CNN_;
using NN_;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _CNN_CONV1D_Pool_layer_0_Sub_Control : MonoBehaviour
{
    public Button conv1d_pool_layer_0_b; 
    public InputField conv1d_pool_layer_0_filiter_num_if;
    public InputField conv1d_pool_layer_0_kernel_size_if;
    public Dropdown conv1d_pool_layer_0_activation_dp;
    public InputField conv1d_pool_layer_0_pooling_size_if;

    private int activation_val = 0;

    private void OnEnable()
    {
        conv1d_pool_layer_0_filiter_num_if.text = CNN.CONV_Pool_Layer_0.filiters_num;
        conv1d_pool_layer_0_kernel_size_if.text = CNN.CONV_Pool_Layer_0.kernel_size;
        conv1d_pool_layer_0_activation_dp.value = activation_val;
        conv1d_pool_layer_0_pooling_size_if.text = CNN.CONV_Pool_Layer_0.pooling_size;
    }
    // Start is called before the first frame update
    void Start()
    {
        conv1d_pool_layer_0_filiter_num_if.text = CNN.CONV_Pool_Layer_0.filiters_num;
        conv1d_pool_layer_0_kernel_size_if.text= CNN.CONV_Pool_Layer_0.kernel_size;
        conv1d_pool_layer_0_activation_dp.value = 0;
        conv1d_pool_layer_0_pooling_size_if.text = CNN.CONV_Pool_Layer_0.pooling_size;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Para_Set()
    {
        CNN.CONV_Pool_Layer_0.filiters_num = conv1d_pool_layer_0_filiter_num_if.text;
        CNN.CONV_Pool_Layer_0.kernel_size = conv1d_pool_layer_0_kernel_size_if.text;
        CNN.CONV_Pool_Layer_0.activation = conv1d_pool_layer_0_activation_dp.options[conv1d_pool_layer_0_activation_dp.value].text;
        CNN.CONV_Pool_Layer_0.pooling_size = conv1d_pool_layer_0_pooling_size_if.text;

        activation_val = conv1d_pool_layer_0_activation_dp.value;

        Text title = conv1d_pool_layer_0_b.transform.Find("Text (Legacy)").GetComponent<Text>();
        title.text = "CONV2D / Pool layer" + "(" + CNN.CONV_Pool_Layer_0.filiters_num + " filiters, " + 
                     CNN.CONV_Pool_Layer_0.kernel_size + " kernel size)";
    }
}
