using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    public int PatientId { get; set; } = -1;

    public void ExitApplication()
    {
        Application.Quit();
    }
}
