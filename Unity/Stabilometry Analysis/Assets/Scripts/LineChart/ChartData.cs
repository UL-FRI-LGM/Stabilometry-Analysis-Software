using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChartData
{
    public float value;
    public MyDateTime time;

    public ChartData(float value, MyDateTime time)
    {
        this.value = value;
        this.time = time;
    }
}
