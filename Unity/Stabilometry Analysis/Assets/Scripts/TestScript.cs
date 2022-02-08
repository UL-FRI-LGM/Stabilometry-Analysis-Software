using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //StabilometryTask test = new StabilometryTask();
        //test.Test();
        MyDateTime newDate = new MyDateTime(202212231854);
        Debug.Log($"Year {newDate.year} Month {newDate.month} Day {newDate.day} Hour {newDate.hour} Minutes {newDate.minutes}");
        Debug.Log($"{newDate.ToString()}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
