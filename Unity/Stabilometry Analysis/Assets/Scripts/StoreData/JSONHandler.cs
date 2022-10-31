using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace StabilometryAnalysis
{
    public static class JSONHandler
    {
        #region Variables
        private static string rawFolder = "RawData",
            dataFolder = "Data",
            JSONFolder = "JSON";

        #endregion

        /// <summary>
        /// Calculates and saves the values into the database
        /// </summary>
        /// <param name="data"></param>
        public static StabilometryMeasurement SaveValues(int newID, List<DataPoint>[] data, int patientID, Pose selectedPose, MyDateTime dateTime)
        {
            StabilometryMeasurement measurement = new StabilometryMeasurement();
            measurement.ID = newID;
            measurement.patientID = patientID;
            measurement.pose = selectedPose;
            measurement.dateTime = dateTime;

            measurement.eyesOpenSolidSurface = (data[0] != null) ? new StabilometryTask(data[0]) : null;
            measurement.eyesClosedSolidSurface = (data[1] != null) ? new StabilometryTask(data[1]) : null;
            measurement.eyesOpenSoftSurface = (data[2] != null) ? new StabilometryTask(data[2]) : null;
            measurement.eyesClosedSoftSurface = (data[3] != null) ? new StabilometryTask(data[3]) : null;

            string fileName = $"{dataFolder}{measurement.ID}.json";
            SaveDrawingJson(measurement.GetDrawingData(), fileName);

            string rawFileName = $"{rawFolder}{measurement.ID}.json";
            SaveRawJSON(data, rawFileName);

            return measurement;
        }

        /// <summary>
        /// Saves Raw data as a JSON document, used for future expansions.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fileName"></param>
        private static void SaveRawJSON(List<DataPoint>[] data, string fileName)
        {
            string json = JsonHelper.ToJson(data);

            string jsonDirectory = $@"{Application.persistentDataPath}\{JSONFolder}\{rawFolder}";

            if (!Directory.Exists(jsonDirectory))
                Directory.CreateDirectory(jsonDirectory);

            string newFilePath = $@"{jsonDirectory}\{fileName}";

            File.WriteAllText(newFilePath, json);
        }

        /// <summary>
        /// Saves data as a JSON document, this data will be used for drawing
        /// </summary>
        /// <param name="data"></param>
        private static void SaveDrawingJson(DrawingTaskValues[] data, string fileName)
        {
            string json = JsonHelper.ToJson(data);

            string jsonDirectory = $@"{Application.persistentDataPath}\{JSONFolder}\{dataFolder}";

            if (!Directory.Exists(jsonDirectory))
                Directory.CreateDirectory(jsonDirectory);

            string newFilePath = $@"{jsonDirectory}\{fileName}";

            File.WriteAllText(newFilePath, json);
        }

        public static StabilometryMeasurement GetJSONFile(StabilometryMeasurement measurement)
        {
            string fileName = $"{dataFolder}{measurement.ID}.json";
            string filePath = $@"{Application.persistentDataPath}\{JSONFolder}\{dataFolder}\{fileName}";

            string json = File.ReadAllText(filePath);
            StabilometryMeasurement result = JsonHelper.DataFromJson(json, measurement);

            return result;
        }

        /// <summary>
        /// Used when deleting the whole database.
        /// </summary>
        public static void DeleteAllData()
        {
            string path = $@"{Application.persistentDataPath}\{JSONFolder}";

            if (!Directory.Exists(path))
                return;
            
            //else

            DirectoryInfo dir = new DirectoryInfo(path);
            dir.Delete(true);
        }

        /// <summary>
        /// Used when deleting a measurement
        /// </summary>
        /// <param name="fileID"></param>
        public static void DeleteJSONFile(int fileID)
        {
            Debug.LogWarning("DeleteJSON File Not implemented");
        }

        /// <summary>
        /// Used when deleting a patient.
        /// </summary>
        /// <param name="fileIDs"></param>
        public static void DeleteJSONFiles(List<int> fileIDs)
        {
            Debug.LogWarning("DeleteJSON File Not implemented");
        }
    }
}