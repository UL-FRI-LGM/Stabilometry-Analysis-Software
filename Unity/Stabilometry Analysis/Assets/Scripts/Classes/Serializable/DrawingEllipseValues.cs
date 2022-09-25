using System;
using UnityEngine;

namespace StabilometryAnalysis
{
    [Serializable]
    public class DrawingEllipseValues
    {
        #region Variables
        public float area = 0;

        public Vector2[] eigenVectors = null;

        public float semiMajorAxis = -1;
        public float semiMinorAxis = -1;
        #endregion

        public DrawingEllipseValues(EllipseValues values)
        {
            area = values.area;
            eigenVectors = values.eigenVectors;
            semiMajorAxis = values.semiMajorAxis;
            semiMinorAxis = values.semiMinorAxis;
        }
    }
}