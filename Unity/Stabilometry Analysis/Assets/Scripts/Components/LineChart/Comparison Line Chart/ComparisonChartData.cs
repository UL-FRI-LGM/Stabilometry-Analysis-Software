using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StabilometryAnalysis.Task;

namespace StabilometryAnalysis
{
    public class ComparisonChartData
    {
        public float
            firstValue = -1,
            secondValue = -1;

        public MyDateTime firstTime,
            secondTime;
        public string unit = "";

        public ComparisonChartData(float firstValue, float secondValue, MyDateTime firstTime, MyDateTime secondTime, string unit)
        {
            this.firstValue = firstValue;
            this.secondValue = secondValue;
            this.firstTime = firstTime;
            this.secondTime = secondTime;
            this.unit = unit;
        }

        public float GetLargestValue()
        {
            return (firstValue >= secondValue)? firstValue : secondValue;
        }

        /// <summary>
        /// Returns first or second value.
        /// </summary>
        /// <param name="index">0 for firstValue or 1 for second value</param>
        /// <returns></returns>
        public float GetValue(int index)
        {
            if (index == 0)
                return firstValue;

            //else
            return secondValue;
        }
    }
}