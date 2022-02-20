using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabilometryImageScript : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private GameObject StabilometryLine = null,
        EllipseLine = null;
    #endregion

    public void DrawImage(StabilometryTask stabilometryTask)
    {

    }

    private void DrawStabilometryPath(List<DataPoint> stabilometryData)
    {
        
    }

    private void DrawEllipsPath(List<Vector2> ellipseData)
    {

    }
}
