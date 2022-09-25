using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace StabilometryAnalysis
{
    public class StabilometryDataElementScript : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        private TextMeshProUGUI
            swayPathTotalValue = null,
            swayPathAPValue = null,
            swayPathMLValue = null,

            meanSwayVelocityTotalValue = null,
            meanSwayVelocityAPValue = null,
            meanSwayVelocityMLValue = null,

            swayAverageAmplitudeAPValue = null,
            swayAverageAmplitudeMLValue = null,

            swayMaximalAmplitudeAPValue = null,
            swayMaximalAmplitudeMLValue = null,

            meanDistanceValue = null,
            conf95EllipseAreaValue = null;

        private string
            normalUnit = "cm",
            velocityUnit = "cm/s",
            areaUnit = "cm²";

        #endregion

        public void UpdateData(StabilometryTask task)
        {
            if (task != null)
                SetData(task);
            else
                ClearData();
        }

        private void SetData(StabilometryTask task)
        {
            swayPathTotalValue.text = ConvertToString(task.swayPath, normalUnit);
            swayPathAPValue.text = ConvertToString(task.swayPathAP, normalUnit);
            swayPathMLValue.text = ConvertToString(task.swayPathML, normalUnit);

            meanSwayVelocityTotalValue.text = ConvertToString(task.meanSwayVelocity, velocityUnit);
            meanSwayVelocityAPValue.text = ConvertToString(task.meanSwayVelocityAP, velocityUnit);
            meanSwayVelocityMLValue.text = ConvertToString(task.meanSwayVelocityML, velocityUnit);

            swayAverageAmplitudeAPValue.text = ConvertToString(task.swayAverageAmplitudeAP, normalUnit);
            swayAverageAmplitudeMLValue.text = ConvertToString(task.swayAverageAmplitudeML, normalUnit);

            swayMaximalAmplitudeAPValue.text = ConvertToString(task.swayMaximalAmplitudeAP, normalUnit);
            swayMaximalAmplitudeMLValue.text = ConvertToString(task.swayMaximalAmplitudeML, normalUnit);

            meanDistanceValue.text = ConvertToString(task.meanDistance, normalUnit);
            conf95EllipseAreaValue.text = ConvertToString(task.confidence95Ellipse.area, areaUnit);
        }

        private string ConvertToString(float value, string unit)
        {
            return $"{value} {unit}";
        }

        private void ClearData()
        {
            swayPathTotalValue.text = "";
            swayPathAPValue.text = "";
            swayPathMLValue.text = "";

            meanSwayVelocityTotalValue.text = "";
            meanSwayVelocityAPValue.text = "";
            meanSwayVelocityMLValue.text = "";

            swayAverageAmplitudeAPValue.text = "";
            swayAverageAmplitudeMLValue.text = "";

            swayMaximalAmplitudeAPValue.text = "";
            swayMaximalAmplitudeMLValue.text = "";

            meanDistanceValue.text = "";
            conf95EllipseAreaValue.text = "";
        }
    }
}