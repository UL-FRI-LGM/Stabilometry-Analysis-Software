using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    #region Variables
    public Patient CurrentPatient { get; private set; } = null;

    // Cached references
    public DatabaseScript Database { get; private set; } = null;

    public MenuSwitching MenuSwitching { get; private set; } = null;

    #endregion

    private void Awake()
    {
        Database = GetComponent<DatabaseScript>();
        MenuSwitching = GetComponent<MenuSwitching>();
    }

    public void DeleteCurrentPatient()
    {

        if (CurrentPatient != null)
            Database.DeletePatient(CurrentPatient);
        else
            Debug.LogError($"Patient does not exist.");
    }

    /// <summary>
    /// Adds a new patient and selects the patient.
    /// </summary>
    /// <param name="patient"></param>
    public void AddPatient(Patient patient)
    {
        patient.ID = Database.GetLastPatientID() + 1;
        Database.AddPatient(patient);
        SelectPatient(patient);
    }

    /// <summary>
    /// Updates the currently selected patient
    /// </summary>
    /// <param name="patient"></param>
    public void UpdatePatient(Patient patient)
    {
        Database.UpdatePatient(patient);   
    }

    /// <summary>
    /// Set a new patient
    /// </summary>
    /// <param name="patient"></param>
    public void SelectPatient(Patient patient)
    {
        CurrentPatient = patient;
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}
