using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DateLineScript : MonoBehaviour
{
    #region Variables
    [SerializeField] private TextMeshProUGUI dateText = null;

    #endregion

    public void SetText(MyDateTime date)
    {
        dateText.text = date.ToStringShort();
    }
}
