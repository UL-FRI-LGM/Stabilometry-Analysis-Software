using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

public class JsonHelper
{
    [Serializable]
    private class Wrapper
    {
        public List<DataPoint> eyesOpenSolidSurface;
        public List<DataPoint> eyesClosedSolidSurface;
        public List<DataPoint> eyesOpenSoftSurface;
        public List<DataPoint> eyesClosedSoftSurface;
    }

    //public static T[] FromJson<T>(string json)
    //{
    //    Wrapper wrapper = JsonUtility.FromJson<Wrapper>(json);

    //    return wrapper.items;
    //}

    /// <summary>
    /// Converts stabilometry data to json string.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string ToJson(List<DataPoint>[] data)
    {
        Wrapper wrapper = new Wrapper();
        wrapper.eyesOpenSolidSurface = data[0];
        wrapper.eyesClosedSolidSurface = data[1];
        wrapper.eyesOpenSoftSurface = data[2];
        wrapper.eyesClosedSoftSurface = data[3];
        
        return JsonUtility.ToJson(wrapper);
    }
}
