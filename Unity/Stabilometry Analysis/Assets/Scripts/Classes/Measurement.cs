using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Measurement
{
    public int ID,
        patientID,
        fileName,
        parameters1ID,
        parameters2ID,
        parameters3ID,
        parameters4ID;

    public Pose pose;

    public DateTime dateTime;


    public Measurement()
    {
        this.ID = -1;
        this.patientID = -1;
        this.fileName = -1;
        this.pose = Pose.BothLegsJoinedParallel;
        this.dateTime = DateTime.MinValue;
        this.parameters1ID = -1;
        this.parameters2ID = -1;
        this.parameters3ID = -1;
        this.parameters4ID = -1;
    }

    public Measurement(int ID, int patientID, int fileName, Pose pose, DateTime dateTime, int parameters1ID, int parameters2ID, int parameters3ID, int parameters4ID)
    {
        this.ID = ID;
        this.patientID = patientID;
        this.fileName = fileName;
        this.pose = pose;
        this.dateTime = dateTime;
        this.parameters1ID = parameters1ID;
        this.parameters2ID = parameters2ID;
        this.parameters3ID = parameters3ID;
        this.parameters4ID = parameters4ID;
    }
}
