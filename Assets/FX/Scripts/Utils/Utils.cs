using System;
using System.Linq;
using UnityEngine;



public class Utils
{
    public static float CalculateDistanceFromPoints(Vector3[] points)
    {
        float distance = 0f;
        points.Aggregate((firstVector, secondVector) => { distance += Vector3.Distance(firstVector, secondVector); return secondVector; });
        return distance;
    }

}



