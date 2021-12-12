using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DataUploadMenuScript : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private FileImporter[] fileImporters = null;

    [SerializeField]
    private TMP_Dropdown positionDropdown = null;

    public MainScript mainScript { get; set; } = null;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CancelButton()
    {
        ClearAllInputFields();        
    }

    public void SaveButton()
    { 
        SaveValues(GetData());
        ClearAllInputFields();
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

        int lastMeasurementID =  mainScript.database.GetLastMeasurementID();
        //int lastFileName = 
        //int lastParametersID = mainScript.database.GetLastParametersID();

        //SaveJson(data);

        //mainScript.database.
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

    /// <summary>
    /// Saves data as a JSON document
    /// </summary>
    /// <param name="data"></param>
    private void SaveJson(List<DataPoint>[] data, string fileName)
    {
    }
}
