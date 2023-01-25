using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace StabilometryAnalysis
{
    public class BackgroundBlockerScript : MonoBehaviour
    {
        [SerializeField]
        private GameObject warningWidnowPrefab = null,
            lineChartPrefab = null,
            screenBlocker = null;

        private GameObject 
            warningWidnowInstance = null,
            lineChartInstance = null;


        private const string DELETE = "Delete";

        public void StartPatientDeletion(Patient patient, InitialMenuScript menuScript)
        {
            screenBlocker.SetActive(true);
            string message = $"{DELETE} {patient.FullName()}?";
            CreateWarningWindow(message, menuScript.DeletePatient);
        }

        public void StartDatabaseDeletion(InitialMenuScript menuScript)
        {
            screenBlocker.SetActive(true);
            string message = $"{DELETE} entire database?";
            CreateWarningWindow(message, menuScript.ClickConfirmDatabaseDeletion);
        }

        public void StartMeasurmentDeletion(StabilometryMeasurementScript menuScript)
        {
            screenBlocker.SetActive(true);
            string message = $"{DELETE} current measurement?";
            CreateWarningWindow(message, menuScript.DeleteCurrentMeasurement);
        }

        private void CreateWarningWindow(string message, UnityAction function)
        {
            warningWidnowInstance = Instantiate(warningWidnowPrefab, screenBlocker.transform);
            WarningWindowScript result = warningWidnowInstance.GetComponent<WarningWindowScript>();

            result.cancelButton.onClick.AddListener(Cancel);

            result.confirmButton.onClick.AddListener(function);
            result.confirmButton.onClick.AddListener(Cancel);

            result.message.text = message;
        }

        public void CreateChart(int lineChartIndex, StabilometryAnalysisParameterMenuScript parentScript)
        {
            lineChartInstance = Instantiate(lineChartPrefab, screenBlocker.transform);
            LineChartScript chartScript = lineChartInstance.GetComponent<LineChartScript>();
            chartScript.SetParent(lineChartIndex, parentScript);


        }

        public void Cancel()
        {
            if (warningWidnowInstance != null)
                Destroy(warningWidnowInstance);

            if (lineChartInstance != null)
                Destroy(lineChartInstance);

            screenBlocker.SetActive(false);

        }
    }
}
