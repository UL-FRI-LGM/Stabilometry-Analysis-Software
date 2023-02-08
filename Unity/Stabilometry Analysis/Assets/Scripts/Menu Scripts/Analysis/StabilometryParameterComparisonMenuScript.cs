using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace StabilometryAnalysis
{
    using static ChartSupportScript;

    public class StabilometryParameterComparisonMenuScript : LineChartParentScript
    {
        #region Variables
        public MainScript mainScript { get; set; } = null;

        [SerializeField]
        private AccordionToggler[] parameterTogglers = null;

        [SerializeField]
        private GameObject
            comparisonLineChartPrefab = null,
            chartHolder = null,
            chartMask = null;

        [SerializeField] private ScrollbarScript scrollbarScript = null;

        [SerializeField]
        private AccordionRadioHandler
            firstPoseRadioHandler = null,
            secondPoseRadioHandler = null,
            taskRadioHandler = null;

        [SerializeField]
        private AccordionDropdownSelector
            minimumDuration = null,
            maximumDuration = null,
            firstDate = null,
            lastDate = null;

        [SerializeField] private BackgroundBlockerScript backgroundBlocker = null;
        [SerializeField]
        private TextMeshProUGUI
            firstDataNameText = null,
            secondDataNameText = null;

        private RectTransform chartHolderRect = null;

        private List<GameObject> instantiatedCharts = null;

        private List<StabilometryMeasurement> patientData = null;
        // Smaller data size based on pose.
        private List<StabilometryMeasurement>
            firstPoseData = null,
            secondPoseData = null;

        private float initialChartYposition = 0;
        private float previousChartAreaSize = 0;
        private float chartAreaSize = 0;
        private float previousScrollbarValue = 0;
        private bool scrollbarSet = false;

        private Task currentTask = Task.EYES_OPEN_SOLID_SURFACE;

        #endregion

        private void Awake()
        {
            //Debug.LogError("Implement averageing the first parameter and fix the changing of tasks.");

            chartHolderRect = chartHolder.GetComponent<RectTransform>();

            initialChartYposition = chartHolderRect.localPosition.y;

            instantiatedCharts = new List<GameObject>();
        }

        private void OnEnable()
        {
            patientData = SortMeasurements(mainScript.database.GetAllMeasurements(mainScript.currentPatient));

            if (patientData.Count <= 0)
            {
                mainScript.menuSwitching.OpenInitialMenu();
                return;
            }

            SetDataLimiters(patientData);
            currentTask = taskRadioHandler.selectedTask;

            UpdateFirstPose();
            UpdateSecondPose();

            UpdateChartNameTexts();

            UpdateCharts();

            if (backgroundBlocker.hasData)
                backgroundBlocker.ReEnable();
        }

        private void Update()
        {
            HandleValuesChanged();
        }

        private void HandleValuesChanged()
        {
            bool updateCharts = false;

            if (firstPoseRadioHandler.valueChanged || secondPoseRadioHandler.valueChanged || taskRadioHandler.valueChanged)
            {
                currentTask = taskRadioHandler.selectedTask;

                UpdateFirstPose();
                UpdateSecondPose();

                taskRadioHandler.valueChanged = false;
                updateCharts = true;
            }

            if (DataLimiterChanged())
            {
                UpdateFirstPose();
                UpdateSecondPose();

                updateCharts = true;
            }

            if (HasAnyToggleChanged())
                updateCharts = true;

            if (updateCharts)
            {
                UpdateChartNameTexts();
                UpdateCharts();
            }

            // Handle scrollbar
            if (scrollbarSet && scrollbarScript.valuePositon != previousScrollbarValue)
                UpdatePosition(scrollbarScript.valuePositon);
        }

        private void UpdateFirstPose()
        {
            firstPoseData = GetRelevantData(patientData, firstPoseRadioHandler.selectedPose, currentTask, firstDate.dateValue, lastDate.dateValue,
                    minimumDuration.durationValue, maximumDuration.durationValue);

            firstPoseRadioHandler.valueChanged = false;
        }

        private void UpdateSecondPose()
        {
            if (secondPoseRadioHandler.selectedPose == Pose.AVERAGE_FIRST_POSE)
            {
                List<StabilometryMeasurement> allRelevantData = mainScript.database.GetAllMeasurements(firstPoseRadioHandler.selectedPose);

                secondPoseData = GetRelevantData(allRelevantData, firstPoseRadioHandler.selectedPose, currentTask, minimumDuration.durationValue, maximumDuration.durationValue);
            }
            else
                secondPoseData = GetRelevantData(patientData, secondPoseRadioHandler.selectedPose, currentTask, firstDate.dateValue, lastDate.dateValue,
                    minimumDuration.durationValue, maximumDuration.durationValue);

            secondPoseRadioHandler.valueChanged = false;
        }

        private void SetDataLimiters(List<StabilometryMeasurement> data)
        {
            //Debug.LogError("Move these things to a static class");
            List<MyDateTime> dateList = new List<MyDateTime>();
            List<int> durationList = new List<int>();

            foreach (StabilometryMeasurement element in data)
            {
                if (!ListHasDate(dateList, element.dateTime))
                    dateList.Add(element.dateTime);

                durationList.AddRange(GetDurations(durationList, element));
            }

            bool isLover = true;

            // Date List should be already sorted.
            firstDate.SetDates(dateList, isLover);
            lastDate.SetDates(dateList, !isLover);

            durationList = OrderList(durationList);

            minimumDuration.SetDurations(durationList, isLover);
            maximumDuration.SetDurations(durationList, !isLover);

        }

        private bool DataLimiterChanged()
        {
            bool result = minimumDuration.valueChanged || maximumDuration.valueChanged || firstDate.valueChanged || lastDate.valueChanged;

            minimumDuration.valueChanged = false;
            maximumDuration.valueChanged = false;
            firstDate.valueChanged = false;
            lastDate.valueChanged = false;

            return result;
        }

        private void UpdatePosition(float newValue)
        {
            previousScrollbarValue = newValue;

            chartHolderRect.localPosition =
                new Vector3(
                    chartHolderRect.localPosition.x,
                    ConvertToPosition(newValue, chartAreaSize, ((RectTransform)chartMask.transform).rect.size.y, initialChartYposition),
                    chartHolderRect.localPosition.z);
        }

        private float ConvertToPosition(float newValue, float chartAreaSize, float maskSize, float initialAreaPosition)
        {
            // 0 mmeans at the top, 1 means at the bottom.

            float maxChartPosition = initialAreaPosition + chartAreaSize - maskSize * 0.8f;

            float result = newValue * (maxChartPosition - initialChartYposition) + initialChartYposition;
            return result;
        }

        /// <summary>
        /// Checks if any toggler has been changed.
        /// </summary>
        /// <returns></returns>
        private bool HasAnyToggleChanged()
        {
            bool result = false;

            foreach (AccordionToggler toggler in parameterTogglers)
            {
                if (toggler.ToggleChanged)
                {
                    toggler.ToggleChanged = false;
                    result = true;
                }
            }

            return result;
        }

        private void UpdateChartNameTexts()
        {
            firstDataNameText.text = $"{GetPoseName(firstPoseRadioHandler.selectedPose)}, {GetTaskName(currentTask)}";
            secondDataNameText.text = $"{GetPoseName(secondPoseRadioHandler.selectedPose)}, {GetTaskName(currentTask)}";
        }

        /// <summary>
        /// Spawns charts and updates scrollbar.
        /// </summary>
        private void UpdateCharts()
        {
            SpawnCharts(GetChosenParameters(this.parameterTogglers));

            // Update Scrollbar size.
            float maskSize = ((RectTransform)chartMask.transform).rect.size.y;
            chartAreaSize = GetCurrentChartAreaSize(instantiatedCharts, maskSize);

            scrollbarScript.SetSize(chartAreaSize, maskSize);

            scrollbarSet = true;

            if (previousChartAreaSize != chartAreaSize)
            {
                previousChartAreaSize = chartAreaSize;
                scrollbarScript.valuePositon = 0;
            }
        }

        /// <summary>
        /// Destroys all charts and spawns new ones.
        /// </summary>
        /// <param name="allParameters"></param>
        /// <param name="allTasks"></param>
        private void SpawnCharts(List<Parameter> allParameters)
        {
            foreach (GameObject instance in instantiatedCharts)
                Destroy(instance);

            instantiatedCharts = new List<GameObject>();

            for (int i = 0; i < allParameters.Count; i++)
            {
                GameObject instance = Instantiate(comparisonLineChartPrefab, chartHolder.transform);
                ComparisonLineChartScript chartScript = instance.GetComponent<ComparisonLineChartScript>();

                bool smallChart = true;

                chartScript.SetSize(smallChart);
                chartScript.SetPosition(i, chartHolder.GetComponent<RectTransform>().rect.size);

                List<ComparisonChartData> chartData;

                if (secondPoseRadioHandler.selectedPose != Pose.AVERAGE_FIRST_POSE)
                    chartData = GetCurrentChartData(allParameters[i]);
                else
                    chartData = GetCurrentChartDataWithAverage(allParameters[i], currentTask);

                chartScript.SetChartData(chartData, allParameters[i]);

                chartScript.SetParent(i, this, backgroundBlocker);
                instantiatedCharts.Add(instance);
            }
        }

        private List<ComparisonChartData> GetCurrentChartData(Parameter currentParameter)
        {
            List<ComparisonChartData> result = new List<ComparisonChartData>();

            int higherCount = (firstPoseData.Count >= secondPoseData.Count) ? firstPoseData.Count : secondPoseData.Count;

            for (int i = 0; i < higherCount; i++)
            {
                string unit = "";

                float firstValue = -1f;
                MyDateTime firstTime = null;

                float secondValue = -1f;
                MyDateTime secondTime = null;

                if (firstPoseData.Count > i)
                {
                    ChartData firstData = firstPoseData[i].GetData(currentParameter);

                    firstValue = firstData.GetTaskValue(currentTask);
                    firstTime = firstData.time;
                    unit = firstData.unit;
                }

                if (secondPoseData.Count > i)
                {
                    ChartData secondData = secondPoseData[i].GetData(currentParameter);

                    secondValue = secondData.GetTaskValue(currentTask);
                    secondTime = secondData.time;
                    unit = secondData.unit;
                }

                result.Add(new ComparisonChartData(firstValue, secondValue, firstTime, secondTime, unit));
            }

            return result;
        }

        private List<ComparisonChartData> GetCurrentChartDataWithAverage(Parameter currentParameter, Task task)
        {
            List<ComparisonChartData> result = new List<ComparisonChartData>();

            float averageValue = GetAverageData(secondPoseData, currentParameter, task);

            for (int i = 0; i < firstPoseData.Count; i++)
            {
                ChartData firstData = firstPoseData[i].GetData(currentParameter);

                string unit = firstData.unit;
                float firstValue = firstData.GetTaskValue(currentTask);

                MyDateTime firstTime = firstData.time;

                float secondValue = averageValue;
                MyDateTime secondTime = null;

                result.Add(new ComparisonChartData(firstValue, secondValue, firstTime, secondTime, unit));
            }

            return result;
        }

        private static float GetAverageData(List<StabilometryMeasurement> allRelevantData, Parameter parameter, Task task)
        {
            float result = 0;

            foreach (StabilometryMeasurement data in allRelevantData)
                result += data.GetData(parameter).GetTaskValue(task);

            return result / (float)allRelevantData.Count;
        }

        public void BackButtonClick()
        {
            mainScript.menuSwitching.OpenPreviousMenu();
        }

        public override void OpenAnalysisMenu(int index)
        {
            throw new System.NotImplementedException();
        }
    }
}