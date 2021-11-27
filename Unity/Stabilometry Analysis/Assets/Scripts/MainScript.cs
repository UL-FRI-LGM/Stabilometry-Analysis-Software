using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    #region Variables
    private Patient selectedPatient = null;

    // Cached references
    public DatabaseScript Database { 
        get { return database; } 
    }
    private DatabaseScript database = null;
    
    public MenuSwitching MenuSwitching
    {
        get { return menuSwitching;}
    }

    private MenuSwitching menuSwitching = null;
    #endregion

    private void Awake()
    {
        database = GetComponent<DatabaseScript>();
        menuSwitching = GetComponent<MenuSwitching>();
    }

    public void DeleteCurrentPatient()
    {

        if (selectedPatient != null)
            database.DeletePatient(selectedPatient.ID);
        else
            Debug.LogError($"Patient does not exist.");
    }

    /// <summary>
    /// Set a new patient
    /// </summary>
    /// <param name="patient"></param>
    public void SelectPatient(Patient patient)
    {
        selectedPatient = patient;
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}
