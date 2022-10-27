using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Agent : MonoBehaviour
{
    List<Vector3> WayPoints = new List<Vector3>();
    float t = 0f;
    int i = 0;
    public Vector3 CurrentPosition;
    public void SetPath(Path path)
    {
        WayPoints = path.WayPoints;
    }
    void Start()
    {
        CurrentPosition = transform.position;
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

