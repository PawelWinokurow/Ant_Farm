using System;

[Serializable]
public class EdgeSerializable
{
    public Vector3Serializable fromPosition;
    public Vector3Serializable toPosition;
    public float edgeWeight;
    public bool isWalkable;

    public EdgeSerializable(VertexSerializable from, VertexSerializable to, float edgeWeight)
    {
        this.fromPosition = from.position;
        this.toPosition = to.position;
        this.edgeWeight = edgeWeight;
        this.isWalkable = true;
    }
}