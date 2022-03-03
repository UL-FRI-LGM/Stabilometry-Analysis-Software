using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Axes;

public class StabilometryTask
{
    #region Variables
    public int ID;
    public float duration,
        frequency;

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
    public EllipseValues confidence95EllipseArea;

    public List<Vector2> stabilometryDrawData = null;

    private float drawingErrorValue = 0.1f;

    #endregion

    public StabilometryTask()
    {
    }

    public StabilometryTask(List<DataPoint> stabilometryData)
    {
        duration = CalculateDuration(stabilometryData);
        frequency = CalculateFrequency(stabilometryData);

        List<DataPoint> filteredData = FilterData(stabilometryData);

        stabilometryDrawData = PrepareDataForDrawing(filteredData);


        swayPath = CalculateSwayPath(filteredData, Both);
        swayPathAP = CalculateSwayPath(filteredData, AP);
        swayPathML = CalculateSwayPath(filteredData, ML);
        meanDistance = CalculateMeanDistance(filteredData);

        meanSwayVelocity = CalculateMeanSwayVelocity(filteredData, Both);
        meanSwayVelocityAP = CalculateMeanSwayVelocity(filteredData, AP);
        meanSwayVelocityML = CalculateMeanSwayVelocity(filteredData, ML);

        swayAverageAmplitudeAP = CalculateAverageAmplitude(filteredData, AP, swayPathAP);
        swayAverageAmplitudeML = CalculateAverageAmplitude(filteredData, ML, swayPathML);

        swayMaximalAmplitudeAP = CalculateMaximalAmplitude(filteredData, AP);
        swayMaximalAmplitudeML = CalculateMaximalAmplitude(filteredData, ML);

        confidence95EllipseArea = new EllipseValues(filteredData);
    }

    /// <summary>
    /// Filters the data to remove noise.
    /// </summary>
    /// <param name="unfilteredData"></param>
    /// <returns></returns>
    private List<DataPoint> FilterData(List<DataPoint> unfilteredData)
    {
        // TODO: add filters
        return unfilteredData;
    }

    private List<Vector2> PrepareDataForDrawing(List<DataPoint> unfilteredData)
    {
        List<Vector2> result = new List<Vector2>();

        Vector2 previousValue = unfilteredData[0].GetVecotor2(Both);
        result.Add(previousValue);

        for (int i = 1; i < unfilteredData.Count; i++)
        {
            Vector2 currentValue = unfilteredData[i].GetVecotor2(Both);
            Vector2 difference = currentValue - previousValue;

            if (difference.magnitude > drawingErrorValue)
            {
                previousValue = currentValue;
                result.Add(currentValue);
            }
        }

        return result;
    }

    private float CalculateFrequency(List<DataPoint> unfilteredData)
    {
        float result = 0;

        for (int i = 1; i < unfilteredData.Count; i++)
            result += unfilteredData[i].time - unfilteredData[i - 1].time;

        return result / (unfilteredData.Count - 1);
    }

    private float CalculateDuration(List<DataPoint> unfilteredData)
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
    private float CalculateSwayPath(List<DataPoint> stabilometryData, Axes axes)
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
    private float CalculateMeanDistance(List<DataPoint> stabilometryData)
    {
        if (stabilometryData.Count <= 1)
            return 0f;
        //else

        float result = 0f;


        Vector2 firstValue = stabilometryData[0].GetVecotor2(Both);

        for (int i = 1; i < stabilometryData.Count; i++)
            result += Vector2.Distance(stabilometryData[i].GetVecotor2(Both), firstValue);

        return result / stabilometryData.Count;
    }

    /// <summary>
    /// Mean velocity.
    /// </summary>
    /// <param name="stabilometryData"></param>
    /// <param name="axes"></param>
    /// <returns></returns>
    private float CalculateMeanSwayVelocity(List<DataPoint> stabilometryData, Axes axes)
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
    private float CalculateAverageAmplitude(List<DataPoint> stabilometryData, Axes axes, float swayPath)
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
    private float CalculateMaximalAmplitude(List<DataPoint> stabilometryData, Axes axes)
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

}