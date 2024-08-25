import sys
import warnings
import os
from sklearn.metrics import accuracy_score
from keras.src.callbacks import EarlyStopping

warnings.filterwarnings("ignore")
import numpy as np
import pandas as pd
from comm import normalize_fun, csv_fun
import matplotlib.pyplot as plt
import tensorflow as tf
from tensorflow.keras import layers
from sklearn import preprocessing

# data_floder = 'E:/unity_pro/My_workV1.1/Assets/StreamingAssets/SourcesFolder/Data_Process/'
# model_floder = './model/model_output/'
data_floder = os.getcwd()
data_floder = os.path.abspath(os.path.join(data_floder, 'Assets', "StreamingAssets\SourcesFolder\Data_Process"))
data_floder += '/'
model_floder = os.getcwd()
model_floder = os.path.abspath(os.path.join(model_floder, 'Assets', "Scrips\Work\py\model\model_output"))
model_floder += '/'

validation_split = 0.20
verbose = 0  # 不输出epoch进度条


# _NN_model.h5
# _CNN_model.h5


def model_finetune(object_model, data_source, loss, epoch_num):
    (X_train, X_test, Y_train, Y_test, labels) = get_data(data_source=data_source)
    model_path = model_floder + "_" + object_model + "2D" + "_model.h5"
    model = tf.keras.models.load_model(model_path)

    for i in range(len(model.layers) - 1):
        model.layers[i].trainable = False
    # compile
    model.compile(optimizer='adam', loss=loss, metrics=[tf.keras.metrics.SparseCategoricalAccuracy()])

    # fit
    model.fit(X_train, Y_train, epochs=epoch_num, validation_split=validation_split, verbose=verbose)

    # model save
    model.save(model_path)

    print(len(X_train), len(X_test), len(Y_train), len(Y_test)) #临时调试
    # print message
    if verbose:
        print(len(X_train), len(X_test), len(Y_train), len(Y_test))
        print(len(model.layers))
        model.summary()

    # model evaluate
    test_loss, test_acc = model.evaluate(X_test, Y_test, verbose=verbose)
    print('\nTest accuracy:', test_acc)

    # predit
    y_predit_label = []
    y_predit_val = model.predict(X_test, verbose=verbose)
    y_predit_index = np.argmax(y_predit_val, axis=1)
    for i in range(len(y_predit_index)):
        label = labels[y_predit_index[i]]
        y_predit_label.append(label)

    return y_predit_label


def model_pretrain(data_source,
                   training_cycles, optimizer, learning_rate, loss,
                   vaildation_spilt, batch_size, fit_generator,
                   features_num,
                   conv1dpool_layer0, conv1dpool_layer1, conv1dpool_layer2,
                   classes_num):
    conv1dpool_layer0_filiters_num = conv1dpool_layer0[0]
    conv1dpool_layer0_kernel_size = conv1dpool_layer0[1]
    conv1dpool_layer0_activation = conv1dpool_layer0[2]
    conv1dpool_layer0_pooling_size = conv1dpool_layer0[3]
    conv1dpool_layer1_filiters_num = conv1dpool_layer1[0]
    conv1dpool_layer1_kernel_size = conv1dpool_layer1[1]
    conv1dpool_layer1_activation = conv1dpool_layer1[2]
    conv1dpool_layer1_pooling_size = conv1dpool_layer1[3]
    conv1dpool_layer2_filiters_num = conv1dpool_layer2[0]
    conv1dpool_layer2_kernel_size = conv1dpool_layer2[1]
    conv1dpool_layer2_activation = conv1dpool_layer2[2]
    conv1dpool_layer2_pooling_size = conv1dpool_layer2[3]

    (X_train, X_test, Y_train, Y_test, labels) = get_data(data_source=data_source)
    print(len(X_train), len(X_test), len(Y_train), len(Y_test))  # 临时调试
    model_path = model_floder + "_" + 'CNN' + "2D" + "_model.h5"

    model = tf.keras.Sequential()

    model.add(layers.Conv2D(filters=conv1dpool_layer0_filiters_num,
                            kernel_size=(
                                conv1dpool_layer0_kernel_size, conv1dpool_layer0_kernel_size),
                            padding='same', strides=1, activation=conv1dpool_layer0_activation,
                            input_shape=(
                                X_train.shape[1], X_train.shape[2], X_train.shape[3])))
    model.add(
        layers.MaxPool2D(pool_size=(conv1dpool_layer0_pooling_size, conv1dpool_layer0_pooling_size),
                         padding='same'))
    # 添加dropout层简化模型
    model.add(layers.Dropout(0.4))

    model.add(layers.Conv2D(filters=conv1dpool_layer1_filiters_num,
                            kernel_regularizer=tf.keras.regularizers.l2(0.01),
                            kernel_size=(
                                conv1dpool_layer1_kernel_size, conv1dpool_layer1_kernel_size),
                            padding='same', strides=1, activation=conv1dpool_layer1_activation))
    model.add(
        layers.MaxPool2D(pool_size=(conv1dpool_layer1_pooling_size, conv1dpool_layer1_pooling_size),
                         padding='same'))

    model.add(layers.Conv2D(filters=conv1dpool_layer2_filiters_num,
                            kernel_size=(
                                conv1dpool_layer2_kernel_size, conv1dpool_layer2_kernel_size),
                            padding='same', strides=1, activation=conv1dpool_layer2_activation))
    model.add(
        layers.MaxPool2D(pool_size=(conv1dpool_layer2_pooling_size, conv1dpool_layer2_pooling_size),
                         padding='same'))

    model.add(layers.Flatten())

    model.add(layers.Dense(classes_num, activation='softmax'))

    # optimizer
    if optimizer == 'Adam':
        optimizer = tf.keras.optimizers.Adam(learning_rate)
    elif optimizer == 'Adadelta':
        optimizer = tf.keras.optimizers.Adadelta(learning_rate)
    elif optimizer == 'Adafactor':
        optimizer = tf.keras.optimizers.Adafactor(learning_rate)
    elif optimizer == 'Adagrad':
        optimizer = tf.keras.optimizers.Adagrad(learning_rate)
    elif optimizer == 'AdamW':
        optimizer = tf.keras.optimizers.AdamW(learning_rate)
    elif optimizer == 'Adamax':
        optimizer = tf.keras.optimizers.Adamax(learning_rate)
    elif optimizer == 'Nadam':
        optimizer = tf.keras.optimizers.Nadam(learning_rate)
    elif optimizer == 'RMSprop':
        optimizer = tf.keras.optimizers.RMSprop(learning_rate)
    elif optimizer == 'SGD':
        optimizer = tf.keras.optimizers.SGD(learning_rate)

    # losses
    if loss == 'SparseCategoricalCrossentropy':
        loss = tf.keras.losses.SparseCategoricalCrossentropy()
    elif loss == 'BinaryCrossentropy':
        loss = tf.keras.losses.BinaryCrossentropy()
    elif loss == 'MeanSquaredError':
        loss = tf.keras.losses.MeanSquaredError()

    # compile
    model.compile(optimizer=optimizer,
                  loss=loss,
                  metrics=[tf.keras.metrics.SparseCategoricalAccuracy()])  # 'acc'

    # add earlystopping
    earlystop_callback = EarlyStopping(
        monitor='val_sparse_categorical_accuracy', min_delta=0.0001,
        patience=1)

    # fit
    model.fit(X_train, Y_train, epochs=training_cycles, batch_size=batch_size,
              validation_split=vaildation_spilt,
              verbose=verbose)  # callbacks=[earlystop_callback]

    # model save
    model.save(model_path)

    # print message
    if verbose:
        model.summary()

    # model evaluate
    test_loss, test_acc = model.evaluate(X_test, Y_test, verbose=verbose)
    print('\nTest accuracy:', test_acc)


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
    # get labels names
    labels = np.unique(test_data['label'].values)
    labels = sorted(labels, key=str.lower)

    # 文本标签转化为数字标签
    le = preprocessing.LabelEncoder()  # 获取一个LabelEncoder
    train_data['label'] = le.fit_transform(train_data['label'])  # 使用训练好的LabelEncoder对原数据进行编码
    test_data['label'] = le.fit_transform(test_data['label'])  # 使用训练好的LabelEncoder对原数据进行编码
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

    # 将数据1X120转成3X40
    X_train = X_train.reshape((train_data_len, int(features_num / 3), 3))
    X_test = X_test.reshape((test_data_len, int(features_num / 3), 3))

    # 对数据进行归一化处理
    X_train = normalize_fun.normalize_3dimensions(X_train)
    X_test = normalize_fun.normalize_3dimensions(X_test)

    # 增加灰度维度防止conv2d报错
    X_train = np.expand_dims(X_train, -1)
    X_test = np.expand_dims(X_test, -1)

    return X_train, X_test, Y_train, Y_test, labels


def model_fine_tune():
    mode = int(sys.argv[1])

    if mode == 1:
        data_source = sys.argv[2]
        loss = sys.argv[3]
        epoch_num = 50 if sys.argv[4] == '0' else int(sys.argv[4])
        print('mode=', mode,
              'data_source=', data_source,
              'loss=', loss,
              'epoch_num=', epoch_num)

        model_finetune(object_model='CNN',
                       data_source=data_source,
                       loss=loss,
                       epoch_num=epoch_num)
    elif mode == 2:
        data_source = 'r' if sys.argv[2] == '0' else sys.argv[2]
        training_cycles = 10 if sys.argv[3] == '0' else int(sys.argv[3])
        optimizer = "Adam" if sys.argv[4] == '0' else sys.argv[4]
        learning_rate = 0.001 if sys.argv[5] == '0' else float(sys.argv[5])
        loss = "SparseCategoricalCrossentropy" if sys.argv[6] == '0' else sys.argv[6]
        vaildation_spilt = 0.25 if sys.argv[7] == '0' else (float(sys.argv[7]) / 100)
        batch_size = 10 if sys.argv[8] == '0' else int(sys.argv[8])
        fit_generator = True if sys.argv[9] == "True" else False
        features_num = 6 if sys.argv[10] == '0' else int(sys.argv[10])
        conv1dpool_layer0_filters = 32 if sys.argv[11] == '0' else int(sys.argv[11])
        conv1dpool_layer0_kersize = 3 if sys.argv[12] == '0' else int(sys.argv[12])
        conv1dpool_layer0_act = "relu" if sys.argv[13] == '0' else sys.argv[13]
        conv1dpool_layer0_poolsize = 2 if sys.argv[14] == '0' else int(sys.argv[14])
        conv1dpool_layer1_filters = 64 if sys.argv[15] == '0' else int(sys.argv[15])
        conv1dpool_layer1_kersize = 3 if sys.argv[16] == '0' else int(sys.argv[16])
        conv1dpool_layer1_act = "relu" if sys.argv[17] == '0' else sys.argv[17]
        conv1dpool_layer1_poolsize = 2 if sys.argv[18] == '0' else int(sys.argv[18])
        conv1dpool_layer2_filters = 128 if sys.argv[19] == '0' else int(sys.argv[19])
        conv1dpool_layer2_kersize = 3 if sys.argv[20] == '0' else int(sys.argv[20])
        conv1dpool_layer2_act = "relu" if sys.argv[21] == '0' else sys.argv[21]
        conv1dpool_layer2_poolsize = 2 if sys.argv[22] == '0' else int(sys.argv[22])
        classes_num = 1 if sys.argv[23] == '0' else int(sys.argv[23])
        conv1dpool_layer0 = [conv1dpool_layer0_filters, conv1dpool_layer0_kersize,
                             conv1dpool_layer0_act, conv1dpool_layer0_poolsize]
        conv1dpool_layer1 = [conv1dpool_layer1_filters, conv1dpool_layer1_kersize,
                             conv1dpool_layer1_act, conv1dpool_layer1_poolsize]
        conv1dpool_layer2 = [conv1dpool_layer2_filters, conv1dpool_layer2_kersize,
                             conv1dpool_layer2_act, conv1dpool_layer2_poolsize]
        print("data_source=", data_source,
              "training_cycles=", training_cycles,
              "optimizer=", optimizer,
              "learning_rate=", learning_rate,
              "loss=", loss,
              "vaildation_spilt=", vaildation_spilt,
              "batch_size=", batch_size,
              "fit_generator=", fit_generator,
              "features_num=", features_num,
              "conv1dpool_layer0=", conv1dpool_layer0,
              "conv1dpool_layer1=", conv1dpool_layer1,
              "conv1dpool_layer2=", conv1dpool_layer2,
              "classes_num=", classes_num)

        model_pretrain(data_source=data_source,
                       training_cycles=training_cycles, optimizer=optimizer, learning_rate=learning_rate,
                       loss=loss,
                       vaildation_spilt=vaildation_spilt, batch_size=batch_size,
                       fit_generator=fit_generator,
                       features_num=features_num,
                       conv1dpool_layer0=conv1dpool_layer0,
                       conv1dpool_layer1=conv1dpool_layer1,
                       conv1dpool_layer2=conv1dpool_layer2,
                       classes_num=classes_num)


# 单独运行Python时用
# predit = model_finetune('CNN', "r", "SparseCategoricalCrossentropy", 50)
# print(predit)
# model_pretrain(data_source='r',
#                training_cycles=int(10), optimizer="Adam", learning_rate=float(0.001),
#                loss="SparseCategoricalCrossentropy",
#                vaildation_spilt=float(20.0 / 100), batch_size=int(10),
#                fit_generator=True,
#                features_num=int(3),  # 剔除角速度
#                conv1dpool_layer0=[int(32), int(3), 'relu', int(2)],
#                conv1dpool_layer1=[int(64), int(3), 'relu', int(2)],
#                conv1dpool_layer2=[int(128), int(3), 'relu', int(2)],
#                classes_num=int(3))

# Unity与Python联合运行时用
model_fine_tune()
