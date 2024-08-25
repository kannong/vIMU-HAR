import numpy as np
import pandas as pd
from scipy import signal

from comm import csv_fun
import matplotlib.pyplot as plt

floder = 'E:/unity_pro/My_workV0.1/Assets/StreamingAssets/SourcesFolder/IMUSim_temp/my_imu_datas/imureal/raw_data/UserA/half_half/0/'
new_path_suffix = '_raw_data_half.csv'
step = 4

class down_samping():
    def __init__(self, filepath, object_name):
        self.filepath = filepath
        self.object_name = object_name
        self.new_data_path = floder + self.object_name + new_path_suffix

    def down_samping_save(self):
        self.data_process()  # 获得测试集，训练集与归一化参数
        self.save_train_test_tocsv()  # 保存到csv

    def data_process(self):
        '''
        函数功能: 获得测试集，训练集
        :return: 无
        '''
        # get raw data
        self.file_header = csv_fun.get_csv_header(self.filepath)
        self.raw_data = pd.read_csv(self.filepath).values

        # down samping
        self.new_data = self.raw_data[1::step]  # 取奇数行

        # draw
        # self.draw_figure()

        print('new len=',len(self.new_data))

    def save_train_test_tocsv(self):
        '''
        函数功能：保存训练集与测试集到对应csv文件
        :return: 无
        '''
        # save to csv
        csv_fun.csv_init(file_path=self.new_data_path, csv_head=self.file_header)
        csv_fun.csv_append_datas(file_path=self.new_data_path, data_rows=self.new_data, data_len=len(self.new_data))

    def draw_figure(self):
        raw_data = self.raw_data[:,0:3]
        new_data = self.new_data[:,0:3]
        # 1: 时域图
        # 绘制原始信号
        plt.figure(1)
        plt.subplot(211)
        plt.plot(raw_data)
        # 绘制降采样后的信号
        plt.subplot(212)
        plt.plot(new_data)
        # 2: 频谱图
        plt.figure(2)
        y_fft_r = np.fft.fft(raw_data)
        y_fft_n = np.fft.fft(new_data)
        plt.subplot(211)
        plt.plot(np.abs(y_fft_r))
        plt.subplot(212)
        plt.plot(np.abs(y_fft_n))
        # 3: 低通滤波后信号
        # 指定滤波器参数
        fs = 1000.0  # 采样频率
        fc = 10.0  # 截止频率
        order = 2  # 滤波器阶数
        # 计算归一化截止频率
        wc = 4 * fc / fs
        # 设计低通巴特沃斯滤波器
        b, a = signal.butter(order, wc, btype='low')
        # 应用滤波器
        filtered_data_r = signal.filtfilt(b, a, raw_data, axis=0)
        filtered_data_e = signal.filtfilt(b, a, new_data, axis=0)
        plt.figure(3)
        plt.subplot(211)
        plt.plot(np.abs(filtered_data_r))
        plt.subplot(212)
        plt.plot(np.abs(filtered_data_e))
        # 4: 低通滤波后频谱图
        plt.figure(4)
        filter_y_fft_r = np.fft.fft(filtered_data_r)
        filter_y_fft_n = np.fft.fft(filtered_data_e)
        plt.subplot(211)
        plt.plot(np.abs(filter_y_fft_r))
        plt.subplot(212)
        plt.plot(np.abs(filter_y_fft_n))
        plt.show()

def down_samping_test(path, object_name):
    down = down_samping(filepath=path,object_name=object_name)
    down.down_samping_save()

path = 'E:/unity_pro/My_workV0.0/Assets/StreamingAssets/SourcesFolder/IMUSim_temp/my_imu_datas/imureal/raw_data/UserA/0/Ankle_Right_Leg_r_upleg_JNT_ankle0_imureal_data.csv'
down_samping_test(path, 'Ankle_Right_Leg_r_upleg_JNT_ankle0_imureal')
path = 'E:/unity_pro/My_workV0.0/Assets/StreamingAssets/SourcesFolder/IMUSim_temp/my_imu_datas/imureal/raw_data/UserA/0/Knee_Kick_Right_Leg_r_upleg_JNT_knee0_imureal_data.csv'
down_samping_test(path, 'Knee_Kick_Right_Leg_r_upleg_JNT_knee0_imureal')
path = 'E:/unity_pro/My_workV0.0/Assets/StreamingAssets/SourcesFolder/IMUSim_temp/my_imu_datas/imureal/raw_data/UserA/0/Reverse_Lunge_Right_Leg_r_upleg_JNT_rev0_imureal_data.csv'
down_samping_test(path, 'Reverse_Lunge_Right_Leg_r_upleg_JNT_rev0_imureal')
path = 'E:/unity_pro/My_workV0.0/Assets/StreamingAssets/SourcesFolder/IMUSim_temp/my_imu_datas/imureal/raw_data/UserA/0/Sidetoside_Body_hips_JNT_sidetoside0_imureal_data.csv'
down_samping_test(path, 'Sidetoside_Body_hips_JNT_sidetoside0_imureal')
path = 'E:/unity_pro/My_workV0.0/Assets/StreamingAssets/SourcesFolder/IMUSim_temp/my_imu_datas/imureal/raw_data/UserA/0/Walking_Right_Leg_r_upleg_JNT_walk0_imureal_data.csv'
down_samping_test(path, 'Walking_Right_Leg_r_upleg_JNT_walk0_imureal')
