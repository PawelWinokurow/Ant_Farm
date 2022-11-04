using System;
using System.Collections.Generic;

[Serializable]
public class VertexSerializable
{
    public string Id;
    public Vector3Serializable Position;
    public float PathWeight;
    public List<Vector3Serializable> Neighbours { get; set; }
    public bool IsCentralVertex { get; set; }

    public VertexSerializable(string id, Vector3Serializable position, List<Vector3Serializable> neighbours, bool isCentralVertex)
    {
        Id = id;
        Position = position;
        PathWeight = float.MaxValue;
        Neighbours = neighbours;
        IsCentralVertex = isCentralVertex;
    }

    public void ResetPathWeight()
    {
        PathWeight = float.MaxValue;
    }

}