using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StabilometryAnalysis
{
    public abstract class AccordionRadioHandler : MonoBehaviour
    {
        public Pose selectedPose = Pose.BOTH_LEGS_JOINED_PARALLEL;
        public Task selectedTask = Task.EYES_OPEN_SOLID_SURFACE;

        public bool valueChanged { get; set; } = true;

        abstract public void Select(int index);
    }
}