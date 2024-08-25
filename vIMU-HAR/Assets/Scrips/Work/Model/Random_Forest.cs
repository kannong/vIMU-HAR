using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;
using static UnityEngine.UIElements.UxmlAttributeDescription;

namespace RandomForest
{
    public class Random_Forest : MonoBehaviour
    {
        public static string max_depth; // The maximum depth of the tree.
        public static string min_samples_split; // The minimum number of samples required to split an internal node
        public static string min_samples_leaf; // The minimum number of samples required to be at a leaf node.
        public static string max_features; // The number of features to consider when looking for the best split
                                            //- If int, then consider `max_features` features at each split.
                                            //- If float, then `max_features` is a fraction and
                                            //  `round(max_features* n_features)` features are considered at each
                                            //  split.
                                            //- If "auto", then `max_features= sqrt(n_features)`.
                                            //- If "sqrt", then `max_features= sqrt(n_features)` (same as "auto").
                                            //- If "log2", then `max_features=log2(n_features)`.
                                            //- If None, then `max_features=n_features`.
        public static string max_leaf_nodes; // Grow a tree with ``max_leaf_nodes`` in best-first fashion.
        public static string n_estimators; // The number of trees in the forest.
        public static string n_jobs; // The number of jobs to run in parallel.
                                     // :meth:`fit`, :meth:`predict`,:meth:`decision_path` and :meth:`apply`
                                     // are all parallelized over the trees.
                                     // ``None`` means 1 unless in a :obj:`joblib.parallel_backend` context.
                                     // ``-1`` means using all processors.
        public static string max_samples; // If bootstrap is True, the number of samples to draw from X to train each base estimator.
                                          //- If None(default), then draw `X.shape[0]` samples.
                                          //- If int, then draw `max_samples` samples.
                                          //- If float, then draw `max_samples* X.shape[0]` samples.Thus,
                                          //    `max_samples` should be in the interval `(0.0, 1.0]`.
        public static string bootstrap; // Whether bootstrap samples are used when building trees.
                                        // If False, the whole dataset is used to build each tree.
        public static string oob_score; // Whether to use out-of-bag samples to estimate the generalization score.
                                        // Only available if bootstrap=True.
        public static string warm_start;// When set to ``True``, reuse the solution of the previous call to fit
                                        // and add more estimators to the ensemble,
                                        // otherwise, just fit a whole new forest.

        // Start is called before the first frame update
        void Start()
        {
            max_depth = "";
            min_samples_split = "2";
            min_samples_leaf = "1";
            max_features = "auto";
            max_leaf_nodes = "";
            n_estimators = "100";
            n_jobs = "";
            max_samples = "";
            bootstrap = "True";
            oob_score = "False";
            warm_start = "False";
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
