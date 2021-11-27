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

    // Start is called before the first frame update
    void Start()
    {
        
    }

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
        Patient patient = PreparePatient(nameInput.text, surnameInput.text, notesComponent.text);

        if (patient != null)
        {
            if (addingPatient)
                mainScript.Database.
                    else

        }
        else
            Debug.LogError("Patient was null.");

        mainScript.MenuSwitching.OpenInitialMenu();
    }

    private Patient PreparePatient(string name, string surname, string notes)
    {
        Patient result = null;

        return result;
    }

    public void NameInputChanged(string text)
    {
        canSave = (text != "");
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
