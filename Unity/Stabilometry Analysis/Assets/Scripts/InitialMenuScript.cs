using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace StabilometryAnalysis
{
    public class InitialMenuScript : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        private GameObject editPatientButton = null,
            addStabilometryButton = null,
            analysisButton = null,
            deletePatientButon = null;

        [SerializeField]
        private NotesComponent notesComponent = null;
        
        [SerializeField]
        private MainScript mainScript = null;
        #endregion

        /// <summary>
        /// Selects the correct patient.
        /// </summary>
        /// <param name="patient"></param>
        public void SelectPatient(Patient patient)
        {
            SetButtonsInteractable(patient);

            if (patient == null)
                notesComponent.SetText("");
            else
                notesComponent.SetText(patient.Notes);

        }

        /// <summary>
        /// Handles enabling and disabling buttons.
        /// </summary>
        /// <param name="patient"></param>
        private void SetButtonsInteractable(Patient patient)
        {
            bool patientSelected = (patient != null);

            editPatientButton.GetComponent<Button>().interactable = patientSelected;
            addStabilometryButton.GetComponent<Button>().interactable = patientSelected;
            deletePatientButon.GetComponent<Button>().interactable = patientSelected;

            bool patientHasData = false;
            if (patientSelected)
                patientHasData = (mainScript.database.GetNumberOfDataEntries(patient) > 0);

            analysisButton.GetComponent<Button>().interactable = patientHasData;
        }

        public void OnInputFieldChange()
        {
            Debug.LogWarning("Function not implemented.");
        }

        private void OnEnable()
        {
            mainScript.menuHeaderScript.EnableDropdown(true);
        }

        private void OnDisable()
        {
            mainScript.menuHeaderScript.EnableDropdown(false);
        }

        #region Button functions

        public void AddPatientButton(GameObject addPatientMenu)
        {
            //mainScrpit.Menu
            mainScript.addEditPatientMenu.StartAddingPatient();
            mainScript.menuSwitching.OpenMenu(addPatientMenu);
        }

        public void EditPatientButton(GameObject editPatientMenu)
        {
            mainScript.addEditPatientMenu.StartEditingPatient();
            mainScript.menuSwitching.OpenMenu(editPatientMenu);
        }

        public void AddAnalysisDataButton(GameObject upploadDataMenu)
        {
            mainScript.menuSwitching.OpenMenu(upploadDataMenu);
        }

        public void AnalysisButton(GameObject analysisMenu)
        {
            mainScript.menuSwitching.OpenMenu(analysisMenu);
        }

        public void ReportButton(GameObject reportMenu)
        {
            mainScript.menuSwitching.OpenMenu(reportMenu);
        }

        public void DeletePatientButton()
        {
            mainScript.DeleteCurrentPatient();
        }

        public void StartDeletionOfAllData()
        {
            ShowDataDeletionWarning();
        }

        public void DeleteAllData()
        {
            mainScript.database.CloseDatabase();

            DirectoryInfo directory = new DirectoryInfo(Application.persistentDataPath);

            foreach (FileInfo file in directory.GetFiles())
            {
                file.Delete();
            }

            foreach (DirectoryInfo dir in directory.GetDirectories())
            {
                dir.Delete(true);
            }

            mainScript.ExitApplication();
        }

        // TODO make this work
        private void ShowDataDeletionWarning()
        {
            Debug.LogWarning("make this work");
            DeleteAllData();
        }
        #endregion
    }
}