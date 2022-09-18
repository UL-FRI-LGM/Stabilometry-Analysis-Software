using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabilometryImagesMenuScript : MonoBehaviour
{
    #region Variables
    public MainScript mainScript { get; set; } = null;
    [SerializeField] private GameObject stabilometryImagePrefab = null;

    private string cmUnit = "cm";
    private string cmSqUnit = "cm²";
    private string cmOverSUnit = "cm/s";

    #endregion

    private string ConvertToString(float value, string unit)
    {
        return "";
    }

    
}
