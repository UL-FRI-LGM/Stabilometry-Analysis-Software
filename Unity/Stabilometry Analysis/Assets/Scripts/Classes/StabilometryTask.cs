using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StabilometryAnalysis.Axes;

namespace StabilometryAnalysis
{
    public class StabilometryTask
    {
        #region Variables
        public int ID;
        public float duration,
            sampleTime;

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
        public EllipseValues confidence95Ellipse;

        public List<Vector2> stabilometryDrawData = null;

        private float drawingErrorValue = 0.001f;

        #endregion

        public StabilometryTask()
        {
        }

        public StabilometryTask(List<DataPoint> stabilometryData)
        {

            duration = CalculateDuration(stabilometryData);
            sampleTime = CalculateSampleTime(stabilometryData);

            confidence95Ellipse = new EllipseValues(stabilometryData);

            stabilometryDrawData = PrepareDataForDrawing(stabilometryData, confidence95Ellipse.mean);

            swayPath = CalculateSwayPath(stabilometryData, BOTH);
            swayPathAP = CalculateSwayPath(stabilometryData, AP);
            swayPathML = CalculateSwayPath(stabilometryData, ML);
            meanDistance = CalculateMeanDistance(stabilometryData);

            meanSwayVelocity = CalculateMeanSwayVelocity(stabilometryData, BOTH);
            meanSwayVelocityAP = CalculateMeanSwayVelocity(stabilometryData, AP);
            meanSwayVelocityML = CalculateMeanSwayVelocity(stabilometryData, ML);

            swayAverageAmplitudeAP = CalculateAverageAmplitude(stabilometryData, AP, swayPathAP);
            swayAverageAmplitudeML = CalculateAverageAmplitude(stabilometryData, ML, swayPathML);

            swayMaximalAmplitudeAP = CalculateMaximalAmplitude(stabilometryData, AP);
            swayMaximalAmplitudeML = CalculateMaximalAmplitude(stabilometryData, ML);

        }

        private List<Vector2> PrepareDataForDrawing(List<DataPoint> unfilteredData, Vector2 mean)
        {
            List<Vector2> result = new List<Vector2>();

            Vector2 previousValue = unfilteredData[0].GetVecotor2(BOTH);
            result.Add(previousValue - mean);

            for (int i = 1; i < unfilteredData.Count - 1; i++)
            {
                Vector2 currentValue = unfilteredData[i].GetVecotor2(BOTH);
                Vector2 difference = currentValue - previousValue;

                if (difference.magnitude > drawingErrorValue)
                {
                    previousValue = currentValue;
                    result.Add(currentValue - mean);
                }
            }

            result.Add(unfilteredData[unfilteredData.Count - 1].GetVecotor2(BOTH) - mean);

            return result;
        }

        private static float CalculateSampleTime(List<DataPoint> unfilteredData)
        {
            float differenceSum = 0;

            for (int i = 1; i < unfilteredData.Count; i++)
                differenceSum += unfilteredData[i].time - unfilteredData[i - 1].time;

            float averageValue = differenceSum / (unfilteredData.Count - 1);

            return 1 / (averageValue);
        }

        private static float CalculateDuration(List<DataPoint> unfilteredData)
        {
            float result = unfilteredData[unfilteredData.Count - 1].time
                - unfilteredData[0].time;

            return result;
        }

        /// <summary>
        /// Sums all the path.
        /// </summary>
        /// <param name="stabilometryData"></param>
        /// <param name="axes"></param>
        /// <returns></returns>
        private static float CalculateSwayPath(List<DataPoint> stabilometryData, Axes axes)
        {
            if (stabilometryData.Count <= 1)
                return 0f;
            //else

            float result = 0f;


            Vector2 previousValue = stabilometryData[0].GetVecotor2(axes);

            for (int i = 1; i < stabilometryData.Count; i++)
            {
                result += Vector2.Distance(previousValue, stabilometryData[i].GetVecotor2(axes));

                previousValue = stabilometryData[i].GetVecotor2(axes);
            }

            return result;
        }

        /// <summary>
        /// Mean distance of from the starting point.
        /// </summary>
        /// <param name="stabilometryData"></param>
        /// <returns></returns>
        private static float CalculateMeanDistance(List<DataPoint> stabilometryData)
        {
            if (stabilometryData.Count <= 1)
                return 0f;
            //else

            float result = 0f;


            Vector2 firstValue = stabilometryData[0].GetVecotor2(BOTH);

            for (int i = 1; i < stabilometryData.Count; i++)
                result += Vector2.Distance(stabilometryData[i].GetVecotor2(BOTH), firstValue);

            return result / stabilometryData.Count;
        }

        /// <summary>
        /// Mean velocity.
        /// </summary>
        /// <param name="stabilometryData"></param>
        /// <param name="axes"></param>
        /// <returns></returns>
        private static float CalculateMeanSwayVelocity(List<DataPoint> stabilometryData, Axes axes)
        {

            if (stabilometryData.Count <= 1)
                return 0f;
            //else

            float result = 0f;

            Vector2 previousValue = stabilometryData[0].GetVecotor2(axes);
            float previousTime = stabilometryData[0].time;

            for (int i = 1; i < stabilometryData.Count; i++)
            {
                result += Vector2.Distance(stabilometryData[i].GetVecotor2(axes), previousValue) / (stabilometryData[i].time - previousTime);
                previousValue = stabilometryData[i].GetVecotor2(axes);
                previousTime = stabilometryData[i].time;
            }

            return result / (stabilometryData.Count - 1);
        }


        /// <summary>
        /// Calculates the average amplitude in given direction.
        /// </summary>
        /// <param name="stabilometryData"></param>
        /// <param name="axes"></param>
        /// <param name="swayPath"></param>
        /// <returns></returns>
        private static float CalculateAverageAmplitude(List<DataPoint> stabilometryData, Axes axes, float swayPath)
        {

            if (stabilometryData.Count <= 1)
                return 0f;
            //else

            int directionChanges = 1;

            float previousValue = (axes == ML) ? stabilometryData[0].x : stabilometryData[0].y;
            float currentValue = (axes == ML) ? stabilometryData[1].x : stabilometryData[1].y;

            bool valueIncreasing = (currentValue > previousValue);

            for (int i = 1; i < stabilometryData.Count; i++)
            {
                currentValue = (axes == ML) ? stabilometryData[i].x : stabilometryData[i].y;

                if (valueIncreasing && (currentValue < previousValue))
                {
                    valueIncreasing = false;
                    directionChanges++;
                }
                else if (!valueIncreasing && (currentValue > previousValue))
                {
                    valueIncreasing = true;
                    directionChanges++;
                }

                previousValue = currentValue;
            }

            return swayPath / directionChanges;
        }

        /// <summary>
        /// Returns the maximum amplitude in given axis.
        /// </summary>
        /// <param name="stabilometryData"></param>
        /// <param name="axes"></param>
        /// <returns></returns>
        private static float CalculateMaximalAmplitude(List<DataPoint> stabilometryData, Axes axes)
        {

            if (stabilometryData.Count <= 1)
                return 0f;
            //else

            float maxValue = (axes == ML) ? stabilometryData[0].x : stabilometryData[0].y;
            float minValue = maxValue;

            foreach (DataPoint point in stabilometryData)
            {
                float compareValue = (axes == ML) ? point.x : point.y;

                if (compareValue > maxValue)
                    maxValue = compareValue;

                if (compareValue < minValue)
                    minValue = compareValue;
            }

            return maxValue - minValue;
        }

        public string GetDuration()
        {
            return $"{System.Math.Ceiling(duration)}";
        }

        #region Testing

        public static void TestFunctions()
        {
            List<DataPoint> testData = new List<DataPoint>();
            testData.Add(new DataPoint(0.0f, 1, 1));
            testData.Add(new DataPoint(0.1f, 3, 4));
            testData.Add(new DataPoint(0.2f, 5, 2));
            testData.Add(new DataPoint(0.3f, 2, 2));

            TetsCalculateSampleTime(testData);

            TestCalculateDuration(testData);

            TestCalculateSwayPath(testData);

            TestCalculateMeanDistance(testData);

            TestCalculateMeanSwayVelocity(testData);

            TestCalculateAverageAmplitude(testData);

            TestCalculateMaximalAmplitude(testData);

            EllipseValues.TestFunctions();
        }

        private static void TetsCalculateSampleTime(List<DataPoint> testData)
        {
            float correctValue = 10;

            float test = CalculateSampleTime(testData);

            bool pass = correctValue == test;

            if (!pass)
                Debug.LogError($"{correctValue} is not the same as {test}");

            Debug.Log($"Frequency calculation passes {pass}");
        }

        private static void TestCalculateDuration(List<DataPoint> testData)
        {
            float correctValue = 0.3f;

            float test = CalculateDuration(testData);

            bool pass = correctValue == test;

            if (!pass)
                Debug.LogError($"{correctValue} is not the same as {test}");

            Debug.Log($"Frequency calculation passes {pass}");

        }

        /// <summary>
        /// Sums all the path.
        /// </summary>
        /// <param name="stabilometryData"></param>
        /// <param name="axes"></param>
        /// <returns></returns>
        private static void TestCalculateSwayPath(List<DataPoint> testData)
        {
            float correctValueBoth = Mathf.Sqrt(13) + Mathf.Sqrt(8) + 3;

            float testBoth = CalculateSwayPath(testData, BOTH);

            bool passBoth = correctValueBoth == testBoth;

            if (!passBoth)
                Debug.LogError($"{correctValueBoth} is not the same as {testBoth}");

            float correctValueAP = 5;

            float testAP = CalculateSwayPath(testData, AP);
            bool passAP = correctValueAP == testAP;

            if (!passAP)
                Debug.LogError($"{correctValueAP} is not the same as {testAP}");


            float correctValueML = 7;

            float testML = CalculateSwayPath(testData, ML);
            bool passML = correctValueML == testML;

            if (!passML)
                Debug.LogError($"{correctValueML} is not the same as {testML}");


            Debug.Log($"Sway Path calculation passes {passBoth && passAP && passML}");
        }

        /// <summary>
        /// Mean distance from the starting point.
        /// </summary>
        /// <param name="stabilometryData"></param>
        /// <returns></returns>
        private static void TestCalculateMeanDistance(List<DataPoint> testData)
        {
            float correctValue = (Mathf.Sqrt(13) + Mathf.Sqrt(17) + Mathf.Sqrt(2)) / 4;

            float test = CalculateMeanDistance(testData);

            bool pass = correctValue == test;

            if (!pass)
                Debug.LogError($"{correctValue} is not the same as {test}");

            Debug.Log($"Mean Distance calculation passes {pass}");
        }

        /// <summary>
        /// Mean velocity.
        /// </summary>
        /// <param name="stabilometryData"></param>
        /// <param name="axes"></param>
        /// <returns></returns>
        private static void TestCalculateMeanSwayVelocity(List<DataPoint> testData)
        {

            float correctValueBoth = (Mathf.Sqrt(13) + Mathf.Sqrt(8) + 3) / (0.1f * 3f);

            float testBoth = CalculateMeanSwayVelocity(testData, BOTH);

            bool passBoth = System.Math.Round(correctValueBoth, 4) == System.Math.Round(testBoth, 4);

            if (!passBoth)
                Debug.LogError($"{correctValueBoth} is not the same as {testBoth}");

            float correctValueAP = 5 / (0.1f * 3f);

            float testAP = CalculateMeanSwayVelocity(testData, AP);
            bool passAP = System.Math.Round(correctValueAP, 4) == System.Math.Round(testAP, 4);

            if (!passAP)
                Debug.LogError($"{correctValueAP} is not the same as {testAP}");


            float correctValueML = 7 / (0.1f * 3f);

            float testML = CalculateMeanSwayVelocity(testData, ML);
            bool passML = System.Math.Round(correctValueML, 4) == System.Math.Round(testML, 4);

            if (!passML)
                Debug.LogError($"{correctValueML} is not the same as {testML}");


            Debug.Log($"Mean Sway Velocity calculation passes {passBoth && passAP && passML}");
        }


        /// <summary>
        /// Calculates the average amplitude in given direction.
        /// </summary>
        /// <param name="stabilometryData"></param>
        /// <param name="axes"></param>
        /// <param name="swayPath"></param>
        /// <returns></returns>
        private static void TestCalculateAverageAmplitude(List<DataPoint> testData)
        {
            float correctValueAP = 2.5f;

            float swayPathAP = CalculateSwayPath(testData, AP);

            float testAP = CalculateAverageAmplitude(testData, AP, swayPathAP);
            bool passAP = correctValueAP == testAP;

            if (!passAP)
                Debug.LogError($"{correctValueAP} is not the same as {testAP}");

            float correctValueML = 3.5f;

            float swayPathML = CalculateSwayPath(testData, ML);

            float testML = CalculateAverageAmplitude(testData, ML, swayPathML);
            bool passML = correctValueML == testML;

            if (!passML)
                Debug.LogError($"{correctValueML} is not the same as {testML}");


            Debug.Log($"Average Amplitude calculation passes {passAP && passML}");

        }

        /// <summary>
        /// Returns the maximum amplitude in given axis.
        /// </summary>
        /// <param name="stabilometryData"></param>
        /// <param name="axes"></param>
        /// <returns></returns>
        private static void TestCalculateMaximalAmplitude(List<DataPoint> testData)
        {

            float correctValueAP = 3f;

            float testAP = CalculateMaximalAmplitude(testData, AP);
            bool passAP = correctValueAP == testAP;

            if (!passAP)
                Debug.LogError($"{correctValueAP} is not the same as {testAP}");

            float correctValueML = 4f;

            float testML = CalculateMaximalAmplitude(testData, ML);
            bool passML = correctValueML == testML;

            if (!passML)
                Debug.LogError($"{correctValueML} is not the same as {testML}");


            Debug.Log($"Maximal Amplitude calculation passes {passAP && passML}");
        }

        #endregion
    }
}