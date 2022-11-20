using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace StabilometryAnalysis
{

    public class TimeSelectorScript : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown yearDropdown = null;
        [SerializeField] private TMP_Dropdown monthDropdown = null;
        [SerializeField] private TMP_Dropdown dayDropdown = null;
        [SerializeField] private TMP_Dropdown hourDropdown = null;
        [SerializeField] private TMP_Dropdown minutesDropdown = null;

        // [current year - 30, current year]
        private int year = 0;
        // [1, 12]
        private int month = 0;
        // [1, n] <- n can be 28,29,30,31 
        private int day = 0;
        // [0, 23]
        private int hour = 0;
        // [0, 59]
        private int minute = 0;

        private bool fillingYear = false;
        private bool fillingMonth = false;
        private bool fillingDay = false;
        private bool fillingHour = false;
        private bool fillingMinutes = false;

        private const int MAX_MONTHS = 12;
        private const int MAX_HOURS = 23;
        private const int MAX_MINUTES = 59;

        private void OnEnable()
        {
            SetCurrentTime();
        }

        private void SetCurrentTime()
        {
            DateTime timeNow = DateTime.Now;
            year = timeNow.Year;
            month = timeNow.Month;
            day = timeNow.Day;
            hour = timeNow.Hour;
            minute = timeNow.Minute;

            FillYear();
            FillMonth();
            FillDay();
            FillHour();
            FillMinutes();
        }

        private void FillYear()
        {
            fillingYear = true;
            int currentYear = DateTime.Now.Year;

            List<string> data = new List<string>();

            for (int i = 0; i < 30; i++)
                data.Add((currentYear - i).ToString());

            yearDropdown.ClearOptions();
            yearDropdown.AddOptions(data);

            year = currentYear;
            fillingYear = false;

            // This evokes fill month.
            yearDropdown.value = 0;
            yearDropdown.RefreshShownValue();
        }

        private void FillMonth()
        {
            fillingMonth = true;
            int lastMonth = GetLastValidMonth(year);

            List<string> data = new List<string>();

            int monthToSelect = 1;

            for (int i = 1; i <= lastMonth; i++)
            {
                data.Add(GetMonthName(i));
                if (i == month)
                    monthToSelect = i;
            }

            month = monthToSelect;

            monthDropdown.ClearOptions();
            monthDropdown.AddOptions(data);

            fillingMonth = false;

            // This evokes fill day.
            monthDropdown.value = monthToSelect - 1;
            monthDropdown.RefreshShownValue();
        }

        private string GetMonthName(int month)
        {
            switch (month)
            {
                case 1:
                    return "January";
                case 2:
                    return "February";
                case 3:
                    return "March";
                case 4:
                    return "April";
                case 5:
                    return "May";
                case 6:
                    return "June";
                case 7:
                    return "July";
                case 8:
                    return "August";
                case 9:
                    return "September";
                case 10:
                    return "October";
                case 11:
                    return "November";
                case 12:
                    return "December";
                default:
                    Debug.LogError($"Month {month} is not defined.");
                    return "Unknwon";
            }
        }

        private void FillDay()
        {
            fillingDay = true;

            int lastDay = GetLastValidDay(year, month);

            List<string> data = new List<string>();

            int dayToSelect = 0;

            for (int i = 1; i <= lastDay; i++)
            {
                data.Add(i.ToString()); ;
                if (i == day)
                    dayToSelect = i;
            }

            day = dayToSelect;

            dayDropdown.ClearOptions();
            dayDropdown.AddOptions(data);

            fillingDay = false;

            // This evokes fill hour.
            dayDropdown.value = dayToSelect - 1;
            dayDropdown.RefreshShownValue();
        }

        private void FillHour()
        {
            fillingHour = true;
            int lastHour = GetLastValidHour(year, month, day);

            int hourToSelect = 0;

            List<string> data = new List<string>();

            for (int i = 0; i <= lastHour; i++)
            {
                data.Add(i.ToString()); ;
                if (i == hour)
                    hourToSelect = i;
            }

            hourDropdown.ClearOptions();
            hourDropdown.AddOptions(data);

            fillingHour = false;

            // This evokes fill minutes.
            hourDropdown.value = hourToSelect;
            hourDropdown.RefreshShownValue();
        }

        private void FillMinutes()
        {
            fillingMinutes = true;

            int lastMinute = GetLastValidMinute(year, month, day, hour);
            List<string> data = new List<string>();

            for (int i = 0; i <= lastMinute; i++)
            {
                data.Add(i.ToString()); ;
                if (i == minute)
                    lastMinute = i;
            }

            minutesDropdown.ClearOptions();
            minutesDropdown.AddOptions(data);

            fillingMinutes = false;

            minutesDropdown.value = lastMinute;
            minutesDropdown.RefreshShownValue();
        }

        private int GetLastValidMonth(int selectedYear)
        {
            if (DateTime.Now.Year > selectedYear)
                return MAX_MONTHS;

            //else
            return DateTime.Now.Month;
        }

        private int GetLastValidDay(int selectedYear, int selectedMonth)
        {
            int maxDays = GetMaxDays(selectedYear, selectedMonth);

            if (DateTime.Now.Year > selectedYear)
                return maxDays;

            //else if year is the same
            if (DateTime.Now.Month > selectedMonth)
                return maxDays;

            //else
            return DateTime.Now.Day;
        }

        private int GetMaxDays(int year, int month)
        {
            switch (month)
            {
                case 1:
                    return 31;
                case 2:

                    if (DateTime.IsLeapYear(year))
                        return 29;
                    else
                        return 28;
                case 3:
                    return 31;
                case 4:
                    return 30;
                case 5:
                    return 31;
                case 6:
                    return 30;
                case 7:
                    return 31;
                case 8:
                    return 31;
                case 9:
                    return 30;
                case 10:
                    return 31;
                case 11:
                    return 30;
                case 12:
                    return 31;
                default:
                    Debug.LogError($"Month {month} is not defined.");
                    return 31;
            }

        }

        private int GetLastValidHour(int selectedYear, int selectedMonth, int selectedDay)
        {
            if (DateTime.Now.Year > selectedYear)
                return MAX_HOURS;

            //else if year is the same
            if (DateTime.Now.Month > selectedMonth)
                return MAX_HOURS;

            //else if month is the same
            if (DateTime.Now.Day > selectedDay)
                return MAX_MINUTES;

            //else
            return DateTime.Now.Hour;
        }

        private int GetLastValidMinute(int selectedYear, int selectedMonth, int selectedDay, int selectedHour)
        {
            if (DateTime.Now.Year > selectedYear)
                return MAX_MINUTES;

            //else if year is the same
            if (DateTime.Now.Month > selectedMonth)
                return MAX_MINUTES;

            //else if month is the same
            if (DateTime.Now.Day > selectedDay)
                return MAX_MINUTES;

            //else if day is the same
            if (DateTime.Now.Hour > selectedHour)
                return MAX_MINUTES;

            //else
            return DateTime.Now.Minute;
        }

        public void YearChanged()
        {
            if (fillingYear)
                return;

            //else
            year = int.Parse(yearDropdown.options[yearDropdown.value].text);
            FillMonth();
        }

        public void MonthChanged()
        {
            if (fillingMonth)
                return;
            //else
            month = monthDropdown.value + 1;
            FillDay();
        }

        public void DayChanged()
        {
            if (fillingDay)
                return;
            //else
            day = dayDropdown.value + 1;
            FillHour();
        }

        public void HourChanged()
        {
            if (fillingHour)
                return;
            //else
            hour = hourDropdown.value;

            FillMinutes();
        }

        public void MinuteChanged()
        {
            if (fillingMinutes)
                return;
            //else
            minute = minutesDropdown.value;
        }

        public MyDateTime GetDate()
        {
            return new MyDateTime(year, month, day, hour, minute);
        }
    }
}
