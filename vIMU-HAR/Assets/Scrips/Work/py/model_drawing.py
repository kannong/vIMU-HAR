import warnings

from keras.src.callbacks import EarlyStopping

warnings.filterwarnings("ignore")
import numpy as np
import pandas as pd
from comm import normalize_fun, csv_fun
import matplotlib.pyplot as plt
import tensorflow as tf
from tensorflow.keras import layers

model_floder = './model/model_output/'
dot_img_file = './picture/CNN1D_3classs.png'
use_2d = 0

def model_drawing(object_model):
    model_path = model_floder + "_" + object_model + "_model.h5"
    if use_2d:
        model_path = model_floder + "_" + object_model + "2D" + "_model.h5"
    model = tf.keras.models.load_model(model_path)
    tf.keras.utils.plot_model(model, to_file=dot_img_file, show_shapes=True)

model_drawing("CNN")