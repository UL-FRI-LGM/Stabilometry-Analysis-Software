using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace StabilometryAnalysis
{
    public class MenuHeaderScript : MonoBehaviour
    {
        #region Variables
        [System.NonSerialized]
        public MainScript mainScript = null;

        [SerializeField] private InputDropdownComponent inputDropDownScript = null;
        #endregion

        public void EnableDropdown(bool enable)
        {
            inputDropDownScript.dropdown.interactable = enable;
        }

        /// <summary>
        /// Gets all patients from the database 
        /// </summary>
        public void SetPatientDropdown()
        {
            inputDropDownScript.SetPatientDropdown();
        }

        /// <summary>
        /// Selects the patient in the drpodown.
        /// </summary>
        /// <param name="patient"></param>
        public void SelectPatient(Patient patient)
        {
            inputDropDownScript.SelectPatient(patient);
        }

        public void ExitSoftwareButton()
        {
            Application.Quit();
        }
    }
}