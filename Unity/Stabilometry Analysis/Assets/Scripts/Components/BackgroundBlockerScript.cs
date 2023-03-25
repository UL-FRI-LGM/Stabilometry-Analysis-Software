using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace StabilometryAnalysis
{
    public class BackgroundBlockerScript : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        private GameObject warningWidnowPrefab = null,
            lineChartPrefab = null,
            comparisonLineChartPrefab = null,
            screenBlocker = null;

        private GameObject
            warningWidnowInstance = null,
            lineChartInstance = null,
            comparisonLineChartInstance = null;

        public bool hasData { get; set; } = false;

        private const string DELETE = "Delete";
        #endregion

        public void StartPatientDeletion(Patient patient, InitialMenuScript menuScript)
        {
            screenBlocker.SetActive(true);
            string message = $"{DELETE} {patient.FullName()}?";
            CreateWarningWindow(message, menuScript.DeletePatient);
        }

        public void StartDatabaseDeletion(InitialMenuScript menuScript)
        {
            screenBlocker.SetActive(true);
            string message = $"{DELETE} entire database and exit application?";
            CreateWarningWindow(message, menuScript.ClickConfirmDatabaseDeletion);
        }

        public void StartMeasurmentDeletion(StabilometryMeasurementScript menuScript)
        {
            screenBlocker.SetActive(true);
            string message = $"{DELETE} current measurement?";
            CreateWarningWindow(message, menuScript.DeleteCurrentMeasurement);
        }

        public void StartExitingApplication()
        {
            screenBlocker.SetActive(true);
            string message = $"Exit Application?";
            CreateWarningWindow(message, LocationPointer.mainScript.ExitApplication);
        }

        private void CreateWarningWindow(string message, UnityAction function)
        {
            DestroyAll();

            warningWidnowInstance = Instantiate(warningWidnowPrefab, screenBlocker.transform);
            WarningWindowScript result = warningWidnowInstance.GetComponent<WarningWindowScript>();

            result.cancelButton.onClick.AddListener(Cancel);

            result.confirmButton.onClick.AddListener(function);
            result.confirmButton.onClick.AddListener(Cancel);

            result.message.text = message;
        }

        public void CreateChart(List<ChartData> chartData, Parameter chosenParameter, List<Task> allTasks, 
            int lineChartIndex, LineChartParentScript parentScript)
        {
            DestroyAll();

            screenBlocker.SetActive(true);

            lineChartInstance = Instantiate(lineChartPrefab, screenBlocker.transform);
            StandardLineChartScript chartScript = lineChartInstance.GetComponent<StandardLineChartScript>();
            bool largeChart = false;
            chartScript.SetSize(largeChart);
            chartScript.SetChartData(chartData, chosenParameter, allTasks);
            chartScript.SetParent(lineChartIndex, parentScript, this);

            hasData = true;
        }

        public void CreateComparisonChart(List<ComparisonChartData> chartData, Parameter chosenParameter, int lineChartIndex, LineChartParentScript parentScript)
        {
            DestroyAll();

            screenBlocker.SetActive(true);

            comparisonLineChartInstance = Instantiate(comparisonLineChartPrefab, screenBlocker.transform);
            ComparisonLineChartScript chartScript = comparisonLineChartInstance.GetComponent<ComparisonLineChartScript>();
            bool largeChart = false;
            chartScript.SetSize(largeChart);
            chartScript.SetChartData(chartData, chosenParameter);
            chartScript.SetParent(lineChartIndex, parentScript, this);

            hasData = true;
        }

        public void Disable()
        {
            screenBlocker.SetActive(false);
        }

        public void ReEnable()
        {
            screenBlocker.SetActive(true);
        }

        private void DestroyAll()
        {
            if (warningWidnowInstance != null)
                Destroy(warningWidnowInstance);

            if (lineChartInstance != null)
                Destroy(lineChartInstance);

            if (comparisonLineChartInstance != null)
                Destroy(comparisonLineChartInstance);
        }

        public void Cancel()
        {
            hasData = false;

            screenBlocker.SetActive(false);
        }
    }
}
