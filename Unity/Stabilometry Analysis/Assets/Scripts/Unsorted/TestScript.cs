﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StabilometryAnalysis
{
    public class TestScript : MonoBehaviour
    {
        public GameObject stabilometryImage;
        public GameObject canvas;

        // Start is called before the first frame update
        void Start()
        {

            //Debug.Log(Rounder.RoundFloat(0.000421f));
            //Debug.Log(Rounder.RoundFloat(100f));
            //Debug.Log(Rounder.RoundFloat(0.001f));
            //Debug.Log(Rounder.RoundFloat(231.025231f));

            
            //List<DataPoint> testPoints = new List<DataPoint>();
            //testPoints.Add(new DataPoint(0.0019984f, 0.1845152f, 1));
            //testPoints.Add(new DataPoint(0.007996f, 0.1845535f, 2));

            //EllipseValues testValues = new EllipseValues(testPoints);

            //GameObject instance = Instantiate(stabilometryImage, canvas.transform);
            //instance.GetComponent<StabilometryImageScript>().TestEllipse(testValues.ellipsePoints);

            //StabilometryTask.TestFunctions();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}