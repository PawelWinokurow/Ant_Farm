using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controller : MonoBehaviour
{

    public Transform marker;
    private Surface surf;

   

    private void Start()
    {
        surf = Surface.instance;
        Screen.orientation = ScreenOrientation.Portrait;
    }


    public void Tap(Vector3 pos)
    {
        pos = Camera.main.ScreenToWorldPoint(pos);
        int id = surf.PositionToId(pos);
        marker.transform.position = surf.allHex[id].transform.position;
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

