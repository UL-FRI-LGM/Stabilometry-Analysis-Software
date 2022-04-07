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
        Debug.Log(drawingSpace.rect);
        Vector2 valueSpaceSize = new Vector2(drawingSpace.rect.width / (this.chartData.Count), drawingSpace.rect.height);

        //SetYline(this.chartData);
        DrawData(this.chartData, valueSpaceSize, drawingSpace);
        //DrawData(this.chartData);
    }

    private void Update()
    {

    }

    private void DrawData(List<ChartData> data, Vector2 valueSpaceSize, RectTransform drawingSpace)
    {
        float leftmostPosition = drawingSpace.transform.position.x;

        //if (data.Count % 2 != 0)
        //    leftmostPosition -= valueSpaceSize.x* data.Count / 2;
        //else
        //    leftmostPosition -= valueSpaceSize.x * (data.Count / 2 - 0.5f);

        Vector2 startingPosition = new Vector2(leftmostPosition, drawingSpace.transform.position.y);

        Debug.Log(data.Count);

        //Instantiate(dotObject, startingPosition, Quaternion.identity, transform);

        LineObject.GetComponent<RectTransform>().sizeDelta = valueSpaceSize;

        for (int i = 0; i < data.Count; i++)
        {
            Vector2 position = new Vector2(startingPosition.x + i * valueSpaceSize.x, transform.position.y);
            //Instantiate(LineObject, position, Quaternion.identity, transform);
            Instantiate(LineObject, position, Quaternion.identity, lineRenderers[0].transform);
        }
    }

    private void SetYline(List<ChartData> data)
    {
        float largestValue = -1f;
        foreach (ChartData element in data)
            foreach (float value in element.values)
                if (value > largestValue)
                    largestValue = value;

    }

    //private void DrawData(List<ChartData> data)
    //{




    //}
}
