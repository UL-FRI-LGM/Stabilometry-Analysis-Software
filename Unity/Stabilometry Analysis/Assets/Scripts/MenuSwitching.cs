using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StabilometryAnalysis
{
    public class MenuSwitching : MonoBehaviour
    {
        #region Variables

        [SerializeField]
        private GameObject initialMenu = null;

        private GameObject currentMenu = null;
        private List<GameObject> previousMenus = null;

        #endregion

        private void Awake()
        {
            ClearHistory();
            currentMenu = initialMenu;
        }

        private void ClearHistory()
        {
            previousMenus = new List<GameObject>();
        }

        public void OpenMenu(GameObject newMenu)
        {
            if (newMenu == initialMenu)
                ClearHistory();
            else
                previousMenus.Add(currentMenu);

            currentMenu.SetActive(false);
            currentMenu = newMenu;
            newMenu.SetActive(true);
        }

        public void OpenInitialMenu()
        {
            ClearHistory();

            currentMenu.SetActive(false);
            initialMenu.SetActive(true);
            
            currentMenu = initialMenu;
        }

        public void OpenPreviousMenu()
        {
            currentMenu.SetActive(false);
            int lastIndex = previousMenus.Count - 1;
            currentMenu = previousMenus[lastIndex];
            previousMenus.RemoveAt(lastIndex);

            currentMenu.SetActive(true);
        }
    }
}