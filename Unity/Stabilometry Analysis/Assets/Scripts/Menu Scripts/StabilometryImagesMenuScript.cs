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

        private List<GameObject> imageElements = null;
        private List<StabilometryMeasurement> patientData = null;
        private List<StabilometryMeasurement> relevantData = null;

        #endregion

        //public void 
        private void Awake()
        {
            imageElements = new List<GameObject>();
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
        }

        private List<StabilometryMeasurement> GetRelevantData(List<StabilometryMeasurement> allData, Pose currentPose)
        {
            List<StabilometryMeasurement> result = new List<StabilometryMeasurement>();

            foreach (StabilometryMeasurement data in allData)
                if (data.pose == currentPose)
                    result.Add(data);

            return result;
        }

        private void GetPatientData()
        {
            patientData = mainScript.database.GetAllMeasurements(mainScript.currentPatient);
            relevantData = GetRelevantData(patientData, poseRadioHandler.selectedPose);
        }

        private void UpdateImages()
        {
            SetScrollbar(relevantData.Count);
            SpawnElements();
        }

        private void SetScrollbar(int elemnetNumber)
        {

        }

        private void SpawnElements()
        {

        }

        public void OpenAnalysisMenu(StabilometryMeasurement measurement)
        {
            //OpenMenu(measurementMenu);
            //mainScript.stabilometryMeasurementScript.SetData(measurement);
        }

        public void BackButtonClick()
        {
            mainScript.menuSwitching.OpenPreviousMenu();
        }
    }
}