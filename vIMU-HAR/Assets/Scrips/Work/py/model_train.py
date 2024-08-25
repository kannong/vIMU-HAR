import sys

import model.random_forest_model as random_forest
import model.decide_tree_model as decide_tree
import model.NN_model as _NN
import model.CNN_model as _CNN


def model_train_test(object_model):
    if object_model == "Decide_Tree":
        max_depth = None if sys.argv[2] == '0' else int(sys.argv[2])
        min_samples_split = 2 if sys.argv[3] == '0' else int(sys.argv[3])
        min_samples_leaf = 1 if sys.argv[4] == '0' else int(sys.argv[4])
        # max_features =   None if sys.argv[5] == '0' else int(sys.argv[5])
        if sys.argv[5] == '0':
            max_features = None
        elif sys.argv[5] == "auto" or sys.argv[5] == "sqrt" or sys.argv[5] == "log2":
            max_features = sys.argv[5]
        else:
            if sys.argv[5].isdigit():
                max_features = int(sys.argv[5])
            else:
                max_features = None
        max_leaf_nodes = None if sys.argv[6] == '0' else int(sys.argv[6])
        normalize = True if sys.argv[7] == "True" else False
        print("object_model=", object_model,
              "max_depth=", max_depth,
              "min_samples_split=", min_samples_split,
              "min_samples_leaf=", min_samples_leaf,
              "max_features=", max_features,
              "max_leaf_nodes=", max_leaf_nodes,
              "normalize=", normalize)
        dt = decide_tree.DecideTree(max_depth=max_depth,
                                    min_samples_split=min_samples_split,
                                    min_samples_leaf=min_samples_leaf,
                                    max_features=max_features,
                                    max_leaf_nodes=max_leaf_nodes,
                                    normalize=normalize)
        dt.model_train()

    elif object_model == "Random_Forest":
        max_depth = None if sys.argv[2] == '0' else int(sys.argv[2])
        min_samples_split = 2 if sys.argv[3] == '0' else int(sys.argv[3])
        min_samples_leaf = 1 if sys.argv[4] == '0' else int(sys.argv[4])
        if sys.argv[5] == '0':
            max_features = None
        elif sys.argv[5] == "auto" or sys.argv[5] == "sqrt" or sys.argv[5] == "log2":
            max_features = sys.argv[5]
        else:
            if sys.argv[5].isdigit():
                max_features = int(sys.argv[5])
            else:
                max_features = None
        max_leaf_nodes = None if sys.argv[6] == '0' else int(sys.argv[6])
        n_estimators = 100 if sys.argv[7] == '0' else int(sys.argv[7])
        n_jobs = None if sys.argv[8] == '0' else int(sys.argv[8])
        max_samples = None if sys.argv[9] == '0' else int(sys.argv[9])
        bootstrap = True if sys.argv[10] == "True" else False
        oob_score = True if sys.argv[11] == "True" else False
        warm_start = True if sys.argv[12] == "True" else False
        normalize = True if sys.argv[13] == "True" else False
        print("object_model=", object_model,
              "max_depth=", max_depth,
              "min_samples_split=", min_samples_split,
              "min_samples_leaf=", min_samples_leaf,
              "max_features=", max_features,
              "max_leaf_nodes=", max_leaf_nodes,
              "n_estimators=", n_estimators,
              "n_jobs=", n_jobs,
              "max_samples=", max_samples,
              "bootstrap=", bootstrap,
              "oob_score=", oob_score,
              "warm_start=", warm_start,
              "normalize=", normalize)
        rf = random_forest.RandomForest(max_depth=max_depth,
                                        min_samples_split=min_samples_split, min_samples_leaf=min_samples_leaf,
                                        max_features=max_features, max_leaf_nodes=max_leaf_nodes,
                                        n_estimators=n_estimators, n_jobs=n_jobs,
                                        max_samples=max_samples,
                                        bootstrap=bootstrap, oob_score=oob_score, warm_start=warm_start,
                                        normalize=normalize)
        rf.model_train()

    elif object_model == "NN":
        data_source = 'r' if sys.argv[2] == '0' else sys.argv[2]
        training_cycles = 10 if sys.argv[3] == '0' else int(sys.argv[3])
        optimizer = "Adam" if sys.argv[4] == '0' else sys.argv[4]
        learning_rate = 0.001 if sys.argv[5] == '0' else float(sys.argv[5])
        loss = "SparseCategoricalCrossentropy" if sys.argv[6] == '0' else sys.argv[6]
        vaildation_spilt = 0.25 if sys.argv[7] == '0' else (float(sys.argv[7]) / 100)
        batch_size = 10 if sys.argv[8] == '0' else int(sys.argv[8])
        fit_generator = True if sys.argv[9] == "True" else False
        features_num = 6 if sys.argv[10] == '0' else int(sys.argv[10])
        dense_layer0_neu_num = 32 if sys.argv[11] == '0' else int(sys.argv[11])
        dense_layer0_act = "relu" if sys.argv[12] == '0' else sys.argv[12]
        dense_layer1_neu_num = 32 if sys.argv[13] == '0' else int(sys.argv[13])
        dense_layer1_act = "relu" if sys.argv[14] == '0' else sys.argv[14]
        classes_num = 1 if sys.argv[15] == '0' else int(sys.argv[15])
        dense_layer0 = [dense_layer0_neu_num, dense_layer0_act]
        dense_layer1 = [dense_layer1_neu_num, dense_layer1_act]
        print("data_source=", data_source,
              "training_cycles=", training_cycles,
              "optimizer=", optimizer,
              "learning_rate=", learning_rate,
              "loss=", loss,
              "vaildation_spilt=", vaildation_spilt,
              "batch_size=", batch_size,
              "fit_generator=", fit_generator,
              "features_num=", features_num,
              "dense_layer0=", dense_layer0,
              "dense_layer1=", dense_layer1,
              "classes_num=", classes_num)
        nn = _NN._NN(data_source=data_source,
                     training_cycles=training_cycles, optimizer=optimizer, learning_rate=learning_rate,
                     loss=loss,
                     vaildation_spilt=vaildation_spilt, batch_size=batch_size,
                     fit_generator=fit_generator,
                     features_num=features_num,
                     dense_layer0=dense_layer0, dense_layer1=dense_layer1,
                     classes_num=classes_num)
        nn.model_build()
        nn.modle_train()

    elif object_model == "CNN":
        data_source = 'r' if sys.argv[2] == '0' else sys.argv[2]
        training_cycles = 10 if sys.argv[3] == '0' else int(sys.argv[3])
        optimizer = "Adam" if sys.argv[4] == '0' else sys.argv[4]
        learning_rate = 0.001 if sys.argv[5] == '0' else float(sys.argv[5])
        loss = "SparseCategoricalCrossentropy" if sys.argv[6] == '0' else sys.argv[6]
        vaildation_spilt = 0.25 if sys.argv[7] == '0' else (float(sys.argv[7]) / 100)
        batch_size = 10 if sys.argv[8] == '0' else int(sys.argv[8])
        fit_generator = True if sys.argv[9] == "True" else False
        features_num = 6 if sys.argv[10] == '0' else int(sys.argv[10])
        conv1dpool_layer0_filters = 32 if sys.argv[11] == '0' else int(sys.argv[11])
        conv1dpool_layer0_kersize = 3 if sys.argv[12] == '0' else int(sys.argv[12])
        conv1dpool_layer0_act = "relu" if sys.argv[13] == '0' else sys.argv[13]
        conv1dpool_layer0_poolsize = 2 if sys.argv[14] == '0' else int(sys.argv[14])
        conv1dpool_layer1_filters = 64 if sys.argv[15] == '0' else int(sys.argv[15])
        conv1dpool_layer1_kersize = 3 if sys.argv[16] == '0' else int(sys.argv[16])
        conv1dpool_layer1_act = "relu" if sys.argv[17] == '0' else sys.argv[17]
        conv1dpool_layer1_poolsize = 2 if sys.argv[18] == '0' else int(sys.argv[18])
        conv1dpool_layer2_filters = 128 if sys.argv[19] == '0' else int(sys.argv[19])
        conv1dpool_layer2_kersize = 3 if sys.argv[20] == '0' else int(sys.argv[20])
        conv1dpool_layer2_act = "relu" if sys.argv[21] == '0' else sys.argv[21]
        conv1dpool_layer2_poolsize = 2 if sys.argv[22] == '0' else int(sys.argv[22])
        classes_num = 1 if sys.argv[23] == '0' else int(sys.argv[23])
        conv1dpool_layer0 = [conv1dpool_layer0_filters, conv1dpool_layer0_kersize,
                             conv1dpool_layer0_act, conv1dpool_layer0_poolsize]
        conv1dpool_layer1 = [conv1dpool_layer1_filters, conv1dpool_layer1_kersize,
                             conv1dpool_layer1_act, conv1dpool_layer1_poolsize]
        conv1dpool_layer2 = [conv1dpool_layer2_filters, conv1dpool_layer2_kersize,
                             conv1dpool_layer2_act, conv1dpool_layer2_poolsize]
        print("data_source=", data_source,
              "training_cycles=", training_cycles,
              "optimizer=", optimizer,
              "learning_rate=", learning_rate,
              "loss=", loss,
              "vaildation_spilt=", vaildation_spilt,
              "batch_size=", batch_size,
              "fit_generator=", fit_generator,
              "features_num=", features_num,
              "conv1dpool_layer0=", conv1dpool_layer0,
              "conv1dpool_layer1=", conv1dpool_layer1,
              "conv1dpool_layer2=", conv1dpool_layer2,
              "classes_num=", classes_num)
        _cnn = _CNN._CNN(data_source=data_source,
                         training_cycles=training_cycles, optimizer=optimizer, learning_rate=learning_rate,
                         loss=loss,
                         vaildation_spilt=vaildation_spilt, batch_size=batch_size,
                         fit_generator=fit_generator,
                         features_num=features_num,
                         conv1dpool_layer0=conv1dpool_layer0,
                         conv1dpool_layer1=conv1dpool_layer1,
                         conv1dpool_layer2=conv1dpool_layer2,
                         classes_num=classes_num)
        _cnn.model_build()
        _cnn.modle_train()


# 单独运行Python时用
# dt = decide_tree.DecideTree(max_depth=None,
#                             min_samples_split=2,
#                             min_samples_leaf=1,
#                             max_features=None,
#                             max_leaf_nodes=None,
#                             normalize=True)
# dt.model_train()

# rf = random_forest.RandomForest(max_depth=None,
#                                 min_samples_split=2, min_samples_leaf=1,
#                                 max_features=None, max_leaf_nodes=None,
#                                 n_estimators=100, n_jobs=-1,
#                                 max_samples=None,
#                                 bootstrap=True, oob_score=False, warm_start=False,
#                                 normalize=True)
# rf.model_train()

# _nn = _NN._NN(data_source='r',
#               training_cycles=int(50), optimizer="Adam", learning_rate=float(0.001),
#               loss="SparseCategoricalCrossentropy",
#               vaildation_spilt=float(20.0 / 100), batch_size=int(10),
#               fit_generator=True,
#               features_num=int(3), # 剔除角速度
#               dense_layer0=[int(32), "relu"], dense_layer1=[int(32), "relu"],
#               classes_num=int(3))
# _nn.model_build()
# _nn.modle_train()

# _cnn = _CNN._CNN(data_source='r',
#                  training_cycles=int(10), optimizer="Adam", learning_rate=float(0.001),
#                  loss="SparseCategoricalCrossentropy",
#                  vaildation_spilt=float(20.0 / 100), batch_size=int(10),
#                  fit_generator=True,
#                  features_num=int(3),  # 剔除角速度
#                  conv1dpool_layer0=[int(32), int(3), 'relu', int(2)],
#                  conv1dpool_layer1=[int(64), int(3), 'relu', int(2)],
#                  conv1dpool_layer2=[int(128), int(3), 'relu', int(2)],
#                  classes_num=int(3))
# _cnn.model_build()
# _cnn.modle_train()

# Unity与Python联合运行时用
model_train_test(sys.argv[1])
