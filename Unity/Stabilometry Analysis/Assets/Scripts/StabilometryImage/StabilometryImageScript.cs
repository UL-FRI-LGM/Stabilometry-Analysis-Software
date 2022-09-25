using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace StabilometryAnalysis
{
    public class StabilometryImageScript : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        private GameObject TestDot = null;

        [SerializeField]
        private UILineRenderer pathLine = null,
            ellipseLine = null;

        private float multiplicator = 2500f;
        #endregion

        // To be removed 
        private void Awake()
        {
            //List<DataPoint> dataPoints = new List<DataPoint>();

            //dataPoints.Add(new DataPoint(0, 0.005f, 0.005f));
            //dataPoints.Add(new DataPoint(0, 0.01f, 0.01f));
            //dataPoints.Add(new DataPoint(0, 0.019f, 0.015f));
            //dataPoints.Add(new DataPoint(0, 0.019f, 0.015f));
            //dataPoints.Add(new DataPoint(0, 0.019f, -0.01f));
            //dataPoints.Add(new DataPoint(0, -0.019f, -0.01f));

            //StabilometryTask task = new StabilometryTask(dataPoints);
            //DrawImage(task);
        }

        public void DrawImage(StabilometryTask stabilometryTask)
        {
            DrawStabilometryPath(stabilometryTask.stabilometryDrawData);

            DrawEllipsPath(stabilometryTask.confidence95Ellipse.GetEllipsePoints(40));
        }

        ///// <summary>
        ///// Draws a stabilometry path. The data should be centered in fist data point.
        ///// </summary>
        ///// <param name="stabilometryData"></param>
        private void DrawStabilometryPath(List<Vector2> stabilometryData)
        {
            Vector2[] points = new Vector2[stabilometryData.Count];

            for (int i = 0; i < stabilometryData.Count; i++)
                points[i] = stabilometryData[i] * multiplicator;

            pathLine.Points = points;
        }

        ///// <summary>
        ///// Draws an elipsis
        ///// </summary>
        ///// <param name="ellipseData"></param>
        private void DrawEllipsPath(List<Vector2> ellipseData)
        {
            Vector2[] points = new Vector2[ellipseData.Count + 1];

            for (int i = 0; i < ellipseData.Count; i++)
                points[i] = ellipseData[i] * multiplicator;

            points[points.Length - 1] = ellipseData[0] * multiplicator;

            ellipseLine.Points = points;
        }
    }
}