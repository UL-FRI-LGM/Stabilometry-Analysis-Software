using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using TMPro;
using static Parameter;

public class LineChartScript : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private GameObject
        LineObject = null,
        DataLine = null;

    [SerializeField]
    private GameObject[]
        dotObjects = null,
        yAxisLines = null;

    [SerializeField] private TextMeshProUGUI title = null;

    [SerializeField] private TextMeshProUGUI[] valueTexts = null;

    [SerializeField] private UILineRenderer[] lineRenderers = null;
    [SerializeField] private GameObject dotsHolder = null;

    private RectTransform chartRect = null;
    private List<ChartData> chartData = null;

    private RectTransform[] dotRects = null;

    private List<GameObject> spawnedObjects = null;

    private Parameter chosenParameter = Parameter.SWAY_PATH_TOTAL;

    #endregion

    private void Awake()
    {
        spawnedObjects = new List<GameObject>();

        this.chartRect = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();

        this.dotRects = new RectTransform[dotObjects.Length];

        for (int i = 0; i < this.dotObjects.Length; i++)
            this.dotRects[i] = this.dotObjects[i].GetComponent<RectTransform>();
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

        SetChartData(this.chartData, this.chosenParameter);

    }

    public void SetChartTitle(Parameter chosenParameter)
    {
        this.chosenParameter = chosenParameter;
        SetTitle(chosenParameter);
    }

    public void SetChartData(List<ChartData> chartData, Parameter chosenParameter)
    {
        this.chartData = chartData;
        this.chosenParameter = chosenParameter;

        SetTitle(chosenParameter);
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

    /// <summary>
    /// Prepares the data to be spawned.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="leftmostPosition"></param>
    /// <param name="valueConverter"></param>
    /// <param name="lineRect"></param>
    /// <returns></returns>
    private List<Vector2>[] PrepareDataForLineRenderers(List<ChartData> data, float leftmostPosition, float valueConverter, RectTransform lineRect)
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

                switch (AddNTimes(previousValue, data[i].values[k], nextValue))
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

    /// <summary>
    /// Used for determining if a line should be added.
    /// </summary>
    /// <param name="previous"></param>
    /// <param name="current"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    private int AddNTimes(float previous, float current, float next)
    {
        if (current < 0)
            return 0;

        else if (previous >= 0 && next >= 0)
            return 2;

        // else

        return 1;
    }

    /// <summary>
    /// Spawn dots that represent values.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="xPosition"></param>
    /// <param name="valueConverter"></param>
    /// <param name="number"></param>
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

    /// <summary>
    /// Set values for Y bar.
    /// </summary>
    /// <param name="highestValue"></param>
    /// <param name="drawingSpace"></param>
    private void SetYAxis(float highestValue, RectTransform drawingSpace)
    {
        float increment = highestValue / (valueTexts.Length - 1);

        for (int i = 0; i < valueTexts.Length; i++)
            valueTexts[i].text = $"{i * increment}";

        RepositionLines(drawingSpace);
    }

    /// <summary>
    /// Reposition lines for the y axis.
    /// </summary>
    /// <param name="drawingSpace"></param>
    private void RepositionLines(RectTransform drawingSpace)
    {
        float increase = drawingSpace.rect.height / 4f;
        //Debug.Log(drawingSpace.rect.height);
        for (int i = 0; i < yAxisLines.Length; i++)
        {
            float newYPosition = drawingSpace.localPosition.y + (i - yAxisLines.Length / 2) * increase;

            RectTransform rect = yAxisLines[i].GetComponent<RectTransform>();

            rect.localPosition = new Vector2(rect.localPosition.x, newYPosition);
        }
    }

    /// <summary>
    /// Gets the largest value for calculating y axis values
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private float GetLargestValue(List<ChartData> data)
    {
        float largestValue = -1f;
        foreach (ChartData element in data)
            foreach (float value in element.values)
                if (value > largestValue)
                    largestValue = value;

        return largestValue;
    }

    /// <summary>
    /// Makes the highest value better for division with values.
    /// </summary>
    /// <param name="maxValue"></param>
    /// <returns></returns>
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

        return (Mathf.Floor(maxValue / 4) + 1) * 4;
    }

    /// <summary>
    /// Sets the title of the chart.
    /// </summary>
    /// <param name="parameter"></param>
    private void SetTitle(Parameter parameter)
    {
        switch (parameter)
        {
            case (SWAY_PATH_TOTAL):
                title.text = "Sway Path Total [cm]";
                break;
            case (SWAY_PATH_AP):
                title.text = "Sway Path AP [cm]";
                break;
            case (SWAY_PATH_ML):
                title.text = "Sway Path ML [cm]";
                break;
            case (MEAN_SWAY_VELOCITY_TOTAL):
                title.text = "Mean Sway Velocity Total [cm/s]";
                break;
            case (MEAN_SWAY_VELOCITY_AP):
                title.text = "Mean Sway Velocity AP [cm/s]";
                break;
            case (MEAN_SWAY_VELOCITY_ML):
                title.text = "Mean Sway Velocity ML [cm/s]";
                break;
            case (SWAY_AVERAGE_AMPLITUDE_AP):
                title.text = "Sway Average Amplitude AP [cm]";
                break;
            case (SWAY_AVERAGE_AMPLITUDE_ML):
                title.text = "Sway Average Amplitude ML [cm]";
                break;
            case (SWAY_MAXIMAL_AMPLITUDE_AP):
                title.text = "Sway Maximal Amplitude AP [cm]";
                break;
            case (SWAY_MAXIMAL_AMPLITUDE_ML):
                title.text = "Sway Maximal Amplitude ML [cm]";
                break;
            case (MEAN_DISTANCE):
                title.text = "Mean Distance [cm]";
                break;
            case (ELLIPSE_AREA):
                title.text = "95% Ellipse Area Value [cm²]";
                break;
            default:
                Debug.LogError($"{parameter} is not defined!");
                break;
        }
    }
}