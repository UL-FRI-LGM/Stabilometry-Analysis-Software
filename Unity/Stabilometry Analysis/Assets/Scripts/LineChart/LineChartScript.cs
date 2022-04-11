using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class LineChartScript : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private GameObject
        LineObject = null,
        dotObject = null;

    [SerializeField] private UILineRenderer[] lineRenderers = null;

    private RectTransform imageRect = null;
    private List<ChartData> chartData = null;
    #endregion

    private void Awake()
    {
        this.imageRect = GetComponent<RectTransform>();
        Debug.Log($"{imageRect.anchoredPosition} and width/height {imageRect.sizeDelta}");
    }

    private void Start()
    {
        List<ChartData> data = new List<ChartData>(new ChartData[10]);

        for (int i = 0; i < data.Count; i++)
        {
            float[] values = new float[4];
            values[0] = i;
            data[i] = new ChartData(values, null);
        }
        SetChartData(data);
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
        RectTransform drawingSpace = lineRenderers[0].GetComponent<RectTransform>();

        Vector2 valueSpaceSize = new Vector2(drawingSpace.rect.width / (float)(this.chartData.Count), drawingSpace.rect.height);

        float maxValue = GetLargestValue(this.chartData);
        DrawData(this.chartData, valueSpaceSize, drawingSpace, maxValue);
        //DrawData(this.chartData);
    }

    private void Update()
    {

    }

    private void DrawData(List<ChartData> data, Vector2 valueSpaceSize, RectTransform drawingSpace, float largestValue)
    {
        float leftmostPosition = 0;

        if (data.Count % 2 != 0)
            leftmostPosition -= valueSpaceSize.x * data.Count / 2;
        else
            leftmostPosition -= valueSpaceSize.x * (data.Count / 2 - 0.5f);

        Vector2 startingPosition = new Vector2(leftmostPosition, 0);

        //Debug.Log(data.Count);

        //Instantiate(dotObject, startingPosition, Quaternion.identity, transform);

        float valueCoverter = valueSpaceSize.y / largestValue;

        RectTransform lineRect = LineObject.GetComponent<RectTransform>();
        lineRect.sizeDelta = valueSpaceSize;

        RectTransform dotRect = dotObject.GetComponent<RectTransform>();

        //valueSpaceSize.x
        for (int i = 0; i < data.Count; i++)
        {
            lineRect.anchoredPosition = new Vector2(startingPosition.x + i * lineRect.rect.width, startingPosition.y);
            GameObject slice = Instantiate(LineObject, lineRenderers[0].transform);

            dotRect.anchoredPosition = new Vector2(0, data[i].values[0] * valueCoverter);
            Instantiate(dotObject, slice.transform);
        }
    }

    private float GetLargestValue(List<ChartData> data)
    {
        float largestValue = -1f;
        foreach (ChartData element in data)
            foreach (float value in element.values)
                if (value > largestValue)
                    largestValue = value;
        
        return largestValue;
    }
}
