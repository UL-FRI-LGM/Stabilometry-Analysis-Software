using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDateTime
{
    #region Variables

    private const float
        minuteMult = 0.0001f,
        hourMult = 0.01f,
        dayMult = 1,
        monthMult = 100,
        yearMult = 10000;

    public int
        year = 0,
        month = 0,
        day = 0,
        hour = 0,
        minutes = 0;

    #endregion

    public MyDateTime(int year, int month, int day, int hour, int minutes)
    {
        this.year = year;
        this.month = month;
        this.day = day;
        this.hour = hour;
        this.minutes = minutes;
    }

    /// <summary>
    /// Takes in myDateTime format and fills the correct values.
    /// </summary>
    /// <param name="myDateTime"></param>
    public MyDateTime(float myDateTime)
    {
        year = (int)(myDateTime / yearMult);
        myDateTime -= year * yearMult;

        month = (int)(myDateTime / monthMult);
        myDateTime -= month * monthMult;

        day = (int)(myDateTime / dayMult);
        myDateTime -= day * dayMult;

        hour = (int)(myDateTime / hourMult);

        minutes = (int)((myDateTime - hour * hourMult) / minuteMult);
    }

    /// <summary>
    /// Returns the string version of the myDateFormat.
    /// </summary>
    /// <returns></returns>
    override public string ToString()
    {
        return GetFloat().ToString();
    }

    public string ToStringShort()
    {
        string hourString = (hour < 10) ? $"0{hour}" : hour.ToString();
        string minuteString = (minutes < 10) ? $"0{minutes}" : minutes.ToString();

        string dayString = (day < 10) ? $"0{day}" : day.ToString();
        string monthString = (month < 10) ? $"0{month}" : month.ToString();
        
        int shortYear = year % 100;
        string yearString = (shortYear < 10) ? $"0{shortYear}" : shortYear.ToString();

        return $"{hourString}:{minuteString}\n{dayString}.{monthString}.{yearString}";
    }

    /// <summary>
    /// Returns the date in format YYYYMMDDhhmm.
    /// </summary>
    /// <returns></returns>
    public float GetFloat()
    {
        float result = 0;
        result += minutes * minuteMult;
        result += hour * hourMult;
        result += day * dayMult;
        result += month * monthMult;
        result += year * yearMult;

        return result;
    }
}
