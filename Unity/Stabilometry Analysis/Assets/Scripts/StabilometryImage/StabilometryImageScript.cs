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

    Vector2 position = Vector2.zero; 
    #endregion

    private void Start()
    {
        DrawImage(null);
    }

    public void DrawImage(StabilometryTask stabilometryTask)
    {
        position = transform.position;

        List<Vector2> testPositions = new List<Vector2>();
        testPositions.Add(new Vector2(0, 0));
        testPositions.Add(new Vector2(0, 50));
        testPositions.Add(new Vector2(50, 0));
        testPositions.Add(new Vector2(100, 100));
        testPositions.Add(new Vector2(100, -100));
        testPositions.Add(new Vector2(-100, -230));

        //DrawStabilometryPath(testPositions);
        //DrawEllipsPath(testPositions);
    }

    /// <summary>
    /// Draws a stabilometry path. The data should be centered in fist data point.
    /// </summary>
    /// <param name="stabilometryData"></param>
    private void DrawStabilometryPath(List<Vector2> stabilometryData)
    {
        for (int i = 1; i < stabilometryData.Count; i++)
            DrawLine(stabilometryData[i], stabilometryData[i - 1], StabilometryLine);
    }

    /// <summary>
    /// Draws an elipsis
    /// </summary>
    /// <param name="ellipseData"></param>
    private void DrawEllipsPath(List<Vector2> ellipseData)
    {
        for (int i = 1; i < ellipseData.Count; i++)
            DrawLine(ellipseData[i], ellipseData[i - 1], EllipseLine);

        DrawLine(ellipseData[ellipseData.Count - 1], ellipseData[0], EllipseLine);
    }

    /// <summary>
    /// Connects two points with a linePrefab.
    /// </summary>
    /// <param name="currentPosition"></param>
    /// <param name="previousPosition"></param>
    /// <param name="linePrefab"></param>
    private void DrawLine(Vector2 currentPosition, Vector2 previousPosition, GameObject linePrefab)
    {
        Vector2 difference = currentPosition - previousPosition;
        Vector2 adjustedLinePosition = position + (currentPosition + previousPosition) / 2;
        GameObject instance = Instantiate(linePrefab, adjustedLinePosition, Quaternion.FromToRotation(Vector3.right, difference), DataHolder.transform);

        RectTransform rect = instance.GetComponent<RectTransform>();

        rect.sizeDelta = new Vector2(difference.magnitude, rect.sizeDelta.y);

    }

}
