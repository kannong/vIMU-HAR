using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RandomForest;
using UnityEngine.UI;
using DecideTree;
public class Random_Forest_Control : MonoBehaviour
{
    public InputField max_depth_if;
    public InputField min_samples_split_if;
    public InputField min_samples_leaf_if;
    public InputField max_features_if;
    public InputField max_leaf_nodes_if;
    public InputField n_estimators_if;
    public InputField n_jobs_if;
    public InputField max_samples_if;
    public Toggle bootstrap_tg;
    public Toggle oob_score_tg;
    public Toggle warm_start_tg;

    private void OnEnable()
    {
        max_depth_if.text = Random_Forest.max_depth;
        min_samples_split_if.text = Random_Forest.min_samples_split;
        min_samples_leaf_if.text = Random_Forest.min_samples_leaf;
        max_features_if.text = Random_Forest.max_features;
        max_leaf_nodes_if.text = Random_Forest.max_leaf_nodes;
        n_estimators_if.text = Random_Forest.n_estimators;
        n_jobs_if.text = Random_Forest.n_jobs;
        max_samples_if.text = Random_Forest.max_samples;
        bootstrap_tg.isOn = (Random_Forest.bootstrap == "True") ? true : false;
        oob_score_tg.isOn = (Random_Forest.oob_score == "True") ? true : false;
        warm_start_tg.isOn = (Random_Forest.warm_start == "True") ? true : false;

    }
    // Start is called before the first frame update
    void Start()
    {
        // Set defalut value
        max_depth_if.text = Random_Forest.max_depth;
        min_samples_split_if.text = Random_Forest.min_samples_split;
        min_samples_leaf_if.text = Random_Forest.min_samples_leaf;
        max_features_if.text = Random_Forest.max_features;
        max_leaf_nodes_if.text = Random_Forest.max_leaf_nodes;
        n_estimators_if.text = Random_Forest.n_estimators;
        n_jobs_if.text = Random_Forest.n_jobs;
        max_samples_if.text = Random_Forest.max_samples;
        bootstrap_tg.isOn = (Random_Forest.bootstrap == "True")? true: false;
        oob_score_tg.isOn = (Random_Forest.oob_score == "True") ? true : false;
        warm_start_tg.isOn = (Random_Forest.warm_start == "True") ? true : false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Para_Set()
    {
        Random_Forest.max_depth = max_depth_if.text;
        Random_Forest.min_samples_split = min_samples_split_if.text;
        Random_Forest.min_samples_leaf = min_samples_leaf_if.text;
        Random_Forest.max_features = max_features_if.text;
        Random_Forest.max_leaf_nodes = max_leaf_nodes_if.text;
        Random_Forest.n_estimators = n_estimators_if.text;
        Random_Forest.n_jobs = n_jobs_if.text;
        Random_Forest.max_samples = max_samples_if.text;
        Random_Forest.bootstrap = (bootstrap_tg.isOn == true) ? "True" : "False";
        Random_Forest.oob_score = (oob_score_tg.isOn == true) ? "True" : "False";
        Random_Forest.warm_start = (warm_start_tg.isOn == true) ? "True" : "False";
    }


}
