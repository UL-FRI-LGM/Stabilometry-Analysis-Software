using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace StabilometryAnalysis
{
    public class InputDropdownComponent : MonoBehaviour
    {
        #region Variables
        public TMP_Dropdown dropdown = null;
        [SerializeField] TMP_InputField inputField = null;
        [SerializeField] private MenuHeaderScript menuHeader = null;

        private List<Patient> allPatients = null;
        private List<Patient> relevantPatients = null;
        #endregion

        void Start()
        {
            dropdown.onValueChanged.AddListener(delegate
            {
                SetPatient(dropdown.value);
            });

            SetPatientDropdown();

        }

        /// <summary>
        /// Gets all patients from the database 
        /// </summary>
        public void SetPatientDropdown()
        {
            allPatients = menuHeader.mainScript.database.GetAllPatients();
            UpdateDropdown(allPatients, "");
        }

        private void UpdateDropdown(List<Patient> allPatients, string text)
        {
            relevantPatients = GetRelevantPatients(allPatients, text);

            Debug.Log(text);

            dropdown.ClearOptions();

            List<string> options = new List<string>();
            options.Add("No patient selected");

            for (int i = 0; i < relevantPatients.Count; i++)
                options.Add(PatientToString(relevantPatients[i]));

            dropdown.AddOptions(options);
            StartCoroutine(RefreshDropdown());

        }

        private IEnumerator RefreshDropdown()
        {
            yield return new WaitForEndOfFrame();
            dropdown.enabled = false;
            dropdown.enabled = true;
            dropdown.Show();

            inputField.Select();
        }

        private static List<Patient> GetRelevantPatients(List<Patient> allPatients, string text)
        {
            if (text == "")
                return allPatients;

            List<Patient> result = new List<Patient>();
            foreach (Patient patient in allPatients)
                if (IsPatientRelevant(patient, text))
                    result.Add(patient);

            return result;
        }

        private static bool IsPatientRelevant(Patient patient, string text)
        {
            string id = patient.ID.ToString();
            string fullName = patient.FullName().ToLower();

            return id.Contains(text) || fullName.Contains(text.ToLower());
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

            menuHeader.mainScript.SelectPatient(selectedPatient);
        }

        public void OnSelect()
        {
            dropdown.Show();
            dropdown.RefreshShownValue();
        }

        public void SelectDropdownElement()
        {
            inputField.SetTextWithoutNotify("");
        }

        /// <summary>
        /// Selects the patient in the drpodown.
        /// </summary>
        /// <param name="patient"></param>
        public void SelectPatient(Patient patient)
        {

            if (patient == null)
            {
                dropdown.value = 0;
                dropdown.RefreshShownValue();
                return;
            }

            //else

            for (int i = 0; i < relevantPatients.Count; i++)
            {
                if (patient.ID == relevantPatients[i].ID)
                {
                    dropdown.value = i + 1;
                    dropdown.RefreshShownValue();
                    return;
                }
            }
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

        public void TextValueChange()
        {
            UpdateDropdown(allPatients, inputField.text);
        }
    }
}
