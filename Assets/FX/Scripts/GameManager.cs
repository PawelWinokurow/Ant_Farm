using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private NavMeshSurface navSurfaceBack;
    public GameObject back;
    public List<Hexagon> hexagons;
    public Hexagon hexagonFloor;
    public Hexagon hexagonWall;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        navSurfaceBack = back.GetComponent<NavMeshSurface>();
    }

    void Start()
    {
        navSurfaceBack.BuildNavMesh();
    }

    public void AddFloor(Hexagon hex)
    {
        Vector3 pos = hex.transform.position;
        hexagons.Remove(hex);
        GameObject.Destroy(hex.gameObject);
        navSurfaceBack.BuildNavMesh();
    }


}
