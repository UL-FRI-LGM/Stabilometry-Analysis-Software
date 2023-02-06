using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StabilometryAnalysis
{
    using static ChartSupportScript;

    public class StabilometryImagesMenuScript : MonoBehaviour
    {
        #region Variables
        public MainScript mainScript { get; set; } = null;
        [SerializeField] private GameObject stabilometryImageElementPrefab = null;
        [SerializeField] private ScrollbarScript scrollbarScript = null;
        [SerializeField] private AccordionRadioHandler poseRadioHandler = null;
        [SerializeField] private GameObject imageElementObjectHolder = null;
        [SerializeField] private GameObject measurementMenu = null;

        [SerializeField]
        private AccordionDropdownSelector
           minimumDuration = null,
           maximumDuration = null,
           firstDate = null,
           lastDate = null;

        private RectTransform imageElementObjectRect = null;
        private float prefabElementWidth = 0;

        private List<StabilometryImageElementScript> imageElementScripts = null;
        private List<StabilometryMeasurement> patientData = null;
        private List<StabilometryMeasurement> relevantData = null;

        private int startDisplayNumber = 5;

        private float previousScrollbarValue = 0;
        private float initialElementObjectXPosition = 0;

        private bool scrollbarSet = false;

        #endregion

        //public void 
        private void Awake()
        {
            prefabElementWidth = stabilometryImageElementPrefab.GetComponent<RectTransform>().rect.width;
            imageElementScripts = new List<StabilometryImageElementScript>();
            imageElementObjectRect = imageElementObjectHolder.GetComponent<RectTransform>();
            initialElementObjectXPosition = imageElementObjectRect.localPosition.x;
        }

        private void OnEnable()
        {
            GetPatientData();
            UpdateImages();
            poseRadioHandler.valueChanged = false;
        }

        private void Update()
        {
            if (poseRadioHandler.valueChanged)
            {
                relevantData = GetRelevantData(patientData, poseRadioHandler.selectedPose, firstDate.dateValue, lastDate.dateValue,
                    minimumDuration.durationValue, maximumDuration.durationValue);
                poseRadioHandler.valueChanged = false;

                UpdateImages();
            }

            if (DataLimiterChanged())
            {
                relevantData = GetRelevantData(patientData, poseRadioHandler.selectedPose, firstDate.dateValue, lastDate.dateValue,
                    minimumDuration.durationValue, maximumDuration.durationValue);
                UpdateImages();
            }

            if (scrollbarSet && scrollbarScript.valuePositon != previousScrollbarValue)
                UpdatePosition(scrollbarScript.valuePositon, relevantData.Count);
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

        private void GetPatientData()
        {
            patientData = SortMeasurements(mainScript.database.GetAllMeasurements(mainScript.currentPatient));
            for (int i = 0; i < patientData.Count; i++)
                patientData[i] = JSONHandler.GetJSONFile(patientData[i]);

            if (patientData.Count <= 0)
            {
                mainScript.menuSwitching.OpenInitialMenu();
                return;
            }

            SetDataLimiters(patientData);

            relevantData = GetRelevantData(patientData, poseRadioHandler.selectedPose, firstDate.dateValue, lastDate.dateValue,
                    minimumDuration.durationValue, maximumDuration.durationValue);
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

        private void UpdateImages()
        {
            // Reset position
            imageElementObjectRect.localPosition = new Vector3(
                initialElementObjectXPosition,
                imageElementObjectRect.localPosition.y,
                imageElementObjectRect.localPosition.z);

            SetScrollbar(relevantData.Count);
            scrollbarSet = true;

            SpawnElements(relevantData);
        }

        //private List<StabilometryMeasurement> GetRelevantData(List<StabilometryMeasurement> allData, Pose currentPose)
        //{
        //    List<StabilometryMeasurement> result = new List<StabilometryMeasurement>();

        //    foreach (StabilometryMeasurement data in allData)
        //        if (data.pose == currentPose)
        //            result.Add(data);

        //    return result;
        //}

        private void UpdatePosition(float newValue, float totalElementNumber)
        {
            previousScrollbarValue = newValue;

            float newXPosition = ConvertToPosition(newValue, totalElementNumber, initialElementObjectXPosition);

            imageElementObjectRect.localPosition =
                new Vector3(
                    newXPosition,
                    imageElementObjectRect.localPosition.y,
                    imageElementObjectRect.localPosition.z);

            SetElementVisibilities(newXPosition);

        }

        private float ConvertToPosition(float newValue, float totalElementNumber, float initialElementObjectXPosition)
        {
            // 0 mmeans scrollbar at the left, 1 means scrollbar at the right.
            float maxChartPosition = initialElementObjectXPosition - (totalElementNumber - startDisplayNumber) * prefabElementWidth;

            float result = newValue * (maxChartPosition - initialElementObjectXPosition) + initialElementObjectXPosition;
            return result;
        }

        private void SetScrollbar(int elementNumber)
        {
            scrollbarScript.SetSize(elementNumber, startDisplayNumber);
        }

        private void SpawnElements(List<StabilometryMeasurement> dataToSpawn)
        {
            foreach (StabilometryImageElementScript element in imageElementScripts)
                Destroy(element.gameObject);

            imageElementScripts = new List<StabilometryImageElementScript>();

            for (int i = 0; i < dataToSpawn.Count; i++)
            {
                GameObject instance = Instantiate(stabilometryImageElementPrefab, imageElementObjectHolder.transform);
                RectTransform rect = instance.GetComponent<RectTransform>();

                StabilometryImageElementScript element = instance.GetComponent<StabilometryImageElementScript>();
                rect.anchoredPosition += new Vector2(i * rect.rect.width, 0);

                element.SetData(dataToSpawn[i], this);
                element.SetVisible(i <= 4);

                imageElementScripts.Add(element);
            }
        }

        private void SetElementVisibilities(float newXPosition)
        {
            int firstVisibleIndex = (int)((initialElementObjectXPosition - newXPosition) / prefabElementWidth);
            int lastVisibleIndex = firstVisibleIndex + startDisplayNumber;

            for (int i = 0; i < imageElementScripts.Count; i++)
            {
                bool isVisible = (i >= firstVisibleIndex && i <= lastVisibleIndex);
                imageElementScripts[i].SetVisible(isVisible);
            }
        }

        public void OpenAnalysisMenu(StabilometryMeasurement measurement)
        {
            mainScript.menuSwitching.OpenMenu(measurementMenu);
            mainScript.stabilometryMeasurementScript.SetData(measurement);
        }

        public void BackButtonClick()
        {
            mainScript.menuSwitching.OpenPreviousMenu();
        }
    }
}