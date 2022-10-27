using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Utils
{
    public static List<Vector3> NormalizePath(List<Vector3> path, float length)
    {
        var normalizedPath = new List<Vector3>() { path[0] };
        var goal = path[path.Count - 1];
        path.RemoveAt(path.Count - 1);

        var currentVector = path[1] - path[0];
        var remainingLength = Vector3.Distance(currentVector, new Vector3(0, 0, 0));
        var normalizedVector = Vector3.Normalize(currentVector) * length;
        path.RemoveAt(0);

        while (path.Count != 0)
        {
            while (remainingLength >= length)
            {
                normalizedPath.Add(normalizedPath[normalizedPath.Count - 1] + normalizedVector);
                remainingLength -= length;
            }
            currentVector = path[0] - normalizedPath[normalizedPath.Count - 1];
            remainingLength = Vector3.Distance(currentVector, new Vector3(0, 0, 0));
            normalizedVector = Vector3.Normalize(currentVector) * length;
            path.RemoveAt(0);
        }
        normalizedPath.Add(goal);

        return normalizedPath;
    }
}