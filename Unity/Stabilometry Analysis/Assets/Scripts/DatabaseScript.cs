using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System;
using System.Data;

public class DatabaseScript : MonoBehaviour
{

    #region Variables
    private const string PatientTableName = "PatientTable",
        MeasurementTableName = "MeasurementTable",
        ParameterTableName = "ParametersTable",
        DatabaseName = "database.db";

    private static readonly string[] PatientTableColumnNames = {
        "EntryID", "Name", "Surname", "Notes" };
    private static readonly string[] PatientTableColumnValues = {
        "INTEGER PRIMARY KEY UNIQUE NOT NULL", "TEXT NOT NULL", "TEXT", "TEXT" };

    //TODO: define all parameters
    private static readonly string[] ParametersTableColumnNames =
    {
        "EntryID"};
    private static readonly string[] ParametersTableColumnValues =
    {
        "INTEGER PRIMARY KEY UNIQUE NOT NULL"};


    private static readonly string[] MeasurementTableColumnNames =
    {
        "EntryID", "PatientID", "Parameter1ID", "Parameter2ID","Parameter3ID","Parameter4ID"
    };
    private static readonly string[] MeasurementTableColumnValues =
    {
        "INTEGER PRIMARY KEY UNIQUE NOT NULL", "INTEGER", "INTEGER", "INTEGER", "INTEGER", "INTEGER"};

    private SqliteConnection connection = null;
    #endregion

    private void Awake()
    {
        OpenDatabase();
        HandleDatabaseTables();
    }

    private void OpenDatabase()
    {
        string connectionPath = "URI=file:" + Application.dataPath + @"/" + DatabaseName;

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

        if (!TableExists(ParameterTableName))
            CreateTable(ParameterTableName, ParametersTableColumnNames, ParametersTableColumnValues, "");

        if (!TableExists(MeasurementTableName))
        {
            string foreignKeys = $", FOREIGN KEY({MeasurementTableColumnNames[1]}) REFERENCES {PatientTableName} ({PatientTableColumnNames[0]})"
                + $", FOREIGN KEY({MeasurementTableColumnNames[2]}) REFERENCES {ParameterTableName} ({ParametersTableColumnNames[0]})"
                + $", FOREIGN KEY({MeasurementTableColumnNames[3]}) REFERENCES {ParameterTableName} ({ParametersTableColumnNames[0]})"
                + $", FOREIGN KEY({MeasurementTableColumnNames[4]}) REFERENCES {ParameterTableName} ({ParametersTableColumnNames[0]})";

            Debug.Log(foreignKeys);
            CreateTable(MeasurementTableName, MeasurementTableColumnNames, MeasurementTableColumnValues, foreignKeys);
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
    public void CreatePatient(string name, string surname, string notes)
    {
        string[] columns = GetSmallerStringArray(PatientTableColumnNames, 1, -1);

        string[] values = { name, surname, notes };
        IDataReader reader = InsertIntoTable(PatientTableName, columns, values);

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
        Debug.Log(query);
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
    #endregion

    #region Update
    /// <summary>
    /// Updates the patients data.
    /// </summary>
    /// <param name="patientID"></param>
    /// <param name="name"></param>
    /// <param name="surname"></param>
    /// <param name="notes"></param>
    public void UpdatePatient(int patientID, string name, string surname, string notes)
    {
    }
    #endregion

    #region Delete

    /// <summary>
    /// Deletes the given patient.
    /// </summary>
    /// <param name="patientID"></param>
    public void DeletePatient(int patientID)
    {

    }

    /// <summary>
    /// Deletes the measurement.
    /// </summary>
    /// <param name="measurementID"></param>
    public void DeleteMeasurement(int measurementID)
    {

    }

    /// <summary>
    /// Deletes the given parameter.
    /// </summary>
    /// <param name="parameter"></param>
    private void DeleteParameter(int parameter)
    {

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