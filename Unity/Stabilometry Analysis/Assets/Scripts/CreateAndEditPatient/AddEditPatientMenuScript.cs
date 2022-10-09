using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace StabilometryAnalysis
{
    public class AddEditPatientMenuScript : MonoBehaviour
    {
        #region Variables
        public MainScript mainScript { get; set; } = null;

        [SerializeField]
        private TMP_InputField nameInput = null,
            surnameInput = null;

        [SerializeField]
        private NotesComponent notesComponent = null;

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
            mainScript.menuHeaderScript.SelectPatient(null);
            addingPatient = true;
        }

        public void StartEditingPatient()
        {
            addingPatient = false;
            FillPatientData(mainScript.currentPatient);

        }

        private void FillPatientData(Patient patient)
        {
            if (patient == null)
                Debug.LogError("Patient was null");

            // else

            nameInput.text = patient.Name;
            surnameInput.text = patient.Surname;
            notesComponent.SetText(patient.Notes);
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
                    patient.ID = mainScript.currentPatient.ID;
                    mainScript.UpdatePatient(patient);
                }
            }
            else
                Debug.LogError("Patient was null.");

            ClearAllFields();
            mainScript.menuHeaderScript.SelectPatient(patient);
            mainScript.menuSwitching.OpenPreviousMenu();
        }

        public void NameInputChanged()
        {
            canSave = (nameInput.text != "");
        }

        public void CancelButton()
        {
            ClearAllFields();
            mainScript.menuSwitching.OpenPreviousMenu();

        }

        private void ClearAllFields()
        {
            nameInput.text = "";
            surnameInput.text = "";
            notesComponent.SetText("");
        }
    }
}