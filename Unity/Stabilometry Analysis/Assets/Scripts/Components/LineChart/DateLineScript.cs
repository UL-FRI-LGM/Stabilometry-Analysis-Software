using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace StabilometryAnalysis
{
    public class DateLineScript : MonoBehaviour
    {
        #region Variables
        [SerializeField] private TextMeshProUGUI dateText = null;

        #endregion

        public void SetText(MyDateTime date, bool show)
        {
            dateText.text = date.ToStringShortNewLine();
            dateText.gameObject.SetActive(show);
        }
    }
}