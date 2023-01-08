using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StabilometryAnalysis
{
    public class StabilometryImagesMenuScript : MonoBehaviour
    {
        #region Variables
        public MainScript mainScript { get; set; } = null;
        [SerializeField] private GameObject stabilometryImageElementPrefab = null;
        [SerializeField] private ScrollbarScript scrollbarScript = null;
        [SerializeField] private AccordionRadioHandler poseRadioHandler = null;
        [SerializeField] private GameObject imageElementObjectHolder = null;
        [SerializeField] private GameObject measurementMenu = null;

        private RectTransform imageElementObjectRect = null;
        private float prefabElementWidth = 0;

        private List<StabilometryImageElementScript> imageElementScripts = null;
        private List<StabilometryMeasurement> patientData = null;
        private List<StabilometryMeasurement> relevantData = null;

        private int startDisplayNumber = 5;

        private float previousScrollbarValue = 0;
        private float imageElementObjectSize = 0;
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
        }

        private void Update()
        {
            if (poseRadioHandler.valueChanged)
            {
                GetRelevantData(patientData, poseRadioHandler.selectedPose);
                poseRadioHandler.valueChanged = false;

                UpdateImages();
            }

            if (scrollbarSet && scrollbarScript.valuePositon != previousScrollbarValue)
                UpdatePosition(scrollbarScript.valuePositon, relevantData.Count);
        }

        private void GetPatientData()
        {
            patientData = mainScript.database.GetAllMeasurements(mainScript.currentPatient);
            relevantData = GetRelevantData(patientData, poseRadioHandler.selectedPose);
        }

        private void UpdateImages()
        {
            SetScrollbar(relevantData.Count);
            scrollbarSet = true;

            SpawnElements(relevantData);
        }

        private List<StabilometryMeasurement> GetRelevantData(List<StabilometryMeasurement> allData, Pose currentPose)
        {
            List<StabilometryMeasurement> result = new List<StabilometryMeasurement>();

            foreach (StabilometryMeasurement data in allData)
                if (data.pose == currentPose)
                    result.Add(data);

            return result;
        }

        private void UpdatePosition(float newValue, float totalElementNumber)
        {
            previousScrollbarValue = newValue;

            imageElementObjectRect.localPosition =
                new Vector3(
                    ConvertToPosition(newValue, totalElementNumber, initialElementObjectXPosition),
                    imageElementObjectRect.localPosition.y,
                    imageElementObjectRect.localPosition.z);
        }

        private float ConvertToPosition(float newValue, float totalElementNumber, float initialElementObjectXPosition)
        {
            // 0 mmeans at the left, 1 means right.
            float maxChartPosition = initialElementObjectXPosition - (totalElementNumber - 1) * prefabElementWidth;

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

                element.SetData(i, dataToSpawn[i], this);
                element.SetVisible(isElementVisible());

                imageElementScripts.Add(element);
            }
        }

        private bool isElementVisible()
        {
            return true;
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