using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StabilometryAnalysis
{
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

        [SerializeField]
        private AccordionDropdownSelector
            minimumDuration = null,
            maximumDuration = null,
            firstDate = null,
            lastDate = null;

        private RectTransform chartHolderRect = null;

        private List<GameObject> instantiatedCharts = null;
        private Vector2 lineChartSize = new Vector2(590, 300);
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
            firstPosition = lineChartSize / 2f - chartHolder.GetComponent<RectTransform>().rect.size / 2f;
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

            patientData = mainScript.database.GetAllMeasurements(mainScript.currentPatient);
            SetDataLimiters(patientData);

            relevantData = GetRelevantData(patientData, poseRadioHandler.selectedPose);
            hasData = true;
            UpdateCharts();
        }

        private void Update()
        {
            if (poseRadioHandler.valueChanged)
            {
                poseRadioHandler.valueChanged = false;
                relevantData = GetRelevantData(patientData, poseRadioHandler.selectedPose);

                UpdateCharts();
            }

            if (DataLimiterChanged())
            {
                relevantData = GetRelevantData(patientData, poseRadioHandler.selectedPose);
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

        private List<int> OrderList(List<int> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                for (int k = i - 1; 0 <= k; k--)
                {
                    if (list[k + 1] > list[k])
                        break;
                    //else

                    int temp = list[k];
                    list[k] = list[k + 1];
                    list[k + 1] = temp;
                }
            }

            return list;
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

        private List<int> GetDurations(List<int> durationList, StabilometryMeasurement data)
        {
            List<int> result = new List<int>();

            int eyesOpenSolidSurfaceDuration = data.eyesOpenSolidSurface.GetDuration();

            if (data.eyesOpenSolidSurface != null && !durationList.Contains(eyesOpenSolidSurfaceDuration))
                result.Add(eyesOpenSolidSurfaceDuration);

            int eyesClosedSolidSurfaceDuration = data.eyesClosedSolidSurface.GetDuration();

            if (data.eyesClosedSolidSurface != null && !durationList.Contains(eyesClosedSolidSurfaceDuration))
                if (!result.Contains(eyesClosedSolidSurfaceDuration))
                    result.Add(eyesClosedSolidSurfaceDuration);

            int eyesOpenSoftSurfaceDuration = data.eyesOpenSoftSurface.GetDuration();

            if (data.eyesOpenSoftSurface != null && !durationList.Contains(eyesOpenSoftSurfaceDuration))
                if (!result.Contains(eyesOpenSoftSurfaceDuration))
                    result.Add(eyesOpenSoftSurfaceDuration);

            int eyesClosedSoftSurfaceDuration = data.eyesClosedSoftSurface.GetDuration();

            if (data.eyesClosedSoftSurface != null && !durationList.Contains(eyesClosedSoftSurfaceDuration))
                if (!result.Contains(eyesClosedSoftSurfaceDuration))
                    result.Add(eyesClosedSoftSurfaceDuration);

            return result;
        }

        private List<StabilometryMeasurement> TrimData(List<StabilometryMeasurement> inputData)
        {
            List<StabilometryMeasurement> result = new List<StabilometryMeasurement>();

            foreach (StabilometryMeasurement element in inputData)
            {
                StabilometryMeasurement modifiedValue = ModifyData(element);
                if (modifiedValue != null)
                    result.Add(modifiedValue);
            }

            return result;
        }

        private StabilometryMeasurement ModifyData(StabilometryMeasurement element)
        {
            if (firstDate.dateValue == null)
                return element;

            Debug.Log(element.dateTime.IsSmaller(firstDate.dateValue));

            if (element.dateTime.IsSmaller(firstDate.dateValue) || element.dateTime.IsGreater(lastDate.dateValue))
                return null;
            // else

            StabilometryMeasurement result = element.Duplicate();
            result.eyesOpenSolidSurface = CheckDuration(result.eyesOpenSolidSurface);
            result.eyesClosedSolidSurface = CheckDuration(result.eyesClosedSolidSurface);
            result.eyesOpenSoftSurface = CheckDuration(result.eyesOpenSoftSurface);
            result.eyesClosedSoftSurface = CheckDuration(result.eyesClosedSoftSurface);

            if (result.eyesOpenSolidSurface == null && result.eyesClosedSolidSurface == null
                && result.eyesOpenSoftSurface == null && result.eyesClosedSoftSurface == null)
                return null;

            return result;
        }

        private StabilometryTask CheckDuration(StabilometryTask task)
        {
            if (task.duration < minimumDuration.durationValue || task.duration > maximumDuration.durationValue)
                return null;

            return task;
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

        private List<StabilometryMeasurement> GetRelevantData(List<StabilometryMeasurement> allData, Pose currentPose)
        {
            List<StabilometryMeasurement> temp = new List<StabilometryMeasurement>();

            foreach (StabilometryMeasurement data in allData)
                if (data.pose == currentPose)
                    temp.Add(data);

            return TrimData(temp);
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
                RectTransform instanceTransfrom = (RectTransform)instance.transform;
                instanceTransfrom.sizeDelta = lineChartSize;
                instanceTransfrom.localPosition = GetNewPosition(i, firstPosition, lineChartSize);

                LineChartScript chartScript = instance.GetComponent<LineChartScript>();

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
        /// Checks parameters to see which charts to spawn.
        /// </summary>
        /// <param name="parameterTogglers"></param>
        /// <returns></returns>
        private List<Parameter> GetChosenParameters(AccordionToggler[] parameterTogglers)
        {
            List<Parameter> result = new List<Parameter>();

            for (int i = 0; i < parameterTogglers.Length; i++)
            {
                if (parameterTogglers[i].GetToggle().isOn)
                    result.Add((Parameter)i);
            }

            return result;
        }

        /// <summary>
        /// Checks to see which tasks have been chosen.
        /// </summary>
        /// <param name="taskTogglers"></param>
        /// <returns></returns>
        private List<Task> GetChosenTasks(AccordionToggler[] taskTogglers)
        {
            List<Task> result = new List<Task>();

            for (int i = 0; i < taskTogglers.Length; i++)
                if (taskTogglers[i].GetToggle().isOn)
                    result.Add((Task)i);

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

        /// <summary>
        /// Gets the position for the next line chart to spawn.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="originalPosition"></param>
        /// <param name="chartSize"></param>
        /// <returns></returns>
        private Vector2 GetNewPosition(int index, Vector2 originalPosition, Vector2 chartSize)
        {
            float x = (index % 2 == 0) ? originalPosition.x : -originalPosition.x;
            int row = index / 2;
            float y = originalPosition.y - row * (chartSize.y + 20);

            return new Vector2(x, y);
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
    }
}