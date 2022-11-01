using UnityEngine;
using System.Collections.Generic;

public class PathVertex
{
    public string Id;
    public Vector3 Position;
    public float PathWeight;
    public List<PathEdge> Edges { get; set; }


    public PathVertex(string id, Vector3 position)
    {
        Id = id;
        Position = position;
        PathWeight = float.MaxValue;
        Edges = new List<PathEdge>();
    }

    public void ResetPathWeight()
    {
        PathWeight = float.MaxValue;
    }

}