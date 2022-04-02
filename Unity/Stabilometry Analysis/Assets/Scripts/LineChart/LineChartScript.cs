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
        RectTransform drawingSpace = lineRenderers[0].GetComponent<RectTransform>();
        Vector2 valueSpaceSize = new Vector2(drawingSpace.rect.width / this.chartData.Count, drawingSpace.rect.height);

        SetXLine(this.chartData, valueSpaceSize.x, drawingSpace);
        SetYline(this.chartData);
        DrawData(this.chartData);
    }

    private void Update()
    {

    }

    private void SetXLine(List<ChartData[]> data, float elementWidth, RectTransform drawingSpace)
    {
        float leftmostPosition = drawingSpace.transform.localPosition.y;

        if (data.Count % 2 != 0)
            leftmostPosition -= elementWidth * data.Count / 2;
        else
            leftmostPosition -= elementWidth * (data.Count / 2 + 0.5f);

        Vector2 startingPosition = new Vector2(leftmostPosition, drawingSpace.transform.localPosition.y);

        for (int i = 0; i < data.Count; i++)
            Debug.Log(startingPosition.x + i * elementWidth);
    }

    private void SetYline(List<ChartData[]> data)
    {
        float largestValue = -1f;
        foreach (ChartData[] group in data)
        {
            foreach (ChartData elemnt in group)
            {
                if (elemnt.value > largestValue)
                    largestValue = elemnt.value;
            }
        }


    }

    private void DrawData(List<ChartData[]> data)
    {
        
        


    }
}
