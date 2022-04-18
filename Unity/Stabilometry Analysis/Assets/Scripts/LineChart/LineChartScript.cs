﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using TMPro;

public class LineChartScript : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject LineObject = null;

    [SerializeField] private GameObject[] dotObjects = null;
    [SerializeField] private TextMeshProUGUI[] valueTexts = null;

    [SerializeField] private UILineRenderer[] lineRenderers = null;
    [SerializeField] private GameObject dotsHolder = null;

    private RectTransform chartRect = null;
    private List<ChartData> chartData = null;

    private RectTransform[] dotRects = null;

    #endregion

    private void Awake()
    {
        this.chartRect = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();

        this.dotRects = new RectTransform[dotObjects.Length];

        for (int i = 0; i < this.dotObjects.Length; i++)
            this.dotRects[i] = this.dotObjects[i].GetComponent<RectTransform>();
    }

    private void Start()
    {
        List<ChartData> data = new List<ChartData>(new ChartData[10]);

        for (int i = 0; i < data.Count; i++)
        {
            float[] values = new float[4] { i, i * 2, i / 2f, i *i };
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
        SetYAxis(maxValue);
        DrawData(this.chartData, valueSpaceSize, this.chartRect, maxValue);
    }

    private void DrawData(List<ChartData> data, Vector2 valueSpaceSize, RectTransform drawingSpace, float largestValue)
    {
        float leftmostPosition = 0;

        if (data.Count % 2 != 0)
            leftmostPosition -= valueSpaceSize.x * data.Count / 2;
        else
            leftmostPosition -= valueSpaceSize.x * (data.Count / 2 - 0.5f);

        Vector2 startingPosition = new Vector2(leftmostPosition, 0);


        float valueConverter = valueSpaceSize.y / largestValue;

        RectTransform lineRect = LineObject.GetComponent<RectTransform>();

        lineRect.sizeDelta = new Vector2(valueSpaceSize.x, drawingSpace.rect.height);

        SpawnElements(data, drawingSpace, startingPosition, valueConverter, lineRect);
    }

    private void SpawnElements(List<ChartData> data, RectTransform drawingSpace, Vector2 startingPosition, float valueConverter, RectTransform lineRect)
    {

        for (int i = 0; i < lineRenderers.Length; i++)
            lineRenderers[i].Points = new Vector2[data.Count];

        for (int i = 0; i < data.Count; i++)
        {
            // Spawn slice objects
            lineRect.anchoredPosition = new Vector2(startingPosition.x + i * lineRect.rect.width, startingPosition.y);
            GameObject slice = Instantiate(LineObject, drawingSpace.transform);

            float xPosition = startingPosition.x + i * lineRect.rect.width;

            SpawnDots(data[i], xPosition, valueConverter, i);
        }
    }

    private void SpawnDots(ChartData data, float xPosition, float valueConverter, int number)
    {
        for (int i = 0; i < data.values.Length; i++)
        {
            if (data.values[i] >= 0)
            {
                Vector2 newPosition = new Vector2(xPosition, data.values[i] * valueConverter);
                dotRects[i].anchoredPosition = newPosition;
                Instantiate(dotObjects[i], dotsHolder.transform);

                lineRenderers[i].Points[number] = new Vector2(xPosition, data.values[i] * valueConverter);
            }
        }
    }

    private void SetYAxis(float highestValue)
    {
        float increment = highestValue/(valueTexts.Length - 1);

        for (int i = 0; i < valueTexts.Length; i++)
            valueTexts[i].text = $"{i * increment}";

    }

    ///// <summary>
    ///// Converts the value to the appropriate position. Min values are presumed to be 0.
    ///// </summary>
    ///// <param name="value"></param>
    ///// <param name="maxValue"></param>
    ///// <param name="newMin"></param>
    ///// <param name="newMax"></param>
    ///// <returns></returns>
    //private float ConvertPosition(float value, float maxValue, float newMin, float newMax)
    //{
    //    return value * (newMax - newMin) / maxValue + newMin;
    //}

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
