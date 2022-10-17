using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Test : MonoBehaviour
{
    public GameObject go;
    public GameObject surface;
    private NavMeshSurface navSurface;

    void Awake()
    {
        navSurface = surface.GetComponent<NavMeshSurface>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Input.GetKey(KeyCode.A));
        go.SetActive(Input.GetKey(KeyCode.A));
        navSurface.BuildNavMesh();
    }
}
