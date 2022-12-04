using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace StabilometryAnalysis
{
    public class DataUploadMenuScript : MonoBehaviour
    {
        #region Variables
        [SerializeField] private FileImporter[] fileImporters = null;
        [SerializeField] private TMP_Dropdown positionDropdown = null;
        [SerializeField] private Button saveButton = null;
        [SerializeField] private TimeSelectorScript timeSelector = null;
        [SerializeField] private TMP_InputField lineSeparatorInputFiled = null;

        public MainScript mainScript { get; set; } = null;

        //[SerializeField] private StabilometryImageScript imageSciprt = null;
        #endregion

        private void Update()
        {
            if (!saveButton.interactable)
                saveButton.interactable = fileImporters[0].pathFound
                    || fileImporters[1].pathFound
                    || fileImporters[2].pathFound
                    || fileImporters[3].pathFound;
        }

        public void CancelButton()
        {
            mainScript.menuSwitching.OpenPreviousMenu();
        }

        public void SaveButton()
        {
            string dataSeparator = lineSeparatorInputFiled.text;
            if (dataSeparator == "")
                dataSeparator = ";";

            List<DataPoint>[] data = GetData(dataSeparator);

            bool dataPresent = false;
            foreach (List<DataPoint> list in data)
            {
                if (list != null)
                    dataPresent = true;
            }

            if (dataPresent)
            {
                int newID = mainScript.database.GetLastMeasurementID() + 1;
                StabilometryMeasurement measurement = 
                    JSONHandler.SaveValues(
                        newID, 
                        data, 
                        mainScript.currentPatient.ID, 
                        GetSelectedPose(), 
                        timeSelector.GetDate());

                if (measurement != null)
                    mainScript.database.AddMeasurement(measurement);

                mainScript.menuSwitching.OpenPreviousMenu();
            }
        }

        /// <summary>
        /// Gets data from file importers
        /// </summary>
        /// <returns></returns>
        private List<DataPoint>[] GetData(string dataSeparator)
        {
            List<DataPoint>[] result = new List<DataPoint>[4];

            for (int i = 0; i < fileImporters.Length; i++)
                result[i] = fileImporters[i].ReadData(dataSeparator);

            return result;
        }

        private void OnDisable()
        {
            ClearAllInputFields();
        }

        /// <summary>
        /// Clears all input fields.
        /// </summary>
        private void ClearAllInputFields()
        {
            foreach (FileImporter importer in fileImporters)
                importer.Clear();

            saveButton.interactable = false;

            positionDropdown.value = 0;
            positionDropdown.RefreshShownValue();
        }

        private Pose GetSelectedPose()
        {
            return (Pose)positionDropdown.value;
        }
    }
}