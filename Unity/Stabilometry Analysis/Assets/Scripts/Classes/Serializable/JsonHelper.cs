using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace StabilometryAnalysis
{
    public class JsonHelper
    {
        [Serializable]
        private class RawDataWrapper
        {
            public List<DataPoint> eyesOpenSolidSurface;
            public List<DataPoint> eyesClosedSolidSurface;
            public List<DataPoint> eyesOpenSoftSurface;
            public List<DataPoint> eyesClosedSoftSurface;
        }

        [Serializable]
        private class DrawingDataWrapper
        {
            public DrawingTaskValues eyesOpenSolidSurface;
            public DrawingTaskValues eyesClosedSolidSurface;
            public DrawingTaskValues eyesOpenSoftSurface;
            public DrawingTaskValues eyesClosedSoftSurface;
        }

        public static StabilometryMeasurement DataFromJson(string json, StabilometryMeasurement measurement)
        {
            DrawingDataWrapper wrapper = JsonUtility.FromJson<DrawingDataWrapper>(json);

            measurement.eyesOpenSolidSurface = GetTaskValues(measurement.eyesOpenSolidSurface, wrapper.eyesOpenSolidSurface);
            measurement.eyesClosedSolidSurface = GetTaskValues(measurement.eyesClosedSolidSurface, wrapper.eyesClosedSolidSurface);
            measurement.eyesOpenSoftSurface = GetTaskValues(measurement.eyesOpenSoftSurface, wrapper.eyesOpenSoftSurface);
            measurement.eyesClosedSoftSurface = GetTaskValues(measurement.eyesClosedSoftSurface, wrapper.eyesClosedSoftSurface);

            return measurement;
        }

        private static StabilometryTask GetTaskValues(StabilometryTask result, DrawingTaskValues values)
        {
            if (result == null)
                return null;
            //else

            result.stabilometryDrawData = values.linePoints;
            result.confidence95Ellipse.area = values.ellipseValues.area;
            result.confidence95Ellipse.eigenVectors = values.ellipseValues.eigenVectors;
            result.confidence95Ellipse.semiMajorAxis = values.ellipseValues.semiMajorAxis;
            result.confidence95Ellipse.semiMinorAxis = values.ellipseValues.semiMinorAxis;

            return result;
        }

        /// <summary>
        /// Converts stabilometry data to json string.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ToJson(List<DataPoint>[] data)
        {
            RawDataWrapper wrapper = new RawDataWrapper();
            wrapper.eyesOpenSolidSurface = data[0];
            wrapper.eyesClosedSolidSurface = data[1];
            wrapper.eyesOpenSoftSurface = data[2];
            wrapper.eyesClosedSoftSurface = data[3];

            return JsonUtility.ToJson(wrapper);
        }

        /// <summary>
        /// Converts stabilometry task to json string.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ToJson(DrawingTaskValues[] data)
        {
            DrawingDataWrapper wrapper = new DrawingDataWrapper();
            wrapper.eyesOpenSolidSurface = data[0];
            wrapper.eyesClosedSolidSurface = data[1];
            wrapper.eyesOpenSoftSurface = data[2];
            wrapper.eyesClosedSoftSurface = data[3];

            return JsonUtility.ToJson(wrapper);
        }
    }
}