using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Utils
{
    public static List<Vector3> NormalizePath(List<Vector3> path, float length)
    {
        var normalizedPath = new List<Vector3>() { path[0] };
        path.RemoveAt(0);
        var lastPoint = path[path.Count - 1];

        while (path.Count != 0)
        {
            var currentVector = path[0] - normalizedPath[normalizedPath.Count - 1];
            var remainingLength = Vector3.Distance(currentVector, new Vector3(0, 0, 0));
            var normalizedVector = Vector3.Normalize(currentVector) * length;
            while (remainingLength >= length)
            {
                normalizedPath.Add(normalizedPath[normalizedPath.Count - 1] + normalizedVector);
                remainingLength -= length;
            }
            path.RemoveAt(0);
        }
        path.Add(lastPoint);
        return normalizedPath;
    }
}