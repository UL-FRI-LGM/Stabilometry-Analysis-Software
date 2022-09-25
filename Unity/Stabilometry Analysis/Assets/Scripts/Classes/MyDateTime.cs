using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StabilometryAnalysis
{
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

        public string GetDisplayDateTime()
        {
            string hourString = (hour >= 10) ? hour.ToString() : $"0{hour}";
            string minutesString = (minutes >= 10) ? minutes.ToString() : $"0{minutes}";
            string time = $"{hourString}:{minutesString}";

            string dayString = (day >= 10) ? day.ToString() : $"0{day}";
            string date = $"{day}.{GetMonthWord(month)}.{year}";

            return $"{time}, {date}";
        }

        private string GetMonthWord(int month)
        {
            switch(month){
                case (1):
                    return "January";
                case (2):
                    return "February";
                case (3):
                    return "March";
                case (4):
                    return "April";
                case (5):
                    return "May";
                case (6):
                    return "June";
                case (7):
                    return "July";
                case (8):
                    return "August";
                case (9):
                    return "September";
                case (10):
                    return "October";
                case (11):
                    return "November";
                case (12):
                    return "December";
            }

            return $"unknown month {month}";
        }
    }
}