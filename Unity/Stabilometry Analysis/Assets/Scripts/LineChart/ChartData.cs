using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StabilometryAnalysis.Task;

namespace StabilometryAnalysis
{
    public class ChartData
    {
        public float
            eyesOpenSolidSurfaceValue = -1,
            eyesClosedSolidSurfaceValue = -1,
            eyesOpenSoftSurfaceValue = -1,
            eyesClosedSoftSurfaceValue = -1;
        public MyDateTime time;

        public ChartData(float eyesOpenSolidSurfaceValue, float eyesClosedSolidSurfaceValue,
            float eyesOpenSoftSurfaceValue, float eyesClosedSoftSurfaceValue, MyDateTime time)
        {
            this.eyesOpenSolidSurfaceValue = eyesOpenSolidSurfaceValue;
            this.eyesClosedSolidSurfaceValue = eyesClosedSolidSurfaceValue;
            this.eyesOpenSoftSurfaceValue = eyesOpenSoftSurfaceValue;
            this.eyesClosedSoftSurfaceValue = eyesClosedSoftSurfaceValue;
            this.time = time;
        }

        public float GetLargestValue()
        {
            float largestSolidSurface = (eyesOpenSolidSurfaceValue >= eyesClosedSolidSurfaceValue) ? eyesOpenSolidSurfaceValue : eyesClosedSolidSurfaceValue;
            float largestSoftSurface = (eyesOpenSoftSurfaceValue >= eyesClosedSoftSurfaceValue) ? eyesOpenSoftSurfaceValue : eyesClosedSoftSurfaceValue;

            return (largestSolidSurface >= largestSoftSurface) ? largestSolidSurface : largestSoftSurface;
        }

        public float GetTaskValue(Task task)
        {
            switch (task)
            {
                case (EYES_OPEN_SOLID_SURFACE):
                    return eyesOpenSolidSurfaceValue;
                case (EYES_CLOSED_SOLID_SURFACE):
                    return eyesClosedSolidSurfaceValue;
                case (EYES_OPEN_SOFT_SURFACE):
                    return eyesOpenSoftSurfaceValue;
                case (EYES_CLOSED_SOFT_SURFACE):
                    return eyesClosedSoftSurfaceValue;
            }

            Debug.LogError($"Task {task} was not selected.");
            return -1;
        }
    }
}