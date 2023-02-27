using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StabilometryAnalysis.Pose;
using static StabilometryAnalysis.Parameter;
using static StabilometryAnalysis.Task;

namespace StabilometryAnalysis
{
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
            this.dateTime = null;
            this.pose = Pose.BOTH_LEGS_JOINED_PARALLEL;
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
            this.dateTime = dateTime;
            this.pose = pose;
            this.eyesOpenSolidSurface = eyesOpenSolidSurface;
            this.eyesClosedSolidSurface = eyesClosedSolidSurface;
            this.eyesOpenSoftSurface = eyesOpenSoftSurface;
            this.eyesClosedSoftSurface = eyesClosedSoftSurface;
        }

        public StabilometryMeasurement Duplicate()
        {
            return new StabilometryMeasurement(
                this.ID,
                this.patientID,
                this.dateTime,
                this.pose,
                this.eyesOpenSolidSurface,
                this.eyesClosedSolidSurface,
                this.eyesOpenSoftSurface,
                this.eyesClosedSoftSurface);
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

        public ChartData GetData(Parameter parameter)
        {
            float eyesOpenSolidSurfaceValue = GetValue(parameter, eyesOpenSolidSurface);
            float eyesClosedSolidSurfaceValue = GetValue(parameter, eyesClosedSolidSurface);
            float eyesOpenSoftSurfaceValue = GetValue(parameter, eyesOpenSoftSurface);
            float eyesClosedSoftSurfaceValue = GetValue(parameter, eyesClosedSoftSurface);

            return new ChartData(eyesOpenSolidSurfaceValue, eyesClosedSolidSurfaceValue,
                eyesOpenSoftSurfaceValue, eyesClosedSoftSurfaceValue, dateTime, Units.GetUnit(parameter));
        }

        private float GetValue(Parameter parameter, StabilometryTask task)
        {
            if (task == null)
                return -1;

            switch (parameter)
            {
                case (SWAY_PATH_TOTAL):
                    return task.swayPath;
                case (SWAY_PATH_AP):
                    return task.swayPathAP;
                case (SWAY_PATH_ML):
                    return task.swayPathML;
                case (MEAN_SWAY_VELOCITY_TOTAL):
                    return task.meanSwayVelocity;
                case (MEAN_SWAY_VELOCITY_AP):
                    return task.meanSwayVelocityAP;
                case (MEAN_SWAY_VELOCITY_ML):
                    return task.meanSwayVelocityML;
                case (SWAY_AVERAGE_AMPLITUDE_AP):
                    return task.swayAverageAmplitudeAP;
                case (SWAY_AVERAGE_AMPLITUDE_ML):
                    return task.swayAverageAmplitudeML;
                case (SWAY_MAXIMAL_AMPLITUDE_AP):
                    return task.swayMaximalAmplitudeAP;
                case (SWAY_MAXIMAL_AMPLITUDE_ML):
                    return task.swayMaximalAmplitudeML;
                case (MEAN_DISTANCE):
                    return task.meanDistance;
                case (ELLIPSE_AREA):
                    return task.confidence95Ellipse.area;
            }

            //else
            Debug.LogError($"Parameter {parameter} was not defined.");
            return -1;
        }

        public bool HasTaskData(Task task)
        {
            switch (task)
            {
                case EYES_OPEN_SOLID_SURFACE:
                    return (eyesOpenSolidSurface != null);
                case EYES_CLOSED_SOLID_SURFACE:
                    return (eyesClosedSolidSurface != null);
                case EYES_OPEN_SOFT_SURFACE:
                    return (eyesOpenSoftSurface != null);
                case EYES_CLOSED_SOFT_SURFACE:
                    return (eyesClosedSoftSurface != null);
            }

            Debug.LogError($"Task {task} is not defined.");
            return false;
        }
    }
}