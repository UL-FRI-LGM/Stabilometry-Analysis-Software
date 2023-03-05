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
        private List<Patient> dropdownPatients = null;

        #endregion

        void Start()
        {
            dropdown.onValueChanged.AddListener(delegate { SetPatient(dropdown.value); });
            SetPatientDropdown();
        }

        /// <summary>
        /// Updates dropdown without refreshing the values.
        /// </summary>
        /// <param name="allPatients"></param>
        /// <param name="text"></param>
        private void UpdateDropdown(List<Patient> allPatients, string text)
        {
            relevantPatients = GetRelevantPatients(allPatients, text);
            dropdownPatients = new List<Patient>();

            if (menuHeader.mainScript.currentPatient == null)
            {
                Patient emptyPatient = new Patient(0, "No patient selected", "", "");
                dropdownPatients.Add(emptyPatient);

                for (int i = 0; i < relevantPatients.Count; i++)
                    dropdownPatients.Add(relevantPatients[i]);
            }
            else
            {
                dropdownPatients.Add(menuHeader.mainScript.currentPatient);

                for (int i = 0; i < relevantPatients.Count; i++)
                    if (relevantPatients[i].ID != menuHeader.mainScript.currentPatient.ID)
                        dropdownPatients.Add(relevantPatients[i]);
            }

            List<string> options = new List<string>();
            for (int i = 0; i < dropdownPatients.Count; i++)
                options.Add(PatientToString(dropdownPatients[i]));

            dropdown.ClearOptions();
            dropdown.AddOptions(options);
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

            if (index < 0 || index > dropdownPatients.Count)
                Debug.LogError($"index {index} was outside the bounds of 0 and {allPatients.Count}.");
            else if (index >= 0)
                selectedPatient = dropdownPatients[index];

            menuHeader.mainScript.SelectPatient(selectedPatient);
            SetPatientDropdown();
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

        private bool MatchedPatientsChanged()
        {
            string newText = inputField.text;
            List<Patient> newPatients = GetRelevantPatients(allPatients, newText);

            if (newPatients.Count != relevantPatients.Count)
                return true;

            //else
            for (int i = 0; i < newPatients.Count; i++)
                if (newPatients[i] != relevantPatients[i])
                    return true;

            //else
            return false;
        }

        public void OnSelect()
        {

            TextValueChange();

            dropdown.Show();
            dropdown.RefreshShownValue();
        }

        public void SelectDropdownElement()
        {
            inputField.SetTextWithoutNotify("");

        }

        public void EnableInputDropdown(bool enable)
        {
            inputField.interactable = enable;
            dropdown.interactable = enable;
        }

        /// <summary>
        /// Gets all patients from the database 
        /// </summary>
        public void SetPatientDropdown()
        {
            allPatients = menuHeader.mainScript.database.GetAllPatients();
            UpdateDropdown(allPatients, "");
        }


        public void TextValueChange()
        {
            if (MatchedPatientsChanged())
            {
                UpdateDropdown(allPatients, inputField.text);
                StartCoroutine(RefreshDropdown());
            }
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

            for (int i = 0; i < dropdownPatients.Count; i++)
            {
                if (patient.ID == dropdownPatients[i].ID)
                {
                    dropdown.value = i;
                    dropdown.RefreshShownValue();
                    return;
                }
            }

            //else
            SetPatientDropdown();
        }
    }
}
