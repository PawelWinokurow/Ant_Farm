using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class HexagonTransform
{
    public static HexagonSerializable ToSerializable(Hexagon hexagon)
    {
        return new HexagonSerializable()
        {
            Id = hexagon.id,
            HexType = hexagon.type,
            Position = VectorTransform.ToSerializable(hexagon.position)
        };
    }

}