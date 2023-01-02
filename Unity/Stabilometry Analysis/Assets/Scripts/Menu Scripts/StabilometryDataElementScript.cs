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
            swayPathTotalValue.text = ConvertToString(task.swayPath, Units.NORMAL_UNIT);
            swayPathAPValue.text = ConvertToString(task.swayPathAP, Units.NORMAL_UNIT);
            swayPathMLValue.text = ConvertToString(task.swayPathML, Units.NORMAL_UNIT);

            meanSwayVelocityTotalValue.text = ConvertToString(task.meanSwayVelocity, Units.VELOCITY_UNIT);
            meanSwayVelocityAPValue.text = ConvertToString(task.meanSwayVelocityAP, Units.VELOCITY_UNIT);
            meanSwayVelocityMLValue.text = ConvertToString(task.meanSwayVelocityML, Units.VELOCITY_UNIT);

            swayAverageAmplitudeAPValue.text = ConvertToString(task.swayAverageAmplitudeAP, Units.NORMAL_UNIT);
            swayAverageAmplitudeMLValue.text = ConvertToString(task.swayAverageAmplitudeML, Units.NORMAL_UNIT);

            swayMaximalAmplitudeAPValue.text = ConvertToString(task.swayMaximalAmplitudeAP, Units.NORMAL_UNIT);
            swayMaximalAmplitudeMLValue.text = ConvertToString(task.swayMaximalAmplitudeML, Units.NORMAL_UNIT);

            meanDistanceValue.text = ConvertToString(task.meanDistance, Units.NORMAL_UNIT);
            conf95EllipseAreaValue.text = ConvertToString(task.confidence95Ellipse.area, Units.AREA_UNIT);
        }

        private string ConvertToString(float value, string unit)
        {
            string valueWithDecimals =  string.Format("{0:0.00######}", Rounder.RoundFloat(value));

            return $"{valueWithDecimals} {unit}";
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