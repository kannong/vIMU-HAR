import numpy as np


def get_mean_filler_data(data_arr, window_left, window_right):
    '''
    函数功能: 得到均值滤波后的一维数据数组
    :param data_arr: 数据数组
    :param window_left: 窗口左取样数
    :param window_right: 窗口右取样数
    :return: 均值滤波后的数据数组
    '''
    arr_size = len(data_arr)
    window_size = window_left + window_right
    if arr_size > window_size:
        mean_res = ava_mean_filler(data_arr, window_left, window_right)
        temp = data_arr[arr_size - window_size:arr_size]
        res = np.append(mean_res, temp).reshape(-1, 1)  # 剩余用原始数据填充
    else:
        print('数据长度不足')
        res = []
    return res


def ava_mean_filler(data_arr, window_left, window_right):
    '''
    函数功能: 一维数据数组的单次窗口均值滤波 范围: [-window_left,window_right] 最后window_left+window_right个数据用原数据替代
    :param data_arr: 数据数组
    :param window_left: 窗口左取样数
    :param window_right: 窗口右取样数
    :return:  均值滤波后的数据数组
    '''
    res = []
    size = len(data_arr)
    for i in range(window_left, size - window_right):
        sum = 0
        # 滑窗
        for j in range(-window_left, window_right + 1):
            sum += data_arr[i + j]
        sum /= (window_left + window_right + 1)
        res.append(sum)
    return np.array(res)
