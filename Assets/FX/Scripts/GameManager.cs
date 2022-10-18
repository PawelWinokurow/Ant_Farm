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
    public Hexagon hexagonFloor;
    public Hexagon hexagonWall;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        navSurface = GetComponent<NavMeshSurface>();
    }

    void Start()
    {

    }



    public void AddFloor(Hexagon hex)
    {
        Vector3 pos = hex.transform.position;
        hexagons.Remove(hex);
        GameObject.Destroy(hex.gameObject);
        Instantiate(hexagonFloor, pos, Quaternion.identity,transform);
    }

    
    public void AddPath(Hexagon hex)
    {
        Vector3 pos = hex.transform.position;
        hexagons.Remove(hex);
        GameObject.Destroy(hex.gameObject);
        Instantiate(hexagonFloor, pos, Quaternion.identity, transform);
        navSurface.BuildNavMesh();
    }

    
}
