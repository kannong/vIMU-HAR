import pandas as pd

def get_window_data(file_path, row_len, data_window_size, data_window_step):
    '''
    函数功能:得到窗口数据
    :param file_path: 文件路径
    :param row_len: 行数（包括表头）
    :param data_window_size: 一个窗口的采样点数量
    :param data_window_step: 窗口步长
    :return: 是否成功 成功:返回生成的所有窗口的数据列表 失败: False
    '''
    window_data = []
    data = pd.read_csv(file_path)
    if row_len >= data_window_size:
        for i in range(0, row_len - data_window_size, data_window_step):
            one_window_data = next_window(data, i, data_window_size)
            window_data.append(one_window_data)
        return window_data
    else:
        print('当前数据未有一个数据窗口长度，数据窗口长度为：', data_window_size)
        return False

def get_window_data_from_datas(data, data_len, data_window_size, data_window_step):
    '''
    函数功能:得到窗口数据
    :param data: 输入数据
    :param data_len: 数据长度
    :param data_window_size: 一个窗口的采样点数量
    :param data_window_step: 窗口步长
    :return: 是否成功 成功:返回生成的所有窗口的数据列表 失败: False
    '''
    window_data = []
    if data_len >= data_window_size:
        for i in range(0, data_len - data_window_size, data_window_step):
            one_window_data = next_window(data, i, data_window_size)
            window_data.append(one_window_data)
        return window_data
    else:
        print('当前数据未有一个数据窗口长度，数据窗口长度为：', data_window_size)
        return False

def next_window(data, i, data_window_size):
    '''
    函数功能：读取下一个窗口的数据
    :param data: 从csv中读取的数据
    :param i: 当前行的索引
    :param data_window_size: 一个窗口的采样点数量
    :return: 一个窗口的数据列表
    '''
    one_window = data[i:i + data_window_size]
    return one_window
