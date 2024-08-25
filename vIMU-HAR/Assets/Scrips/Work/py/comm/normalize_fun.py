"""Normalize features"""

import numpy as np
import matplotlib.pyplot as plt


def normalize(features):
    '''
    函数功能: 数据归一化
    :param features: 数据
    :return: 归一化后的数据以及归一化参数 （测试集与训练集归一化参数要相同）
    '''

    features_normalized = np.copy(features).astype(float)

    # 计算均值
    features_mean = np.mean(features_normalized, 0)
    features_mean = np.nan_to_num(features_mean)

    # 计算标准差
    features_deviation = np.std(features_normalized, 0)
    features_deviation = np.nan_to_num(features_deviation)

    # 标准化操作
    if features.shape[0] > 1:
        features_normalized -= features_mean

    # 防止除以0
    features_deviation[features_deviation == 0] = 1
    features_normalized /= features_deviation

    new_mean = np.mean(features_normalized, axis=0)
    new_std = np.std(features_normalized, axis=0)
    # print('finish')

    return features_normalized, features_mean, features_deviation


def normalize_designate_paras(features, features_mean, features_deviation):
    '''
    函数功能: 指定归一化参数对数据进行归一化，用于对预测数据归一化
    :param features: 数据
    :param features_mean: 均值
    :param features_deviation: 标准差
    :return: 归一化处理后的数据
    '''
    features_normalized = np.copy(features).astype(float)

    # 标准化操作
    features_normalized = np.subtract(features_normalized, features_mean)
    features_normalized = np.divide(features_normalized, features_deviation)

    new_mean = np.mean(features_normalized, axis=0)
    new_std = np.std(features_normalized, axis=0)
    # print('finish')

    return features_normalized


def normalize_3dimensions(features):
    '''
    函数功能: 三维数据归一化
    :param features: 数据
    :return: 无
    '''

    features_normalized = np.copy(features).astype(float)

    # 分别将所有样本归一化
    for i in range(len(features_normalized)):
        one_data = np.copy(features_normalized[i]).astype(float)
        # 绘制原始信号
        # plt.subplot(211)
        # plt.plot(one_data)

        # 计算均值
        features_mean = np.mean(one_data, axis=0)
        features_mean = np.nan_to_num(features_mean)

        # 计算标准差
        features_deviation = np.std(one_data, axis=0)
        features_deviation = np.nan_to_num(features_deviation)

        # 标准化操作
        if features.shape[0] > 1:
            features_normalized[i] -= features_mean

        # 防止除以0
        for ij in range(one_data.shape[1]):
            features_deviation[ij] = 1 if features_deviation[ij] == 0 else features_deviation[ij]
        features_normalized[i] /= features_deviation

        # 绘制归一化后的信号
        # plt.subplot(212)
        # plt.plot(features_normalized[i])
        # plt.show()

        new_mean = np.mean(features_normalized[i], axis=0)
        new_std = np.std(features_normalized[i], axis=0)
        # print('finish')

    return features_normalized

def normalize_2dimensions(features):
    '''
    函数功能: 二维数据归一化
    :param features: 数据
    :return: 无
    '''

    features_normalized = np.copy(features).astype(float)

    # 分别将所有样本归一化
    for i in range(len(features_normalized)):
        one_data = np.copy(features_normalized[i]).astype(float)
        # 绘制原始信号
        # plt.subplot(211)
        # plt.plot(one_data)

        # 计算均值
        features_mean = np.mean(one_data)
        features_mean = np.nan_to_num(features_mean)

        # 计算标准差
        features_deviation = np.std(one_data)
        features_deviation = np.nan_to_num(features_deviation)

        # 标准化操作
        if features.shape[0] > 1:
            features_normalized[i] -= features_mean

        # 防止除以0
        features_deviation = 1 if features_deviation == 0 else features_deviation
        features_normalized[i] /= features_deviation

        # 绘制归一化后的信号
        # plt.subplot(212)
        # plt.plot(features_normalized[i])
        # plt.show()

        new_mean = np.mean(features_normalized[i], axis=0)
        new_std = np.std(features_normalized[i], axis=0)
        # print('finish')

    return features_normalized

