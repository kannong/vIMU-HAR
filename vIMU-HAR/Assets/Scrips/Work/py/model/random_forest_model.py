from sklearn.tree import export_graphviz
import pandas as pd
from comm import normalize_fun, csv_fun
from sklearn.metrics import confusion_matrix
from sklearn.ensemble import RandomForestClassifier
import pydotplus
import os
# floder = 'E:/unity_pro/My_workV0.1/Assets/StreamingAssets/SourcesFolder/Data_Process/'
floder = os.getcwd()
floder = os.path.abspath(os.path.join(floder,'Assets',"StreamingAssets\SourcesFolder\Data_Process"))
floder += '/'

class RandomForest:
    def __init__(self,
                 max_depth, min_samples_split, min_samples_leaf, max_features, max_leaf_nodes,
                 n_estimators, n_jobs, max_samples,
                 bootstrap, oob_score, warm_start,
                 normalize=True,
                 random_state=42):
        self.train_file_path = floder + 'all_train_data.csv'
        self.test_file_path = floder + 'all_test_data.csv'
        self.file_header = csv_fun.get_csv_header(self.train_file_path)
        self.feature_names = self.file_header[1:len(self.file_header)-1]
        self.target_names = ['knee','reverse']
        self.normalize = normalize
        self.train_data = pd.read_csv(self.train_file_path)
        self.test_data = pd.read_csv(self.test_file_path)
        self.train_data_len = self.train_data.shape[0]
        self.test_data_len = self.test_data.shape[0]
        self.column_num = self.train_data.shape[1] - 1
        self.label_num = 1
        self.features_num = self.column_num - self.label_num
        self.rf_clf = RandomForestClassifier(max_depth=max_depth,
                                             min_samples_split=min_samples_split, min_samples_leaf=min_samples_leaf,
                                             max_features=max_features, max_leaf_nodes=max_leaf_nodes,
                                             n_estimators=n_estimators, n_jobs=n_jobs,
                                             max_samples=max_samples,
                                             bootstrap=bootstrap, oob_score=oob_score, warm_start=warm_start,
                                             random_state=random_state)
        self.get_data()

    def get_data(self):
        '''
        函数功能：获取训练集与测试集
        :return: 无
        '''
        # 得到训练集与测试集
        self.X_train = self.train_data.iloc[:, 1:self.column_num].values.reshape((self.train_data_len, self.features_num))
        self.Y_train = self.train_data.iloc[:, 0:self.label_num].values.reshape((self.train_data_len, self.label_num))
        self.X_test = self.test_data.iloc[:, 1:self.column_num].values.reshape((self.test_data_len, self.features_num))
        self.Y_test = self.test_data.iloc[:, 0:self.label_num].values.reshape((self.test_data_len, self.label_num))
        # 是否对数据进行归一化处理
        if self.normalize:
            self.X_train, self.features_mean, self.features_deviation = normalize_fun.normalize(self.X_train)
            self.X_test = normalize_fun.normalize_designate_paras(features=self.X_test,
                                                                  features_mean=self.features_mean,
                                                                  features_deviation=self.features_deviation)

    def model_train(self):
        '''
        函数功能: 随机森林模型的训练，测试与模型评估
        :return: 无
        '''
        # 模型训练
        self.rf_clf.fit(self.X_train, self.Y_train)

        # # 随机森林画图
        # i = 0
        # for per_rf in self.rf_clf.estimators_:
        #     dot_data = export_graphviz(per_rf,
        #                                out_file=None,
        #                                feature_names=self.feature_names,
        #                                class_names=self.target_names,
        #                                filled=True,
        #                                rounded=True,
        #                                special_characters=True)
        #     graph = pydotplus.graph_from_dot_data(dot_data)
        #     i = i + 1
        #     graph.write_pdf('model_output/rf_model/' + str(i) + "DTtree.pdf")

        # 模型预测与评估
        y_train_pred = self.rf_clf.predict(self.X_test)
        score = self.rf_clf.score(self.X_test, self.Y_test)
        cmx = confusion_matrix(self.Y_test, y_train_pred)
        print('confusion_matrix = ', cmx)
        print('score = ', score)
        # print('oob score = ', self.rf_clf.oob_score)
        # print('features important value are: ')
        # for name, score in zip(self.feature_names, self.rf_clf.feature_importances_):
        #     print(name, score)

    def model_predit(self, x_test):
        '''
        函数功能: 对输入的数据进行预测
        :param x_test: 输入特征数据
        :return: 预测值对于实际类别标签值
        '''
        # 是否对数据进行归一化处理
        if self.normalize:
            x_test = normalize_fun.normalize_designate_paras(features=x_test,
                                                             features_mean=self.features_mean,
                                                             features_deviation=self.features_deviation)

        # 预测类别
        y_predit_val = self.rf_clf.predict(x_test)
        y_predit_label = self.target_names[int(y_predit_val)]
        return y_predit_label
