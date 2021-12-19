using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DataPoint
{
    public float x,
        y,
        time;
    
    public DataPoint(float time, float x, float y)
    {
        this.time = time;
        this.x = x;
        this.y = y;
    }

    public DataPoint(string time, string x, string y)
    {
        this.time = float.Parse(time);
        this.x = float.Parse(x);
        this.y = float.Parse(y);
    }
}
