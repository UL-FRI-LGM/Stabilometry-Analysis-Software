using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnitialMenuScript : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private GameObject EditPatientButton = null,
        AddStabilometryButton = null,
        AnalysisButton = null,
        ReportButton = null,
        DeletePatientButon = null;
    #endregion

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Button functions

    public void AddPatientButton()
    {

    }

    public void DeletePatientButton()
    {

    }

    public void UpploadDataButton()
    {

    }

    public void EditDataButton()
    {

    }

    public void OnInputFieldChanged()
    {

    }

    public void ExitSoftwareButton()
    {
        Application.Quit();
    }
    #endregion
}
