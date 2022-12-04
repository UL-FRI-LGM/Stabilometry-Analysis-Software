using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StabilometryAnalysis
{
    public class MyDateTime
    {
        #region Variables

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

        

        public MyDateTime(string myDateTime)
        {
            string[] values = myDateTime.Split('.');

            year = int.Parse(values[0]);
            month = int.Parse(values[1]);
            day = int.Parse(values[2]);
            hour = int.Parse(values[3]);
            minutes = int.Parse(values[4]);
        }

        /// <summary>
        /// Returns the string version of the myDateFormat.
        /// </summary>
        /// <returns></returns>
        public string ToDatabaseString()
        {
            return $"{year}.{month}.{day}.{hour}.{minutes}";
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

        public override string ToString()
        {
            string hourString = (hour >= 10) ? hour.ToString() : $"0{hour}";
            string minutesString = (minutes >= 10) ? minutes.ToString() : $"0{minutes}";
            string time = $"{hourString}:{minutesString}";

            string dayString = (day >= 10) ? day.ToString() : $"0{day}";
            string date = $"{day}.{GetMonthWord(month)}.{year}";

            return $"{time}, {date}";
        }

        public string GetDateString()
        {
            return $"{year} {GetMonthWord(month)} {day}";
        }

        public string GetTimeString()
        {
            return $"{hour:00}:{minutes:00}";
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