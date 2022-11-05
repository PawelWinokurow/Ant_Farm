using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class HexagonTransform
{
    public static HexagonSerializable ToSerializable(Hexagon hexagon)
    {
        return new HexagonSerializable()
        {
            Id = hexagon.Id,
            HexType = hexagon.Type,
            Position = VectorTransform.ToSerializable(hexagon.Position)
        };
    }

}