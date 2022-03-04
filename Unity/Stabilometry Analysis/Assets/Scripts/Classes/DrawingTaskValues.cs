using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DrawingTaskValues
{
    public List<Vector2> linePoints = null;
    public List<Vector2> ellipsePoints = null;

    public DrawingTaskValues(StabilometryTask task)
    {
        linePoints = task.stabilometryDrawData;
        ellipsePoints = task.confidence95Ellipse.ellipsePoints;
    }
}
