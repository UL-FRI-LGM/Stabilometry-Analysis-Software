using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using TMPro;

public class FileImporter : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI displayText = null;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Opens a file explorer
    /// </summary>
    private void OpenExplorer()
    {
        Process process = new Process();

        string arguments = "search:type = .csv";

        process.StartInfo = new ProcessStartInfo("explorer.exe", arguments);

        process.Start();
    }

    public void ImportDataButton()
    {
        OpenExplorer();
    }
}
