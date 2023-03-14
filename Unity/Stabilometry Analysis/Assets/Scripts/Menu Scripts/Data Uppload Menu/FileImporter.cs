using System.Collections;
using UnityEngine;
using TMPro;
using SimpleFileBrowser;
using System.IO;
using System.Collections.Generic;
using System;

namespace StabilometryAnalysis
{
    public class FileImporter : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        private TextMeshProUGUI displayText = null;

        public bool pathFound { get; private set; } = false;

        private string path = "None";
        private const string xColumnName = "copX";
        private const string yColumnName = "copX";
        private const string TimeColumnName = "f_time";


        #endregion

        public void ImportDataButton()
        {
            Clear();

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
        public List<DataPoint> ReadData(string dataSeparator, DataUnits unit)
        {
            if (!pathFound)
                return null;

            float multiplicator = GetMultiplicator(unit);

            //else

            List<DataPoint> result = new List<DataPoint>();

            StreamReader reader = new StreamReader(path);

            string line;
            bool firstLine = true;
            int xIndex = -1;
            int yIndex = -1;
            int timeIndex = -1;

            while ((line = reader.ReadLine()) != null)
            {
                if (firstLine)
                {
                    string[] columns = line.Split(new[] { dataSeparator }, StringSplitOptions.RemoveEmptyEntries);

                    xIndex = GetIndex(columns, xColumnName);
                    yIndex = GetIndex(columns, yColumnName);
                    timeIndex = GetIndex(columns, TimeColumnName);

                    // If the program doesn't find all titles.
                    if (xIndex == -1 || yIndex == -1 || timeIndex == -1)
                        return null;

                    firstLine = false;
                    continue;
                }

                // else
                string[] row = line.Split(new[] { dataSeparator }, StringSplitOptions.RemoveEmptyEntries);
                result.Add(new DataPoint(row[timeIndex], row[xIndex], row[yIndex], multiplicator));
            }

            reader.Close();
            return result;
        }

        private float GetMultiplicator(DataUnits unit)
        {            
            switch (unit)
            {
                case (DataUnits.mm):
                    return 0.1f;
                case (DataUnits.cm):
                    return 1f;
                case (DataUnits.dm):
                    return 10f;
                case (DataUnits.m):
                    return 100f;
            }

            // By default it is cm
            return 1f;
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

        private static int GetIndex(string[] columns, string columnTitle)
        {
            for (int i = 0; i < columns.Length; i++)
                if (columns[i] == columnTitle)
                    return i;

            Debug.Log($"{columnTitle} was not found in columns.");
            return -1;
        }
    }
}