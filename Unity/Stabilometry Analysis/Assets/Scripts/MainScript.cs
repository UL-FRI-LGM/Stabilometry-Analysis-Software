using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    #region Variables
    public Patient CurrentPatient { get { return currentPatient; } }
    private Patient currentPatient = null;

    // Cached references
    public DatabaseScript Database { get { return database; } }
    private DatabaseScript database = null;

    public MenuSwitching MenuSwitching
    {
        get { return menuSwitching; }
    }

    private MenuSwitching menuSwitching = null;
    #endregion

    private void Awake()
    {
        database = GetComponent<DatabaseScript>();
        menuSwitching = GetComponent<MenuSwitching>();
    }

    private void Start()
    {
        
    }

    private void SetPatientDropdown()
    {
        List<Patient> allPatients = database.
    }

    public void DeleteCurrentPatient()
    {

        if (currentPatient != null)
            database.DeletePatient(currentPatient);
        else
            Debug.LogError($"Patient does not exist.");
    }

    /// <summary>
    /// Adds a new patient and selects the patient.
    /// </summary>
    /// <param name="patient"></param>
    public void AddPatient(Patient patient)
    {
        patient.ID = database.GetLastPatientID() + 1;
        database.AddPatient(patient);
        SelectPatient(patient);
    }

    /// <summary>
    /// Updates the currently selected patient
    /// </summary>
    /// <param name="patient"></param>
    public void UpdatePatient(Patient patient)
    {
        database.UpdatePatient(patient);   
    }

    /// <summary>
    /// Set a new patient
    /// </summary>
    /// <param name="patient"></param>
    public void SelectPatient(Patient patient)
    {
        currentPatient = patient;
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}
