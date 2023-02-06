using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StabilometryAnalysis
{
    public class LineObjectScript : MonoBehaviour
    {
        private DataDisplayerScript dataDisplayer = null;
        private StandardLineChartScript parentScript = null;

        private ChartData chartData = null; 
        private int index = -1;

        private bool hovering = false;

        public void SetParentScript(int index, ChartData chartData, StandardLineChartScript parentScript)
        {
            this.index = index;
            this.chartData = chartData;
            this.dataDisplayer = LocationPointer.mainScript.DataDisplayerScript;
            this.parentScript = parentScript;
        }

        private void Update()
        {
            if (hovering)
                dataDisplayer.SetPosition(Input.mousePosition);
        }

        public void ButtonClick()
        {
            parentScript.ButtonClicked(index);
            hovering = false;

            dataDisplayer.EnableObject(false);
        } 
        
        public void OnPointerEnter()
        {
            hovering = true;
            dataDisplayer.EnableObject(true);
            dataDisplayer.SetValues(chartData);
        }

        public void OnPointerExit()
        {
            hovering = false;
            dataDisplayer.EnableObject(false);
        }

    }
}