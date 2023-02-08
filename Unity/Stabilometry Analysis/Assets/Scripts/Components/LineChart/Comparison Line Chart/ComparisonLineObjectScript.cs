using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StabilometryAnalysis
{
    public class ComparisonLineObjectScript : MonoBehaviour
    {
        private ComparisonDataDisplayerScript comparisonDataDisplayer = null;
        private ComparisonLineChartScript parentScript = null;

        private ComparisonChartData comparisonChartData = null; 
        private int index = -1;

        private bool hovering = false;

        public void SetParentScript(int index, ComparisonChartData comparisonChartData, ComparisonLineChartScript parentScript)
        {
            this.index = index;
            this.comparisonChartData = comparisonChartData;
            this.comparisonDataDisplayer = LocationPointer.mainScript.ComparisonDataDisplayerScript;
            this.parentScript = parentScript;
        }

        private void Update()
        {
            if (hovering)
                comparisonDataDisplayer.SetPosition(Input.mousePosition);
        }
        
        public void OnPointerEnter()
        {
            hovering = true;
            comparisonDataDisplayer.EnableObject(true);
            comparisonDataDisplayer.SetValues(comparisonChartData);
        }

        public void OnPointerExit()
        {
            hovering = false;
            comparisonDataDisplayer.EnableObject(false);
        }
    }
}