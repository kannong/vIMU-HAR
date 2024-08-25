import numpy as np
import pandas as pd
from sklearn import preprocessing

mode = 0
floder = 'E:/unity_pro/My_workV0.1/Assets/StreamingAssets/SourcesFolder/Data_Process/'


if mode == 0:
    sex = pd.Series(["male", "female", "female", "male"])
    le = preprocessing.LabelEncoder()  # 获取一个LabelEncoder
    le = le.fit(["male", "female"])  # 训练LabelEncoder, 把male编码为0，female编码为1
    sex = le.transform(sex)  # 使用训练好的LabelEncoder对原数据进行编码
    print(sex)
else:
    labels = ['knee','ankle','sidetoside']
    file_path = floder + 'test_data.csv'
    data = pd.read_csv(file_path)
    le = preprocessing.LabelEncoder()  # 获取一个LabelEncoder
    le = le.fit(labels)  # 训练LabelEncoder
    data['label'] = le.transform(data['label'])  # 使用训练好的LabelEncoder对原数据进行编码
    print(data['label'])



