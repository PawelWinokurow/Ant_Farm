using System.Collections.Generic;
using UnityEngine;


public class Digger : MonoBehaviour, Mob
{
    public List<Vector3> WayPoints { set; get; }
    float t = 0f;
    int i = 0;

    public Vector3 CurrentPosition { set; get; }
    private Job job;

    void Start()
    {
        CurrentPosition = transform.position;
        WayPoints = new List<Vector3>();
    }

    public void SetPath(Path path)
    {
        WayPoints = path.WayPoints;
    }

    public bool HasJob()
    {
        return job != null;
    }

    public void SetJob(Job job)
    {
        this.job = job;
    }


    void Update()
    {
        for (int i = 0; i < WayPoints.Count - 1; i++)
        {
            Debug.DrawLine(WayPoints[i], WayPoints[i + 1], Color.blue);
        }


        if (WayPoints.Count != 0 && i < WayPoints.Count - 1)
        {
            if (t < 1f)
            {
                t += Time.deltaTime * 20f;
                transform.position = Vector3.Lerp(WayPoints[i], WayPoints[i + 1], t);
                CurrentPosition = transform.position;
            }
            else
            {
                i++;
                t = 0;
            }
        }
        else
        {
            WayPoints.Clear();
            i = 0;
            t = 0;
        }
    }

}

