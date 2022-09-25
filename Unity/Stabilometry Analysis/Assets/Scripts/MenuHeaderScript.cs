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

        [SerializeField]
        private TMP_Dropdown patientDropdown = null;

        private List<Patient> allPatients = null;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            patientDropdown.onValueChanged.AddListener(delegate
            {
                SetPatient(patientDropdown.value);
            });

            SetPatientDropdown();
        }

        /// <summary>
        /// Gets all patients from the database 
        /// </summary>
        public void SetPatientDropdown()
        {
            allPatients = mainScript.database.GetAllPatients();

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
        /// Gives main script the correct patient.
        /// </summary>
        /// <param name="index"></param>
        private void SetPatient(int index)
        {
            Patient selectedPatient = null;

            if (index < 0 || index > allPatients.Count)
                Debug.LogError($"index {index} was outside the bounds of 0 and {allPatients.Count}.");
            else if (index > 0)
                selectedPatient = allPatients[index - 1];

            mainScript.SelectPatient(selectedPatient);
        }

        public void EnableDropdown(bool enable)
        {
            patientDropdown.interactable = enable;
        }

        /// <summary>
        /// Selects the patient in the drpodown.
        /// </summary>
        /// <param name="patient"></param>
        public void SelectPatient(Patient patient)
        {

            if (patient == null)
            {
                patientDropdown.value = 0;
                patientDropdown.RefreshShownValue();
                return;
            }

            //else

            for (int i = 0; i < allPatients.Count; i++)
            {
                if (patient.ID == allPatients[i].ID)
                {
                    patientDropdown.value = i + 1;
                    patientDropdown.RefreshShownValue();
                    return;
                }
            }
        }

        public void ExitSoftwareButton()
        {
            Application.Quit();
        }
    }
}