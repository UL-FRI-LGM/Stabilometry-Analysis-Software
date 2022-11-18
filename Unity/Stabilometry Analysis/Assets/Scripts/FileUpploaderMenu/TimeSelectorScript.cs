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

        private int year = 0;
        private int month = 0;
        private int day = 0;
        private int hour = 0;
        private int minute = 0;

        private bool fillingYear = false;
        private bool fillingMonth = false;
        private bool fillingDay = false;
        private bool fillingHour = false;
        private bool fillingMinute = false;

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
        }

        private void FillYear()
        {
            fillingYear = true;
            int currentYear = DateTime.Now.Year;

            yearDropdown.ClearOptions();

            List<string> data = new List<string>();

            for (int i = 0; i < 30; i++)
                data.Add((currentYear - i).ToString());

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
            int lastMonth = GetLastValidMonth();

            List<string> data = new List<string>();

            int monthToSelect = 0;

            for (int i = 0; i <= lastMonth; i++)
            {
                data.Add(GetMonthName(i));
                if (i == month)
                    monthToSelect = i;
            }

            month = monthToSelect;

            fillingMonth = false;

            // This evokes fill day.
            monthDropdown.value = monthToSelect;
            monthDropdown.RefreshShownValue();
        }

        private string GetMonthName(int month)
        {
            switch (month)
            {
                case 0:
                    return "January";
                case 1:
                    return "February";
                case 2:
                    return "March";
                case 3:
                    return "April";
                case 4:
                    return "May";
                case 5:
                    return "June";
                case 6:
                    return "July";
                case 7:
                    return "August";
                case 8:
                    return "September";
                case 9:
                    return "October";
                case 10:
                    return "November";
                case 11:
                    return "December";
                default:
                    Debug.LogError($"Month {month} is not defined.");
                    return "Unknwon";
            }
        }

        private void FillDay()
        {
            fillingDay = true;

            int lastDay = GetLastValidDay();


            List<string> data = new List<string>();

            int dayToSelect = 0;

            for (int i = 0; i <= lastDay; i++)
            {
                data.Add(i.ToString()); ;
                if (i == day)
                    dayToSelect = i;
            }

            day = dayToSelect;

            fillingDay = false;

            // This evokes fill day.
            monthDropdown.value = dayToSelect;
            monthDropdown.RefreshShownValue();
        }
        
        private void FillHour()
        {
            fillingHour = true;
            int lastHour = GetLastValidHour();

            fillingHour = false;
        }

        private void FillMinute()
        {
            fillingMinute = true;

            int lastMinute = GetLastValidMinute();

            float currentMinute = DateTime.Now.Minute;

            fillingMinute = false;
        }

        private int GetLastValidMonth()
        {
            return 0;
        }

        private int GetLastValidDay()
        {
            return 0;
        }

        private int GetLastValidHour()
        {
            return 0;
        }

        private int GetLastValidMinute()
        {
            return 0;
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
            FillDay();
        }

        public void DayChanged()
        {
            if (fillingDay)
                return;
            //else
            FillHour();
        }

        public void HourChanged()
        {
            if (fillingHour)
                return;
            //else
            FillMinute();
        }

        public MyDateTime GetDate()
        {
            return new MyDateTime(year, month, day, hour, minute);
        }
    }
}
