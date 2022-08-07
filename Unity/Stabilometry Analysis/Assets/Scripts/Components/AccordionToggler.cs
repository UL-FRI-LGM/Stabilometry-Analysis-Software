using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccordionToggler : MonoBehaviour
{
    #region Variables
    public Toggle toggle { get; set; } = null;
    public AnalysisMenuScript analysisMenuScript {get; set;} = null;
    #endregion

    private void Awake()
    {
        toggle = transform.GetChild(0).GetComponent<Toggle>();
    }

    public void ChangeToggle()
    {
        toggle.isOn = !toggle.isOn;
        analysisMenuScript.ToggleValueChanged();
    }

}
