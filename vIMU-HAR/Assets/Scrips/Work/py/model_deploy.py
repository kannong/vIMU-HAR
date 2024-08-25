import sys
import warnings
import os

warnings.filterwarnings("ignore")
import tensorflow as tf

# model_floder = './model/model_output/'

model_floder = os.getcwd()
model_floder = os.path.abspath(os.path.join(model_floder, 'Assets', "Scrips\Work\py\model\model_output"))
model_floder += '/'


model_path = model_floder + "_" + 'CNN' + "2D" + "_model.h5"
model = tf.keras.models.load_model(model_path)
save_path = sys.argv[1]

# 转换模型
converter = tf.lite.TFLiteConverter.from_keras_model(model)
tflite_model = converter.convert()
open(save_path, "wb").write(tflite_model)
# Load TFLite model and allocate tensors.
interpreter = tf.lite.Interpreter(model_path="CNN_Model.tflite")
interpreter.allocate_tensors()

# Get input and output tensors.
input_details = interpreter.get_input_details()
output_details = interpreter.get_output_details()

# 输出信息
print('save path=', sys.argv[1])
print(input_details)
print(output_details)
