using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StabilometryAnalysis
{
    public class LineObjectScript : MonoBehaviour
    {
        private LineChartScript parentScript = null;
        private int index = -1;

        public void SetParentScript(int index, LineChartScript parentScript)
        {
            this.index = index;
            this.parentScript = parentScript;
        }

        public void ButtonClick()
        {
            parentScript.ButtonClicked(index);
        }    
    }
}