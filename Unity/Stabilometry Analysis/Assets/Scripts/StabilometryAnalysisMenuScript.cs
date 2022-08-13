using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabilometryAnalysisMenuScript : MonoBehaviour
{
    #region Variables
    public MainScript mainScript { get; set; } = null;
    #endregion

    public void OpenMenu(GameObject menu)
    {
        mainScript.menuSwitching.OpenMenu(menu);
    }
}
