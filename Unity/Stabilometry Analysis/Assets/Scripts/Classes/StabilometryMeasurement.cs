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

    private const string
        BothLegsJoinedParallelString = "Both Legs Joined Parallel",
        BothLegs30AngleString = "Both Legs 30 Angle",
        BothLegsParallelApartString = "Both Legs Parallel Apart",
        TandemLeftFrontString = "Tandem Left Front",
        TandemRightFrontString = "Tandem Right Front",
        LeftLegString = "Left Leg",
        RightLegString = "Right Leg";
    #endregion

    public StabilometryMeasurement()
    {
        this.ID = -1;
        this.patientID = -1;
        this.pose = Pose.BothLegsJoinedParallel;
        this.dateTime = null;
        this.eyesOpenSolidSurface = null;
        this.eyesClosedSolidSurface = null;
        this.eyesOpenSoftSurface = null;
        this.eyesClosedSoftSurface = null;
    }

    public StabilometryMeasurement(int ID, int patientID, Pose pose, MyDateTime dateTime,
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

    public string PoseToString()
    {
        switch (pose)
        {
            case (BothLegsJoinedParallel):
                return BothLegsJoinedParallelString;
            case (BothLegs30Angle):
                return BothLegs30AngleString;
            case (BothLegsParallelApart):
                return BothLegsParallelApartString;
            case (TandemLeftFront):
                return TandemLeftFrontString;
            case (TandemRightFront):
                return TandemRightFrontString;
            case (LeftLeg):
                return LeftLegString;
            case (RightLeg):
                return RightLegString;
            default:
                Debug.LogError($"Pose {pose} is not defined.");
                return "Error";
        }
    }

    public void StringToPose(string poseString)
    {
        switch (poseString)
        {
            case (BothLegsJoinedParallelString):
                this.pose = BothLegsJoinedParallel;
                break;
            case (BothLegs30AngleString):
                this.pose = BothLegs30Angle;
                break;
            case (BothLegsParallelApartString):
                this.pose = BothLegsParallelApart;
                break;
            case (TandemLeftFrontString):
                this.pose = TandemLeftFront;
                break;
            case (TandemRightFrontString):
                this.pose = TandemRightFront;
                break;
            case (LeftLegString):
                this.pose = LeftLeg;
                break;
            case (RightLegString):
                this.pose = RightLeg;
                break;
            default:
                Debug.LogError($"{poseString} is not defined.");
                break;
        }
    }
}
