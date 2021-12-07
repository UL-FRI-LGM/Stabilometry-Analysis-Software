using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialMenuScript : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private GameObject EditPatientButton = null,
        AddStabilometryButton = null,
        AnalysisButton = null,
        ReportButton = null,
        DeletePatientButon = null;

    [SerializeField]
    private NotesComponent notesComponent = null;

    [System.NonSerialized]
    public MainScript mainScrpit = null;
    #endregion

    /// <summary>
    /// Selects the correct patient.
    /// </summary>
    /// <param name="patient"></param>
    public void SelectPatient(Patient patient)
    {
        SetButtonsActive(patient);

        if (patient == null)
            notesComponent.SetText("");
        else
            notesComponent.SetText(patient.Notes);

    }
    
    /// <summary>
    /// Handles enabling and disabling buttons.
    /// </summary>
    /// <param name="patient"></param>
    private void SetButtonsActive(Patient patient)
    {
        bool patientSelected = (patient != null);

        EditPatientButton.SetActive(patientSelected);
        AddStabilometryButton.SetActive(patientSelected);
        DeletePatientButon.SetActive(patientSelected);

        bool patientHasData = false;
        if (patientSelected)
            patientHasData = (mainScrpit.Database.GetNumberOfDataEntries(patient) > 0);

        AnalysisButton.SetActive(patientHasData);
        ReportButton.SetActive(patientHasData)
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
