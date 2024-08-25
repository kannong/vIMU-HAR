import numpy as np
import matplotlib.pyplot as plt

demision = 1

if demision == 2:
    # 生成一个二维数组作为示例数据
    data = np.random.random((256, 256))

    # 计算二维傅里叶变换
    fft = np.fft.fft2(data)
    fft[0] = 0  # 去除直流分量
    print('fft.len=', len(fft))

    # 计算频谱图
    # magnitude_spectrum = 20 * np.log(np.abs(fft))
    magnitude_spectrum = 20*np.abs(fft)

    # 绘制原始图像和频谱图
    fig, (ax1, ax2) = plt.subplots(1, 2)
    ax1.imshow(data, cmap='gray')
    ax1.set_title('Original Image')
    ax2.imshow(magnitude_spectrum, cmap='gray')
    ax2.set_title('Magnitude Spectrum')
    plt.show()

else:
    # 生成一个长度为256的实数组
    x = np.linspace(0, 2 * np.pi, 256)
    y = np.sin(x) + 0.5 * np.sin(3 * x) + 0.2 * np.sin(5 * x)

    # 计算傅里叶变换
    y_fft = np.fft.fft(y)
    y_fft[0] = 0  # 去除直流分量
    print('y_fft.len=', len(y_fft))

    # 计算相应的频率
    freq = np.fft.fftfreq(len(y), x[1] - x[0])

    # 绘制原始信号
    plt.subplot(211)
    plt.plot(x, y)

    # 绘制频谱图
    plt.subplot(212)
    plt.plot(freq, np.abs(y_fft))

    plt.show()
