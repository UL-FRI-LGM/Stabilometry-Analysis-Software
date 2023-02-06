using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        private Task currentTask = Task.EYES_CLOSED_SOFT_SURFACE;
        #endregion

        private void Awake()
        {
            Debug.LogError("Implement averageing the first parameter and fix the chart positions.");

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

            firstPoseData = GetRelevantData(patientData, firstPoseRadioHandler.selectedPose, firstDate.dateValue, lastDate.dateValue,
                minimumDuration.durationValue, maximumDuration.durationValue);

            secondPoseData = GetRelevantData(patientData, secondPoseRadioHandler.selectedPose, firstDate.dateValue, lastDate.dateValue,
                minimumDuration.durationValue, maximumDuration.durationValue);

            currentTask = taskRadioHandler.selectedTask;

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

            if (firstPoseRadioHandler.valueChanged)
            {
                firstPoseData = GetRelevantData(patientData, firstPoseRadioHandler.selectedPose, firstDate.dateValue, lastDate.dateValue,
                    minimumDuration.durationValue, maximumDuration.durationValue);

                firstPoseRadioHandler.valueChanged = false;
                updateCharts = true;
            }

            if (secondPoseRadioHandler.valueChanged)
            {
                secondPoseData = GetRelevantData(patientData, secondPoseRadioHandler.selectedPose, firstDate.dateValue, lastDate.dateValue,
                    minimumDuration.durationValue, maximumDuration.durationValue);

                secondPoseRadioHandler.valueChanged = false;
                updateCharts = true;
            }

            if (taskRadioHandler.valueChanged)
            {
                currentTask = taskRadioHandler.selectedTask;
                taskRadioHandler.valueChanged = false;
                updateCharts = true;
            }

            if (HasAnyToggleChanged())
                updateCharts = true;

            if (DataLimiterChanged())
                updateCharts = true;

            if (updateCharts)
                UpdateCharts();

            // Handle scrollbar
            if (scrollbarSet && scrollbarScript.valuePositon != previousScrollbarValue)
                UpdatePosition(scrollbarScript.valuePositon);
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
                chartScript.SetChartData(GetCurrentChartData(allParameters[i]), allParameters[i]);

                chartScript.SetParent(i, this, backgroundBlocker);
                instantiatedCharts.Add(instance);
            }
        }

        private List<ComparisonChartData> GetCurrentChartData(Parameter currentParameter)
        {
            List<ComparisonChartData> result = new List<ComparisonChartData>();

            int higherCount = (firstPoseData.Count >= secondPoseData.Count) ? firstPoseData.Count : secondPoseData.Count;

            for(int i = 0; i < higherCount; i++)
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