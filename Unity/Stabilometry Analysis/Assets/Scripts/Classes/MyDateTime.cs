using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDateTime
{
    #region Variables

    private const long hourMult = 100,
        dayMult = 10000,
        monthMult = 1000000,
        yearMult = 100000000;

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
    public MyDateTime(long myDateTime)
    {
        year = (int)(myDateTime / yearMult);
        myDateTime -= year * yearMult;

        month = (int)(myDateTime / monthMult);
        myDateTime -= month * monthMult;

        day = (int)(myDateTime / dayMult);
        myDateTime -= day * dayMult;

        hour = (int)(myDateTime / hourMult);

        minutes = (int)(myDateTime - hour * hourMult);
    }

    /// <summary>
    /// Returns the string version of the myDateFormat.
    /// </summary>
    /// <returns></returns>
    override public string ToString()
    {
        return GetInt().ToString();
    }

    /// <summary>
    /// Returns the date in format YYYYMMDDhhmm.
    /// </summary>
    /// <returns></returns>
    public long GetInt()
    {
        long result = 0;
        result += minutes;
        result += hour * hourMult;
        result += day * dayMult;
        result += month * monthMult;
        result += year * yearMult;

        return result;
    }
}
