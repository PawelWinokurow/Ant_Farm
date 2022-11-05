using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HEX_TYPE
{
    EMPTY, SOIL, BUILDING, FOOD, BASE
}

public class Hexagon : MonoBehaviour, Constructable
{
    public static float Radius = 2f;
    public string Id { get; set; }
    public HEX_TYPE HexType { get; set; }
    public Vector3 Position { get; set; }
    public bool IsEmpty { get => HexType == HEX_TYPE.EMPTY; }
    public bool IsSoil { get => HexType == HEX_TYPE.SOIL; }
    public bool IsFood { get => HexType == HEX_TYPE.FOOD; }
    public bool IsBase { get => HexType == HEX_TYPE.BASE; }
    public float Work { get; set; }

    public Hexagon AssignProperties(Hexagon hex)
    {
        Id = hex.Id;
        HexType = hex.HexType;
        Position = hex.Position;
        Work = hex.Work;
        return this;
    }

    public static Hexagon CreateHexagon(string Id, Hexagon hexPrefab, Vector3 hexPosition, Transform parent, HEX_TYPE hexType)
    {
        Hexagon hex = Instantiate(hexPrefab, hexPosition, Quaternion.identity, parent);
        hex.Position = hexPosition;
        hex.Id = Id;
        hex.HexType = hexType;
        hex.Work = 50f;
        return hex;
    }

    public void Clear()
    {
        for (int i = 0; i < this.transform.childCount; i++)//удаляет чайлды старой графики
        {
            GameObject.Destroy(this.transform.GetChild(i).gameObject);
        }
    }

}

