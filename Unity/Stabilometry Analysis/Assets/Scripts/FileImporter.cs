using System.Collections;
using UnityEngine;
using TMPro;
using SimpleFileBrowser;
using System.IO;
using System.Collections.Generic;

public class FileImporter : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI displayText = null;

    public bool pathFound { get; private set; } = false;

    private string path = "None";

    public void ImportDataButton()
    {
        // Set filters for file explorer.
        FileBrowser.SetFilters(true, ".csv");
        FileBrowser.SetDefaultFilter(".csv");

        StartCoroutine(ShowLoadDialogCoroutine());
    }

    IEnumerator ShowLoadDialogCoroutine()
    {
        // Show a load file dialog and wait for a response from user
        // Load file/folder: both, Allow multiple selection: true
        // Initial path: default (Documents), Initial filename: empty
        // Title: "Load File", Submit button text: "Load"
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, false, null, null, "Load Files and Folders", "Load");

        // Dialog is closed
        // Print whether the user has selected some files/folders or cancelled the operation (FileBrowser.Success)
        if (FileBrowser.Success)
        {
            // Print paths of the selected files (FileBrowser.Result) (null, if FileBrowser.Success is false)
            if (FileBrowser.Result.Length > 1)
                Debug.LogError(FileBrowser.Result.Length + " is too many results.");

            path = FileBrowser.Result[0];
            displayText.text = path;
            pathFound = true;

        }
        else
        {
            pathFound = false;

        }
    }

    /// <summary>
    /// Reads the data from the CSV file if the path is found.
    /// </summary>
    /// <returns></returns>
    public List<DataPoint> ReadData()
    {
        if (!pathFound)
            return null;

        //else

        List<DataPoint> result = new List<DataPoint>();

        StreamReader reader = new StreamReader(path);
        
        string line;
        bool firstLine = true;

        while ((line = reader.ReadLine()) != null)
        {
            if (firstLine)
            {
                firstLine = false;
                continue;
            }

            // else
            string[] row = line.Split(',');
            result.Add(new DataPoint(row[0], row[1], row[2]));
        }

        reader.Close();
        return result;
    }

    /// <summary>
    /// Clears all values.
    /// </summary>
    public void Clear()
    {
        path = "None";
        pathFound = false;
        displayText.text = path;
    }
}
