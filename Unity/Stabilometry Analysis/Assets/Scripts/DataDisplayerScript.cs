using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace StabilometryAnalysis
{
    public class DataDisplayerScript : MonoBehaviour
    {
        #region Variables
        [SerializeField] private TextMeshProUGUI 
            dateText = null,
            timeText = null;

        [SerializeField] private TextMeshProUGUI solidSurfaceEyesOpenText = null,
            solidSurfaceEyesClosedText = null,
            softSurfaceEyesOpenText = null,
            softSurfaceEyesClosedText = null;
        #endregion

        public void ActivateDataDisplayer(ChartData dataPoint)
        {

        }

        private void SetPosition()
        {

        }

        private void SetValues(ChartData dataPoint)
        {
            dateText.text = dataPoint.time.GetDateString();
            timeText.text = dataPoint.time.GetTimeString();

            solidSurfaceEyesOpenText.text = CheckIfNull(dataPoint.eyesOpenSolidSurfaceValue, dataPoint.unit);
            solidSurfaceEyesClosedText.text = CheckIfNull(dataPoint.eyesClosedSolidSurfaceValue, dataPoint.unit);
            softSurfaceEyesOpenText.text = CheckIfNull(dataPoint.eyesOpenSoftSurfaceValue, dataPoint.unit);
            softSurfaceEyesClosedText.text = CheckIfNull(dataPoint.eyesClosedSoftSurfaceValue, dataPoint.unit);
        }

        private string CheckIfNull(float value, string unit)
        {
            if (value < 0)
                return "";
            //else

            return $"{value} {unit}";
        }
    }
}