using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DecideTree;
using System;

public class Decide_Tree_Sub_Control : MonoBehaviour
{
    public InputField max_depth_if;
    public InputField min_samples_split_if;
    public InputField min_samples_leaf_if;
    public InputField max_features_if;
    public InputField max_leaf_nodes_if;

    private void OnEnable()
    {
        max_depth_if.text = Decide_Tree.max_depth;
        min_samples_split_if.text = Decide_Tree.min_samples_split;
        min_samples_leaf_if.text = Decide_Tree.min_samples_leaf;
        max_features_if.text = Decide_Tree.max_features;
        max_leaf_nodes_if.text = Decide_Tree.max_leaf_nodes;

    }
    // Start is called before the first frame update
    void Start()
    {
        // Set defalut value
        max_depth_if.text = Decide_Tree.max_depth;
        min_samples_split_if.text = Decide_Tree.min_samples_split;
        min_samples_leaf_if.text = Decide_Tree.min_samples_leaf;
        max_features_if.text = Decide_Tree.max_features;
        max_leaf_nodes_if.text = Decide_Tree.max_leaf_nodes;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Para_Set()
    {
        Decide_Tree.max_depth = max_depth_if.text;
        Decide_Tree.min_samples_split = min_samples_split_if.text;
        Decide_Tree.min_samples_leaf = min_samples_leaf_if.text;
        Decide_Tree.max_features = max_features_if.text;
        Decide_Tree.max_leaf_nodes = max_leaf_nodes_if.text;
    }
}
