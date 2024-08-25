import sys

import numpy as np
import pandas as pd
import comm.get_windows as win
from comm import csv_fun

sampling_frequency = 10  # 10Hz
features = ['fc', 'mf', 'rmsf', 'vf', 'std']
axis = ['acc_x', 'acc_y', 'acc_z']


class Frequency_Features():
    def __init__(self, raw_data_path, frequency_features_path, window_size, window_step):
        self.raw_data_path = raw_data_path
        self.frequency_features_path = frequency_features_path
        self.raw_data = pd.read_csv(self.raw_data_path)
        self.row_len = len(self.raw_data) + 1
        self.raw_data = self.raw_data.values[:, 0:len(axis)]  # 取出需要进行fft的数据
        self.raw_window_data = win.get_window_data_from_datas(data=self.raw_data,
                                                              data_len=self.row_len,
                                                              data_window_size=window_size,
                                                              data_window_step=window_step)

    def frequency_features_extraction(self):
        self.get_all_frequency_domain_feature()
        self.save_frequency_features_tocsv()

    def get_all_frequency_domain_feature(self):
        window_len = len(self.raw_window_data)
        x_datas = []
        y_datas = []
        z_datas = []
        for i in range(window_len):
            one_window = self.raw_window_data[i]
            x_raw = one_window[:, 0]
            y_raw = one_window[:, 1]
            z_raw = one_window[:, 2]
            x_datas.append(x_raw.flatten())
            y_datas.append(y_raw.flatten())
            z_datas.append(z_raw.flatten())

        self.x_fre_features = self.get_frequency_domain_feature(data=x_datas, sampling_frequency=sampling_frequency)
        self.y_fre_features = self.get_frequency_domain_feature(data=y_datas, sampling_frequency=sampling_frequency)
        self.z_fre_features = self.get_frequency_domain_feature(data=z_datas, sampling_frequency=sampling_frequency)

    def save_frequency_features_tocsv(self):
        '''
        函数功能：保存频率特征到csv文件
        :return: 无
        '''
        # save to csv
        file_header = []
        for axis_index in range(len(axis)):
            for features_index in range(len(features)):
                header = axis[axis_index] + '_' + features[features_index]
                file_header.append(header)

        features_datas = np.concatenate((self.x_fre_features,
                                         self.y_fre_features,
                                         self.z_fre_features), axis=1)  # axis=1表示对应行的数组进行拼接
        csv_fun.csv_init(file_path=self.frequency_features_path, csv_head=file_header)
        csv_fun.csv_append_datas(file_path=self.frequency_features_path,
                                 data_rows=features_datas, data_len=len(features_datas))

    def get_frequency_domain_feature(self, data, sampling_frequency):
        """
        提取 5个 频域特征: 重心频率 平均频率 均方根频率 频率方差 频率标准差

        @param data: shape 为 (m, n) 的 2D array 数据，其中，m 为样本个数， n 为样本（信号）长度
        @param sampling_frequency: 采样频率
        @return: shape 为 (m, 5)  的 2D array 数据，其中，m 为样本个数。即 每个样本的4个频域特征
        """
        data_fft = np.fft.fft(data, axis=1)
        m, N = data_fft.shape  # 样本个数 和 信号长度

        # 傅里叶变换是对称的，只需取前半部分数据，否则由于 频率序列 是 正负对称的，会导致计算 重心频率求和 等时正负抵消
        mag = np.abs(data_fft)[:, : N // 2]  # 信号幅值
        freq = np.fft.fftfreq(N, 1 / sampling_frequency)[: N // 2]
        # mag = np.abs(data_fft)[: , N // 2: ]  # 信号幅值
        # freq = np.fft.fftfreq(N, 1 / sampling_frequency)[N // 2: ]

        ps = mag ** 2 / N  # 功率谱

        fc = np.sum(freq * ps, axis=1) / np.sum(ps, axis=1)  # 重心频率
        mf = np.mean(ps, axis=1)  # 平均频率
        rmsf = np.sqrt(np.sum(ps * np.square(freq), axis=1) / np.sum(ps, axis=1))  # 均方根频率

        freq_tile = np.tile(freq.reshape(1, -1), (m, 1))  # 复制 m 行
        fc_tile = np.tile(fc.reshape(-1, 1), (1, freq_tile.shape[1]))  # 复制 列，与 freq_tile 的列数对应
        vf = np.sum(np.square(freq_tile - fc_tile) * ps, axis=1) / np.sum(ps, axis=1)  # 频率方差

        std = np.sqrt(vf)  # 频率标准差

        features = [fc, mf, rmsf, vf, std]  # 重心频率 平均频率 均方根频率 频率方差 频率标准差

        return np.array(features).T


def get_all_frequency_features(raw_data_path, frequency_features_path, window_size, window_step):
    print('raw_data_path=', sys.argv[1],
          'frequency_features_path=', sys.argv[2],
          'window_size=', sys.argv[3],
          'window_step=', sys.argv[4])
    fre_feas = Frequency_Features(raw_data_path=raw_data_path, frequency_features_path=frequency_features_path,
                                  window_size=int(window_size), window_step=int(window_step))
    fre_feas.frequency_features_extraction()


# 与unity联合运行时用
get_all_frequency_features(raw_data_path=sys.argv[1], frequency_features_path=sys.argv[2],
                           window_size=sys.argv[3], window_step=sys.argv[4])

# 单独运行时用
# raw_path = 'E:/unity_pro/My_workV0.3/Assets/StreamingAssets/SourcesFolder/IMUSim/Knee_Kick_hips_JNT_label_data.csv'
# fre_feas_path = 'E:/unity_pro/My_workV0.3/Assets/StreamingAssets/SourcesFolder/IMUSim/Knee_Kick_hips_JNT_frequency_feature_data.csv'
# get_all_frequency_features(raw_data_path=raw_path, frequency_features_path=fre_feas_path,
#                            window_size=40, window_step=20)
