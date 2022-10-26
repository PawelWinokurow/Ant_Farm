using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Agent : MonoBehaviour
{
    List<Vector3> WayPoints = new List<Vector3>();
    float t = 0f;
    int i = 0;

    public void SetPath(Path path)
    {
        WayPoints = path.WayPoints;
    }

    void Update()
    {
        if (WayPoints.Count != 0 && i < WayPoints.Count - 1)
        {
            if (t < 1f)
            {
                t += Time.deltaTime / 0.2f;
                transform.position = Vector3.Lerp(WayPoints[i], WayPoints[i + 1], t);
            }
            else
            {
                i++;
                t = 0;
            }
        }
    }

}

