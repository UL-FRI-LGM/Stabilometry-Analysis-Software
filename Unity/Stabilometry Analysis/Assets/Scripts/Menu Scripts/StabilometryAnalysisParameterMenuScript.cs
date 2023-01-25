using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StabilometryAnalysis
{
    using static ChartSupportScript;

    // Note about spawning charts. Start with spawning only one chart (because of speed).
    // This one chart should be stretched trhough the entire space.
    // When clicking on a chart toggler change sizes.
    // When clicking on any chart it opens the menu with data.
    public class StabilometryAnalysisParameterMenuScript : MonoBehaviour
    {
        #region Variables
        public MainScript mainScript { get; set; } = null;

        [SerializeField]
        private AccordionToggler[]
            parameterTogglers = null,
            taskTogglers = null;

        [SerializeField]
        private GameObject
            lineChartPrefab = null,
            chartHolder = null,
            chartMask = null;

        [SerializeField] private ScrollbarScript scrollbarScript = null;

        [SerializeField] private GameObject measurementMenu = null;
        [SerializeField] private AccordionRadioHandler poseRadioHandler = null;
        [SerializeField] private BackgroundBlockerScript backgroundBlocker = null;

        [SerializeField]
        private AccordionDropdownSelector
            minimumDuration = null,
            maximumDuration = null,
            firstDate = null,
            lastDate = null;

        private RectTransform chartHolderRect = null;

        private List<GameObject> instantiatedCharts = null;
        
        private Vector2 firstPosition = new Vector2();

        private List<StabilometryMeasurement> patientData = null;
        // Smaller data size based on pose.
        private List<StabilometryMeasurement> relevantData = null;

        private bool hasData = false;
        private bool chartsSpawned = false;

        private float initialChartYposition = 0;
        private float previousChartAreaSize = 0;
        private float chartAreaSize = 0;
        private float previousScrollbarValue = 0;
        private bool scrollbarSet = false;
        #endregion

        private void Awake()
        {
            chartHolderRect = chartHolder.GetComponent<RectTransform>();

            initialChartYposition = chartHolderRect.localPosition.y;
            SetToggleDependencies();

            instantiatedCharts = new List<GameObject>();
        }

        private void SetToggleDependencies()
        {
            foreach (AccordionToggler toggler in parameterTogglers)
                toggler.AnalysisMenuScript = this;

            foreach (AccordionToggler toggler in taskTogglers)
                toggler.AnalysisMenuScript = this;
        }

        private void OnEnable()
        {
            hasData = false;
            chartsSpawned = false;

            patientData = SortMeasurements(mainScript.database.GetAllMeasurements(mainScript.currentPatient));

            if(patientData.Count <= 0)
            {
                mainScript.menuSwitching.OpenInitialMenu();
                return;
            }

            SetDataLimiters(patientData);

            relevantData = GetRelevantData(patientData, poseRadioHandler.selectedPose, firstDate.dateValue, lastDate.dateValue,
                minimumDuration.durationValue, maximumDuration.durationValue);
            hasData = true;
            UpdateCharts();
        }

        private void Update()
        {
            if (poseRadioHandler.valueChanged)
            {
                poseRadioHandler.valueChanged = false;
                relevantData = GetRelevantData(patientData, poseRadioHandler.selectedPose, firstDate.dateValue, lastDate.dateValue,
                    minimumDuration.durationValue, maximumDuration.durationValue);

                UpdateCharts();
            }

            if (DataLimiterChanged())
            {
                relevantData = GetRelevantData(patientData, poseRadioHandler.selectedPose, firstDate.dateValue, lastDate.dateValue,
                    minimumDuration.durationValue, maximumDuration.durationValue);
                UpdateCharts();
            }
            else if (HasAnyToggleChanged())
                UpdateCharts();

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

        private bool ListHasDate(List<MyDateTime> list, MyDateTime date)
        {
            foreach (MyDateTime element in list)
            {
                if (date.IsTheSame(element))
                    return true;
            }

            return false;
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

            foreach (AccordionToggler toggler in taskTogglers)
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
            SpawnCharts(GetChosenParameters(this.parameterTogglers), GetChosenTasks(this.taskTogglers));

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
        private void SpawnCharts(List<Parameter> allParameters, List<Task> selectedTasks)
        {
            foreach (GameObject instance in instantiatedCharts)
                Destroy(instance);

            instantiatedCharts = new List<GameObject>();

            for (int i = 0; i < allParameters.Count; i++)
            {
                GameObject instance = Instantiate(lineChartPrefab, chartHolder.transform);
                LineChartScript chartScript = instance.GetComponent<LineChartScript>();

                bool smallChart = true;

                chartScript.SetSize(i, chartHolder.GetComponent<RectTransform>().rect.size, smallChart);
                chartScript.SetChartData(GetCurrentChartData(allParameters[i]), allParameters[i], selectedTasks);

                chartScript.SetParent(i, this);
                instantiatedCharts.Add(instance);
            }
        }

        private List<ChartData> GetCurrentChartData(Parameter currentParameter)
        {
            List<ChartData> result = new List<ChartData>();

            foreach (StabilometryMeasurement measurement in relevantData)
                result.Add(measurement.GetData(currentParameter));

            return result;
        }

        /// <summary>
        /// Get the size of the area for currently spawned charts.
        /// </summary>
        /// <param name="allInstances"></param>
        /// <param name="maskSize"></param>
        /// <returns></returns>
        private float GetCurrentChartAreaSize(List<GameObject> allInstances, float maskSize)
        {
            if (allInstances.Count < 2)
                return maskSize;

            // else
            RectTransform firstRect = (RectTransform)allInstances[0].transform;
            RectTransform lastRect = (RectTransform)allInstances[allInstances.Count - 1].transform;

            float result = Mathf.Abs((firstRect.localPosition.y + firstRect.rect.height / 2f) - (lastRect.localPosition.y - lastRect.rect.height / 2f));

            return result;
        }        

        public void OpenAnalysisMenu(StabilometryMeasurement measurement)
        {
            foreach (StabilometryMeasurement element in patientData)
            {
                if (element.ID == measurement.ID)
                {
                    mainScript.menuSwitching.OpenMenu(measurementMenu);
                    mainScript.stabilometryMeasurementScript.SetData(measurement);
                    break;
                }
            }
        }

        /// <summary>
        /// Gets the measurement and adds the values stored in the jason.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public StabilometryMeasurement GetMeasurement(int index)
        {
            StabilometryMeasurement result = JSONHandler.GetJSONFile(relevantData[index]);
            return result;
        }

        public void BackButtonClick()
        {
            mainScript.menuSwitching.OpenPreviousMenu();
        }

        public void ClosePopupLineChart()
        {
            backgroundBlocker.Cancel();
        }

        public void ExpandChart(int lineChartIndex)
        {
            backgroundBlocker.CreateChart(lineChartIndex, this);
        }
    }
}