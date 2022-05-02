using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        swayPathTotalValue.text = $"{task.swayPath} {normalUnit}";
        swayPathAPValue.text = $"{task.swayPathAP} {normalUnit}";
        swayPathMLValue.text = $"{task.swayPathML} {normalUnit}";

        meanSwayVelocityTotalValue.text = $"{task.meanSwayVelocity} {velocityUnit}";
        meanSwayVelocityAPValue.text = $"{task.meanSwayVelocityAP} {velocityUnit}";
        meanSwayVelocityMLValue.text = $"{task.meanSwayVelocityML} {velocityUnit}";

        swayAverageAmplitudeAPValue.text = $"{task.swayAverageAmplitudeAP} {normalUnit}";
        swayAverageAmplitudeMLValue.text = $"{task.swayAverageAmplitudeML} {normalUnit}";

        swayMaximalAmplitudeAPValue.text = $"{task.swayMaximalAmplitudeAP} {normalUnit}";
        swayMaximalAmplitudeMLValue.text = $"{task.swayMaximalAmplitudeML} {normalUnit}";

        meanDistanceValue.text = $"{task.meanDistance} {normalUnit}";
        conf95EllipseAreaValue.text = $"{task.confidence95Ellipse.area} {areaUnit}";
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
