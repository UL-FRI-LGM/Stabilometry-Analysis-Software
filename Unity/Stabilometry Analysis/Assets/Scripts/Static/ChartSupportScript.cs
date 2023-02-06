using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StabilometryAnalysis
{
    public class ChartSupportScript
    {
        /// <summary>
        /// Gets the position for the next line chart to spawn.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="originalPosition"></param>
        /// <param name="chartSize"></param>
        /// <returns></returns>
        public static Vector2 GetNewPosition(int index, Vector2 originalPosition, Vector2 chartSize)
        {
            float x = (index % 2 == 0) ? originalPosition.x : -originalPosition.x;
            int row = index / 2;
            float y = originalPosition.y - row * (chartSize.y + 20);

            return new Vector2(x, y);
        }

        public static bool ListHasDate(List<MyDateTime> list, MyDateTime date)
        {
            foreach (MyDateTime element in list)
            {
                if (date.IsTheSame(element))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Checks parameters to see which charts to spawn.
        /// </summary>
        /// <param name="parameterTogglers"></param>
        /// <returns></returns>
        public static List<Parameter> GetChosenParameters(AccordionToggler[] parameterTogglers)
        {
            List<Parameter> result = new List<Parameter>();

            for (int i = 0; i < parameterTogglers.Length; i++)
            {
                if (parameterTogglers[i].GetToggle().isOn)
                    result.Add((Parameter)i);
            }

            return result;
        }

        public static List<int> OrderList(List<int> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                for (int k = i - 1; 0 <= k; k--)
                {
                    if (list[k + 1] > list[k])
                        break;
                    //else

                    int temp = list[k];
                    list[k] = list[k + 1];
                    list[k + 1] = temp;
                }
            }

            return list;
        }

        public static List<StabilometryMeasurement> GetRelevantData(List<StabilometryMeasurement> allData, Pose currentPose, 
            MyDateTime firstDate, MyDateTime lastDate, float minimumDuration, float maximumDuration)
        {
            List<StabilometryMeasurement> temp = new List<StabilometryMeasurement>();

            foreach (StabilometryMeasurement data in allData)
                if (data.pose == currentPose)
                    temp.Add(data);

            return TrimData(temp, firstDate, lastDate, minimumDuration, maximumDuration);
        }

        private static List<StabilometryMeasurement> TrimData(List<StabilometryMeasurement> inputData, 
            MyDateTime firstDate, MyDateTime lastDate, float minimumDuration, float maximumDuration)
        {
            List<StabilometryMeasurement> result = new List<StabilometryMeasurement>();

            foreach (StabilometryMeasurement element in inputData)
            {
                StabilometryMeasurement modifiedValue = ModifyData(element, firstDate, lastDate, minimumDuration, maximumDuration);

                if (modifiedValue != null)
                    result.Add(modifiedValue);
            }

            return result;
        }

        private static StabilometryMeasurement ModifyData(StabilometryMeasurement element, 
            MyDateTime firstDate, MyDateTime lastDate, float minimumDuration, float maximumDuration)
        {

            // If first date value is not defined
            if (firstDate == null)
                return element;

            if (element.dateTime.IsSmaller(firstDate) || element.dateTime.IsGreater(lastDate))
                return null;
            // else

            StabilometryMeasurement result = element.Duplicate();
            result.eyesOpenSolidSurface = CheckDuration(result.eyesOpenSolidSurface, minimumDuration, maximumDuration);
            result.eyesClosedSolidSurface = CheckDuration(result.eyesClosedSolidSurface, minimumDuration, maximumDuration);
            result.eyesOpenSoftSurface = CheckDuration(result.eyesOpenSoftSurface, minimumDuration, maximumDuration);
            result.eyesClosedSoftSurface = CheckDuration(result.eyesClosedSoftSurface, minimumDuration, maximumDuration);

            if (result.eyesOpenSolidSurface == null && result.eyesClosedSolidSurface == null
                && result.eyesOpenSoftSurface == null && result.eyesClosedSoftSurface == null)
                return null;

            return result;
        }

        public static List<int> GetDurations(List<int> durationList, StabilometryMeasurement data)
        {
            List<int> result = new List<int>();

            if (data.eyesOpenSolidSurface != null)
            {
                int eyesOpenSolidSurfaceDuration = data.eyesOpenSolidSurface.GetDuration();

                if (data.eyesOpenSolidSurface != null && !durationList.Contains(eyesOpenSolidSurfaceDuration))
                    result.Add(eyesOpenSolidSurfaceDuration);
            }

            if (data.eyesClosedSolidSurface != null)
            {
                int eyesClosedSolidSurfaceDuration = data.eyesClosedSolidSurface.GetDuration();

                if (data.eyesClosedSolidSurface != null && !durationList.Contains(eyesClosedSolidSurfaceDuration))
                    if (!result.Contains(eyesClosedSolidSurfaceDuration))
                        result.Add(eyesClosedSolidSurfaceDuration);
            }

            if (data.eyesOpenSoftSurface != null)
            {
                int eyesOpenSoftSurfaceDuration = data.eyesOpenSoftSurface.GetDuration();

                if (data.eyesOpenSoftSurface != null && !durationList.Contains(eyesOpenSoftSurfaceDuration))
                    if (!result.Contains(eyesOpenSoftSurfaceDuration))
                        result.Add(eyesOpenSoftSurfaceDuration);
            }

            if (data.eyesClosedSoftSurface != null)
            {
                int eyesClosedSoftSurfaceDuration = data.eyesClosedSoftSurface.GetDuration();

                if (data.eyesClosedSoftSurface != null && !durationList.Contains(eyesClosedSoftSurfaceDuration))
                    if (!result.Contains(eyesClosedSoftSurfaceDuration))
                        result.Add(eyesClosedSoftSurfaceDuration);
            }

            return result;
        }

        private static StabilometryTask CheckDuration(StabilometryTask task, float minimumDuration, float maximumDuration)
        {
            if (task == null)
                return null;

            if (task.GetDuration() < minimumDuration || task.GetDuration() > maximumDuration)
                return null;

            return task;
        }

        public static List<StabilometryMeasurement> SortMeasurements(List<StabilometryMeasurement> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                for (int k = i - 1; 0 <= k; k--)
                {
                    MyDateTime nextTime = list[k + 1].dateTime;
                    MyDateTime currentTime = list[k].dateTime;

                    if (nextTime.IsTheSame(currentTime) || nextTime.IsGreater(currentTime))
                        break;
                    //else

                    StabilometryMeasurement temp = list[k];
                    list[k] = list[k + 1];
                    list[k + 1] = temp;
                }
            }

            return list;
        }
    }
}
