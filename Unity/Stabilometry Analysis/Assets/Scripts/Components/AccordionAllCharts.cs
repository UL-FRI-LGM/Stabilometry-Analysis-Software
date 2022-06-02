using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccordionAllCharts : MonoBehaviour
{
    #region Variables
    // Tasks
    [SerializeField]
    private Toggle solidSurfaceEyesOpenToggle = null,
        solidSurfaceEyesClosedToggle = null,
        softSurfaceEyesOpenToggle = null,
        softSurfaceEyesClosedToggle = null;

    // Pose
    [SerializeField]
    private Toggle bothLegsJoinedParallelToggle = null,
        bothLegs30AngleToggle = null,
        bothLegsParallelToggle = null,
        tandemLeftFrontToggle = null,
        tandemRightFrontToggle = null,
        leftLegToggle = null,
        rightLegToggle = null;

    // Data Type
    [SerializeField]
    private Toggle swayPathToggle = null,
        swayPathAPToggle = null,
        swayPathMLToggle = null,

        meanSwayVelocityToggle = null,
        meanSwayVelocityAPToggle = null,
        meanSwayVelocityMLToggle = null,

        averageAmplitudeAPToggle = null,
        averageAmplitudeMLToggle = null,
        
        maximalAmplitudeAPToggle = null,
        maximalAmplitudeMLToggle = null,
        
        meanDistanceToggle = null,
        conf95EllipseAreaToggle = null;

    // Time Frame

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
