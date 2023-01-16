using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace StabilometryAnalysis
{
    public class AccordionDropdownSelector : MonoBehaviour
    {
        public bool valueChanged { get; set; } = false;
        public float durationValue { get; set; } = 0;
        public MyDateTime dateValue { get; set; } = null;
        [SerializeField] private TMP_Dropdown dropdown = null;

        private List<int> durations = null;
        private List<MyDateTime> dates = null;

        private bool isDateLimiter = false;
        private bool fillingData = false;

        public void SetDurations(List<int> durations, bool isLower)
        {
            isDateLimiter = false;
            this.durations = durations;
            SetDropdown(DurationsToString(durations), isLower);
            ChangeValue();
        }

        public void SetDates(List<MyDateTime> dates, bool isLower)
        {
            isDateLimiter = true;
            this.dates = dates;
            SetDropdown(DatesToString(dates), isLower);

            ChangeValue();
        }

        public void ChangeValue()
        {
            if (fillingData)
                return;

            if (isDateLimiter)
                this.dateValue = dates[dropdown.value];
            else
                this.durationValue = durations[dropdown.value];

            valueChanged = true;
        }

        private List<string> DurationsToString(List<int> durations)
        {
            List<string> result = new List<string>();
            foreach (int element in durations)
                result.Add(element.ToString());

            return result;
        }

        private List<string> DatesToString(List<MyDateTime> dates)
        {
            List<string> result = new List<string>();
            foreach (MyDateTime element in dates)
                result.Add(element.ToStringShort());

            return result;
        }

        private void SetDropdown(List<string> data, bool isLower)
        {
            fillingData = true;
            dropdown.ClearOptions();
            dropdown.AddOptions(data);
            fillingData = false;
            
            int index = (isLower)? 0 : data.Count - 1;

            dropdown.value = index;
        }
    }
}
