using UnityEngine;

public interface Hexagon
{
    public static float Radius = 2f;
    public string Id { get; set; }
    public HEX_TYPE Type { get; set; }
    public Vector3 Position { get; set; }
}