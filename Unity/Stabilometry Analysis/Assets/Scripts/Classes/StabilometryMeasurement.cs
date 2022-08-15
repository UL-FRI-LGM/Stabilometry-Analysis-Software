using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Pose;

public class StabilometryMeasurement
{
    #region Variables
    public int ID,
        patientID;

    public StabilometryTask eyesOpenSolidSurface,
        eyesClosedSolidSurface,
        eyesOpenSoftSurface,
        eyesClosedSoftSurface;

    public Pose pose;

    public MyDateTime dateTime;

    #endregion

    public StabilometryMeasurement()
    {
        this.ID = -1;
        this.patientID = -1;
        this.pose = Pose.BOTH_LEGS_JOINED_PARALLEL;
        this.dateTime = null;
        this.eyesOpenSolidSurface = null;
        this.eyesClosedSolidSurface = null;
        this.eyesOpenSoftSurface = null;
        this.eyesClosedSoftSurface = null;
    }

    public StabilometryMeasurement(int ID, int patientID, MyDateTime dateTime, Pose pose,
        StabilometryTask eyesOpenSolidSurface, StabilometryTask eyesClosedSolidSurface,
        StabilometryTask eyesOpenSoftSurface, StabilometryTask eyesClosedSoftSurface)
    {
        this.ID = ID;
        this.patientID = patientID;
        this.pose = pose;
        this.dateTime = dateTime;
        this.eyesOpenSolidSurface = eyesOpenSolidSurface;
        this.eyesClosedSolidSurface = eyesClosedSolidSurface;
        this.eyesOpenSoftSurface = eyesOpenSoftSurface;
        this.eyesClosedSoftSurface = eyesClosedSoftSurface;
    }

    public DrawingTaskValues[] GetDrawingData()
    {
        DrawingTaskValues[] result = new DrawingTaskValues[4];
        result[0] = (eyesOpenSolidSurface != null) ? new DrawingTaskValues(eyesOpenSolidSurface) : null;
        result[1] = (eyesClosedSolidSurface != null) ? new DrawingTaskValues(eyesClosedSolidSurface) : null;
        result[2] = (eyesOpenSoftSurface != null) ? new DrawingTaskValues(eyesOpenSoftSurface) : null;
        result[3] = (eyesClosedSoftSurface != null) ? new DrawingTaskValues(eyesClosedSoftSurface) : null;

        return result;
    }
}
