using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Axes;

// Formulas for ellipse was taken from https://www1.udel.edu/biology/rosewc/kaap686/reserve/cop/center%20of%20position%20conf95.pdf
public class EllipseValues
{
    #region Variables

    public float area = 0;
    public List<Vector2> ellipsePoints = null;

    private const float ellipse95Multiplicator = 5.991f;

    #endregion

    
    public EllipseValues(List<DataPoint> stabilometryData)
    {
        if (stabilometryData.Count < 1)
            return;
        // else

        List<Vector2> normalizedVector = NormalizeVector(stabilometryData);

        Debug.Log(CalculateMean(normalizedVector));

        CMatrix cMatrix = CalculateCMatrix(normalizedVector, CalculateMean(normalizedVector));

        float[] eigenvalues = CalculateEigenVectors(cMatrix);

        float semiMajorAxis = Mathf.Sqrt(eigenvalues[0] / (normalizedVector.Count - 1));
        float semiMinorAxis = Mathf.Sqrt(eigenvalues[1] / (normalizedVector.Count - 1));

        // Calculates the smallest area of ellipse where at least 95% of the data points are located.
        this.area = ellipse95Multiplicator * Mathf.PI * semiMajorAxis * semiMinorAxis;

        this.ellipsePoints = CalculateEllipsePoints(semiMajorAxis, semiMinorAxis, eigenvalues, cMatrix);
    }

    private Vector2 CalculateMean(List<Vector2> stabilometryData)
    {
        Vector2 result = new Vector2();

        foreach (Vector2 point in stabilometryData)
            result += point;

        return result / stabilometryData.Count;
    }

    private CMatrix CalculateCMatrix(List<Vector2> stabilometryData, Vector2 mean)
    {
        CMatrix result = new CMatrix();

        foreach (Vector2 point in stabilometryData)
        {
            Vector2 modifiedPoint = point - mean;
            result.Cxx += Mathf.Pow(modifiedPoint.x, 2);
            result.Cxy += modifiedPoint.x * modifiedPoint.y;
            result.Cyy += Mathf.Pow(modifiedPoint.y, 2);
        }

        return result;
    }

    /// <summary>
    /// Calculates eigenVectors from cMatrix. The first eigen value is larger than the second.
    /// </summary>
    /// <param name="cMatrix"></param>
    /// <returns></returns>
    private float[] CalculateEigenVectors(CMatrix cMatrix)
    {
        float[] result = new float[2];

        float firstPart = (cMatrix.Cxx + cMatrix.Cyy) / 2f;

        float secondPart = Mathf.Sqrt(Mathf.Pow(cMatrix.Cxy, 2) + Mathf.Pow((cMatrix.Cxx - cMatrix.Cyy) / 2, 2));

        result[0] = firstPart + secondPart;
        result[1] = firstPart - secondPart;

        return result;
    }

    /// <summary>
    /// Returns list of points
    /// </summary>
    /// <param name="stabilometryData"></param>
    /// <returns></returns>
    private List<Vector2> NormalizeVector(List<DataPoint> stabilometryData)
    {
        List<Vector2> result = new List<Vector2>();

        Vector2 firstValue = stabilometryData[0].GetVecotor2(Both);

        foreach (DataPoint point in stabilometryData)
            result.Add(point.GetVecotor2(Both) - firstValue);

        return result;
    }

    /// <summary>
    /// Calculates points that will be used for drawing ellipse.
    /// </summary>
    /// <param name="semiMajorAxis"></param>
    /// <param name="semiMinorAxis"></param>
    /// <param name="eigenvalues"></param>
    /// <param name="cMatrix"></param>
    /// <returns></returns>
    private List<Vector2> CalculateEllipsePoints(float semiMajorAxis, float semiMinorAxis, float[] eigenvalues, CMatrix cMatrix)
    {
        List<Vector2> result = new List<Vector2>();

        Vector2 eigenvector0 = CalculateEigenVector(eigenvalues[0], cMatrix);
        Vector2 eigenvector1 = CalculateEigenVector(eigenvalues[0], cMatrix);

        List<float> radianValues = GetRadianValues(10);

        float sqrtMultip = Mathf.Sqrt(ellipse95Multiplicator);

        foreach (float value in radianValues)
        {
            
            Vector2 finalValue = sqrtMultip  *(Mathf.Cos(value) * semiMajorAxis * eigenvector0 
                + Mathf.Sin(value) * semiMinorAxis * eigenvector1);

            result.Add(finalValue);
        }

        return result;
    }

    private Vector2 CalculateEigenVector(float eigenvalue, CMatrix cMatrix)
    {
        Vector2 result = new Vector2();

        float tempValue = (cMatrix.Cxx - eigenvalue)/cMatrix.Cxy;

        result.x = 1f / Mathf.Sqrt(1 + Mathf.Pow(tempValue, 2));
        result.y = -tempValue * result.x;

        return result;
    }

    /// <summary>
    /// Returns the number of radian values starting from 0 to 2pi. 
    /// Values have the same distance between them.
    /// </summary>
    /// <param name="startValue"></param>
    /// <param name="valueNumber"></param>
    /// <returns></returns>
    private List<float> GetRadianValues(int valueNumber)
    {
        List<float> result = new List<float>();

        if (valueNumber <= 0)
        {
            Debug.LogError($"{valueNumber} is lower than 0");
            return result;
        }

        float increment = 2 * Mathf.PI / valueNumber;

        for (int i = 0; i < valueNumber; i++)
            result.Add(increment * i);
        
        return result;
    }
}