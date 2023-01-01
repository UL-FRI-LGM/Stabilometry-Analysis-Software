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

        // In cm.
        private const float realSize = 10f;
        #endregion

        public void DrawImage(StabilometryTask stabilometryTask)
        {
            float multiplicator = DetermineMultiplicator();

            DrawStabilometryPath(stabilometryTask.stabilometryDrawData, multiplicator);
            DrawEllipsPath(stabilometryTask.confidence95Ellipse.GetEllipsePoints(40), multiplicator);
        }

        private float DetermineMultiplicator()
        {
            float imageXSize = GetComponent<RectTransform>().rect.width;

            return imageXSize / realSize;
        }

        ///// <summary>
        ///// Draws a stabilometry path. The data should be centered in fist data point.
        ///// </summary>
        ///// <param name="stabilometryData"></param>
        private void DrawStabilometryPath(List<Vector2> stabilometryData, float multiplicator)
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
        private void DrawEllipsPath(List<Vector2> ellipseData, float multiplicator)
        {
            Vector2[] points = new Vector2[ellipseData.Count + 1];

            for (int i = 0; i < ellipseData.Count; i++)
                points[i] = ellipseData[i] * multiplicator;

            points[points.Length - 1] = ellipseData[0] * multiplicator;

            ellipseLine.Points = points;
        }
    }
}