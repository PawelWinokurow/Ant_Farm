using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return  (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static float RemapClamp(this float value, float from1, float to1, float from2, float to2)
    {
        return Mathf.Clamp((value - from1) / (to1 - from1) * (to2 - from2) + from2,Mathf.Min( from2, to2), Mathf.Max(from2, to2));

    }

    public static void SetGlobalScale(Transform transform, Vector3 globalScale)
    {
        transform.localScale = Vector3.one;
        transform.localScale = new Vector3(globalScale.x / transform.lossyScale.x, globalScale.y / transform.lossyScale.y, globalScale.z / transform.lossyScale.z);
    }

    public static Vector3 Intersect(Vector3 planeP, Vector3 planeN, Vector3 rayP, Vector3 rayD)//пересечение прямой и плоскости
    {
        var d = Vector3.Dot(planeP, -planeN);
        var t = -(d + Vector3.Dot(rayP, planeN)) / Vector3.Dot(rayD, planeN);
        return rayP + t * rayD;
    }
}
