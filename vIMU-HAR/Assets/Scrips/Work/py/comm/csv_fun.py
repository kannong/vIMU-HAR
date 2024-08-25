import csv


def csv_init(file_path='save_csv.csv', csv_head=[]):
    '''
    函数功能: 创建csv文件
    :param file_path: 文件路径
    :param csv_head: 表头
    :return: 新建的csv对象
    '''
    f = open(file_path, 'w', encoding='utf-8', newline="")
    csv_write = csv.writer(f)
    if len(csv_head):
        csv_write.writerow(csv_head)

    return csv_write

def csv_append_data(file_path, data_row):
    '''
    函数功能: 往存在的csv文件中追加一行
    :param file_path: 文件路径
    :param data_rows: 数据
    :param data_len: 行数
    :return: 无
    '''
    f = open(file_path, 'a+', encoding='utf-8', newline="")
    csv_write = csv.writer(f)
    if len(data_row):
        csv_write.writerow(data_row)

def csv_append_datas(file_path, data_rows, data_len):
    '''
    函数功能: 往存在的csv文件中追加多行
    :param file_path: 文件路径
    :param data_rows: 数据
    :param data_len: 行数
    :return: 无
    '''
    f = open(file_path, 'a+', encoding='utf-8', newline="")
    csv_write = csv.writer(f)
    if len(data_rows):
        for i in range(data_len):
            csv_write.writerow(data_rows[i])

def get_csv_header(file_path):
    '''
    函数功能：获取csv文件的表头
    :param file_path: 文件路径
    :return: csv文件表头
    '''
    with open(file_path, 'r', encoding='utf-8') as file:
        csv_reader = csv.reader(file)
        header = next(csv_reader)
    # print(header)
    return header

def get_csv_len(file_path):
    '''
    函数功能：统计csv文件的行数（包含表头）
    :param file_path: 文件路径
    :return: csv文件行数
    '''
    total_lines = sum(1 for line in open(file_path))
    # print('The total lines is ', total_lines)
    return total_lines
