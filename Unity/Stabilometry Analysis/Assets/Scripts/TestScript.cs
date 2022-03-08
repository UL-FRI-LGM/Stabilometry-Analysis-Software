using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public GameObject stabilometryImage;
    public GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        //List<DataPoint> testPoints = new List<DataPoint>();
        //testPoints.Add(new DataPoint(0.0019984f, 0.1845152f, 1));
        //testPoints.Add(new DataPoint(0.007996f, 0.1845535f, 2));

        //EllipseValues testValues = new EllipseValues(testPoints);

        //GameObject instance = Instantiate(stabilometryImage, canvas.transform);
        //instance.GetComponent<StabilometryImageScript>().TestEllipse(testValues.ellipsePoints);

        EllipseValues.TestFunctions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
