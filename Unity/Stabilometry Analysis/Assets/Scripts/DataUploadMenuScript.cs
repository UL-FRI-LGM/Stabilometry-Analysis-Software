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
        List<DataPoint>[] data = GetData();

        bool dataPresent = false;
        foreach (List<DataPoint> list in data)
        {
            if (list != null)
                dataPresent = true;
        }

        if (dataPresent)
        {
            SaveValues(data);
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

    /// <summary>
    /// Calculates and saves the values into the database
    /// </summary>
    /// <param name="data"></param>
    private void SaveValues(List<DataPoint>[] data)
    {
        StabilometryMeasurement measurement = new StabilometryMeasurement();
        measurement.ID = mainScript.database.GetLastMeasurementID() + 1;
        measurement.patientID = mainScript.currentPatient.ID;
        measurement.pose = GetSelectedPose();
        measurement.dateTime = GetSelectedDateTime();

        measurement.eyesOpenSolidSurface = (data[0] != null) ? new StabilometryTask(data[0]) : null;
        measurement.eyesClosedSolidSurface = (data[1] != null) ? new StabilometryTask(data[1]) : null;
        measurement.eyesOpenSoftSurface = (data[2] != null) ? new StabilometryTask(data[2]) : null;
        measurement.eyesClosedSoftSurface = (data[3] != null) ? new StabilometryTask(data[3]) : null;

        mainScript.database.AddMeasurement(measurement);

        string fileName = $"Data{measurement.ID}.json";
        SaveDrawingJson(measurement.GetDrawingData(), fileName);

        string rawFileName = $"RawData{measurement.ID}.json";
        SaveRawJson(data, rawFileName);
    }

    /// <summary>
    /// Saves Raw data as a JSON document
    /// </summary>
    /// <param name="data"></param>
    private void SaveRawJson(List<DataPoint>[] data, string fileName)
    {
        string json = JsonHelper.ToJson(data);

        string jsonDirectory = $@"{Application.persistentDataPath}\JSON\Raw";

        if (!Directory.Exists(jsonDirectory))
            Directory.CreateDirectory(jsonDirectory);

        string newFilePath = $@"{jsonDirectory}\{fileName}";

        File.WriteAllText(newFilePath, json);
    }

    /// <summary>
    /// Saves data as a JSON document
    /// </summary>
    /// <param name="data"></param>
    private void SaveDrawingJson(DrawingTaskValues[] data, string fileName)
    {
        string json = JsonHelper.ToJson(data);

        string jsonDirectory = $@"{Application.persistentDataPath}\JSON\Data";

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

        saveButton.interactable = false;

        positionDropdown.value = 0;
        positionDropdown.RefreshShownValue();
    }

    private Pose GetSelectedPose()
    {
        Debug.Log("Method GetSelectedPose not implemented.");

        return Pose.BothLegsJoinedParallel;
    }

    private MyDateTime GetSelectedDateTime()
    {
        Debug.Log("Method GetSelectedDateTime not implemented.");
        return new MyDateTime(0, 0, 0, 0, 0);
    }
}
