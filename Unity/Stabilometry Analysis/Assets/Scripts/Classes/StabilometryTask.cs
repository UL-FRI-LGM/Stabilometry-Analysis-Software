using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabilometryTask
{
    public int ID;
    public float 
        swayPath,
        swayPathAP,
        swayPathML,
        meanDistance;

    public float 
        meanSwayVelocity,
        meanSwayVelocityAP,
        meanSwayVelocityML;

    public float 
        swayAverageAmplitudeAP,
        swayAverageAmplitudeML;

    public float 
        swayMaximalAmplitudeAP,
        swayMaximalAmplitudeML;

    //95% of data
    public float standardEllipseArea;

    public StabilometryTask(int ID, float pathLength)
    {
      //  this.ID = ID;
    //    this.pathLength = pathLength;
    }
}
