using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HEX_TYPE
{
    EMPTY, SOIL, BUILDING
}

public class Hexagon : MonoBehaviour
{
    public int id;
    public HEX_TYPE HexType { get; set; }

}

