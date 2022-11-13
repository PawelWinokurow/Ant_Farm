using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HEX_TYPE
{
    EMPTY, SOIL, FOOD, BASE
}

public class FloorHexagon : MonoBehaviour, Hexagon
{
    public string id { get; set; }
    public HEX_TYPE type { get; set; }
    public Vector3 position { get; set; }
    public float work { get; set; }
    public Hexagon child { get; set; }
    public Vertex vertex { get; set; }

    public static FloorHexagon CreateHexagon(string id, FloorHexagon HexPrefab, Vector3 hexPosition, Transform parent, HEX_TYPE hexType, float work)
    {
        FloorHexagon hex = Instantiate(HexPrefab, hexPosition, Quaternion.identity, parent);
        hex.id = id;
        hex.type = hexType;
        hex.position = hexPosition;
        hex.work = work;
        return hex;
    }

    public FloorHexagon AssignProperties(FloorHexagon hex)
    {
        id = hex.id;
        position = hex.position;
        type = hex.type;
        work = hex.work;
        child = hex.child;
        return this;
    }

    public void RemoveChildren()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            GameObject.Destroy(this.transform.GetChild(i).gameObject);
        }
    }

}

