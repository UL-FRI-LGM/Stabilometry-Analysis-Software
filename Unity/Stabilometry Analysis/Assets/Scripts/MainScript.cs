using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    #region Variables
    public int PatientId { get; set; } = -1;

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
        if (PatientId >= 0)
            database.DeletePatient(PatientId);
        else
            Debug.LogError($"Patient with id {PatientId} does not exist.");
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}
