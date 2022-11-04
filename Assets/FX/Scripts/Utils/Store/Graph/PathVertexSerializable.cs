using System;
using System.Collections.Generic;

[Serializable]
public class VertexSerializable
{
    public string Id;
    public Vector3Serializable Position;
    public float PathWeight;

    public VertexSerializable(string id, Vector3Serializable position)
    {
        Id = id;
        Position = position;
        PathWeight = float.MaxValue;
    }

    public void ResetPathWeight()
    {
        PathWeight = float.MaxValue;
    }

}