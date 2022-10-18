using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public NavMeshSurface navSurface;
   // public GameObject back;
    public List<Hexagon> hexagons;
    public List<Hexagon> hexagonsDig;
    public Hexagon hexagonGround;
    public Hexagon hexagonWall;
    public Hexagon hexagonWallDig;
    public static event Action rebuildEvent;

    void Awake()
    {
        instance = this;
        navSurface = GetComponent<NavMeshSurface>();
        hexagonsDig = new List<Hexagon>();
        hexagons = new List<Hexagon>();
    }

   
    public void AddFloor(Hexagon hex)
    {
        Vector3 pos = hex.transform.position;
        hexagons.Remove(hex);
        GameObject.Destroy(hex.gameObject);
        hex = Instantiate(hexagonGround, pos, Quaternion.identity, transform);
        hexagons.Add(hex);
    }

    
    public void DigTarget(Hexagon hex)//если тапнули клетку копать
    {
        Vector3 pos = hex.transform.position;
        hexagons.Remove(hex);
        GameObject.Destroy(hex.gameObject);
        hex= Instantiate(hexagonWallDig, pos, Quaternion.identity, transform);
        hexagonsDig.Add(hex);
        hexagons.Add(hex);
        navSurface.BuildNavMesh();
        rebuildEvent();
    }

    public void Dig(Hexagon hex)//если выкопали
    {
        Vector3 pos = hex.transform.position;
        hexagons.Remove(hex);
        hexagonsDig.Remove(hex);
        GameObject.Destroy(hex.gameObject);
        hex = Instantiate(hexagonGround, pos, Quaternion.identity, transform);
        hexagons.Add(hex);
        navSurface.BuildNavMesh();
        rebuildEvent();
    }


}
