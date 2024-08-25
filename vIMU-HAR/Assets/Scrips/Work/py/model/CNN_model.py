import warnings

from keras.src.callbacks import EarlyStopping

warnings.filterwarnings("ignore")
import numpy as np
import pandas as pd
from comm import normalize_fun, csv_fun
import matplotlib.pyplot as plt
import tensorflow as tf
from tensorflow.keras import layers
from sklearn import preprocessing
import os

# floder = 'E:/unity_pro/My_workV0.1/Assets/StreamingAssets/SourcesFolder/Data_Process/'
# model_path = './model/model_output/_CNN_model.h5'
# model_path2d = './model/model_output/_CNN2D_model.h5'
floder = os.getcwd()
floder = os.path.abspath(os.path.join(floder,'Assets',"StreamingAssets\SourcesFolder\Data_Process"))
floder += '/'
model_floder = os.getcwd()
model_floder = os.path.abspath(os.path.join(model_floder,'Assets',"Scrips\Work\py\model\model_output"))
model_path = model_floder + '/_CNN_model.h5'
model_path2d = model_floder + '/_CNN2D_model.h5'

use_2d = 1
use_fft = 0
samplerate = 100  # 一秒钟才100个点（10ms一个点）
verbose = 0  # 不输出epoch进度条

class _CNN:
    def __init__(self, data_source,
                 training_cycles, optimizer, learning_rate, loss,
                 vaildation_spilt, batch_size, fit_generator,
                 features_num,
                 conv1dpool_layer0, conv1dpool_layer1, conv1dpool_layer2,
                 classes_num):
        self.data_source = data_source
        self.training_cycles = training_cycles
        self.optimizer = optimizer
        self.learning_rate = learning_rate
        self.loss = loss
        self.vaildation_spilt = vaildation_spilt
        self.batch_size = batch_size
        self.fit_generator = fit_generator
        self.strps_per_epoch = 0
        self.vaildation_steps = 0
        self.features_num = features_num
        self.conv1dpool_layer0_filiters_num = conv1dpool_layer0[0]
        self.conv1dpool_layer0_kernel_size = conv1dpool_layer0[1]
        self.conv1dpool_layer0_activation = conv1dpool_layer0[2]
        self.conv1dpool_layer0_pooling_size = conv1dpool_layer0[3]
        self.conv1dpool_layer1_filiters_num = conv1dpool_layer1[0]
        self.conv1dpool_layer1_kernel_size = conv1dpool_layer1[1]
        self.conv1dpool_layer1_activation = conv1dpool_layer1[2]
        self.conv1dpool_layer1_pooling_size = conv1dpool_layer1[3]
        self.conv1dpool_layer2_filiters_num = conv1dpool_layer2[0]
        self.conv1dpool_layer2_kernel_size = conv1dpool_layer2[1]
        self.conv1dpool_layer2_activation = conv1dpool_layer2[2]
        self.conv1dpool_layer2_pooling_size = conv1dpool_layer2[3]
        self.classes_num = classes_num

        self.get_data()

    def get_data(self):
        '''
        函数功能：获取训练集与测试集
        :return: 无
        '''
        # 读取数据
        if (self.data_source == 'r'):
            self.train_file_path = floder + 'raw_all_train_data.csv'
            self.test_file_path = floder + 'raw_all_test_data.csv'
        elif (self.data_source == 'e'):
            self.train_file_path = floder + 'all_train_data.csv'
            self.test_file_path = floder + 'all_test_data.csv'
        self.file_header = csv_fun.get_csv_header(self.train_file_path)
        self.feature_names = self.file_header[1:len(self.file_header)]
        self.train_data = pd.read_csv(self.train_file_path)
        self.test_data = pd.read_csv(self.test_file_path)
        # 文本标签转化为数字标签
        le = preprocessing.LabelEncoder()  # 获取一个LabelEncoder
        self.train_data['label'] = le.fit_transform(self.train_data['label'])  # 使用训练好的LabelEncoder对原数据进行编码
        self.test_data['label'] = le.fit_transform(self.test_data['label'])  # 使用训练好的LabelEncoder对原数据进行编码
        self.train_data_len = self.train_data.shape[0]
        self.test_data_len = self.test_data.shape[0]
        self.column_num = self.train_data.shape[1] - 1 if self.data_source == 'e' else self.train_data.shape[1]
        self.label_num = 1
        self.features_num = self.column_num - self.label_num

        # 得到训练集与测试集
        self.X_train = self.train_data.iloc[:, 1:self.column_num].values.reshape(
            (self.train_data_len, self.features_num))
        self.Y_train = self.train_data.iloc[:, 0:self.label_num].values.reshape((self.train_data_len, self.label_num))
        self.X_test = self.test_data.iloc[:, 1:self.column_num].values.reshape((self.test_data_len, self.features_num))
        self.Y_test = self.test_data.iloc[:, 0:self.label_num].values.reshape((self.test_data_len, self.label_num))

        # 打印训练集与测试集大小，联合运行时记得屏蔽掉！！！！！！
        # print(len(self.X_train),len(self.X_test))

        if use_fft:  # 将其转化为频域分量
            # 计算傅里叶变换
            for i in range(len(self.X_train)):
                # self.X_train = self.X_train - np.mean(self.X_train)  # 去除直流分量
                raw = self.X_train[i]
                self.X_train[i] = np.fft.fft(self.X_train[i],norm='ortho')
                self.X_train[i][0] = 0  # 去除直流分量
                # self.X_train[i] = np.abs(self.X_train[i]) / 120*2   # 我的归一化
                if i == 1:  # 画图看一下
                    x = np.linspace(0, 2 * np.pi, 120)
                    freq = np.fft.fftfreq(120, x[1] - x[0])
                    # 绘制原始信号
                    plt.subplot(211)
                    plt.plot(x, raw)
                    # 绘制频谱图
                    plt.subplot(212)
                    plt.plot(freq, np.abs(self.X_train[i]))
                    plt.show()
            for i in range(len(self.X_test)):
                # self.X_test = self.X_test - np.mean(self.X_test)  # 去除直流分量
                self.X_test[i] = np.fft.fft(self.X_test[i],norm='ortho')
                self.X_test[i][0] = 0  # 去除直流分量
                # self.X_test[i] = np.abs(self.X_test[i]) / 120*2  # 我的归一化

        if use_2d:  # 使用conv2d
            # 将数据1X120转成3X40
            self.X_train = self.X_train.reshape((self.train_data_len, int(self.features_num/3), 3))
            self.X_test = self.X_test.reshape((self.test_data_len, int(self.features_num/3), 3))

            if use_fft == 0:
                # 对数据进行归一化处理
                self.X_train = normalize_fun.normalize_3dimensions(self.X_train)
                self.X_test = normalize_fun.normalize_3dimensions(self.X_test)

            # 增加灰度维度防止conv2d报错
            self.X_train = np.expand_dims(self.X_train, -1)
            self.X_test = np.expand_dims(self.X_test, -1)
        else:
            # 对数据进行归一化处理
            # self.X_train, self.features_mean, self.features_deviation = normalize_fun.normalize(self.X_train)
            # self.X_test = normalize_fun.normalize_designate_paras(features=self.X_test,
            #                                                       features_mean=self.features_mean,
            #                                                       features_deviation=self.features_deviation)
            # 对数据进行归一化处理
            self.X_train = normalize_fun.normalize_2dimensions(self.X_train)
            self.X_test = normalize_fun.normalize_2dimensions(self.X_test)

            # 修改数据shape防止conv1d报错
            self.X_train = self.X_train.reshape((self.X_train.shape[0], self.X_train.shape[1], 1))
            self.X_test = self.X_test.reshape((self.X_test.shape[0], self.X_test.shape[1], 1))
            self.Y_train = self.Y_train.reshape((self.Y_train.shape[0], 1))
            self.Y_test = self.Y_test.reshape((self.Y_test.shape[0], 1))

    def model_build(self):
        self.model = tf.keras.Sequential()

        if use_2d:
            self.model.add(layers.Conv2D(filters=self.conv1dpool_layer0_filiters_num,
                                         kernel_size=(
                                             self.conv1dpool_layer0_kernel_size, self.conv1dpool_layer0_kernel_size),
                                         padding='same', strides=1, activation=self.conv1dpool_layer0_activation,
                                         input_shape=(
                                             self.X_train.shape[1], self.X_train.shape[2], self.X_train.shape[3])))
            self.model.add(
                layers.MaxPool2D(pool_size=(self.conv1dpool_layer0_pooling_size, self.conv1dpool_layer0_pooling_size),
                                 padding='same'))
            # 添加dropout层简化模型
            self.model.add(layers.Dropout(0.4))

            self.model.add(layers.Conv2D(filters=self.conv1dpool_layer1_filiters_num,
                                         kernel_regularizer=tf.keras.regularizers.l2(0.01),
                                         kernel_size=(
                                             self.conv1dpool_layer1_kernel_size, self.conv1dpool_layer1_kernel_size),
                                         padding='same', strides=1, activation=self.conv1dpool_layer1_activation))
            self.model.add(
                layers.MaxPool2D(pool_size=(self.conv1dpool_layer1_pooling_size, self.conv1dpool_layer1_pooling_size),
                                 padding='same'))

            self.model.add(layers.Conv2D(filters=self.conv1dpool_layer2_filiters_num,
                                         kernel_size=(
                                             self.conv1dpool_layer2_kernel_size, self.conv1dpool_layer2_kernel_size),
                                         padding='same', strides=1, activation=self.conv1dpool_layer2_activation))
            self.model.add(
                layers.MaxPool2D(pool_size=(self.conv1dpool_layer2_pooling_size, self.conv1dpool_layer2_pooling_size),
                                 padding='same'))

            self.model.add(layers.Flatten())

            self.model.add(layers.Dense(self.classes_num, activation='softmax'))
        else:
            self.model.add(layers.Conv1D(filters=self.conv1dpool_layer0_filiters_num,
                                         kernel_size=self.conv1dpool_layer0_kernel_size,
                                         padding='same', strides=1, activation=self.conv1dpool_layer0_activation,
                                         input_shape=(self.X_train.shape[1], 1)))
            self.model.add(layers.MaxPool1D(pool_size=self.conv1dpool_layer0_pooling_size, padding='same'))
            # 添加dropout层简化模型
            self.model.add(layers.Dropout(0.4))

            self.model.add(layers.Conv1D(filters=self.conv1dpool_layer1_filiters_num,
                                         kernel_regularizer=tf.keras.regularizers.l2(0.03),
                                         kernel_size=self.conv1dpool_layer1_kernel_size,
                                         padding='same', strides=1, activation=self.conv1dpool_layer1_activation))
            self.model.add(layers.MaxPool1D(pool_size=self.conv1dpool_layer1_pooling_size, padding='same'))
            # 添加dropout层简化模型
            self.model.add(layers.Dropout(0.25))

            self.model.add(layers.Conv1D(filters=self.conv1dpool_layer2_filiters_num,
                                         kernel_size=self.conv1dpool_layer2_kernel_size,
                                         padding='same', strides=1, activation=self.conv1dpool_layer2_activation))
            self.model.add(layers.MaxPool1D(pool_size=self.conv1dpool_layer2_pooling_size, padding='same'))

            self.model.add(layers.Flatten())

            self.model.add(layers.Dense(self.classes_num, activation='softmax'))

    def modle_train(self):
        # optimizer
        if self.optimizer == 'Adam':
            optimizer = tf.keras.optimizers.Adam(self.learning_rate)
        elif self.optimizer == 'Adadelta':
            optimizer = tf.keras.optimizers.Adadelta(self.learning_rate)
        elif self.optimizer == 'Adafactor':
            optimizer = tf.keras.optimizers.Adafactor(self.learning_rate)
        elif self.optimizer == 'Adagrad':
            optimizer = tf.keras.optimizers.Adagrad(self.learning_rate)
        elif self.optimizer == 'AdamW':
            optimizer = tf.keras.optimizers.AdamW(self.learning_rate)
        elif self.optimizer == 'Adamax':
            optimizer = tf.keras.optimizers.Adamax(self.learning_rate)
        elif self.optimizer == 'Nadam':
            optimizer = tf.keras.optimizers.Nadam(self.learning_rate)
        elif self.optimizer == 'RMSprop':
            optimizer = tf.keras.optimizers.RMSprop(self.learning_rate)
        elif self.optimizer == 'SGD':
            optimizer = tf.keras.optimizers.SGD(self.learning_rate)

        # losses
        if self.loss == 'SparseCategoricalCrossentropy':
            loss = tf.keras.losses.SparseCategoricalCrossentropy()
        elif self.loss == 'BinaryCrossentropy':
            loss = tf.keras.losses.BinaryCrossentropy()
        elif self.loss == 'MeanSquaredError':
            loss = tf.keras.losses.MeanSquaredError()

        # compile
        self.model.compile(optimizer=optimizer,
                           loss=loss,
                           metrics=[tf.keras.metrics.SparseCategoricalAccuracy()])  # 'acc'

        # add earlystopping
        earlystop_callback = EarlyStopping(
            monitor='val_sparse_categorical_accuracy', min_delta=0.0001,
            patience=1)

        # fit
        self.model.fit(self.X_train, self.Y_train, epochs=self.training_cycles, batch_size=self.batch_size,
                       validation_split=self.vaildation_spilt,
                       verbose=verbose)  # callbacks=[earlystop_callback]

        # model save
        if use_2d:
            self.model.save(model_path2d)
        else:
            self.model.save(model_path)

        # print message
        if verbose:
            self.model.summary()

        # model evaluate
        test_loss, test_acc = self.model.evaluate(self.X_test, self.Y_test, verbose=verbose)
        print('\nTest accuracy:', test_acc)


    def model_predit(self, x_test):
        '''
        函数功能: 对输入的数据进行预测
        :param x_test: 输入特征数据
        :return: 预测值对于实际类别标签值
        '''
        # 对数据进行归一化处理
        # x_test = normalize_fun.normalize_designate_paras(features=x_test,
        #                                                  features_mean=self.features_mean,
        #                                                  features_deviation=self.features_deviation)
        x_test = normalize_fun.normalize_2dimensions(x_test)

        # 加载模型
        model = tf.keras.models.load_model(model_path)

        # 预测类别
        y_predit_label = []
        y_predit_val = model.predict(x_test, verbose=verbose)
        y_predit_index = np.argmax(y_predit_val, axis=1)
        for i in range(len(y_predit_index)):
            label = self.labels[y_predit_index[i]]
            y_predit_label.append(label)
        return y_predit_label
