using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseHexagon : MonoBehaviour, Hexagon
{
    public string id { get => floorHexagon.id; set => floorHexagon.id = value; }
    public Vector3 position { get => floorHexagon.position; set => floorHexagon.position = value; }
    public HexType type { get => floorHexagon.type; set => floorHexagon.type = value; }
    public FloorHexagon floorHexagon { get; set; }
    public float storage { get; set; }

    public static BaseHexagon CreateHexagon(FloorHexagon parent, BaseHexagon baseHexPrefab)
    {
        BaseHexagon hex = Instantiate(baseHexPrefab, parent.position, Quaternion.identity, parent.transform);
        hex.floorHexagon = parent;
        hex.floorHexagon.child = hex;
        return hex;
    }

}

