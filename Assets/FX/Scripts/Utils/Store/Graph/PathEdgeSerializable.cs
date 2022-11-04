using System;

[Serializable]
public class EdgeSerializable
{
    public Vector3Serializable FromPosition;
    public Vector3Serializable ToPosition;
    public float EdgeWeight;
    public bool IsWalkable;

    public EdgeSerializable(VertexSerializable From, VertexSerializable To, float EdgeWeight)
    {
        this.FromPosition = From.Position;
        this.ToPosition = To.Position;
        this.EdgeWeight = EdgeWeight;
        this.IsWalkable = true;
    }
}