using Csv_Function;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XCharts.Runtime;

public class Data_Clear_Sub_Control : MonoBehaviour
{
    public Dropdown object_select_dp;
    public InputField window_index_if;
    public Text total_windows_t;
    public Text current_windows_index_t;
    public LineChart lineChart;
    public Toggle x_tg;
    public Toggle y_tg;
    public Toggle z_tg;
    private Serie serie_x, serie_y, serie_z;
    private int object_index;
    private int window_index;
    private int total_window_nums;
    private CsvFunction csvf = new CsvFunction("IMUSim");

    private void OnEnable()
    {
        // set object_select_dp options
        Set_Object_Selectdp();
    }

    // Start is called before the first frame update
    void Start()
    {
        // set object_select_dp options
        Set_Object_Selectdp();

        // set default para
        object_index = 0;
        window_index = 0;

        object_select_dp.value = object_index;
        window_index_if.text = window_index.ToString();
        // linechart init
        Linechar_Init();

        // read csv, get object window data
        Get_Object_Windowdata(object_index, window_index,
                              out total_window_nums,
                              out float[] drawing_datas_x,
                              out float[] drawing_datas_y,
                              out float[] drawing_datas_z);

        // reflesh para show
        total_windows_t.text = total_window_nums.ToString();
        current_windows_index_t.text = window_index.ToString();

        // darw linechart
        Linechar_Drawing(drawing_datas_x, drawing_datas_y, drawing_datas_z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Object_Change()
    {
        // change para
        object_index = object_select_dp.value;

        // read csv, get object window data
        Get_Object_Windowdata(object_index, window_index,
                              out total_window_nums,
                              out float[] drawing_datas_x,
                              out float[] drawing_datas_y,
                              out float[] drawing_datas_z);

        // reflesh para show
        total_windows_t.text = total_window_nums.ToString();
        current_windows_index_t.text = window_index.ToString();

        // darw linechart
        Linechar_Drawing(drawing_datas_x, drawing_datas_y, drawing_datas_z);

    }
    public void Window_Change()
    {
        // change para
        window_index = Convert.ToInt32(window_index_if.text);

        // read csv, get object window data
        Get_Object_Windowdata(object_index, window_index,
                              out total_window_nums,
                              out float[] drawing_datas_x,
                              out float[] drawing_datas_y,
                              out float[] drawing_datas_z);

        // reflesh para show
        total_windows_t.text = total_window_nums.ToString();
        current_windows_index_t.text = window_index.ToString();

        // darw linechart
        Linechar_Drawing(drawing_datas_x, drawing_datas_y, drawing_datas_z);

    }

    public void SeriexTG_Change()
    {
        //Debug.Log("x is on"+ x_tg.isOn);
        serie_x.show = x_tg.isOn;
    }

    public void SerieyTG_Change()
    {
        //Debug.Log("y is on" + y_tg.isOn);
        serie_y.show = y_tg.isOn;
    }

    public void SeriezTG_Change()
    {
        //Debug.Log("z is on" + z_tg.isOn);
        serie_z.show = z_tg.isOn;
    }

    public void Linechar_Init()
    {
        if (lineChart == null)
        {
            lineChart = gameObject.AddComponent<LineChart>();
            lineChart.Init();
            lineChart.SetSize(580, 300);
        }

        //设置标题
        lineChart.EnsureChartComponent<Title>().show = true;
        lineChart.EnsureChartComponent<Title>().text = "object" + object_select_dp.value;

        //设置提示框和图例是否显示
        lineChart.EnsureChartComponent<Tooltip>().show = true;
        lineChart.EnsureChartComponent<Legend>().show = true;

        //设置坐标轴
        var xAxis = lineChart.EnsureChartComponent<XAxis>();
        var yAxis = lineChart.EnsureChartComponent<YAxis>();
        xAxis.show = true;
        yAxis.show = true;
        xAxis.type = Axis.AxisType.Category;
        yAxis.type = Axis.AxisType.Value;

        xAxis.splitNumber = 10;
        xAxis.boundaryGap = true;

        //清空默认数据，添加Line类型的Serie用于接收数据
        lineChart.RemoveData();
        serie_x = lineChart.AddSerie<Line>("object" + object_select_dp.value + "_x");
        serie_x.symbol.show = false;
        serie_x.lineType = LineType.Smooth;
        serie_y = lineChart.AddSerie<Line>("object" + object_select_dp.value + "_y");
        serie_y.symbol.show = false;
        serie_y.lineType = LineType.Smooth;
        serie_z = lineChart.AddSerie<Line>("object" + object_select_dp.value + "_z");
        serie_z.symbol.show = false;
        serie_z.lineType = LineType.Smooth;
        serie_x.itemStyle.numericFormatter = "F3";
        serie_y.itemStyle.numericFormatter = "F3";
        serie_z.itemStyle.numericFormatter = "F3";

        //在下一帧刷新整个图表
        lineChart.RefreshChart();

    }

    public void Linechar_Drawing(float[] x, float[]y, float[]z)
    {
        //清空默认数据，添加Line类型的Serie用于接收数据
        lineChart.RemoveData();
        serie_x = lineChart.AddSerie<Line>("object" + object_select_dp.value + " x");
        serie_x.symbol.show = false;
        serie_x.lineType = LineType.Smooth;
        serie_y = lineChart.AddSerie<Line>("object" + object_select_dp.value + " y");
        serie_y.symbol.show = false;
        serie_y.lineType = LineType.Smooth;
        serie_z = lineChart.AddSerie<Line>("object" + object_select_dp.value + " z");
        serie_z.symbol.show = false;
        serie_z.lineType = LineType.Smooth;
        serie_x.itemStyle.numericFormatter = "F3";
        serie_y.itemStyle.numericFormatter = "F3";
        serie_z.itemStyle.numericFormatter = "F3";

        serie_x.show = x_tg.isOn;
        serie_y.show = y_tg.isOn;
        serie_z.show = z_tg.isOn;

        //在下一帧刷新整个图表
        lineChart.RefreshChart();

        for (int i = 0; i < x.Length; i++)
        {
            lineChart.AddXAxisData(i.ToString());
            lineChart.AddData(0, x[i]);
            lineChart.AddData(1, y[i]);
            lineChart.AddData(2, z[i]);
        }
    }

    private void Get_Object_Windowdata(int object_index,int window_index,
                                          out int total_window_nums,
                                          out float[] drawing_datas_x,
                                          out float[] drawing_datas_y,
                                          out float[] drawing_datas_z)
    {
        string[] object_window_data = new string[Data_interception_Control.window_size];
        string raw_data_path = csvf.BinSourcesFolder +
                               Main_Canvas_Control.avatar_name[object_index] + "_" +
                               Main_Canvas_Control.joint_name[object_index] + "_cleared_data.csv";

        // read csv
        Encoding utf = Encoding.GetEncoding("UTF-8");
        string InfoConfig = File.ReadAllText(raw_data_path, utf);
        InfoConfig = InfoConfig.Replace("\r", "");
        InfoConfig = InfoConfig.Replace("\"", "");
        string[] CSVDatas = InfoConfig.Split('\n');
        //Debug.Log("CSVDatas length=" + CSVDatas.Length);

        // get object window data
        for (int i = 0; i < Data_interception_Control.window_size; i++)
        {
            object_window_data[i] = CSVDatas[Data_interception_Control.window_size * window_index + i + 1];
            //Debug.Log("object window data= " + i + object_window_data[i]);
        }

        // get current para
        total_window_nums = (CSVDatas.Length - 1) / Data_interception_Control.window_size;

        // get drawing datas
        drawing_datas_x = new float[Data_interception_Control.window_size];
        drawing_datas_y = new float[Data_interception_Control.window_size];
        drawing_datas_z = new float[Data_interception_Control.window_size];
        for (int i = 0; i < Data_interception_Control.window_size; i++)
        {
            string[] one_row = object_window_data[i].Split(',');
            try
            {
                // Convert the strings to floats
                drawing_datas_x[i] = float.Parse(one_row[0]);
                drawing_datas_y[i] = float.Parse(one_row[1]);
                drawing_datas_z[i] = float.Parse(one_row[2]);
            }
            catch { }

        }

    }

    public void Set_Object_Selectdp()
    {
        // get options 
        List<Dropdown.OptionData> options = object_select_dp.options;
        options.Clear();

        // modify options
        for (int i = 0; i < Main_Canvas_Control.object_num; i++)
        {
            options.Add(new Dropdown.OptionData("Object"+i.ToString()));

        }
        //Debug.Log("object num=" + Main_Canvas_Control.object_num);

        // set target JNT dropdown options
        object_select_dp.options = options;
    }


}
