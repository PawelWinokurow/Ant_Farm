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
    private JobScheduler jobScheduler;

    //TODO remove from controller
    public Agent agent;
    public GameObject cube;
    public GameObject cubeGoal;
    private void Start()
    {
        cam = Camera.main;
        surface = gm.Surface;
        jobScheduler = gm.JobScheduler;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            pos = cam.ScreenToWorldPoint(Input.mousePosition);

            // marker.transform.position = new Vector3(x* surf.w + (z % 2) * 0.5f* surf.w, 0, z* surf.h);
            marker.transform.position = surface.allHex[surface.PositionToId(pos)].transform.position;
            ProcessHexagon(surface.allHex[surface.PositionToId(pos)]);
            // var path = gm.Surface.PathGraph.FindPath(agent.CurrentPosition, marker.transform.position);
            // if (path == null) Debug.Log("Path is null");
            // else
            // {
            //     GameObject.Destroy(cubeGoal);
            //     agent.SetPath(path);
            //     cubeGoal = Instantiate(cube, path.WayPoints[path.WayPoints.Count - 1], Quaternion.identity);
            // }
        }
    }

    public void ProcessHexagon(Hexagon hex)
    {
        if (hex != null && !hex.IsInSpawnArea() && !jobScheduler.IsJobAlreadyCreated(hex.id))
        {

            if (hex.IsDigabble())
            {
                hex.AssignDig();
                jobScheduler.AssignJob(new DigJob(hex));
            }


        }
    }



}

