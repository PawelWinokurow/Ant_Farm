using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controller2 : MonoBehaviour
{
    private Camera cam;
    public Transform marker;
    private Vector3 pos;
    private int x;
    private int z;
    private Surface2 surf;

    private void Start()
    {
        cam = Camera.main;
        surf = Surface2.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            pos = cam.ScreenToWorldPoint(Input.mousePosition);

            // marker.transform.position = new Vector3(x* surf.w + (z % 2) * 0.5f* surf.w, 0, z* surf.h);
            marker.transform.position = surf.allHex[surf.PositionToId(pos)].transform.position;
        }
    }



}

