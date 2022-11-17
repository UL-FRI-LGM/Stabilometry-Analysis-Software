using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace StabilometryAnalysis
{

    public class TimeSelectorScript : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown yearDropdown = null;
        [SerializeField] private TMP_Dropdown monthDropdown = null;
        [SerializeField] private TMP_Dropdown dayDropdown = null;
        [SerializeField] private TMP_Dropdown hourDropdown = null;
        [SerializeField] private TMP_Dropdown minutesDropdown = null;

        public MyDateTime GetDate()
        {
            int year = 2022;
            int month = 1;
            int day = 1;
            int hour = 0;
            int minute = 0;

            return new MyDateTime(year, month, day, hour, minute);
        }
    }
}
