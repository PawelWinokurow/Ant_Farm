using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HEX_TYPE
{
    EMPTY, SOIL, BUILDING
}

public class Hexagon : MonoBehaviour
{
    public string Id { get; set; }
    public HEX_TYPE HexType { get; set; }
    public Vector3 Position { get; set; }
    public bool IsEmpty { get => HexType == HEX_TYPE.EMPTY; }
    public bool IsSoil { get => HexType == HEX_TYPE.SOIL; }


    public static Hexagon CreateHexagon(string Id, Object hexPrefab, Vector3 hexPosition, Transform parent)
    {
        Hexagon hex = (Hexagon)Instantiate(hexPrefab, hexPosition, Quaternion.identity, parent);
        hex.Position = hexPosition;
        hex.Id = Id;
        hex.HexType = HEX_TYPE.EMPTY;
        return hex;
    }
}

