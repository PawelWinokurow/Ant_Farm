using System;

[Serializable]
public class PathEdgeSerializable
{
    public Vector3Serializable FromPosition;
    public Vector3Serializable ToPosition;
    public float EdgeWeight;
    public bool IsWalkable;

    public PathEdgeSerializable(PathVertexSerializable From, PathVertexSerializable To, float EdgeWeight)
    {
        this.FromPosition = From.Position;
        this.ToPosition = To.Position;
        this.EdgeWeight = EdgeWeight;
        this.IsWalkable = true;
    }
}