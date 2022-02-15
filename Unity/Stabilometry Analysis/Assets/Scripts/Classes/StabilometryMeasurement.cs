using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabilometryMeasurement
{
    public int ID,
        patientID,
        fileID;

    public StabilometryTask eyesOpenSolidSurface,
        eyesClosedSolidSurface,
        eyesOpenSoftSurface,
        eyesClosedSoftSurface;

    public Pose pose;

    public MyDateTime dateTime;

    public StabilometryMeasurement()
    {
        this.ID = -1;
        this.patientID = -1;
        this.fileID = -1;
        this.pose = Pose.BothLegsJoinedParallel;
        this.dateTime = null;
        this.eyesOpenSolidSurface = null;
        this.eyesClosedSolidSurface = null;
        this.eyesOpenSoftSurface = null;
        this.eyesClosedSoftSurface = null;
    }

    public StabilometryMeasurement(int ID, int patientID, int fileID, Pose pose, MyDateTime dateTime, 
        StabilometryTask eyesOpenSolidSurface, StabilometryTask eyesClosedSolidSurface,
        StabilometryTask eyesOpenSoftSurface, StabilometryTask eyesClosedSoftSurface)
    {
        this.ID = ID;
        this.patientID = patientID;
        this.fileID = fileID;
        this.pose = pose;
        this.dateTime = dateTime;
        this.eyesOpenSolidSurface = eyesOpenSolidSurface;
        this.eyesClosedSolidSurface = eyesClosedSolidSurface;
        this.eyesOpenSoftSurface = eyesOpenSoftSurface;
        this.eyesClosedSoftSurface = eyesClosedSoftSurface;
    }
}
