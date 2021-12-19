using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class DataUploadMenuScript : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private FileImporter[] fileImporters = null;

    [SerializeField]
    private TMP_Dropdown positionDropdown = null;

    public MainScript mainScript { get; set; } = null;

    #endregion

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
        Measurement measurement = new Measurement();

        measurement.ID =  mainScript.database.GetLastMeasurementID() + 1;

        string fileName = $"{measurement.ID}.json";
        SaveJson(data, fileName);
        
        //int lastFileName = 
        //int lastParametersID = mainScript.database.GetLastParametersID();

        //SaveJson(data);

        

        //mainScript.database.
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

        positionDropdown.value = 0;
        positionDropdown.RefreshShownValue();
    }
}
