using System.Collections;
using UnityEngine;
using TMPro;
using SimpleFileBrowser;
using System.IO;

public class FileImporter : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI displayText = null;

    private bool pathFound = false;

    private string path = "None";

    public void ImportDataButton()
    {
        // Set filters for file explorer.
        FileBrowser.SetFilters(true, ".csv");

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
}
