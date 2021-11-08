using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSwitching : MonoBehaviour
{
    public void OpenMenu(GameObject currentMenu, GameObject newMenu)
    {
        currentMenu.SetActive(false);
        newMenu.SetActive(true);
    }
}
