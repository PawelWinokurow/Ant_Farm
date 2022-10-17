using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public List<Hexagon> hexagons;
    public Hexagon hexagonFloor;
    public Hexagon hexagonWall;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

    }

   
}
