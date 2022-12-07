using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System;
using System.Data;

namespace StabilometryAnalysis
{
    public class DatabaseScript : MonoBehaviour
    {
        #region Variables
        private const string PatientTableName = "PatientTable",
            MeasurementTableName = "MeasurementTable",
            TaskTableName = "StabilometryTaskTable",
            DatabaseName = "database.db";

        private static readonly string[] PatientTableColumnNames = {
        "EntryID", "Name", "Surname", "Notes" };
        private static readonly string[] PatientTableColumnValues = {
        "INTEGER PRIMARY KEY UNIQUE NOT NULL", "TEXT NOT NULL", "TEXT", "TEXT" };

        private static readonly string[] TaskTableColumnNames =
        {
        "EntryID", "Duration", "SampleTime", "SwayPath", "SwayPathAP",
        "SwayPathML", "MeanDistance", "MeanSwayVelocity", "MeanSwayVelocityAP", "MeanSwayVelocityML",
        "SwayAverageAmplitudeAP", "swayAverageAmplitudeML", "SwayMaximalAmplitudeAP", "SwayMaximalAmplitudeML", "ConfidenceEllipseArea"
    };
        private static readonly string[] TaskTableColumnValues =
        {
        "INTEGER PRIMARY KEY UNIQUE NOT NULL", "REAL", "REAL", "REAL", "REAL",
        "REAL", "REAL", "REAL", "REAL", "REAL",
        "REAL", "REAL", "REAL", "REAL", "REAL"
    };

        // Parameters are foreign IDs of Calculated parameters in Parameter table
        private static readonly string[] MeasurementTableColumnNames =
        {
        "EntryID", "PatientID", "DateTime", "Pose",
        "EyesOpenSolidSurfaceID", "EyesClosedSolidSurfaceID","EyesOpenSoftSurfaceID","EyesClosedSoftSurfaceID"
    };
        private static readonly string[] MeasurementTableColumnValues =
        {
        "INTEGER PRIMARY KEY UNIQUE NOT NULL", "INTEGER", "TEXT", "TEXT",
        "INTEGER", "INTEGER", "INTEGER", "INTEGER"};

        private SqliteConnection connection = null;
        #endregion

        private void Awake()
        {
            OpenDatabase();
            HandleDatabaseTables();
        }

        private void OpenDatabase()
        {
            string connectionPath = "URI=file:" + Application.persistentDataPath + @"/" + DatabaseName;

            try
            {
                connection = new SqliteConnection(connectionPath);
                connection.Open();

            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }

        }

        /// <summary>
        /// Checks if all tables exist, if not create them.
        /// </summary>
        private void HandleDatabaseTables()
        {
            if (!TableExists(PatientTableName))
                CreateTable(PatientTableName, PatientTableColumnNames, PatientTableColumnValues, "");

            if (!TableExists(TaskTableName))
                CreateTable(TaskTableName, TaskTableColumnNames, TaskTableColumnValues, "");

            if (!TableExists(MeasurementTableName))
            {
                string foreignKeys = $", FOREIGN KEY({MeasurementTableColumnNames[1]}) REFERENCES {PatientTableName} ({PatientTableColumnNames[0]})"
                    + $", FOREIGN KEY({MeasurementTableColumnNames[4]}) REFERENCES {TaskTableName} ({TaskTableColumnNames[0]})"
                    + $", FOREIGN KEY({MeasurementTableColumnNames[5]}) REFERENCES {TaskTableName} ({TaskTableColumnNames[0]})"
                    + $", FOREIGN KEY({MeasurementTableColumnNames[6]}) REFERENCES {TaskTableName} ({TaskTableColumnNames[0]})"
                    + $", FOREIGN KEY({MeasurementTableColumnNames[7]}) REFERENCES {TaskTableName} ({TaskTableColumnNames[0]})";

                Debug.Log(foreignKeys);

                CreateTable(MeasurementTableName, MeasurementTableColumnNames, MeasurementTableColumnValues, foreignKeys);
            }
        }

        /// <summary>
        /// Closes the database, used for clearing data.
        /// </summary>
        public void CloseDatabase()
        {

            if (connection != null)
            {
                //connection.Dispose();
                connection.Close();
                connection = null;

                Debug.Log("Connection closed.");
            }
        }

        public void DeleteDatabase()
        {
            if (connection != null)
            {
                connection.Dispose();
                connection = null;

                Debug.Log("Connection closed.");
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();

            //System.IO.File.Delete(filePath);
            StartCoroutine(DeleteDatabaseCoroutine());
        }

        IEnumerator DeleteDatabaseCoroutine()
        {
            string filePath = $@"{Application.persistentDataPath}/{DatabaseName}";

            bool isFileLocked = true;

            while (isFileLocked)
            {
                try
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    System.IO.File.Delete(filePath);
                    isFileLocked = false;
                }
                catch
                {
                    Debug.Log("Nope");
                }
                yield return new WaitForEndOfFrame();
            }

        }

        private void OnDestroy()
        {
            if (connection != null)
            {
                connection.Close();
                Debug.Log("App exited");
            }
        }

        #region Create
        /// <summary>
        /// Creates a table with specified name, column names and column types.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="tableColumnNames"></param>
        /// <param name="tableColumnValues"></param>
        private void CreateTable(string tableName, string[] tableColumnNames, string[] tableColumnValues, string foreignKeys)
        {
            string query = $"CREATE TABLE {tableName} ({tableColumnNames[0]} {tableColumnValues[0]}";
            for (int i = 1; i < tableColumnNames.Length; i++)
                query += $", {tableColumnNames[i]} {tableColumnValues[i]}";

            query += $" {foreignKeys})";
            IDataReader reader = ExecuteQuery(query);

            if (reader != null)
                reader.Close();
        }

        /// <summary>
        /// Inserts a new patient into the database.
        /// </summary>
        /// <param name="patient"></param>
        public void AddPatient(Patient patient)
        {
            string[] values = { patient.ID.ToString(), patient.Name, patient.Surname, patient.Notes };
            IDataReader reader = InsertIntoTable(PatientTableName, PatientTableColumnNames, values);

            if (reader != null)
                reader.Close();
        }

        /// <summary>
        /// Adds stabilometry task and if the process was successful return the task ID, otherwise return -1.
        /// </summary>
        /// <param name="stabilometryTask"></param>
        /// <returns></returns>
        public int AddStabilometryTask(StabilometryTask stabilometryTask)
        {
            if (stabilometryTask == null)
                return -1;
            //else

            stabilometryTask.ID = GetLastTaskID() + 1;

            string[] values =
            {
            stabilometryTask.ID.ToString(),
            stabilometryTask.duration.ToString(),
            stabilometryTask.sampleTime.ToString(),
            stabilometryTask.swayPath.ToString(),
            stabilometryTask.swayPathAP.ToString(),
            stabilometryTask.swayPathML.ToString(),
            stabilometryTask.meanDistance.ToString(),
            stabilometryTask.meanSwayVelocity.ToString(),
            stabilometryTask.meanSwayVelocityAP.ToString(),
            stabilometryTask.meanSwayVelocityML.ToString(),
            stabilometryTask.swayAverageAmplitudeAP.ToString(),
            stabilometryTask.swayAverageAmplitudeML.ToString(),
            stabilometryTask.swayMaximalAmplitudeAP.ToString(),
            stabilometryTask.swayMaximalAmplitudeML.ToString(),
            stabilometryTask.confidence95Ellipse.area.ToString()
        };

            IDataReader reader = InsertIntoTable(TaskTableName, TaskTableColumnNames, values);
            if (reader == null)
                return -1;
            //else

            reader.Close();
            return stabilometryTask.ID;
        }

        /// <summary>
        /// Inserts the measurement data into the database
        /// </summary>
        /// <param name="measurements"></param>
        public void AddMeasurement(StabilometryMeasurement measurement)
        {
            int eyesOpenSolidSurfaceID = AddStabilometryTask(measurement.eyesOpenSolidSurface);
            int eyesClosedSolidSurfaceID = AddStabilometryTask(measurement.eyesClosedSolidSurface);
            int eyesOpenSoftSurfaceID = AddStabilometryTask(measurement.eyesOpenSoftSurface);
            int eyesClosedSoftSurfaceID = AddStabilometryTask(measurement.eyesClosedSoftSurface);

            string[] values =
            {
            measurement.ID.ToString(),
            measurement.patientID.ToString(),
            measurement.dateTime.ToDatabaseString(),
            PoseConverter.PoseToString(measurement.pose),
            eyesOpenSolidSurfaceID.ToString(),
            eyesClosedSolidSurfaceID.ToString(),
            eyesOpenSoftSurfaceID.ToString(),
            eyesClosedSoftSurfaceID.ToString(),
        };

            IDataReader reader = InsertIntoTable(MeasurementTableName, MeasurementTableColumnNames, values);
            if (reader != null)
                reader.Close();
        }

        /// <summary>
        /// Inserts values into the table.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columns"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        private IDataReader InsertIntoTable(string tableName, string[] columns, string[] values)
        {
            string query = $"INSERT INTO {tableName} ({columns[0]}";
            for (int i = 1; i < columns.Length; i++)
                query += $",{columns[i]}";

            query += $") VALUES ('{values[0]}'";

            for (int i = 1; i < columns.Length; i++)
                query += $",'{values[i]}'";

            query += ")";
            //Debug.Log(query);
            IDataReader reader = ExecuteQuery(query);

            return reader;
        }
        #endregion

        #region Read
        /// <summary>
        /// Checks if a table with the given name exists.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private bool TableExists(string tableName)
        {
            string query = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}'";
            IDataReader reader = ExecuteQuery(query);
            bool line = reader.Read();

            if (reader != null)
                reader.Close();

            //Debug.Log(line);
            return line;
        }

        #region Get last ID
        /// <summary>
        /// Returns the next patient ID. 
        /// </summary>
        /// <returns></returns>
        public int GetLastPatientID()
        {
            string patientID = PatientTableColumnNames[0];

            return GetLastID(patientID, PatientTableName);
        }

        /// <summary>
        /// Returns the last parameters ID.
        /// </summary>
        /// <returns></returns>
        public int GetLastTaskID()
        {
            string parameterID = TaskTableColumnNames[0];

            return GetLastID(parameterID, TaskTableName);
        }

        /// <summary>
        /// Returns the last measurement ID.
        /// </summary>
        /// <returns></returns>
        public int GetLastMeasurementID()
        {
            string measurementID = MeasurementTableColumnNames[0];

            return GetLastID(measurementID, MeasurementTableName);
        }

        /// <summary>
        /// Get the last ID in a column.
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private int GetLastID(string columnName, string tableName)
        {
            string query = $"SELECT COUNT(*) FROM {tableName}";

            int result = 0;

            IDataReader reader = ExecuteQuery(query);

            // Reader should never be null.
            if (reader == null)
                Debug.LogError("Reader was null");

            // If there are any parameters in the database
            if (reader.Read() && reader.GetInt32(0) > 0)
            {
                reader.Close();

                query = $"SELECT MAX({columnName}) FROM {tableName}";
                reader = ExecuteQuery(query);

                if (reader != null && reader.Read())
                {
                    result = (int)reader.GetInt64(0);
                    Debug.Log(result);
                }
            }

            if (reader != null)
                reader.Close();

            return result;
        }

        #endregion

        /// <summary>
        /// Returns all patient data from the database.
        /// </summary>
        /// <returns></returns>
        public List<Patient> GetAllPatients()
        {
            List<Patient> result = new List<Patient>();

            string patientID = PatientTableColumnNames[0];

            string querry = $"SELECT * FROM  {PatientTableName} ORDER BY {patientID} DESC";

            IDataReader reader = ExecuteQuery(querry);

            if (reader == null)
            {
                Debug.LogError("Reader was null.");
                return null;
            }

            // else
            while (reader.Read())
            {

                int id = (int)reader.GetInt64(0);
                string name = (string)reader.GetValue(1);
                string surname = (string)reader.GetValue(2);
                string notes = (string)reader.GetValue(3);

                Patient patient = new Patient(id, name, surname, notes);

                result.Add(patient);
            }

            reader.Close();

            return result;
        }

        /// <summary>
        /// Returns the number of stabilometry entries for the given patient.
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        public int GetNumberOfDataEntries(Patient patient)
        {
            string query = $"SELECT COUNT(*) FROM {MeasurementTableName} WHERE {MeasurementTableColumnNames[1]} == {patient.ID}";

            int result = 0;

            IDataReader reader = ExecuteQuery(query);

            // Reader should never be null.
            if (reader == null)
                Debug.LogError("Reader was null");

            // If there are any parameters in the database
            if (reader.Read())
                result = reader.GetInt32(0);

            if (reader != null)
                reader.Close();

            return result;
        }

        public List<StabilometryMeasurement> GetAllMeasurements(Patient patient, Pose pose)
        {
            List<StabilometryMeasurement> result = new List<StabilometryMeasurement>();

            string entryIDText = MeasurementTableColumnNames[0];
            string patientIDText = MeasurementTableColumnNames[1];
            string dateTimeText = MeasurementTableColumnNames[2];
            string poseText = MeasurementTableColumnNames[3];

            string querry = $"SELECT * FROM  {MeasurementTableName} WHERE {patientIDText} == {patient.ID} AND {poseText} == '{PoseConverter.PoseToString(pose)}' ORDER BY {dateTimeText} ASC, {entryIDText} ASC;";

            Debug.Log(querry);

            IDataReader reader = ExecuteQuery(querry);

            if (reader == null)
            {
                Debug.LogError("Reader was null.");
                return null;
            }

            // else
            while (reader.Read())
            {
                StabilometryMeasurement entry = new StabilometryMeasurement();

                entry.ID = (int)reader.GetInt64(0);
                entry.patientID = (int)reader.GetInt64(1);
                entry.dateTime = new MyDateTime(reader.GetString(2));
                entry.pose = PoseConverter.StringToPose((string)reader.GetValue(3));

                entry.eyesOpenSolidSurface = GetTask((int)reader.GetInt64(4));
                entry.eyesClosedSolidSurface = GetTask((int)reader.GetInt64(5));
                entry.eyesOpenSoftSurface = GetTask((int)reader.GetInt64(6));
                entry.eyesClosedSoftSurface = GetTask((int)reader.GetInt64(7));

                result.Add(entry);
            }

            reader.Close();

            return result;
        }

        public List<StabilometryMeasurement> GetAllMeasurements(Patient patient)
        {
            List<StabilometryMeasurement> result = new List<StabilometryMeasurement>();

            string entryIDText = MeasurementTableColumnNames[0];
            string patientIDText = MeasurementTableColumnNames[1];
            string dateTimeText = MeasurementTableColumnNames[2];
            string poseText = MeasurementTableColumnNames[3];

            string querry = $"SELECT * FROM  {MeasurementTableName} WHERE {patientIDText} == {patient.ID} ORDER BY {dateTimeText} ASC, {entryIDText} ASC;";

            Debug.Log(querry);

            IDataReader reader = ExecuteQuery(querry);

            if (reader == null)
            {
                Debug.LogError("Reader was null.");
                return null;
            }

            // else
            while (reader.Read())
            {
                StabilometryMeasurement entry = new StabilometryMeasurement();

                entry.ID = (int)reader.GetInt64(0);
                entry.patientID = (int)reader.GetInt64(1);
                entry.dateTime = new MyDateTime(reader.GetString(2));
                entry.pose = PoseConverter.StringToPose((string)reader.GetValue(3));

                entry.eyesOpenSolidSurface = GetTask((int)reader.GetInt64(4));
                entry.eyesClosedSolidSurface = GetTask((int)reader.GetInt64(5));
                entry.eyesOpenSoftSurface = GetTask((int)reader.GetInt64(6));
                entry.eyesClosedSoftSurface = GetTask((int)reader.GetInt64(7));

                result.Add(entry);
            }

            reader.Close();

            return result;
        }

        private StabilometryTask GetTask(int taskID)
        {
            if (taskID < 0)
                return null;

            // else

            string taskIDName = TaskTableColumnNames[0];

            string querry = $"SELECT * FROM  {TaskTableName} WHERE {taskIDName } == {taskID}";

            IDataReader reader = ExecuteQuery(querry);

            if (reader == null)
            {
                Debug.LogError("Reader was null.");
                return null;
            }

            if (reader.Read())
            {
                StabilometryTask result = new StabilometryTask();
                result.ID = (int)reader.GetInt64(0);
                result.duration = reader.GetFloat(1);
                result.sampleTime = reader.GetFloat(2);
                result.swayPath = reader.GetFloat(3);

                result.swayPathAP = reader.GetFloat(4);
                result.swayPathML = reader.GetFloat(5);
                result.meanDistance = reader.GetFloat(6);
                result.meanSwayVelocity = reader.GetFloat(7);
                result.meanSwayVelocityAP = reader.GetFloat(8);
                result.meanSwayVelocityML = reader.GetFloat(9);
                result.swayAverageAmplitudeAP = reader.GetFloat(10);
                result.swayAverageAmplitudeML = reader.GetFloat(11);
                result.swayMaximalAmplitudeAP = reader.GetFloat(12);
                result.swayMaximalAmplitudeML = reader.GetFloat(13);

                EllipseValues ellipseValues = new EllipseValues(reader.GetFloat(14));
                result.confidence95Ellipse = ellipseValues;

                reader.Close();

                return result;
            }

            return null;
        }

        #endregion

        #region Update
        /// <summary>
        /// Updates the patients data.
        /// </summary>
        /// <param name="patient"></param>
        public void UpdatePatient(Patient patient)
        {
            string condition = $"{PatientTableColumnNames[0]} = {patient.ID}";
            string newValues = $"{PatientTableColumnNames[1]} = '{patient.Name}', {PatientTableColumnNames[2]} = '{patient.Surname}', {PatientTableColumnNames[3]} = '{patient.Notes}'";

            string query = $"UPDATE {PatientTableName} SET {newValues} WHERE {condition}";

            IDataReader reader = ExecuteQuery(query);
            if (reader != null)
            {
                reader.Close();
                GetComponent<MainScript>().SelectPatient(patient);
            }

        }
        #endregion

        #region Delete

        /// <summary>
        /// Deletes the given patient, based on the ID.
        /// </summary>
        /// <param name="patientID"></param>
        public void DeletePatient(Patient patient)
        {
            DeleteMeasurements(patient);

            string query = $"DELETE FROM {PatientTableName} WHERE {PatientTableColumnNames[0]} = {patient.ID}";

            IDataReader reader = ExecuteQuery(query);

            if (reader != null)
            {
                reader.Close();
                GetComponent<MainScript>().SelectPatient(null);
            }
        }

        /// <summary>
        /// Deletes the measurement.
        /// </summary>
        /// <param name="measurementID"></param>
        public void DeleteMeasurement(StabilometryMeasurement measurement)
        {

            DeleteTask(measurement.eyesOpenSolidSurface);
            DeleteTask(measurement.eyesClosedSolidSurface);
            DeleteTask(measurement.eyesOpenSoftSurface);
            DeleteTask(measurement.eyesClosedSoftSurface);

            string query = $"DELETE FROM {MeasurementTableName} WHERE {MeasurementTableColumnNames[0]} = {measurement.ID}";
            IDataReader reader = ExecuteQuery(query);

            if (reader != null)
                reader.Close();

        }

        private void DeleteMeasurements(Patient patient)
        {
            List<StabilometryMeasurement> measurements = GetAllMeasurements(patient);
            foreach (StabilometryMeasurement element in measurements)
                DeleteMeasurement(element);
        }

        /// <summary>
        /// Deletes the given task.
        /// </summary>
        /// <param name="parameter"></param>
        private void DeleteTask(StabilometryTask task)
        {
            if (task == null)
                return;
            //else

            string query = $"DELETE FROM {TaskTableName} WHERE {TaskTableColumnNames[0]} = {task.ID}";
            IDataReader reader = ExecuteQuery(query);

            if (reader != null)
                reader.Close();
        }
        #endregion

        /// <summary>
        /// Returns a new string array made from the given array.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="firstIndex">Inclusive</param>
        /// <param name="lastIndex">Exclusive, -1 means last</param>
        /// <returns></returns>
        private string[] GetSmallerStringArray(string[] array, int firstIndex, int lastIndex)
        {
            if (lastIndex < 0 || lastIndex > array.Length)
                lastIndex = array.Length;

            if (firstIndex > lastIndex)
            {
                Debug.LogError($"first index {firstIndex} was larger than final index {lastIndex}");
                return null;
            }
            else if (firstIndex > array.Length - 1)
            {
                Debug.LogError($"first index {firstIndex} was larger than array highest value {array.Length - 1}");
                return null;
            }

            string[] result = new string[lastIndex - firstIndex];
            for (int i = firstIndex; i < lastIndex; i++)
                result[i - firstIndex] = array[i];

            return result;
        }

        /// <summary>
        /// Tries to execute the given query.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>IDataReader if the query was successful, otherwise it returns null.</returns>
        private IDataReader ExecuteQuery(string query)
        {
            //Debug.Log(query);
            try
            {
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = query;

                return command.ExecuteReader();
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
                return null;
            }
        }
    }
}