using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPoint
{
    public float x,
        y,
        time;
    
    public DataPoint(float x, float y, float time)
    {
        this.x = x;
        this.y = y;
        this.time = time;
    }

    public DataPoint(string x, string y, string time)
    {
        this.x = float.Parse(x);
        this.y = float.Parse(y);
        this.time = float.Parse(time);
    }
}
