using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    #region Variables
    public int PatientId { get; set; } = -1;

    // Cached references
    private DatabaseScript database = null;
    #endregion

    private void Start()
    {
        database = GetComponent<DatabaseScript>();
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
