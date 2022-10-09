using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

        [SerializeField] private TextMeshProUGUI dateTimeText;

        private StabilometryMeasurement currentMeasurement = null;

        #endregion

        public void SetData(StabilometryMeasurement measurement)
        {
            currentMeasurement = measurement;

            SetTaskValues(eyesOpenSolidSurfaceData, measurement.eyesOpenSolidSurface);
            SetTaskValues(eyesClosedSolidSurfaceData, measurement.eyesClosedSolidSurface);
            SetTaskValues(eyesOpenSoftSurfaceData, measurement.eyesOpenSoftSurface);
            SetTaskValues(eyesClosedSoftSurfaceData, measurement.eyesClosedSoftSurface);

            eyesOpenSolidSurfaceMeasurementImage.SetData(measurement.eyesOpenSolidSurface);
            eyesClosedSolidSurfaceMeasurementImage.SetData(measurement.eyesClosedSolidSurface);
            eyesOpenSoftSurfaceMeasurementImage.SetData(measurement.eyesOpenSoftSurface);
            eyesClosedSoftSurfaceMeasurementImage.SetData(measurement.eyesClosedSoftSurface);

            SetTime(measurement.dateTime);
        }

        private void SetTaskValues(StabilometryDataElementScript dataElement, StabilometryTask task)
        {
            dataElement.UpdateData(task);
        }

        private void SetTime(MyDateTime dateTime)
        {
            dateTimeText.text = dateTime.GetDisplayDateTime();
        }

        public void DeleteCurrentMeasurement()
        {
            Debug.LogWarning("Delete current measurement from the database");
        }

        public void BackButtonClick()
        {
            mainScript.menuSwitching.OpenPreviousMenu();
        }


    }
}
