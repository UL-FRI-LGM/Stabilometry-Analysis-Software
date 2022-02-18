using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<DataPoint> testData = new List<DataPoint>();

        testData.Add(new DataPoint(1, 2, 1));
        testData.Add(new DataPoint(1, -2, -10));
        testData.Add(new DataPoint(1, 15, 1));
        testData.Add(new DataPoint(1, 15, 1));

        StabilometryTask test = new StabilometryTask(testData);

        Debug.Log(test.confidence95EllipseArea.ellipsePoints.Count);
        //MyDateTime newDate = new MyDateTime(202212231854);
        //Debug.Log($"Year {newDate.year} Month {newDate.month} Day {newDate.day} Hour {newDate.hour} Minutes {newDate.minutes}");
        //Debug.Log($"{newDate.ToString()}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
