using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class HexagonTransform
{
    public static HexagonSerializable ToSerializable(Hexagon hexagon)
    {
        var hexagonSerializable = new HexagonSerializable();
        hexagonSerializable.Id = hexagon.Id;
        hexagonSerializable.HexType = hexagon.HexType;
        hexagonSerializable.Position = VectorTransform.ToSerializable(hexagon.Position);
        return hexagonSerializable;
    }
    public static Hexagon FromSerializable(HexagonSerializable hexagonSerializable, Object hexPrefab, Transform parent)
    {
        return Hexagon.CreateHexagon(hexagonSerializable.Id, hexPrefab, VectorTransform.FromSerializable(hexagonSerializable.Position), parent);
    }
}