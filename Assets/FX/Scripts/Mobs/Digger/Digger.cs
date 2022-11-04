using System;
using System.Collections.Generic;
using UnityEngine;


public class Digger : MonoBehaviour, Mob
{
    public float ConstructionSpeed = 2f;
    public AntAnimator AntAnimator;
    public Vector3 CurrentPosition { set; get; }
    public Job Job { get; set; }
    public Path Path { get; set; }
    public State CurrentState { get; set; }
    public Vector3 InitialPosition { get; set; }
    public Pathfinder Pathfinder { get; set; }
    public bool HasPath { get => Path != null; }
    public bool HasJob { get => Job != null; }
    private float lerpDuration;
    private float t = 0f;
    private Edge currentPathEdge;
    void Awake()
    {
        CurrentPosition = transform.position;
        AntAnimator = GetComponent<AntAnimator>();
        SetState(new IdleState(this));
    }

    public void SetPathfinder(Pathfinder pathfinder)
    {
        Pathfinder = pathfinder;
    }

    public void SetState(State state)
    {
        if (CurrentState != null)
            CurrentState.OnStateExit();

        CurrentState = state;
        gameObject.name = "Digger - " + state.GetType().Name;

        if (CurrentState != null)
            CurrentState.OnStateEnter();
    }

    public void SetPath(Path path)
    {
        if (path != null && path.HasWaypoints)
        {
            Path = path;
            SetCurrentPathEdge();
            SetLerpDuration();
        }
        else
        {
            if (Job != null)
            {
                Job.CancelJob();
            }
            else
            {
                SetState(new IdleState(this));
            }
        }
    }

    private void SetCurrentPathEdge()
    {
        currentPathEdge = Path.WayPoints[0];
        Path.WayPoints.RemoveAt(0);
    }

    public void SetLerpDuration()
    {
        lerpDuration = Vector3.Distance(currentPathEdge.From.Position, currentPathEdge.To.Position);
    }
    public void RemovePath()
    {
        Path = null;
    }

    void DrawDebugPath()
    {
        if (Path.HasWaypoints)
        {
            var path = new List<Edge>() { new Edge() { From = new Vertex("", CurrentPosition), To = currentPathEdge.To } };

            for (int j = 1; j < Path.WayPoints.Count; j++)
            {
                path.Add(Path.WayPoints[j]);
            }
            for (int i = 0; i < path.Count; i++)
            {
                Debug.DrawLine(path[i].From.Position, path[i].To.Position, Color.blue);
            }
        }
    }

    public void RandomWalk()
    {
        SetPath(Pathfinder.RandomWalk(CurrentPosition, InitialPosition, 5));
    }

    public void Move(int speed)
    {
        if (Path != null)
        {
            DrawDebugPath();

            if (t < lerpDuration)
            {
                var a = (float)Mathf.Min(t / lerpDuration, 1f);
                transform.position = Vector3.Lerp(currentPathEdge.From.Position, currentPathEdge.To.Position, a);
                CurrentPosition = transform.position;
                t += Time.deltaTime * speed;
            }
            else
            {
                t -= lerpDuration;
                if (Path.HasWaypoints)
                {
                    SetCurrentPathEdge();
                    if (!currentPathEdge.IsWalkable)
                    {
                        var to = Path.HasWaypoints ? Path.WayPoints[Path.WayPoints.Count - 1].To.Position : currentPathEdge.To.Position;
                        SetPath(Pathfinder.FindPath(CurrentPosition, to, HasJob ? true : false));
                    }
                }
                else
                {
                    RemovePath();
                }

            }
        }
    }



    void Update()
    {
        CurrentState.Tick();
    }

}

