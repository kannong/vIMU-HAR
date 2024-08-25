import sys
import numpy as np
import pandas as pd
import comm.get_windows as win
import comm.csv_fun as cf
from sklearn.model_selection import train_test_split as tr
from comm import normalize_fun, csv_fun
import random
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
    sys.argv[5] = window_size;   # 窗口大小
    sys.argv[6] = window_step;   # 窗口步长
输出：
    train_num：训练集数目
    test_num ：测试集数目
'''
# floder = "E:/unity_pro/My_workV0.0/Assets/StreamingAssets/SourcesFolder/IMUSim_temp/teachers_imu_datas/imusim/all/train_and_test/"
floder = 'E:/unity_pro/My_workV0.1/Assets/StreamingAssets/SourcesFolder/IMUSim_temp/my_imu_datas/imureal/train_test_data/fine-tune19/half_half/3/'

class Train_Test_Spilt_Rawdata():
    def __init__(self, filepath, train_size, object_name, window_size, window_step):
        self.filepath = filepath
        self.train_size = train_size
        self.object_name = object_name
        self.window_size = window_size
        self.window_step = window_step
        self.row_len = cf.get_csv_len(self.filepath)
        self.raw_window_data = win.get_window_data(self.filepath, self.row_len, self.window_size, self.window_step)
        self.new_window_data = []
        self.data_num_examples = len(self.raw_window_data)
        self.label_num = 1
        self.raw_features_num = self.raw_window_data[0].values.shape[1] - self.label_num
        self.raw_features_num -= 3  # 剔除角速度
        self.train_len = (int)(self.data_num_examples * self.train_size)
        self.test_len = (int)(self.data_num_examples - self.train_len)
        self.train_data_path = floder + self.object_name + '_raw_train_data.csv'
        self.test_data_path = floder + self.object_name + '_raw_test_data.csv'

    def train_test_spilt_save(self):
        self.data_process()  # 获得测试集，训练集与归一化参数
        self.save_train_test_tocsv()  # 保存到csv

    def data_process(self):
        '''
        函数功能: 获得测试集，训练集与归一化参数
        :return: 无
        '''
        window_len = len(self.raw_window_data)

        for i in range(window_len):
            one_window = self.raw_window_data[i].values
            column_num = one_window.shape[1]
            column_num -= 3  # 剔除角速度
            one_window = np.hstack((one_window[:, 0:3], one_window[:, 6:(one_window.shape[1])]))
            one_window_features = one_window[:, 0:(column_num - self.label_num)]
            one_window_label = one_window[0, -1]
            one_sample = np.hstack((one_window_label, one_window_features.flatten()))
            self.new_window_data.append(one_sample)

        self.new_window_data = np.random.permutation(self.new_window_data)  # 乱序
        self.new_features_num = self.new_window_data.shape[1] - self.label_num

        self.Train = self.new_window_data[:self.train_len, :]
        self.Test = self.new_window_data[self.train_len:, :]
        self.X_train = self.Train[:, 1:(self.new_window_data.shape[1])].reshape((self.train_len, self.new_features_num))
        self.X_test = self.Test[:, 1:(self.new_window_data.shape[1])].reshape((self.test_len, self.new_features_num))
        self.Y_train = self.Train[:, 0:self.label_num].reshape((self.train_len, self.label_num))
        self.Y_test = self.Test[:, 0:self.label_num].reshape((self.test_len, self.label_num))

    def save_train_test_tocsv(self):
        '''
        函数功能：保存训练集与测试集到对应csv文件
        :return: 无
        '''
        # save to csv
        file_header = []
        file_header.append('label')
        for i in range(self.window_size):
            index = i + 1
            for j in range(self.raw_features_num):
                fea_index = j + 1
                header = str(index) + 'X' + str(fea_index)
                file_header.append(header)

        train = np.concatenate((self.Y_train, self.X_train), axis=1)  # axis=1表示对应行的数组进行拼接
        csv_fun.csv_init(file_path=self.train_data_path, csv_head=file_header)
        csv_fun.csv_append_datas(file_path=self.train_data_path, data_rows=train, data_len=len(train))
        test = np.concatenate((self.Y_test, self.X_test), axis=1)  # axis=1表示对应行的数组进行拼接
        csv_fun.csv_init(file_path=self.test_data_path, csv_head=file_header)
        csv_fun.csv_append_datas(file_path=self.test_data_path, data_rows=test, data_len=len(test))


def spilt_test(filepath, train_spilt, test_spilt, object_name, window_size, window_step):
    path = filepath
    train_total = (float)(train_spilt) / ((float)(train_spilt) + (float)(test_spilt))
    object = object_name
    spilt = Train_Test_Spilt_Rawdata(filepath=path, train_size=train_total, object_name=object,
                                     window_size=int(window_size), window_step=int(window_step))
    spilt.train_test_spilt_save()


# # Unity与Python联合运行时用
# spilt_test(filepath=sys.argv[1], train_spilt=sys.argv[2], test_spilt=sys.argv[3],
#            object_name=sys.argv[4], window_size=sys.argv[5], window_step=sys.argv[6])
# print(sys.argv[1],sys.argv[2],sys.argv[3],sys.argv[4], sys.argv[5],sys.argv[6])

# 单独运行Python时用
# path = 'E:/unity_pro/My_workV0.0/Assets/StreamingAssets/SourcesFolder/IMUSim_temp/my_imu_datas/imusim/9/Sidetoside_r_upleg_JNT_data.csv'
# spilt_test(filepath=path, train_spilt=4.0, test_spilt=1.0,
#            object_name='Sidetoside_r_upleg_JNT', window_size=40, window_step=20)
path = 'E:/unity_pro/My_workV0.0/Assets/StreamingAssets/SourcesFolder/IMUSim_temp/my_imu_datas/imureal/raw_data/UserA/half_half/3/Ankle_Right_Leg_r_upleg_JNT_ankle3_imureal_raw_data_half.csv'
spilt_test(filepath=path, train_spilt=1.0, test_spilt=0.0,
           object_name='2_ankle_half_half', window_size=40, window_step=20)
path = 'E:/unity_pro/My_workV0.0/Assets/StreamingAssets/SourcesFolder/IMUSim_temp/my_imu_datas/imureal/raw_data/UserA/half_half/3/Knee_Kick_Right_Leg_r_upleg_JNT_knee3_imureal_raw_data_half.csv'
spilt_test(filepath=path, train_spilt=1.0, test_spilt=0.0,
           object_name='0_kneekick_half_half', window_size=40, window_step=20)
path = 'E:/unity_pro/My_workV0.0/Assets/StreamingAssets/SourcesFolder/IMUSim_temp/my_imu_datas/imureal/raw_data/UserA/half_half/3/Reverse_Lunge_Right_Leg_r_upleg_JNT_rev3_imureal_raw_data_half.csv'
spilt_test(filepath=path, train_spilt=1.0, test_spilt=0.0,
           object_name='1_reverse_half_half', window_size=40, window_step=20)
path = 'E:/unity_pro/My_workV0.0/Assets/StreamingAssets/SourcesFolder/IMUSim_temp/my_imu_datas/imureal/raw_data/UserA/half_half/3/Sidetoside_Body_hips_JNT_sidetoside3_imureal_raw_data_half.csv'
spilt_test(filepath=path, train_spilt=1.0, test_spilt=0.0,
           object_name='4_sidetoside_half_half', window_size=40, window_step=20)
path = 'E:/unity_pro/My_workV0.0/Assets/StreamingAssets/SourcesFolder/IMUSim_temp/my_imu_datas/imureal/raw_data/UserA/half_half/3/Walking_Right_Leg_r_upleg_JNT_walk3_imureal_raw_data_half.csv'
spilt_test(filepath=path, train_spilt=1.0, test_spilt=0.0,
           object_name='3_walk_half_half', window_size=40, window_step=20)



