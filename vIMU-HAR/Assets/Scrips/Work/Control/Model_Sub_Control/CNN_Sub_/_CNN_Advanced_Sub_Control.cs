using CNN_;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _CNN_Advanced_Sub_Control : MonoBehaviour
{
    public InputField vaildation_spilt_if; //(%)
    public InputField batch_size_if;
    public Toggle fit_generator_tg;
    public Text steps_per_epoch_t;
    public Text vaildation_steps_t;

    private void OnEnable()
    {
        vaildation_spilt_if.text = CNN.vaildation_spilt;
        batch_size_if.text = CNN.batch_size;
        fit_generator_tg.isOn = (CNN.fit_generator == "True") ? true : false;
    }
    // Start is called before the first frame update
    void Start()
    {
        vaildation_spilt_if.text = CNN.vaildation_spilt;
        batch_size_if.text = CNN.batch_size;
        fit_generator_tg.isOn = (CNN.fit_generator == "True") ? true : false;
        steps_per_epoch_t.text = CNN.strps_per_epoch;
        vaildation_steps_t.text = CNN.vaildation_steps;

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Para_Set()
    {
        CNN.vaildation_spilt = vaildation_spilt_if.text;
        CNN.batch_size = batch_size_if.text;
        CNN.fit_generator = (fit_generator_tg.isOn == true) ? "True" : "False";

        //Debug.Log("CNN.vaildation_spilt= " + CNN.vaildation_spilt);
        //Debug.Log("CNN.batch_size= " + CNN.batch_size);
        //Debug.Log("CNN.fit_generator= " + CNN.fit_generator);
        //Debug.Log("CNN.strps_per_epoch= " + CNN.strps_per_epoch);
        //Debug.Log("CNN.vaildation_steps= " + CNN.vaildation_steps);

    }
}
