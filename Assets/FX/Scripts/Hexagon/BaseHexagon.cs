using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseHexagon : MonoBehaviour, Hexagon
{
    public string Id { get => FloorHexagon.Id; set => FloorHexagon.Id = value; }
    public Vector3 Position { get => FloorHexagon.Position; set => FloorHexagon.Position = value; }
    public HEX_TYPE Type { get => FloorHexagon.Type; set => FloorHexagon.Type = value; }
    public FloorHexagon FloorHexagon { get; set; }
    public float Storage { get; set; }

    public static BaseHexagon CreateHexagon(FloorHexagon parent, BaseHexagon baseHexPrefab)
    {
        BaseHexagon hex = Instantiate(baseHexPrefab, parent.Position, Quaternion.identity, parent.transform);
        hex.FloorHexagon = parent;
        hex.FloorHexagon.Child = hex;
        return hex;
    }

}

