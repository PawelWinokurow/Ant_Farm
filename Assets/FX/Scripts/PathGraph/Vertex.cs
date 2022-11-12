using UnityEngine;
using System.Collections.Generic;

public class Vertex
{
    public string id;
    public Vector3 position;
    public float pathWeight;
    public List<Edge> edges { get; set; }
    public List<Vertex> neighbours = new List<Vertex>();
    public bool isCentralVertex { get; set; }
    public FloorHexagon floorHexagon { get; set; }
    public Vertex(string id, Vector3 position, bool isCentralVertex)
    {
        this.id = id;
        this.position = position;
        this.pathWeight = float.MaxValue;
        this.edges = new List<Edge>();
        this.isCentralVertex = isCentralVertex;
    }

    public void ResetPathWeight()
    {
        pathWeight = float.MaxValue;
    }

}