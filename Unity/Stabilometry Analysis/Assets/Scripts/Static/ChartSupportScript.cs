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

        /// <summary>
        /// Used for comparison chart.
        /// </summary>
        /// <param name="allData"></param>
        /// <param name="currentPose"></param>
        /// <param name="currentTask"></param>
        /// <param name="firstDate"></param>
        /// <param name="lastDate"></param>
        /// <param name="minimumDuration"></param>
        /// <param name="maximumDuration"></param>
        /// <returns></returns>
        public static List<StabilometryMeasurement> GetRelevantData(List<StabilometryMeasurement> allData, Pose currentPose, Task currentTask,
            MyDateTime firstDate, MyDateTime lastDate, float minimumDuration, float maximumDuration)
        {
            List<StabilometryMeasurement> temp = new List<StabilometryMeasurement>();

            foreach (StabilometryMeasurement data in allData)
                if (data.pose == currentPose && data.HasTaskData(currentTask))
                    temp.Add(data);

            return TrimData(temp, firstDate, lastDate, minimumDuration, maximumDuration);
        }
        
        /// <summary>
        /// Trims data based on date and duration limiter.
        /// </summary>
        /// <param name="inputData"></param>
        /// <param name="firstDate"></param>
        /// <param name="lastDate"></param>
        /// <param name="minimumDuration"></param>
        /// <param name="maximumDuration"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Used for comparison chart average.
        /// </summary>
        /// <param name="allData"></param>
        /// <param name="currentPose"></param>
        /// <param name="currentTask"></param>
        /// <param name="minimumDuration"></param>
        /// <param name="maximumDuration"></param>
        /// <returns></returns>
        public static List<StabilometryMeasurement> GetRelevantData(List<StabilometryMeasurement> allData, Pose currentPose, Task currentTask,
            float minimumDuration, float maximumDuration)
        {
            List<StabilometryMeasurement> temp = new List<StabilometryMeasurement>();

            foreach (StabilometryMeasurement data in allData)
                if (data.pose == currentPose && data.HasTaskData(currentTask))
                    temp.Add(data);

            return TrimData(temp, minimumDuration, maximumDuration);
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

        /// <summary>
        /// Trims data based on date and duration limiter. Used for comparison chart average.
        /// </summary>
        /// <param name="inputData"></param>
        /// <param name="minimumDuration"></param>
        /// <param name="maximumDuration"></param>
        /// <returns></returns>
        private static List<StabilometryMeasurement> TrimData(List<StabilometryMeasurement> inputData, float minimumDuration, float maximumDuration)
        {
            List<StabilometryMeasurement> result = new List<StabilometryMeasurement>();

            foreach (StabilometryMeasurement element in inputData)
            {
                StabilometryMeasurement modifiedValue = ModifyData(element, minimumDuration, maximumDuration);

                if (modifiedValue != null)
                    result.Add(modifiedValue);
            }

            return result;
        }

        /// <summary>
        /// Used for comparison chart average.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="minimumDuration"></param>
        /// <param name="maximumDuration"></param>
        /// <returns></returns>
        private static StabilometryMeasurement ModifyData(StabilometryMeasurement element, float minimumDuration, float maximumDuration)
        {
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


        public static string GetPoseName(Pose pose)
        {
            switch (pose)
            {
                case (Pose.BOTH_LEGS_JOINED_PARALLEL):
                    return "Both Legs Joined Parallel";
                case (Pose.BOTH_LEGS_30_ANGLE):
                    return "Both Legs 30° Angle";
                case (Pose.BOTH_LEGS_PARALLEL_APART):
                    return "Both Legs Parallel Apart";
                case (Pose.TANDEM_LEFT_FRONT):
                    return "Tandem Left Front";
                case (Pose.TANDEM_RIGHT_FRONT):
                    return "Tandem Right Front";
                case (Pose.LEFT_LEG):
                    return "Left Leg";
                case (Pose.RIGHT_LEG):
                    return "Right Leg";
                case (Pose.AVERAGE_FIRST_POSE):
                    return "First Pose Average";
            }

            return "ERROR";
        }

        public static string GetTaskName(Task task)
        {
            switch (task)
            {
                case (Task.EYES_OPEN_SOLID_SURFACE):
                    return "Solid Surface & Eyes Open";
                case (Task.EYES_CLOSED_SOLID_SURFACE):
                    return "Solid Surface & Eyes Closed";
                case (Task.EYES_OPEN_SOFT_SURFACE):
                    return "Soft Surface & Eyes Open";
                case (Task.EYES_CLOSED_SOFT_SURFACE):
                    return "Soft Surface & Eyes Closed";
            }

            return "ERROR";
        }

    }
}
