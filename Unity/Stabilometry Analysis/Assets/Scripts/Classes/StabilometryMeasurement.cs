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
        this.pose = Pose.BOTH_LEGS_JOINED_PARALLEL;
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
            case (BOTH_LEGS_JOINED_PARALLEL):
                return BothLegsJoinedParallelString;
            case (BOTH_LEGS_30_ANGLE):
                return BothLegs30AngleString;
            case (BOTH_LEGS_PARALLEL_APART):
                return BothLegsParallelApartString;
            case (TANDEM_LEFT_FRONT):
                return TandemLeftFrontString;
            case (TANDEM_RIGHT_FRONT):
                return TandemRightFrontString;
            case (LEFT_LEG):
                return LeftLegString;
            case (RIGHT_LEG):
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
                this.pose = BOTH_LEGS_JOINED_PARALLEL;
                break;
            case (BothLegs30AngleString):
                this.pose = BOTH_LEGS_30_ANGLE;
                break;
            case (BothLegsParallelApartString):
                this.pose = BOTH_LEGS_PARALLEL_APART;
                break;
            case (TandemLeftFrontString):
                this.pose = TANDEM_LEFT_FRONT;
                break;
            case (TandemRightFrontString):
                this.pose = TANDEM_RIGHT_FRONT;
                break;
            case (LeftLegString):
                this.pose = LEFT_LEG;
                break;
            case (RightLegString):
                this.pose = RIGHT_LEG;
                break;
            default:
                Debug.LogError($"{poseString} is not defined.");
                break;
        }
    }
}
