using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StabilometryAnalysisParameterMenuScript  : MonoBehaviour
{
    #region Variables
    public MainScript mainScript { get; set; } = null;

    [SerializeField] private AccordionToggler[] 
        parameterTogglers = null,
        taskTogglers = null;

    [SerializeField] private GameObject
        lineChartPrefab = null,
        chartHolder = null,
        chartMask = null;

    [SerializeField] private ScrollbarScript scrollbarScript = null;

    private List<GameObject> instantiatedCharts = null;
    private Vector2 lineChartSize = new Vector2(590, 300);
    private Vector2 firstPosition = new Vector2();

    #endregion

    private void Awake()
    {
        SetToggleDependencies();

        instantiatedCharts = new List<GameObject>();
        firstPosition = lineChartSize / 2f - chartHolder.GetComponent<RectTransform>().rect.size / 2f;
    }

    private void Start()
    {
        //UpdateCharts();
    }

    public void OpenMenu(GameObject menu)
    {
        mainScript.menuSwitching.OpenMenu(menu);
    }

    public void OpenDataMenu()
    {

    }

    private void OnEnable()
    {

        Pose currentPose = Pose.BOTH_LEGS_JOINED_PARALLEL;

        List<StabilometryMeasurement> measurements =  mainScript.database.GetAllMeasurements(mainScript.currentPatient, currentPose);
        Debug.Log(measurements.Count);
        
        //This doesn't work, fix it.
        //UpdateCharts();
    }

    private void SetToggleDependencies()
    {
        foreach (AccordionToggler toggler in parameterTogglers)
            toggler.analysisMenuScript = this;

        foreach (AccordionToggler toggler in taskTogglers)
            toggler.analysisMenuScript = this;
    }

    private List<Parameter> GetChosenParameters(AccordionToggler[] parameterTogglers)
    {
        List<Parameter> result = new List<Parameter>();

        for (int i = 0; i < parameterTogglers.Length; i++)
            if (parameterTogglers[i].toggle.isOn)
                result.Add((Parameter)i);

        return result;
    }

    private List<Task> GetChosenTasks(AccordionToggler[] taskTogglers)
    {
        List<Task> result = new List<Task>();
        
        for (int i = 0; i < taskTogglers.Length; i++)
            if (taskTogglers[i].toggle.isOn)
                result.Add((Task)i);

        return result;
    }

    private void UpdateCharts()
    {
        SpawnCharts(GetChosenParameters(this.parameterTogglers), GetChosenTasks(this.taskTogglers));

        float maskSize = ((RectTransform)chartMask.transform).rect.size.y;
        scrollbarScript.SetSize(CurrentSize(instantiatedCharts, maskSize), maskSize);
    }

    private float CurrentSize(List<GameObject> allInstances, float maskSize)
    {
        if (allInstances.Count < 2)
            return maskSize;

        // else

        RectTransform firstRect = (RectTransform)allInstances[0].transform;
        RectTransform lastRect = (RectTransform)allInstances[allInstances.Count - 1].transform;

        float result = Mathf.Abs((firstRect.localPosition.y + firstRect.rect.height / 2f) - (lastRect.localPosition.y - lastRect.rect.height / 2f));

        Debug.Log(result + " vs " + maskSize);
        
            return result;
    }

    private void SpawnCharts(List<Parameter> allParameters, List<Task> allTasks)
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

            instance.GetComponent<LineChartScript>().SetChartTitle(allParameters[i]);

            instantiatedCharts.Add(instance);
        }
    }

    private Vector2 GetNewPosition(int index, Vector2 originalPosition, Vector2 chartSize)
    {
        float x = (index % 2 == 0)? originalPosition.x : -originalPosition.x;
        int row = index / 2;
        float y = originalPosition.y - row * (chartSize.y + 20);

        return new Vector2(x, y);
    }

    public void ToggleValueChanged()
    {
        UpdateCharts();
    }
}
