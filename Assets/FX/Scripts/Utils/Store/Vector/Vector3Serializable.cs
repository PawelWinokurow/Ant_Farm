using System;

[Serializable]
public class Vector3Serializable
{
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }

    public Vector3Serializable(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public override string ToString()
    {
        return $"{x}_{y}_{z}";
    }
}
