using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DecideTree
{
    public class Decide_Tree : MonoBehaviour
    {
        public static string max_depth; // The maximum depth of the tree.
        public static string min_samples_split; // The minimum number of samples required to split an internal node
        public static string min_samples_leaf; // The minimum number of samples required to be at a leaf node.
        public static string max_features; // The number of features to consider when looking for the best split
        public static string max_leaf_nodes; // Grow a tree with ``max_leaf_nodes`` in best-first fashion.
        
        // Start is called before the first frame update
        void Start()
        {
            max_depth = "";
            min_samples_split = "2";
            min_samples_leaf = "1";
            max_features = "";
            max_leaf_nodes = "";
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
