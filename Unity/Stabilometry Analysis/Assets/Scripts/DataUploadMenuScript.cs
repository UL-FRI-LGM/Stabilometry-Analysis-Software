using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;

public class DataUploadMenuScript : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private FileImporter[] fileImporters = null;

    [SerializeField]
    private TMP_Dropdown positionDropdown = null;

    [SerializeField]
    private Button saveButton = null;

    public MainScript mainScript { get; set; } = null;

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
        SaveValues(GetData());
        mainScript.menuSwitching.OpenInitialMenu();
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

    /// <summary>
    /// Calculates and saves the values into the database
    /// </summary>
    /// <param name="data"></param>
    private void SaveValues(List<DataPoint>[] data)
    {
        int measurementID =  mainScript.database.GetLastMeasurementID() + 1;

        string fileName = $"{measurementID}.json";

        SaveMeasurement(data, measurementID);
        SaveJson(data, fileName);
    }

    /// <summary>
    /// Saves data as a JSON document
    /// </summary>
    /// <param name="data"></param>
    private void SaveJson(List<DataPoint>[] data, string fileName)
    {
        string json = JsonHelper.ToJson(data);

        string jsonDirectory = $@"{Application.dataPath}\JSON";

        if (!Directory.Exists(jsonDirectory))
            Directory.CreateDirectory(jsonDirectory);

        string newFilePath = $@"{jsonDirectory}\{fileName}";

        File.WriteAllText(newFilePath, json);
    }

    

    /// <summary>
    /// Calculates all measurement values and returns the object.
    /// </summary>
    /// <param name="measurementID"></param>
    /// <returns></returns>
    private void SaveMeasurement(List<DataPoint>[] Data, int measurementID)
    {
        StabilometryMeasurement measurement = new StabilometryMeasurement();

        measurement.ID = measurementID;
        mainScript.database.AddMeasurement(measurement);

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
}
