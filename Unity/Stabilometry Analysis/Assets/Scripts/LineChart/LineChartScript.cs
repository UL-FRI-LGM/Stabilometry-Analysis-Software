using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using TMPro;

public class LineChartScript : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject LineObject = null;

    [SerializeField] private GameObject[] 
        dotObjects = null,
        yAxisLines = null;

    [SerializeField] private TextMeshProUGUI[] valueTexts = null;

    [SerializeField] private UILineRenderer[] lineRenderers = null;
    [SerializeField] private GameObject dotsHolder = null;

    private RectTransform chartRect = null;
    private List<ChartData> chartData = null;

    private RectTransform[] dotRects = null;

    private List<GameObject> spawnedObjects = null;

    #endregion

    private void Awake()
    {
        spawnedObjects = new List<GameObject>();

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
            float[] values = new float[4] { i, i * 2, i / 2f, i * i };
            if (i % 3 == 0)
                values[1] = -1;
            
            data[i] = new ChartData(values, null);
        }
        SetChartData(data);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RespawnChart();
        }
    }

    private void RespawnChart()
    {
        foreach (GameObject spawnedObject in spawnedObjects)
            Destroy(spawnedObject);

        SetChartData(this.chartData);

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
        float modifiedMaxValue = ModifyMaxValue(maxValue);

        SetYAxis(modifiedMaxValue, drawingSpace);
        DrawData(this.chartData, valueSpaceSize, this.chartRect, modifiedMaxValue);
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

        FillLineRenderers(data, startingPosition.x, valueConverter, lineRect);

        for (int i = 0; i < data.Count; i++)
        {
            // Spawn slice objects
            lineRect.anchoredPosition = new Vector2(startingPosition.x + i * lineRect.rect.width, startingPosition.y);
            GameObject slice = Instantiate(LineObject, drawingSpace.transform);

            spawnedObjects.Add(slice);

            float xPosition = startingPosition.x + i * lineRect.rect.width;

            SpawnDots(data[i], xPosition, valueConverter, i);
        }
    }

    private void FillLineRenderers(List<ChartData> data, float leftmostPosition, float valueConverter, RectTransform lineRect)
    {
        List<Vector2>[] drawingData = PrepareDataForLineRenderers(data, leftmostPosition, valueConverter, lineRect);

        for (int i = 0; i < lineRenderers.Length; i++)
        {
            Vector2[] points = new Vector2[drawingData[i].Count];
            for (int k = 0; k < drawingData[i].Count; k++)
                points[k] = drawingData[i][k];

            lineRenderers[i].Points = points;
        } 
    }

    private List<Vector2>[] PrepareDataForLineRenderers(List<ChartData> data,float leftmostPosition, float valueConverter, RectTransform lineRect)
    {
        List<Vector2>[] result = new List<Vector2>[4];

        for (int i = 0; i < result.Length; i++)
            result[i] = new List<Vector2>();

        for (int i = 0; i < data.Count; i++)
        {
            for (int k = 0; k < data[i].values.Length; k++)
            {
                float xPosition = leftmostPosition + i * lineRect.rect.width;
                float yPositon = data[i].values[k] * valueConverter;

                float previousValue = (i <= 0) ? -1 : data[i - 1].values[k];
                float nextValue = (i >= data.Count - 1) ? -1 : data[i + 1].values[k];

                //Debug.Log($"{i} {previousValue} {nextValue}");

                switch(AddNTimes(previousValue, data[i].values[k], nextValue))
                {
                    case (1):
                        result[k].Add(new Vector2(xPosition, yPositon));
                        break;
                    case (2):                        
                        result[k].Add(new Vector2(xPosition, yPositon));
                        result[k].Add(new Vector2(xPosition, yPositon));
                        break;
                }
            }
        }

        return result;
    }

    private int AddNTimes(float previous, float current, float next)
    {
        if (current < 0)
            return 0;

        else if (previous >= 0 && next >= 0)
            return 2;
        
        // else

        return 1;
    }

    private void SpawnDots(ChartData data, float xPosition, float valueConverter, int number)
    {
        for (int i = 0; i < data.values.Length; i++)
        {
            if (data.values[i] >= 0)
            {
                Vector2 newPosition = new Vector2(xPosition, data.values[i] * valueConverter);
                dotRects[i].anchoredPosition = newPosition;
                GameObject dot = Instantiate(dotObjects[i], dotsHolder.transform);

                spawnedObjects.Add(dot);
            }
        }
    }

    private void SetYAxis(float highestValue, RectTransform drawingSpace)
    {
        float increment = highestValue/(valueTexts.Length - 1);

        for (int i = 0; i < valueTexts.Length; i++)
            valueTexts[i].text = $"{i * increment}";
        
        RepositionLines(drawingSpace);
    }

    private void RepositionLines(RectTransform drawingSpace)
    {
        float increase =  drawingSpace.rect.height / 4f;
        Debug.Log(drawingSpace.rect.height);
        for (int i = 0; i < yAxisLines.Length; i++) 
        {
            float newYPosition = drawingSpace.localPosition.y + (i - yAxisLines.Length/2) * increase;

            RectTransform rect = yAxisLines[i].GetComponent<RectTransform>();

            rect.localPosition= new Vector2(rect.localPosition.x, newYPosition);
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

    private float ModifyMaxValue(float maxValue)
    {
        if (maxValue <= 1)
            return 1;

        // else
        if (maxValue <= 2)
            return 2;

        //else
        if (maxValue % 4 == 0)
            return maxValue;

        //else

        return (Mathf.Floor(maxValue / 4) + 1)  * 4;
    }
}
