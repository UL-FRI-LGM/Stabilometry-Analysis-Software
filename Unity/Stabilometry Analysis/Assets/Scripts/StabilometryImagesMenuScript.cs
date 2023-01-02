using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StabilometryAnalysis
{
    public class StabilometryImagesMenuScript : MonoBehaviour
    {
        #region Variables
        public MainScript mainScript { get; set; } = null;
        [SerializeField] private GameObject stabilometryImagePrefab = null;

        #endregion

        //public void 

        public void BackButtonClick()
        {
            mainScript.menuSwitching.OpenPreviousMenu();
        }
    }
}