using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineChartScript : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject LineObject = null;

    private RectTransform imageRect = null;
    private List<ChartData> chartData = null;
    #endregion

    private void Awake()
    {
        this.imageRect = GetComponent<RectTransform>();
    }

    public void SetChartData(List<ChartData> chartData)
    {
        this.chartData = chartData;
        UpdateChart();
    }

    /// <summary>
    /// Draws the chart based on the current data and rect transform size.
    /// </summary>
    public void UpdateChart()
    {
        SetXLine(this.chartData);
        SetYline(this.chartData);
    }

    private void SetXLine(List<ChartData> data)
    {

    }

    private void SetYline(List<ChartData> data)
    {

    }
}
