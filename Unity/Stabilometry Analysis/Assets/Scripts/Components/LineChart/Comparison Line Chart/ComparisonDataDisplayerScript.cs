using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace StabilometryAnalysis
{
    public class ComparisonDataDisplayerScript : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        private TextMeshProUGUI
            firstDateText = null,
            firstTimeText = null,
            secondDateText = null,
            secondTimeText = null;

        [SerializeField]
        private TextMeshProUGUI firstValueText = null,
            secondValueText = null;

        private RectTransform rect = null;
        #endregion

        private void Awake()
        {
            rect = GetComponent<RectTransform>();
        }

        public void SetValues(ComparisonChartData dataPoint)
        {
            SetTexts(firstDateText, firstTimeText, dataPoint.firstTime, firstValueText, dataPoint.firstValue, dataPoint.unit);
            SetTexts(secondDateText, secondTimeText, dataPoint.secondTime, secondValueText, dataPoint.secondValue, dataPoint.unit);
        }

        private void SetTexts(TextMeshProUGUI date, TextMeshProUGUI time, MyDateTime dateTime, TextMeshProUGUI valueText, float value, string unit)
        {
            Debug.Log(dateTime);
            Debug.Log(value);
            if (value < 0)
            {
                date.text = "N/A";
                time.text = "N/A";
                valueText.text = "N/A";
            }
            else
            {
                date.text = dateTime.GetDateString();
                time.text = dateTime.GetTimeString();
                valueText.text = GetDsiplayValue(value, unit);
            }
        }

        private string GetDsiplayValue(float value, string unit)
        {
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