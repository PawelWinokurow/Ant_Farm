using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UtilsTest
{
    [Test]
    public void CalculateDistanceFromPointsTest()
    {
        Vector3[] points = { new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 0) };
        float actual = Utils.CalculateDistanceFromPoints(points);
        Assert.AreEqual(3.41, actual, 0.01);
    }
}
