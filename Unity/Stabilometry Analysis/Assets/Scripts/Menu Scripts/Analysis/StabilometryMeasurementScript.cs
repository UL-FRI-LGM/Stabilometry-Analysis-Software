using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace StabilometryAnalysis
{
    public class StabilometryMeasurementScript : MonoBehaviour
    {
        #region Variables
        public MainScript mainScript { get; set; } = null;

        [SerializeField] private StabilometryDataElementScript
            eyesOpenSolidSurfaceData = null,
            eyesClosedSolidSurfaceData = null,
            eyesOpenSoftSurfaceData = null,
            eyesClosedSoftSurfaceData = null;

        [SerializeField] private MeasurementImageScript
            eyesOpenSolidSurfaceMeasurementImage = null,
            eyesClosedSolidSurfaceMeasurementImage = null,
            eyesOpenSoftSurfaceMeasurementImage = null,
            eyesClosedSoftSurfaceMeasurementImage = null;

        [SerializeField] private TextMeshProUGUI dateTimeText = null;

        [SerializeField] private BackgroundBlockerScript backgroundBlocker = null;

        [SerializeField] private Button previousButton = null;
        [SerializeField] private Button nextButton = null;

        private List<StabilometryMeasurement> allMeasurements = null;
        private int currentIndex = -1;
        private StabilometryMeasurement currentMeasurement = null;

        #endregion

        public void SetData(List<StabilometryMeasurement> measurements, int index)
        {
            this.allMeasurements = measurements;
            this.currentIndex = index;
            this.currentMeasurement = JSONHandler.GetJSONFile(measurements[index]);

            SetTaskValues(eyesOpenSolidSurfaceData, currentMeasurement.eyesOpenSolidSurface);
            SetTaskValues(eyesClosedSolidSurfaceData, currentMeasurement.eyesClosedSolidSurface);
            SetTaskValues(eyesOpenSoftSurfaceData, currentMeasurement.eyesOpenSoftSurface);
            SetTaskValues(eyesClosedSoftSurfaceData, currentMeasurement.eyesClosedSoftSurface);

            eyesOpenSolidSurfaceMeasurementImage.SetData(currentMeasurement.eyesOpenSolidSurface);
            eyesClosedSolidSurfaceMeasurementImage.SetData(currentMeasurement.eyesClosedSolidSurface);
            eyesOpenSoftSurfaceMeasurementImage.SetData(currentMeasurement.eyesOpenSoftSurface);
            eyesClosedSoftSurfaceMeasurementImage.SetData(currentMeasurement.eyesClosedSoftSurface);

            SetTime(currentMeasurement.dateTime);

            SetButtonsInteractable();
        }

        private void SetTaskValues(StabilometryDataElementScript dataElement, StabilometryTask task)
        {
            dataElement.UpdateData(task);
        }

        private void SetTime(MyDateTime dateTime)
        {
            dateTimeText.text = dateTime.ToString();
        }

        public void StartDeletingCurrentMeasurement()
        {
            backgroundBlocker.StartMeasurmentDeletion(this);
        }

        public void DeleteCurrentMeasurement()
        {
            mainScript.DeleteMeasurement(currentMeasurement);
            BackButtonClick();
        }

        public void BackButtonClick()
        {
            mainScript.menuSwitching.OpenPreviousMenu();
        }

        public void PreviousButtonClick()
        {
            SetData(allMeasurements, currentIndex - 1);
        }

        public void NextButtonClick()
        {
            SetData(allMeasurements, currentIndex + 1);
        }

        private void SetButtonsInteractable()
        {
            previousButton.interactable = currentIndex > 0;
            nextButton.interactable = currentIndex < allMeasurements.Count - 1;
        }
    }
}
