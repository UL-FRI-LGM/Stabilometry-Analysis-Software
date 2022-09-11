using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Note about spawning charts. Start with spawning only one chart (because of speed).
// This one chart should be stretched trhough the entire space.
// When clicking on a chart toggler change sizes.
// When clicking on any chart it opens the meni with data.
public class StabilometryAnalysisParameterMenuScript : MonoBehaviour
{
    #region Variables
    public MainScript mainScript { get; set; } = null;

    [SerializeField]
    private AccordionToggler[]
        parameterTogglers = null,
        taskTogglers = null;

    [SerializeField]
    private GameObject
        lineChartPrefab = null,
        chartHolder = null,
        chartMask = null;

    [SerializeField] private ScrollbarScript scrollbarScript = null;

    [SerializeField] private GameObject measurementMenu = null;

    private List<GameObject> instantiatedCharts = null;
    private Vector2 lineChartSize = new Vector2(590, 300);
    private Vector2 firstPosition = new Vector2();

    private List<StabilometryMeasurement> patientData = null;

    private Pose currentPose = Pose.BOTH_LEGS_JOINED_PARALLEL;
    private bool hasData = false;
    private bool chartsSpawned = false;
    #endregion

    private void Awake()
    {
        SetToggleDependencies();

        instantiatedCharts = new List<GameObject>();
        firstPosition = lineChartSize / 2f - chartHolder.GetComponent<RectTransform>().rect.size / 2f;
    }

    private void SetToggleDependencies()
    {
        foreach (AccordionToggler toggler in parameterTogglers)
            toggler.AnalysisMenuScript = this;

        foreach (AccordionToggler toggler in taskTogglers)
            toggler.AnalysisMenuScript = this;
    }

    private void OnEnable()
    {
        hasData = false;
        chartsSpawned = false;
        currentPose = Pose.BOTH_LEGS_JOINED_PARALLEL;

        patientData = mainScript.database.GetAllMeasurements(mainScript.currentPatient, currentPose);
        hasData = true;
        UpdateCharts();
    }

    private void Update()
    {
        if (HasAnyToggleChanged())
        {
            UpdateCharts();
        }
    }

    /// <summary>
    /// Checks if any toggler has been changed.
    /// </summary>
    /// <returns></returns>
    private bool HasAnyToggleChanged()
    {
        bool result = false;
        
        foreach (AccordionToggler toggler in parameterTogglers)
        {
            if (toggler.ToggleChanged)
            {
                toggler.ToggleChanged = false;
                result = true;
            }
        }

        foreach (AccordionToggler toggler in taskTogglers)
        {
            if (toggler.ToggleChanged)
            {
                toggler.ToggleChanged = false;
                result = true;
            }
        }

        return result;
    }

    /// <summary>
    /// Spawns charts and updates scrollbar.
    /// </summary>
    private void UpdateCharts()
    {
        SpawnCharts(GetChosenParameters(this.parameterTogglers), GetChosenTasks(this.taskTogglers));

        // Update Scrollbar size.
        float maskSize = ((RectTransform)chartMask.transform).rect.size.y;
        scrollbarScript.SetSize(GetCurrentChartAreaSize(instantiatedCharts, maskSize), maskSize);
    }

    /// <summary>
    /// Destroys all charts and spawns new ones.
    /// </summary>
    /// <param name="allParameters"></param>
    /// <param name="allTasks"></param>
    private void SpawnCharts(List<Parameter> allParameters, List<Task> selectedTasks)
    {
        foreach (GameObject instance in instantiatedCharts)
            Destroy(instance);

        instantiatedCharts = new List<GameObject>();

        for (int i = 0; i < allParameters.Count; i++)
        {
            GameObject instance = Instantiate(lineChartPrefab, chartHolder.transform);
            RectTransform instanceTransfrom = (RectTransform)instance.transform;
            instanceTransfrom.sizeDelta = lineChartSize;
            instanceTransfrom.localPosition = GetNewPosition(i, firstPosition, lineChartSize);

            LineChartScript chartScript = instance.GetComponent<LineChartScript>();

            chartScript.SetChartData(GetCurrentChartData(allParameters[i]), allParameters[i], selectedTasks);

            chartScript.SetParent(i, this);

            instantiatedCharts.Add(instance);
        }
    }

    private List<ChartData> GetCurrentChartData(Parameter currentParameter)
    {
        List<ChartData> result = new List<ChartData>();

        foreach(StabilometryMeasurement measurement in patientData)
            result.Add(measurement.GetData(currentParameter));

        return result;
    }

    /// <summary>
    /// Checks parameters to see which charts to spanw.
    /// </summary>
    /// <param name="parameterTogglers"></param>
    /// <returns></returns>
    private List<Parameter> GetChosenParameters(AccordionToggler[] parameterTogglers)
    {
        List<Parameter> result = new List<Parameter>();

        for (int i = 0; i < parameterTogglers.Length; i++)
        {
            if (parameterTogglers[i].GetToggle().isOn)
                result.Add((Parameter)i);
        }

        return result;
    }

    /// <summary>
    /// Checks to see which tasks have been chosen.
    /// </summary>
    /// <param name="taskTogglers"></param>
    /// <returns></returns>
    private List<Task> GetChosenTasks(AccordionToggler[] taskTogglers)
    {
        List<Task> result = new List<Task>();

        for (int i = 0; i < taskTogglers.Length; i++)
            if (taskTogglers[i].GetToggle().isOn)
                result.Add((Task)i);

        return result;
    }

    /// <summary>
    /// Get the size of the area for currently spawned charts.
    /// </summary>
    /// <param name="allInstances"></param>
    /// <param name="maskSize"></param>
    /// <returns></returns>
    private float GetCurrentChartAreaSize(List<GameObject> allInstances, float maskSize)
    {
        if (allInstances.Count < 2)
            return maskSize;

        // else
        RectTransform firstRect = (RectTransform)allInstances[0].transform;
        RectTransform lastRect = (RectTransform)allInstances[allInstances.Count - 1].transform;

        float result = Mathf.Abs((firstRect.localPosition.y + firstRect.rect.height / 2f) - (lastRect.localPosition.y - lastRect.rect.height / 2f));

        return result;
    }

    /// <summary>
    /// Gets the position for the next line chart to spawn.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="originalPosition"></param>
    /// <param name="chartSize"></param>
    /// <returns></returns>
    private Vector2 GetNewPosition(int index, Vector2 originalPosition, Vector2 chartSize)
    {
        float x = (index % 2 == 0) ? originalPosition.x : -originalPosition.x;
        int row = index / 2;
        float y = originalPosition.y - row * (chartSize.y + 20);

        return new Vector2(x, y);
    }

    /// <summary>
    /// Used to open other menus.
    /// </summary>
    /// <param name="menu"></param>
    public void OpenMenu(GameObject menu)
    {
        mainScript.menuSwitching.OpenMenu(menu);
    }

    public void OpenAnalysisMenu(int index)
    {
        OpenMenu(measurementMenu);
    }
}
