﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StabilometryAnalysis
{
    public class MainScript : MonoBehaviour
    {
        #region Variables
        public InitialMenuScript initialMenu = null;
        public AddEditPatientMenuScript addEditPatientMenu = null;
        public MenuHeaderScript menuHeaderScript = null;
        public DataUploadMenuScript dataUploadMenuScript = null;
        public StabilometryImagesMenuScript stabilometryImagesMenuScript = null;
        public StabilometryAnalysisParameterMenuScript stabilometryAnalysisParameterMenuScript = null;
        public StabilometryAnalysisMenuScript stabilometryAnalysisMenuScript = null;
        public StabilometryMeasurementScript stabilometryMeasurementScript = null;

        // Cached references
        public Patient currentPatient { get; private set; } = null;
        public DatabaseScript database { get; private set; } = null;
        public MenuSwitching menuSwitching { get; private set; } = null;

        #endregion

        private void Awake()
        {
            database = GetComponent<DatabaseScript>();
            menuSwitching = GetComponent<MenuSwitching>();
            SetAllReferences();

        }

        private void SetAllReferences()
        {
            addEditPatientMenu.mainScript = this;
            menuHeaderScript.mainScript = this;
            dataUploadMenuScript.mainScript = this;
            stabilometryAnalysisMenuScript.mainScript = this;
            stabilometryAnalysisParameterMenuScript.mainScript = this;
            stabilometryMeasurementScript.mainScript = this;

            //stabilometryImagesMenuScript.mainScript = this;
        }

        public void DeleteCurrentPatient()
        {

            if (currentPatient != null)
            {
                database.DeletePatient(currentPatient);
                menuHeaderScript.SetPatientDropdown();
                menuHeaderScript.SelectPatient(null);
            }
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
            menuHeaderScript.SetPatientDropdown();
        }

        /// <summary>
        /// Updates the currently selected patient
        /// </summary>
        /// <param name="patient"></param>
        public void UpdatePatient(Patient patient)
        {
            database.UpdatePatient(patient);
            menuHeaderScript.SetPatientDropdown();
        }

        /// <summary>
        /// Set a new patient
        /// </summary>
        /// <param name="patient"></param>
        public void SelectPatient(Patient patient)
        {
            currentPatient = patient;
            initialMenu.SelectPatient(patient);

            menuSwitching.OpenInitialMenu();
        }

        public void ExitApplication()
        {
            Application.Quit();
        }
    }
}