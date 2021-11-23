using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    // adding is true
    // editing is false
    private bool addingPatient = true;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartAddingPatient()
    {
        addingPatient = true;
    }

    public void StartEditingPatient()
    {
        addingPatient = false;
    }

    public void SaveButton()
    {
        if (nameInput.text.Length > 0)
        {

        }


        mainScript.MenuSwitching.OpenInitialMenu();
    }

    private void ReadData()
    {
        mainScript.Database.
    }

    public void CancelButton()
    {
        mainScript.MenuSwitching.OpenInitialMenu();
    }
}
