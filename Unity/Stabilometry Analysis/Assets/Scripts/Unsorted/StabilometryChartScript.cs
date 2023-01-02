using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StabilometryAnalysis
{
    public class StabilometryChartScript : MonoBehaviour
    {
        #region Variables
        private float filterAccuracyValue = 0.01f;

        #endregion

        /// <summary>
        /// Draws the Stabilometry data as a line.
        /// </summary>
        /// <param name="data"></param>
        public void DrawChart(List<Vector2> data)
        {
            Debug.LogError("I was called");
            List<Vector2> filteredData = RemoveSimilarData(data);

            Debug.Log(data.Count);
            Debug.Log(filteredData.Count);
        }

        /// <summary>
        /// First
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private List<Vector2> RemoveSimilarData(List<Vector2> data)
        {
            Debug.LogError("I was called");
            List<Vector2> result = new List<Vector2>();

            if (data.Count <= 0)
                return result;

            // else

            Vector2 previousValue = data[0];
            Vector2 previousCalcValue = CalculatePosition(previousValue);

            result.Add(previousCalcValue);

            for (int i = 1; i < data.Count; i++)
            {
                Vector2 currentValue = data[i];

                if (currentValue == previousValue)
                    continue;

                // else
                Vector2 currentCalcValue = CalculatePosition(currentValue);

                if (VectorsSimilar(previousCalcValue, currentCalcValue, filterAccuracyValue))
                    continue;

                // else
                result.Add(currentCalcValue);

                previousValue = currentValue;
                previousCalcValue = currentCalcValue;
            }

            return result;
        }

        /// <summary>
        /// Calculates the value for displaying purposes
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private Vector2 CalculatePosition(Vector2 value)
        {
            Debug.LogError("I was called");
            //TODO implement this!
            return value;
        }

        /// <summary>
        /// Determines if the values are similar enough.
        /// </summary>
        /// <param name="firstVector"></param>
        /// <param name="secondVector"></param>
        /// <param name="accuracyValue"> [1,infiniy); 0 - similar only if the vectors are the same</param>
        /// <returns></returns>
        private bool VectorsSimilar(Vector2 firstVector, Vector2 secondVector, float accuracyValue)
        {
            Debug.LogError("I was called");
            Vector2 difference = firstVector - secondVector;

            return difference.sqrMagnitude <= accuracyValue;
        }
    }
}