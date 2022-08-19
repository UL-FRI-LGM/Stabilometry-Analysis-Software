using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccordionToggler : MonoBehaviour
{
    #region Variables
    public bool toggleChanged { get; set; } = false;
    public Toggle toggle { get; set; } = null;
    public StabilometryAnalysisParameterMenuScript  analysisMenuScript {get; set;} = null;
    #endregion

    private void Awake()
    {
        toggle = transform.GetChild(0).GetComponent<Toggle>();
    }

    public void ChangeToggle()
    {
        toggle.isOn = !toggle.isOn;
        toggleChanged = true;
    }

}
