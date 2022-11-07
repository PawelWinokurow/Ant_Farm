using System;
using System.Collections.Generic;
using UnityEngine;


public class Worker : MonoBehaviour, Mob
{
    public string Id { get; set; }
    public float ConstructionSpeed = 2f;
    public int LoadingSpeed = 50;
    public int MaxCarryingWeight = 100;
    public float CarryingWeight = 0;
    public AntAnimator AntAnimator { get; set; }
    public Action Animation { get; set; }
    public Vector3 Position { get => transform.position; }
    public Job Job { get; set; }
    public Path Path { get; set; }
    public State CurrentState { get; set; }
    public Pathfinder Pathfinder { get; set; }
    public SurfaceOperations SurfaceOperations { get; set; }
    public bool HasPath { get => Path != null && CurrentPathEdge != null; }
    public bool HasJob { get => Job != null; }
    private float lerpDuration;
    private float t = 0f;
    public Edge CurrentPathEdge;
    void Awake()
    {
        AntAnimator = GetComponent<AntAnimator>();
        AntAnimator.worker = this;
        SetState(new IdleState(this));
    }

    public void SetState(State state)
    {
        if (CurrentState != null)
            CurrentState.OnStateExit();

        CurrentState = state;
        gameObject.name = "Worker - " + state.GetType().Name;

        if (CurrentState != null)
            CurrentState.OnStateEnter();
    }

    public void SetPath(Path path)
    {
        Path = path;
        if (Path != null)
        {
            if (Path.HasWaypoints) SetCurrentPathEdge();
            else CurrentState.OnStateEnter();
        }
        else
        {
            if (HasJob)
            {
                Job.Cancel();
            }
            else
            {
                SetState(new IdleState(this));
            }
        }
    }

    public void RemovePath()
    {
        Path = null;
    }

    public void SetRandomWalk()
    {
        SetPath(Pathfinder.RandomWalk(Position, 5));
    }

    public void ExpandRandomWalk()
    {
        var newRandomWalk = Pathfinder.RandomWalk(Path.WayPoints[Path.WayPoints.Count - 1].To.Position, 5);
        Path.Length += newRandomWalk.Length;
        Path.WayPoints.AddRange(newRandomWalk.WayPoints);
    }

    public void Move(int speed)
    {
        DrawDebugPath();
        if (!CurrentPathEdge.IsWalkable)
        {
            Rerouting();
        }
        else if (t < lerpDuration)
        {
            var a = (float)Mathf.Min(t / lerpDuration, 1f);
            transform.position = Vector3.Lerp(CurrentPathEdge.From.Position, CurrentPathEdge.To.Position, a);
            t += Time.deltaTime * speed;
        }
        else if (Path.HasWaypoints)
        {
            t -= lerpDuration;
            SetCurrentPathEdge();
        }
        else
        {
            CurrentPathEdge = null;
        }
    }

    private void SetCurrentPathEdge()
    {

        CurrentPathEdge = Path.WayPoints[0];
        Path.WayPoints.RemoveAt(0);
        lerpDuration = Distance.Manhattan(CurrentPathEdge.From.Position, CurrentPathEdge.To.Position);
    }

    public void CancelJob()
    {
        CurrentState.CancelJob();
    }

    void Rerouting()
    {
        var to = Path.HasWaypoints ? Path.WayPoints[Path.WayPoints.Count - 1].To.Position : CurrentPathEdge.To.Position;
        var path = Pathfinder.FindPath(transform.position, to, HasJob ? true : false);
        SetPath(path);
    }

    void Update()
    {
        CurrentState.Tick();
    }

    public void SetRunAnimation()
    {
        Animation = AntAnimator.Run;
    }
    public void SetRunFoodAnimation()
    {
        Animation = AntAnimator.RunFood;
    }
    public void SetIdleAnimation()
    {
        Animation = AntAnimator.Idle;
    }

    void DrawDebugPath()
    {
        if (Path != null && Path.HasWaypoints)
        {
            var path = new List<Edge>() { new Edge() { From = new Vertex("", transform.position, false), To = CurrentPathEdge.To } };

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

}

