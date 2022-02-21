using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabilometryImageScript : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private GameObject StabilometryLine = null,
        EllipseLine = null,
        TestDot = null,
        DataHolder = null;
    #endregion

    private void Start()
    {
        DrawImage(null);
    }

    public void DrawImage(StabilometryTask stabilometryTask)
    {
        List<Vector2> testPositions = new List<Vector2>();
        testPositions.Add(new Vector2(0, 0));
        testPositions.Add(new Vector2(0, 50));
        testPositions.Add(new Vector2(50, 0));
        testPositions.Add(new Vector2(100, 100));

        DrawStabilometryPath(testPositions);

    }

    private void DrawStabilometryPath(List<Vector2> stabilometryData)
    {
        Vector2 firstPosition = stabilometryData[0];

        Vector2 imageCenter = transform.position;

        for (int i = 1; i < stabilometryData.Count; i++)
        {
            Vector2 adjustedPosition = imageCenter + stabilometryData[i] - firstPosition;
            Instantiate(TestDot, adjustedPosition, Quaternion.identity, DataHolder.transform);
            
        }
    }

    private void DrawEllipsPath(List<Vector2> ellipseData)
    {

    }
}
