using UnityEngine;

public interface Hexagon
{
    public static float radius = 2f;
    public string id { get; set; }
    public HexType type { get; set; }
    public Vector3 position { get; set; }
}