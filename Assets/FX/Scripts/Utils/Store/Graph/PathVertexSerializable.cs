using System;
using System.Collections.Generic;

[Serializable]
public class VertexSerializable
{
    public string id;
    public Vector3Serializable position;
    public float pathWeight;
    public List<Vector3Serializable> neighbours { get; set; }
    public bool isCentralVertex { get; set; }

    public VertexSerializable(string id, Vector3Serializable position, List<Vector3Serializable> neighbours, bool isCentralVertex)
    {
        this.id = id;
        this.position = position;
        this.pathWeight = float.MaxValue;
        this.neighbours = neighbours;
        this.isCentralVertex = isCentralVertex;
    }

    public void ResetPathWeight()
    {
        pathWeight = float.MaxValue;
    }

}