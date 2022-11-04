using UnityEngine;
using System.Collections.Generic;

public class Vertex
{
    public string Id;
    public Vector3 Position;
    public float PathWeight;
    public List<Edge> Edges { get; set; }
    public List<Vertex> Neighbours = new List<Vertex>();
    public bool IsCentralVertex { get; set; }

    public Vertex(string id, Vector3 position, bool isCentralVertex)
    {
        Id = id;
        Position = position;
        PathWeight = float.MaxValue;
        Edges = new List<Edge>();
        IsCentralVertex = isCentralVertex;
    }

    public void ResetPathWeight()
    {
        PathWeight = float.MaxValue;
    }

}