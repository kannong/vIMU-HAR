import sys
import numpy as np
import pandas as pd
from sklearn.model_selection import train_test_split as tr
from comm import normalize_fun, csv_fun
import random
import os
from sklearn.metrics import confusion_matrix
from sklearn.ensemble import RandomForestClassifier
# 导入sklearn的数据集
from sklearn.tree import export_graphviz
from sklearn.tree import DecisionTreeClassifier

'''
输入：
    sys.argv[1] = raw_data_path; # 文件路径
    sys.argv[2] = train_spilt;    
    sys.argv[3] = test_spilt;    # 训练集与测试集比例：train_spilt/test_spilt
    sys.argv[4] = object_name;   # 对象名称
输出：
    train_num：训练集数目
    test_num ：测试集数目
'''
# floder = 'E:/unity_pro/My_workV0.1/Assets/StreamingAssets/SourcesFolder/Data_Process/'
floder = os.getcwd()
floder = os.path.abspath(os.path.join(floder,'Assets',"StreamingAssets\SourcesFolder\Data_Process"))
floder += '/'

class Train_Test_Spilt():
    def __init__(self, filepath, train_size, object_name):
        self.filepath = filepath
        # print("feas", self.filepath)
        self.train_size = train_size
        self.object_name = object_name
        self.train_data_path = floder + self.object_name + '_train_data.csv'
        self.test_data_path = floder + self.object_name + '_test_data.csv'
        self.data = pd.read_csv(self.filepath)
        self.data_num_examples = self.data.shape[0]
        self.label_num = 1
        self.features_num = self.data.shape[1] - self.label_num - 1
        self.train_len = (int)(self.data_num_examples * self.train_size)
        self.test_len = (int)(self.data_num_examples - self.train_len)
        self.data_process()  # 获得测试集，训练集与归一化参数

    def data_process(self):
        '''
        函数功能: 获得测试集，训练集与归一化参数
        :return: 无
        '''
        idx = list(self.data.index)  # 将index列表打乱
        random.shuffle(idx)
        self.Train = self.data.loc[idx[:self.train_len]].values
        self.Test = self.data.loc[idx[self.train_len:]].values

        self.X_train = self.Train[:, 1:(self.data.shape[1] - 1)].reshape((self.train_len, self.features_num))
        self.X_test = self.Test[:, 1:(self.data.shape[1] - 1)].reshape((self.test_len, self.features_num))
        self.Y_train = self.Train[:, 0:self.label_num].reshape((self.train_len, self.label_num))
        self.Y_test = self.Test[:, 0:self.label_num].reshape((self.test_len, self.label_num))

    def save_train_test_tocsv(self):
        '''
        函数功能：保存训练集与测试集到对应csv文件
        :return: 无
        '''
        # save to csv
        file_header = csv_fun.get_csv_header(self.filepath)
        train = np.concatenate((self.Y_train, self.X_train), axis=1)  # axis=1表示对应行的数组进行拼接
        csv_fun.csv_init(file_path=self.train_data_path, csv_head=file_header)
        csv_fun.csv_append_datas(file_path=self.train_data_path, data_rows=train, data_len=len(train))
        test = np.concatenate((self.Y_test, self.X_test), axis=1)  # axis=1表示对应行的数组进行拼接
        csv_fun.csv_init(file_path=self.test_data_path, csv_head=file_header)
        csv_fun.csv_append_datas(file_path=self.test_data_path, data_rows=test, data_len=len(test))

        # output
        print(self.train_len, self.test_len)


def spilt_test(filepath, train_spilt, test_spilt, object_name):

    path = filepath
    train_total = (float)(train_spilt) / ((float)(train_spilt) + (float)(test_spilt))
    object = object_name
    # data_process(filepath=path, train_size=train_total, normalize=normalize)
    spilt = Train_Test_Spilt(filepath=path, train_size=train_total, object_name=object)
    spilt.save_train_test_tocsv()


# Unity与Python联合运行时用
spilt_test(filepath=sys.argv[1],
           train_spilt=sys.argv[2], test_spilt=sys.argv[3],
           object_name=sys.argv[4])
# print(sys.argv[1],sys.argv[2],sys.argv[3],sys.argv[4])

# 单独运行Python时用
# path = 'E:/unity_pro/My_workV0.1/Assets/StreamingAssets/SourcesFolder/IMUSim/Ankle_r_upleg_JNT_feature_data.csv'
# spilt_test(filepath=path, train_spilt=4.0, test_spilt=1.0,
#            object_name='ankle')
