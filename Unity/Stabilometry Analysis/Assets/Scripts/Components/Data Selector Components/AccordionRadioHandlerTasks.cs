using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StabilometryAnalysis
{
    public class AccordionRadioHandlerTasks : AccordionRadioHandler
    {
        [SerializeField] private RadioButton[] radioButtons = null;

        private void Awake()
        {
            for (int i = 0; i < radioButtons.Length; i++)
                radioButtons[i].SetVaribales(this, i);
        }

        override public void Select(int index)
        {
            selectedTask = (Task)index;
            valueChanged = true;

            for (int i = 0; i < radioButtons.Length; i++)
                radioButtons[i].SelectButton(i == index);
        }
    }
}