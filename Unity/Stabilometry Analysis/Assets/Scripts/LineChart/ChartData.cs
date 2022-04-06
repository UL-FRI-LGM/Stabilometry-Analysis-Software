using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChartData
{
    public float[] values;
    public MyDateTime time;

    public ChartData(float[] values, MyDateTime time)
    {
        this.values = values;
        this.time = time;
    }
}
