using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Agent : MonoBehaviour
{
    public float Speed = 1.0f;
    float startTime = Time.time;

    List<Vector3> WayPoints = new List<Vector3>();
    Vector3 currentFrom;
    Vector3 currentTo;
    float currentStartTime;


    public void SetPath(Path path)
    {
        WayPoints = path.WayPoints;
        currentTo = WayPoints[0];
        WayPoints.RemoveAt(0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (WayPoints.Count != 0)
        {

            if (Vector3.Distance(transform.position, currentTo) < 0.05f)
            {
                currentFrom = currentTo;
                currentTo = WayPoints[0];
                currentStartTime = Time.time;
                WayPoints.RemoveAt(0);
            }
            Step(currentFrom, currentTo, currentStartTime);
        }
    }

    void Step(Vector3 from, Vector3 to, float startTime)
    {
        float distanceBetween = Vector3.Distance(from, to);
        float distanceTime = (Time.time - startTime) * this.Speed;
        float relativeTime = distanceTime / distanceBetween;
        this.transform.position = Vector3.Lerp(transform.position, to, relativeTime);
    }


}

