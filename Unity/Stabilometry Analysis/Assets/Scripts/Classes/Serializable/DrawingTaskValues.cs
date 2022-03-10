using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DrawingTaskValues
{
    public List<Vector2> linePoints = null;
    public DrawingEllipseValues ellipseValues = null;

    public DrawingTaskValues(StabilometryTask task)
    {
        linePoints = task.stabilometryDrawData;
        ellipseValues = new DrawingEllipseValues(task.confidence95Ellipse);
    }
}
