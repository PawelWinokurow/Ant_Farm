using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HEX_TYPE
{
    EMPTY, SOIL, FOOD, BASE
}

public class FloorHexagon : MonoBehaviour, Hexagon
{
    public string Id { get; set; }
    public HEX_TYPE Type { get; set; }
    public Vector3 Position { get; set; }
    public float Work { get; set; }
    public Hexagon Child { get; set; }
    public Vertex Vertex { get; set; }

    public static FloorHexagon CreateHexagon(string id, FloorHexagon hexPrefab, Vector3 hexPosition, Transform parent, HEX_TYPE hexType, float work)
    {
        FloorHexagon hex = Instantiate(hexPrefab, hexPosition, Quaternion.identity, parent);
        hex.Id = id;
        hex.Type = hexType;
        hex.Position = hexPosition;
        hex.Work = work;
        return hex;
    }

    public FloorHexagon AssignProperties(FloorHexagon hex)
    {
        Id = hex.Id;
        Position = hex.Position;
        Type = hex.Type;
        Work = hex.Work;
        Child = hex.Child;
        return this;
    }

    public void RemoveChildren()
    {
        for (int i = 0; i < this.transform.childCount; i++)//удаляет чайлды старой графики
        {
            GameObject.Destroy(this.transform.GetChild(i).gameObject);
        }
    }

}

