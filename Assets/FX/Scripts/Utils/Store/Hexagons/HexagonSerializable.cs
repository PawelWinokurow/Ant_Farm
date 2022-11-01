using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HexagonSerializable
{
    public string Id { get; set; }
    public HEX_TYPE HexType { get; set; }
    public Vector3Serializable Position { get; set; }
    public bool IsEmpty { get => HexType == HEX_TYPE.EMPTY; }
    public bool IsSoil { get => HexType == HEX_TYPE.SOIL; }

}

