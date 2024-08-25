import os
import pandas as pd

# floder = 'E:/unity_pro/My_workV0.1/Assets/StreamingAssets/SourcesFolder/Data_Process/'
# floder = r"../../../StreamingAssets/SourcesFolder/Data_Process/"

floder = os.getcwd()  # 注意unity联合运行与单独运行Python的工作路径是完全不一样的，注意一下
floder = os.path.abspath(os.path.join(floder, "..", "..", "..", "StreamingAssets\SourcesFolder\Data_Process"))
floder += '/'
file_path = floder + 'raw_all_train_data.csv'
print(file_path)
data = pd.read_csv(file_path)
print('finish')
