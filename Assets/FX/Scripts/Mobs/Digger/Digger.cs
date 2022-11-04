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
    private int i = 0;
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
        if (path != null)
        {
            Path = path;
            ResetMovement();
        }
    }

    void DrawDebugPath()
    {
        if (i < Path.WayPoints.Count)
        {
            var path = new List<Edge>() { new Edge() { From = new Vertex("", CurrentPosition), To = Path.WayPoints[i].To } };

            for (int j = i + 1; j < Path.WayPoints.Count; j++)
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
            if (i < Path.WayPoints.Count)
            {
                if (t < lerpDuration)
                {
                    var a = (float)Mathf.Min(t / lerpDuration, 1f);
                    transform.position = Vector3.Lerp(Path.WayPoints[i].From.Position, Path.WayPoints[i].To.Position, a);
                    CurrentPosition = transform.position;
                    t += Time.deltaTime * speed;
                }
                else
                {
                    i++;
                    t -= lerpDuration;
                    if (i < Path.WayPoints.Count && !Path.WayPoints[i].IsWalkable)
                    {
                        SetPath(Pathfinder.FindPath(CurrentPosition, Path.WayPoints[Path.WayPoints.Count - 1].To.Position));
                    }
                }
            }
            else
            {
                RemovePath();
            }
        }
    }

    public void ResetMovement()
    {
        i = 0;
        if (Path != null)
            lerpDuration = Vector3.Distance(Path.WayPoints[0].From.Position, Path.WayPoints[0].To.Position);
    }
    public void RemovePath()
    {
        Path = null;
    }

    void Update()
    {
        CurrentState.Tick();
    }

}

