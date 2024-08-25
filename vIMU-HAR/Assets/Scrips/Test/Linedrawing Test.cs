using UnityEngine;
#if INPUT_SYSTEM_ENABLED
using Input = XCharts.Runtime.InputHelper;
#endif
using XCharts.Runtime;

namespace XCharts.Example
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class LinedrawingTest : MonoBehaviour
    {
        void Awake()
        {
            AddData();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AddData();
            }
        }

        void AddData()
        {
            var chart = gameObject.GetComponent<SimplifiedLineChart>();
            if (chart == null)
            {
                chart = gameObject.AddComponent<SimplifiedLineChart>();
                chart.Init();
                chart.SetSize(580, 300);
            }

            //设置标题
            chart.EnsureChartComponent<Title>().show = true;
            chart.EnsureChartComponent<Title>().text = "Line Simple";
            chart.GetChartComponent<Title>().subText = "普通折线图";

            //设置提示框和图例是否显示
            chart.EnsureChartComponent<Tooltip>().show = true;
            chart.EnsureChartComponent<Legend>().show = true;

            //设置坐标轴
            var xAxis = chart.EnsureChartComponent<XAxis>();
            var yAxis = chart.EnsureChartComponent<YAxis>();
            xAxis.show = true;
            yAxis.show = true;
            xAxis.type = Axis.AxisType.Category;
            yAxis.type = Axis.AxisType.Value;

            xAxis.splitNumber = 10;
            xAxis.boundaryGap = true;

            //清空默认数据，添加Line类型的Serie用于接收数据
            chart.RemoveData();
            var serie1 = chart.AddSerie<SimplifiedLine>();
            var serie2 = chart.AddSerie<SimplifiedLine>();
            serie1.lineType = LineType.Smooth;
            serie2.lineType = LineType.Smooth;
            serie1.serieName = "random";
            serie2.serieName = "sinx";

            chart.RefreshChart();
            //添加20个数据
            for (int i = 0; i < 20; i++)
            {
                chart.AddXAxisData("x" + i);
                chart.AddData(0, Mathf.Sin(i));
                chart.AddData(1, Random.Range(10, 20));
            }
        }
    }
}