using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StabilometryAnalysis
{
    public class RadioButton : MonoBehaviour
    {
        private Button button = null;
        private Toggle toggle = null;

        private AccordionRadioHandler parentScript = null;
        private int index = -1;

        private void Awake()
        {
            toggle = transform.GetChild(0).GetComponent<Toggle>();
            button = GetComponent<Button>();
        }

        public void SetVaribales(AccordionRadioHandler parentScript, int index)
        {
            this.parentScript = parentScript;
            this.index = index;
        }

        public void ClickButton()
        {
            parentScript.Select(index);
        }

        public void SelectButton(bool select)
        {
            button.interactable = !select;
            toggle.isOn = select;
        }
    }
}
