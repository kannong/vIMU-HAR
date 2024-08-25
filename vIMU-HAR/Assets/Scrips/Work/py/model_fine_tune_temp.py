import sys
import warnings

from keras.src.callbacks import EarlyStopping
from sklearn.metrics import accuracy_score

warnings.filterwarnings("ignore")
import numpy as np
import pandas as pd
from comm import normalize_fun, csv_fun
import matplotlib.pyplot as plt
import tensorflow as tf
from tensorflow.keras import layers
import os
# data_floder = 'E:/unity_pro/My_workV0.0/Assets/StreamingAssets/SourcesFolder/Data_Process/'
data_floder = os.getcwd()
data_floder = os.path.abspath(os.path.join(data_floder,'Assets',"StreamingAssets\SourcesFolder\Data_Process"))
data_floder += '/'
model_floder = './model/model_output/'
epoch_num = 400
validation_split = 0.20
batch_size = 10

# _NN_model.h5
# _CNN_model.h5

use_2d = 1
use_fft = 0


def model_finetune(object_model, data_source, loss, labels):
    (X_train, X_test, Y_train, Y_test) = get_data(data_source=data_source)
    model_path = model_floder + "_" + object_model + "_model.h5"
    if use_2d:
        model_path = model_floder + "_" + object_model + "2D" + "_model.h5"
    model = tf.keras.models.load_model(model_path)
    print(len(model.layers))

    for i in range(len(model.layers) - 1):
        model.layers[i].trainable = False
    # compile
    model.compile(optimizer='adam', loss=loss, metrics=[tf.keras.metrics.SparseCategoricalAccuracy()])

    # add earlystopping
    # earlystop_callback = EarlyStopping(
    #     monitor='val_sparse_categorical_accuracy', min_delta=0.0001,
    #     patience=2)

    # fit
    print(len(X_train), len(X_test), len(Y_train), len(Y_test))
    model.fit(X_train, Y_train, epochs=epoch_num, validation_split=validation_split, batch_size=batch_size,
              )

    # print message
    model.summary()

    # model evaluate
    test_loss, test_acc = model.evaluate(X_test, Y_test)
    print('\nTest accuracy:', test_acc)

    # model save
    model.save(model_path)

    # predit
    y_predit_label = []
    y_predit_val = model.predict(X_test)
    y_predit_index = np.argmax(y_predit_val, axis=1)
    for i in range(len(y_predit_index)):
        label = labels[y_predit_index[i]]
        y_predit_label.append(label)

    return y_predit_label


def get_data(data_source):
    '''
    函数功能：获取训练集与测试集
    :return: 无
    '''
    # 读取数据
    if (data_source == 'r'):
        train_file_path = data_floder + 'imureal_all_train_data.csv'
        test_file_path = data_floder + 'imureal_all_test_data.csv'
    elif (data_source == 'e'):
        train_file_path = data_floder + 'imureal_all_train_data_e.csv'
        test_file_path = data_floder + 'imureal_all_test_data_e.csv'
    file_header = csv_fun.get_csv_header(train_file_path)
    feature_names = file_header[1:len(file_header)]
    train_data = pd.read_csv(train_file_path)
    test_data = pd.read_csv(test_file_path)
    train_data_len = train_data.shape[0]
    test_data_len = test_data.shape[0]
    column_num = train_data.shape[1]
    label_num = 1
    features_num = column_num - label_num

    # 得到训练集与测试集
    X_train = train_data.iloc[:, 1:column_num].values.reshape(
        (train_data_len, features_num))
    Y_train = train_data.iloc[:, 0:label_num].values.reshape((train_data_len, label_num))
    X_test = test_data.iloc[:, 1:column_num].values.reshape((test_data_len, features_num))
    Y_test = test_data.iloc[:, 0:label_num].values.reshape((test_data_len, label_num))

    if use_fft:  # 将其转化为频域分量
        # 计算傅里叶变换
        for i in range(len(X_train)):
            # X_train = X_train - np.mean(X_train)  # 去除直流分量
            raw = X_train[i]
            X_train[i] = np.fft.fft(X_train[i],norm='ortho')
            X_train[i][0] = 0  # 去除直流分量
            # X_train[i] = np.abs(X_train[i]) / 120 * 2  # 我的归一化
            if i == 1:  # 画图看一下
                x = np.linspace(0, 2 * np.pi, 120)
                freq = np.fft.fftfreq(120, x[1] - x[0])
                # 绘制原始信号
                plt.subplot(211)
                plt.plot(x, raw)
                # 绘制频谱图
                plt.subplot(212)
                plt.plot(freq, np.abs(X_train[i]))
                plt.show()
        for i in range(len(X_test)):
            # X_test = X_test - np.mean(X_test)  # 去除直流分量
            X_test[i] = np.fft.fft(X_test[i],norm='ortho')
            X_test[i][0] = 0  # 去除直流分量
            # X_test[i] = np.abs(X_test[i]) / 120 * 2   # 我的归一化

    if use_2d:  # 使用conv2d
        # 将数据1X120转成3X40
        X_train = X_train.reshape((train_data_len, 40, 3))
        X_test = X_test.reshape((test_data_len, 40, 3))

        # 对数据进行归一化处理
        X_train = normalize_fun.normalize_3dimensions(X_train)
        X_test = normalize_fun.normalize_3dimensions(X_test)

        # 增加灰度维度防止conv2d报错
        X_train = np.expand_dims(X_train, -1)
        X_test = np.expand_dims(X_test, -1)
    else:
        # 对数据进行归一化处理
        # X_train, features_mean, features_deviation = normalize_fun.normalize(X_train)
        # X_test = normalize_fun.normalize_designate_paras(features=X_test,
        #                                                  features_mean=features_mean,
        #                                                  features_deviation=features_deviation)
        X_train = normalize_fun.normalize_2dimensions(X_train)
        X_test = normalize_fun.normalize_2dimensions(X_test)

        # 保存归一化参数
        # csv_header = []
        # for i in range(features_num):
        #     csv_header.append(i)
        # csv_fun.csv_init(file_path=data_floder + 'features_mean.csv', csv_head=csv_header)
        # csv_fun.csv_append_data(file_path=data_floder + 'features_mean.csv', data_row=features_mean)
        # csv_fun.csv_init(file_path=data_floder + 'features_deviation.csv', csv_head=csv_header)
        # csv_fun.csv_append_data(file_path=data_floder + 'features_deviation.csv', data_row=features_deviation)

    return X_train, X_test, Y_train, Y_test


def model_fine_tune():
    object_model = sys.argv[1]
    data_source = sys.argv[2]
    loss = sys.argv[3]
    labels_len = int(sys.argv[4])
    label = []
    for i in range(labels_len):
        label.append(sys.argv[5 + i])

    print('object_model=', object_model,
          'data_source=', data_source,
          'loss=', loss,
          'labels=', label)
    model_finetune(object_model=object_model,
                   data_source=data_source,
                   loss=loss,
                   labels=label)


# 单独运行Python时用
predit = model_finetune('CNN', "r", "SparseCategoricalCrossentropy",
                        ['0_kneekick', '1_reverse', '2_ankle', '3_walk', '4_sidetoside'])
# print(predit)

# Unity与Python联合运行时用
# model_fine_tune()
