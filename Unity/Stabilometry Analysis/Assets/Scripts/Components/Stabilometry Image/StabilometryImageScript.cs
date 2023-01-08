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

        private float lowerPrecisionDrawingErrorValue = 5f;
        private float higherPrecisionDrawingErrorValue = 1f;
        #endregion

        public void DrawImage(StabilometryTask stabilometryTask, bool highPrecision)
        {
            float multiplicator = DetermineMultiplicator();

            DrawStabilometryPath(stabilometryTask.stabilometryDrawData, multiplicator, highPrecision);
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
        private void DrawStabilometryPath(List<Vector2> stabilometryData, float multiplicator, bool highPrecision)
        {
            List<Vector2> filteredData = PrepareDataForDrawing(stabilometryData, multiplicator, highPrecision);

            Vector2[] points = new Vector2[filteredData.Count];

            for (int i = 0; i < filteredData.Count; i++)
                points[i] = filteredData[i];

            pathLine.Points = points;
        }

        private List<Vector2> PrepareDataForDrawing(List<Vector2> stabilometryData, float multiplicator, bool highPrecision)
        {
            List<Vector2> result = new List<Vector2>();
            result.Add(stabilometryData[0] * multiplicator);

            for (int i = 1; i < stabilometryData.Count; i++)
            {
                Vector2 newData = stabilometryData[i] * multiplicator;

                Vector2 lastData = result[result.Count - 1];

                if (!DataTooSimilar(lastData, newData, highPrecision))
                    result.Add(newData);
            }

            return result;
        }

        private bool DataTooSimilar(Vector2 previousData, Vector2 newData, bool highPrecision)
        {
            //return false;

            float drawingErrorValue = (highPrecision) ? higherPrecisionDrawingErrorValue : lowerPrecisionDrawingErrorValue;

            Vector2 difference = newData - previousData;

            return difference.magnitude <= drawingErrorValue;
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