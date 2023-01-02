using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace StabilometryAnalysis
{
    public class DataDisplayerScript : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        private TextMeshProUGUI
            dateText = null,
            timeText = null;

        [SerializeField]
        private TextMeshProUGUI solidSurfaceEyesOpenText = null,
            solidSurfaceEyesClosedText = null,
            softSurfaceEyesOpenText = null,
            softSurfaceEyesClosedText = null;

        private RectTransform rect = null;
        #endregion

        private void Awake()
        {
            rect = GetComponent<RectTransform>();
        }

        public void SetValues(ChartData dataPoint)
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
                return "N/A";
            //else

            string displayValue = string.Format("{0:0.00######}", Rounder.RoundFloat(value));

            return $"{displayValue} {unit}";
        }

        public void EnableObject(bool enable)
        {
            this.gameObject.SetActive(enable);
        }

        public void SetPosition(Vector3 mousePosition)
        {
            rect.position = new Vector2(mousePosition.x, mousePosition.y);
        }
    }
}