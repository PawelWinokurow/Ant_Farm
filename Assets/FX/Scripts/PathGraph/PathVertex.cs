using UnityEngine;
using System.Collections.Generic;

public class PathVertex
{
    public Vector3 GeometricalPoint;
    public string Id;
    public List<Edge> Edges { get; set; }

    public float PathWeight;

    public PathVertex(string id, Vector3 geometricalPoint)
    {
        GeometricalPoint = geometricalPoint;
        PathWeight = float.MaxValue;
        Edges = new List<Edge>();
        Id = id;
    }

    public void ResetPathWeight()
    {
        PathWeight = float.MaxValue;
    }

}