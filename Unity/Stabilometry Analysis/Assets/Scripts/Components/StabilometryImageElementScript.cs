using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StabilometryAnalysis
{
    public class StabilometryImageElementScript : MonoBehaviour
    {
        #region Variables
        [SerializeField] StabilometryImageScript
            EyesOpenSolidSurfaceImage = null,
            EyesClosedSolidSurfaceImage = null,
            EyesOpenSoftSurfaceImage = null,
            EyesClosedSoftSurfaceImage = null;

        public StabilometryImagesMenuScript parentScript {get; set;} = null;

        private int index = -1;

        #endregion

        public void SetData()
        {

        }

        //public void ButtonCLicked()
        //{
        //    parentScript.SelectData(index);
        //}
    }
}
