using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Axes;

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

    public Vector2 GetVecotor2(Axes axes)
    {
        switch (axes)
        {
            case (BOTH):
                return new Vector2(x,y);
            case (AP):
                return new Vector2(0,y);
            case (ML):
                return new Vector2(x,0);
            default:
                Debug.LogError($"{axes} does not exist");
                return new Vector2();
                
        }
    }
}
