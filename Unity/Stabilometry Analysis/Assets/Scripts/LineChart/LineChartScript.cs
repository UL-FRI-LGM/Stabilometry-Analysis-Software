using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class LineChartScript : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject LineObject = null;

    [SerializeField] private UILineRenderer[] lineRenderers = null;

    private RectTransform imageRect = null;
    private List<ChartData[]> chartData = null;
    #endregion

    private void Awake()
    {
        this.imageRect = GetComponent<RectTransform>();
        Debug.Log($"{imageRect.anchoredPosition} and width/height {imageRect.sizeDelta}");
    }

    private void Start()
    {
    }

    public void SetChartData(List<ChartData[]> chartData)
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
        DrawData(this.chartData);
    }

    private void Update()
    {
     
    }

    private void SetXLine(List<ChartData[]> data)
    {

    }

    private void SetYline(List<ChartData[]> data)
    {

    }

    private void DrawData(List<ChartData[]> data)
    {
        RectTransform drawingSpace = lineRenderers[0].GetComponent<RectTransform>();
        Vector2 valueSpaceSize = new Vector2(drawingSpace.rect.width / data.Count, drawingSpace.rect.height);


    }
}
