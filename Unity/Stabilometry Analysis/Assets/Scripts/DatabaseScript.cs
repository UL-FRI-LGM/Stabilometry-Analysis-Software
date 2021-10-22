using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System;

public class DatabaseScript : MonoBehaviour
{

    private const string PatientTableName = "PatientTable",
        MeasurementTableName = "MeasurementTable",
        ParameterTableName = "ParametersTable",
        DatabaseName = "database.db";

    private SqliteConnection connection = null;

    private void Awake()
    {
        OpenDatabase();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDatabase()
    {
        string connectionPath = "URI=file:" + Application.dataPath + @"/Database/" + DatabaseName;

        try
        {
            connection = new SqliteConnection(connectionPath);
            
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }

        if (connection != null)
        {
            connection.Close();
            Debug.Log("Done");
        }
    }

    private void OnDestroy()
    {
            
    }
}
