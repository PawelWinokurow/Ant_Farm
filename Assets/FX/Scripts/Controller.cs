using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controller : MonoBehaviour
{
    private Camera cam;
    private Vector3 pos;
    public GameManager gm;
    private Surface surface;
    private JobScheduler jobScheduler;
    private void Start()
    {
        cam = Camera.main;
        surface = gm.Surface;
        jobScheduler = gm.JobScheduler;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pos = cam.ScreenToWorldPoint(Input.mousePosition);

            // marker.transform.position = new Vector3(x* surf.w + (z % 2) * 0.5f* surf.w, 0, z* surf.h);
            // marker.transform.position = surface.allHex[surface.PositionToId(pos)].transform.position;
            ProcessHexagon(surface.allHex[surface.PositionToId(pos)]);

        }
    }

    public void ProcessHexagon(Hexagon hex)
    {
        if (!jobScheduler.IsJobAlreadyCreated(hex.id))
        {
            hex.AssignDig();
            jobScheduler.AssignJob(new DigJob(hex));
        }
    }



}

