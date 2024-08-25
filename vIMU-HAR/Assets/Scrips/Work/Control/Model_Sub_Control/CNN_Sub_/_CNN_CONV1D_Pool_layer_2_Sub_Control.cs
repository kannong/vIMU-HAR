using CNN_;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _CNN_CONV1D_Pool_layer_2_Sub_Control : MonoBehaviour
{
    public Button conv1d_pool_layer_2_b;
    public InputField conv1d_pool_layer_2_filiter_num_if;
    public InputField conv1d_pool_layer_2_kernel_size_if;
    public Dropdown conv1d_pool_layer_2_activation_dp;
    public InputField conv1d_pool_layer_2_pooling_size_if;

    private int activation_val = 0;

    private void OnEnable()
    {
        conv1d_pool_layer_2_filiter_num_if.text = CNN.CONV_Pool_Layer_2.filiters_num;
        conv1d_pool_layer_2_kernel_size_if.text = CNN.CONV_Pool_Layer_2.kernel_size;
        conv1d_pool_layer_2_activation_dp.value = activation_val;
        conv1d_pool_layer_2_pooling_size_if.text = CNN.CONV_Pool_Layer_2.pooling_size;
    }
    // Start is called before the first frame update
    void Start()
    {
        conv1d_pool_layer_2_filiter_num_if.text = CNN.CONV_Pool_Layer_2.filiters_num;
        conv1d_pool_layer_2_kernel_size_if.text = CNN.CONV_Pool_Layer_2.kernel_size;
        conv1d_pool_layer_2_activation_dp.value = 0;
        conv1d_pool_layer_2_pooling_size_if.text = CNN.CONV_Pool_Layer_2.pooling_size;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Para_Set()
    {
        CNN.CONV_Pool_Layer_2.filiters_num = conv1d_pool_layer_2_filiter_num_if.text;
        CNN.CONV_Pool_Layer_2.kernel_size = conv1d_pool_layer_2_kernel_size_if.text;
        CNN.CONV_Pool_Layer_2.activation = conv1d_pool_layer_2_activation_dp.options[conv1d_pool_layer_2_activation_dp.value].text;
        CNN.CONV_Pool_Layer_2.pooling_size = conv1d_pool_layer_2_pooling_size_if.text;

        activation_val = conv1d_pool_layer_2_activation_dp.value;

        Text title = conv1d_pool_layer_2_b.transform.Find("Text (Legacy)").GetComponent<Text>();
        title.text = "CONV2D / Pool layer" + "(" + CNN.CONV_Pool_Layer_2.filiters_num + " filiters, " +
                     CNN.CONV_Pool_Layer_2.kernel_size + " kernel size)";
    }

}
