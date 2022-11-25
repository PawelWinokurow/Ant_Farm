using System;
using System.Collections.Generic;

[Serializable]
public class HexagonSerializable
{
    public string Id { get; set; }
    public HexType HexType { get; set; }
    public Vector3Serializable Position { get; set; }
    public bool IsEmpty { get => HexType == HexType.EMPTY; }
    public bool IsSoil { get => HexType == HexType.SOIL; }
}

