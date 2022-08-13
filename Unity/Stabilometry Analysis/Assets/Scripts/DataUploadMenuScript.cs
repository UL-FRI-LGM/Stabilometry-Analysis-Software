using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;

public class DataUploadMenuScript : MonoBehaviour
{
    #region Variables
    [SerializeField] private FileImporter[] fileImporters = null;
    [SerializeField] private TMP_Dropdown positionDropdown = null;
    [SerializeField] private Button saveButton = null;

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
        mainScript.menuSwitching.OpenInitialMenu();
    }

    public void SaveButton()
    {
        List<DataPoint>[] data = GetData();

        bool dataPresent = false;
        foreach (List<DataPoint> list in data)
        {
            if (list != null)
                dataPresent = true;
        }

        if (dataPresent)
        {
            int newID = mainScript.database.GetLastMeasurementID() + 1;
            StabilometryMeasurement measurement = JSONHandler.SaveValues(newID, data, mainScript.currentPatient.ID, GetSelectedPose(), GetSelectedDateTime());

            if (measurement != null)
                mainScript.database.AddMeasurement(measurement);

            mainScript.menuSwitching.OpenInitialMenu();
        }
    }

    /// <summary>
    /// Gets data from file importers
    /// </summary>
    /// <returns></returns>
    private List<DataPoint>[] GetData()
    {
        List<DataPoint>[] result = new List<DataPoint>[4];

        for (int i = 0; i < fileImporters.Length; i++)
            result[i] = fileImporters[i].ReadData();

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
        Debug.LogWarning("Method GetSelectedPose not implemented.");

        return Pose.BOTH_LEGS_JOINED_PARALLEL;
    }

    private MyDateTime GetSelectedDateTime()
    {
        Debug.LogWarning("Method GetSelectedDateTime not implemented.");
        return new MyDateTime(0, 0, 0, 0, 0);
    }
}
