using System;

[Serializable]
public class PathEdgeSerializable
{
    public PathVertexSerializable From;
    public PathVertexSerializable To;
    public float EdgeWeight;
    public bool IsWalkable;

    public PathEdgeSerializable(PathVertexSerializable From, PathVertexSerializable To, float EdgeWeight)
    {
        this.From = From;
        this.To = To;
        this.EdgeWeight = EdgeWeight;
        this.IsWalkable = true;
    }
}