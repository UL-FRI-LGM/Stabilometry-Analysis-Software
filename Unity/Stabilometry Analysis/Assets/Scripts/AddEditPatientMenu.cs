using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddEditPatientMenu : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private TMP_InputField nameInput = null,
        surnameInput = null;

    [SerializeField]
    private NotesComponent notesComponent = null;

    [SerializeField]
    private MainScript mainScript = null;

    [SerializeField]
    private Button saveButton = null;

    // adding is true
    // editing is false
    private bool addingPatient = true;
    private bool canSave = false;
    #endregion

    // Update is called once per frame
    void Update()
    {
        HandleSaveButton();
    }

    /// <summary>
    /// Enables save button, when the data can be saved
    /// </summary>
    private void HandleSaveButton()
    {
        if (canSave && !saveButton.interactable)
            saveButton.interactable = true;
        else if (!canSave && saveButton.interactable)
            saveButton.interactable = false;
    }

    /// <summary>
    /// Used for preparing the Add&Edit menu for Adding Patient.
    /// </summary>
    public void StartAddingPatient()
    {
        addingPatient = true;
    }

    public void StartEditingPatient()
    {
        //Patient currentPatient
        addingPatient = false;
    }

    /// <summary>
    /// Saves the values for patient Name, Surname and Notes.
    /// </summary>
    public void SaveButton()
    {
        Patient patient = new Patient(nameInput.text, surnameInput.text, notesComponent.text);

        if (patient != null)
        {
            if (addingPatient)
                mainScript.AddPatient(patient);
            else
            {
                patient.ID = mainScript.CurrentPatient.ID;
                mainScript.UpdatePatient(patient);
            }
        }
        else
            Debug.LogError("Patient was null.");

        mainScript.MenuSwitching.OpenInitialMenu();
    }
    
    public void NameInputChanged()
    {
        canSave = (nameInput.text != "");
    }

    private void ReadData()
    {
        // mainScript.Database.
    }

    public void CancelButton()
    {
        mainScript.MenuSwitching.OpenInitialMenu();
    }
}
