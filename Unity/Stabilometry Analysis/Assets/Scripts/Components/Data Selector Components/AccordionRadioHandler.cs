using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StabilometryAnalysis
{
    public class AccordionRadioHandler : MonoBehaviour
    {
        [SerializeField] private RadioButton[] radioButtons = null;
        public Pose selectedPose = Pose.BOTH_LEGS_JOINED_PARALLEL;
        public bool valueChanged = true;

        private void Awake()
        {
            for (int i = 0; i < radioButtons.Length; i++)
                radioButtons[i].SetVaribales(this, i);
        }
        
        public void Select(int index)
        {
            selectedPose = (Pose)index;
            valueChanged = true;

            for (int i = 0; i < radioButtons.Length; i++)
                radioButtons[i].SelectButton(i == index);
        }
    }
}