using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using TMPro;
using static StabilometryAnalysis.Parameter;

namespace StabilometryAnalysis
{
    using static ChartSupportScript;

    public class LineChartScript : MonoBehaviour
    {
        #region Variables
        private StabilometryAnalysisParameterMenuScript parentScript = null;
        private BackgroundBlockerScript backgroundBlockerScript = null;

        private int index = -1;

        [SerializeField]
        private GameObject
            LineObject = null,
            DateLine = null;

        [SerializeField]
        private GameObject[]
            dotObjects = null,
            yAxisLines = null;

        [SerializeField] private TextMeshProUGUI title = null;

        [SerializeField] private TextMeshProUGUI[] valueTexts = null;

        [SerializeField] private UILineRenderer[] lineRenderers = null;
        [SerializeField] private GameObject
            dotsHolder = null,
            expandButton = null,
            shrinkButton = null;

        private RectTransform chartRect = null;
        private List<ChartData> chartData = null;
        private List<Task> selectedTasks = null;

        private RectTransform[] dotRects = null;

        private List<GameObject> spawnedObjects = null;

        private Parameter chosenParameter = Parameter.SWAY_PATH_TOTAL;

        private bool smallChart = true;

        private Vector2 smallLineChartSize = new Vector2(590, 300);
        private Vector2 largeLineChartSize = new Vector2(590, 300);

        #endregion

        private void Awake()
        {
            spawnedObjects = new List<GameObject>();

            this.chartRect = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();

            this.dotRects = new RectTransform[dotObjects.Length];

            for (int i = 0; i < this.dotObjects.Length; i++)
                this.dotRects[i] = this.dotObjects[i].GetComponent<RectTransform>();
        }

        private void RespawnChart()
        {
            foreach (GameObject spawnedObject in spawnedObjects)
                Destroy(spawnedObject);

            SetChartData(this.chartData, this.chosenParameter, this.selectedTasks);

        }

        public void SetChartData(List<ChartData> chartData, Parameter chosenParameter, List<Task> allTasks)
        {
            this.chartData = chartData;
            this.chosenParameter = chosenParameter;
            this.selectedTasks = allTasks;

            SetTitle(chosenParameter);
            UpdateChart();
        }

        /// <summary>
        /// Draws the chart based on the current data and rect transform size.
        /// </summary>
        public void UpdateChart()
        {
            List<ChartData> relevantData = GetRelevantData(this.chartData, this.selectedTasks);

            RectTransform drawingSpace = lineRenderers[0].GetComponent<RectTransform>();

            Vector2 valueSpaceSize = new Vector2(drawingSpace.rect.width / (float)(relevantData.Count), drawingSpace.rect.height);

            float maxValue = GetLargestValue(relevantData);
            float modifiedMaxValue = ModifyMaxValue(maxValue);

            SetYAxis(modifiedMaxValue, drawingSpace);
            DrawData(relevantData, valueSpaceSize, this.chartRect, modifiedMaxValue);
        }

        public void ExpandChart()
        {
            
        }

        public void ShrinkChart()
        {

        }

        private static List<ChartData> GetRelevantData(List<ChartData> allData, List<Task> selectedTasks)
        {
            List<ChartData> result = new List<ChartData>();

            foreach (ChartData element in allData)
            {
                bool addELement = false;

                foreach (Task task in selectedTasks)
                {
                    if (element.GetTaskValue(task) >= 0)
                    {
                        addELement = true;
                        break;
                    }
                }

                if (addELement)
                    result.Add(element);
            }

            return result;
        }

        /// <summary>
        /// Draws data for the given chart
        /// </summary>
        /// <param name="data"></param>
        /// <param name="valueSpaceSize"></param>
        /// <param name="drawingSpace"></param>
        /// <param name="largestValue"></param>
        private void DrawData(List<ChartData> data, Vector2 valueSpaceSize, RectTransform drawingSpace, float largestValue)
        {
            float leftmostPosition = 0;

            bool oddNumber = data.Count % 2 != 0;

            if (oddNumber)
                leftmostPosition -= valueSpaceSize.x * data.Count / 2;
            else
                leftmostPosition -= valueSpaceSize.x * (data.Count / 2 - 0.5f);

            Vector2 startingPosition = new Vector2(leftmostPosition, 0);

            float valueConverter = valueSpaceSize.y / largestValue;

            SpawnElements(data, drawingSpace, startingPosition, valueConverter, valueSpaceSize.x);
        }

        /// <summary>
        /// Spawns all data.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="drawingSpace"></param>
        /// <param name="startingPosition"></param>
        /// <param name="valueConverter"></param>
        /// <param name="xSpaceSize"></param>
        private void SpawnElements(List<ChartData> data, RectTransform drawingSpace, Vector2 startingPosition, float valueConverter, float xSpaceSize)
        {
            bool oddNumber = data.Count % 2 != 0;

            float lineMove = (oddNumber) ? xSpaceSize * 0.5f : 0;

            FillLineRenderers(data, startingPosition.x, valueConverter, xSpaceSize, lineMove);

            for (int i = 0; i < data.Count; i++)
            {
                float xPosition = startingPosition.x + i * xSpaceSize;
                if (oddNumber)
                    xPosition += xSpaceSize * 0.5f;

                SpawnSliceObject(data[i], drawingSpace, startingPosition, valueConverter, xSpaceSize, i, xPosition, ToShow(data.Count, i));
            }
        }

        private static bool ToShow(int dataCount, int index)
        {
            if (dataCount <= 8)
                return true;

            if (index % GetMod(dataCount) == 0)
                return true;

            return false;
        }

        private static int GetMod(int dataCount)
        {
            return (dataCount / 8) + 1;
        }

        private void SpawnSliceObject(ChartData data, RectTransform drawingSpace, Vector2 startingPosition, float valueConverter,
            float xSpaceSize, int i, float xPosition, bool show)
        {
            GameObject slice = Instantiate(LineObject, drawingSpace.transform);
            RectTransform sliceRect = slice.GetComponent<RectTransform>();

            sliceRect.sizeDelta = new Vector2(xSpaceSize, drawingSpace.rect.height);

            sliceRect.anchoredPosition = new Vector2(xPosition, startingPosition.y);

            slice.GetComponent<LineObjectScript>().SetParentScript(i, data, this);

            spawnedObjects.Add(slice);

            SpawnDots(data, xPosition, valueConverter, i);

            SpawnLineObject(sliceRect, data.time, show);
        }

        private void SpawnLineObject(RectTransform drawingSpace, MyDateTime date, bool show)
        {
            GameObject line = Instantiate(DateLine, drawingSpace.transform);
            line.GetComponent<DateLineScript>().SetText(date, show);

        }

        private void FillLineRenderers(List<ChartData> data, float leftmostPosition, float valueConverter, float lineRectWidth, float lineMove)
        {
            List<Vector2>[] drawingData = PrepareDataForLineRenderers(data, leftmostPosition, valueConverter, lineRectWidth, selectedTasks);

            for (int i = 0; i < lineRenderers.Length; i++)
            {
                Vector2[] points = new Vector2[drawingData[i].Count];

                for (int k = 0; k < drawingData[i].Count; k++)
                    points[k] = drawingData[i][k];

                lineRenderers[i].Points = points;
                lineRenderers[i].gameObject.SetActive(points.Length > 0);

                RectTransform rectTransform = lineRenderers[i].GetComponent<RectTransform>();

                rectTransform.anchoredPosition += new Vector2(lineMove, 0);
            }
        }

        public void SetSize(int index, Vector2 chartHolderSize, bool smallChart)
        {
            this.smallChart = smallChart;

            RectTransform instanceTransfrom = (RectTransform)transform;
            instanceTransfrom.sizeDelta = (smallChart)? smallLineChartSize: largeLineChartSize;

            Vector2 firstPosition = instanceTransfrom.sizeDelta / 2f - chartHolderSize / 2f;



            instanceTransfrom.localPosition = GetNewPosition(index, firstPosition, instanceTransfrom.sizeDelta);

        }

        public void ButtonClicked(int index)
        {
            parentScript.OpenAnalysisMenu(parentScript.GetMeasurement(index));
        }

        public void SetParent(int lineChartIndex, StabilometryAnalysisParameterMenuScript parentScript)
        {
            this.parentScript = parentScript;
            this.index = lineChartIndex;
        }

        /// <summary>
        /// Prepares the data to be spawned.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="leftmostPosition"></param>
        /// <param name="valueConverter"></param>
        /// <param name="lineRect"></param>
        /// <returns></returns>
        private List<Vector2>[] PrepareDataForLineRenderers(List<ChartData> data, float leftmostPosition, float valueConverter, float lineRectWidth, List<Task> selectedTasks)
        {
            List<Vector2>[] result = new List<Vector2>[4];

            for (int i = 0; i < result.Length; i++)
                result[i] = new List<Vector2>();

            for (int k = 0; k < selectedTasks.Count; k++)
            {
                Task currentTask = selectedTasks[k];

                for (int i = 0; i < data.Count; i++)
                {
                    float xPosition = leftmostPosition + i * lineRectWidth;
                    float yPositon = data[i].GetTaskValue(currentTask) * valueConverter;

                    float previousValue = (i <= 0) ? -1 : data[i - 1].GetTaskValue(currentTask);
                    float nextValue = (i >= data.Count - 1) ? -1 : data[i + 1].GetTaskValue(currentTask);

                    switch (AddNTimes(previousValue, data[i].GetTaskValue(currentTask), nextValue))
                    {
                        case (1):
                            result[(int)currentTask].Add(new Vector2(xPosition, yPositon));
                            break;
                        case (2):
                            result[(int)currentTask].Add(new Vector2(xPosition, yPositon));
                            result[(int)currentTask].Add(new Vector2(xPosition, yPositon));
                            break;
                    }
                }
            }

            //Debug.Log($"{result[0].Count} {result[1].Count} {result[2].Count} {result[3].Count}");

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
            if (current < 0 || (previous < 0 && next < 0))
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
            for (int i = 0; i < selectedTasks.Count; i++)
            {
                Task currentTask = selectedTasks[i];
                if (data.GetTaskValue(currentTask) >= 0)
                {
                    Vector2 newPosition = new Vector2(xPosition, data.GetTaskValue(currentTask) * valueConverter);
                    dotRects[(int)currentTask].anchoredPosition = newPosition;
                    GameObject dot = Instantiate(dotObjects[(int)currentTask], dotsHolder.transform);

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
            {
                float elementLargestValue = element.GetLargestValue();
                if (elementLargestValue > largestValue)
                    largestValue = elementLargestValue;
            }

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
}