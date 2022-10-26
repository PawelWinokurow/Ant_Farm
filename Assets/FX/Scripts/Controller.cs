using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controller : MonoBehaviour
{
    private Camera cam;
    public Transform marker;
    private Vector3 pos;
    private int x;
    private int z;
    public GameManager gm;
    private Surface surface;

    private void Start()
    {
        cam = Camera.main;
        surface = gm.Surface;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            pos = cam.ScreenToWorldPoint(Input.mousePosition);

            // marker.transform.position = new Vector3(x* surf.w + (z % 2) * 0.5f* surf.w, 0, z* surf.h);
            marker.transform.position = surface.allHex[surface.PositionToId(pos)].transform.position;

        }
    }

    // public void ProcessHexagon(RaycastHit hit)
    // {
    //     Hexagon hex = hit.transform.GetComponent<Hexagon>();
    //     if (hex != null && !hex.IsInSpawnArea() && !scheduler.IsJobAlreadyCreated(hex.id))
    //     {

    //         if (hex.IsDigabble())
    //         {
    //             hex.AssignDig();
    //             hex = surface.AssignDigHex(hex.id);
    //         }

    //         if (hex.IsBuildable())
    //         {
    //             hex.AssignBuild();
    //             hex = surface.AssignBuildHex(hex.id);//build
    //         }
    //         scheduler.AssignJob(new DigJob(hex, digHex =>
    //         {
    //             if (digHex == surface.allHex[digHex.id])
    //             {
    //                 surface.Dig(digHex.id);
    //             }
    //         }));
    //     }
    // }



}

