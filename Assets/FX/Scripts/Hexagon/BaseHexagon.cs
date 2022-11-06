using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseHexagon : MonoBehaviour, Hexagon
{
    public string Id { get; set; }
    public FloorHexagon FloorHexagon { get; set; }
    public Vector3 Position { get; set; }
    public HEX_TYPE Type { get => FloorHexagon.Type; set => FloorHexagon.Type = value; }

    public int Storage { get; set; }

    public static BaseHexagon CreateHexagon(FloorHexagon parent, BaseHexagon baseHexPrefab)
    {
        BaseHexagon hex = Instantiate(baseHexPrefab, parent.Position, Quaternion.identity, parent.transform);
        hex.FloorHexagon = parent;
        hex.Position = parent.Position;
        hex.Id = parent.Id;
        hex.FloorHexagon.Child = hex;
        return hex;
    }

    public BaseHexagon AssignProperties(BaseHexagon hex)
    {
        Id = hex.Id;
        Type = hex.Type;
        Position = hex.Position;
        return this;
    }

}

