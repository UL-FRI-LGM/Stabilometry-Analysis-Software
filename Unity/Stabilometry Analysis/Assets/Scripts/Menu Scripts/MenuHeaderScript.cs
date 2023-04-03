using UnityEngine;

namespace StabilometryAnalysis
{
    public class MenuHeaderScript : MonoBehaviour
    {
        #region Variables
        [System.NonSerialized]
        public MainScript mainScript = null;

        [SerializeField] private InputDropdownComponent inputDropDownScript = null;
        [SerializeField] private BackgroundBlockerScript backgroundBlocker = null;
        [SerializeField] private GameObject dropdownBlocker = null;
        #endregion

        public void EnableDropdown(bool enable)
        {
            inputDropDownScript.EnableInputDropdown(enable);
            dropdownBlocker.SetActive(!enable);
            
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
            backgroundBlocker.StartExitingApplication();
        }
    }
}