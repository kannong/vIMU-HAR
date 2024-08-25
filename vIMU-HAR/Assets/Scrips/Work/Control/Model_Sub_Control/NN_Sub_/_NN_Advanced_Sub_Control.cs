using NN_;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _NN_Advanced_Sub_Control : MonoBehaviour
{
    public InputField vaildation_spilt_if; //(%)
    public InputField batch_size_if;
    public Toggle fit_generator_tg;
    public Text steps_per_epoch_t;
    public Text vaildation_steps_t;

    private void OnEnable()
    {
        vaildation_spilt_if.text = NN.vaildation_spilt;
        batch_size_if.text = NN.batch_size;
        fit_generator_tg.isOn = (NN.fit_generator == "True") ? true : false;
    }
    // Start is called before the first frame update
    void Start()
    {
        vaildation_spilt_if.text = NN.vaildation_spilt;
        batch_size_if.text = NN.batch_size;
        fit_generator_tg.isOn = (NN.fit_generator == "True") ? true : false;
        steps_per_epoch_t.text = NN.strps_per_epoch;
        vaildation_steps_t.text = NN.vaildation_steps;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Para_Set()
    {
        NN.vaildation_spilt = vaildation_spilt_if.text;
        NN.batch_size = batch_size_if.text;
        NN.fit_generator = (fit_generator_tg.isOn == true) ? "True" : "False";

        //Debug.Log("NN.vaildation_spilt= " + NN.vaildation_spilt);
        //Debug.Log("NN.batch_size= " + NN.batch_size);
        //Debug.Log("NN.fit_generator= " + NN.fit_generator);
        //Debug.Log("NN.strps_per_epoch= " + NN.strps_per_epoch);
        //Debug.Log("NN.vaildation_steps= " + NN.vaildation_steps);

    }
}
