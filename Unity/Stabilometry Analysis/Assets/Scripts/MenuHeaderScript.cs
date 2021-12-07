using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuHeaderScript : MonoBehaviour
{
    #region Variables
    [System.NonSerialized]
    public MainScript mainScript = null;

    [SerializeField]
    private TMP_Dropdown patientDropdown = null;

    private List<Patient> allPatients = null; 
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        patientDropdown.onValueChanged.AddListener(delegate
        {
            SelectPatient(patientDropdown.value);
        });
        
        SetPatientDropdown();
    }

    /// <summary>
    /// Gets all patients from the database 
    /// </summary>
    private void SetPatientDropdown()
    {
        allPatients = mainScript.Database.GetAllPatients();

        patientDropdown.ClearOptions();

        List<string> options = new List<string>();
        options.Add("No patient selected");

        for (int i = 0; i < allPatients.Count; i++)
            options.Add(PatientToString(allPatients[i]));

        patientDropdown.AddOptions(options);
    }

    /// <summary>
    /// Returns strign ready for dropdown.
    /// </summary>
    /// <param name="patient"></param>
    /// <returns></returns>
    private string PatientToString(Patient patient)
    { 
        if (patient.Surname == "")
            return $"{patient.Name}, id: {patient.ID}";
        else
            return $"{patient.Name} {patient.Surname}, id: {patient.ID}";
    }

    /// <summary>
    /// Fill gives main script the correct patient.
    /// </summary>
    /// <param name="index"></param>
    private void SelectPatient(int index)
    {
        Patient selectedPatient = null;

        if (index < 0 || index >= allPatients.Count)
            Debug.LogError($"index {index} was outside the bounds of 0 and {allPatients.Count}.");
        else if (index > 0)
            selectedPatient = allPatients[index - 1];

        mainScript.SelectPatient(selectedPatient);
    } 
}
