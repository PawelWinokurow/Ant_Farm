using System.Collections.Generic;
using UnityEngine;


public class Worker : MonoBehaviour, Mob
{
    public float ConstructionSpeed = 2f;
    public float CarryWeight = 100f;
    public AntAnimator AntAnimator { get; set; }
    public Vector3 CurrentPosition { get => transform.position; }
    public Job Job { get; set; }
    public Path Path { get; set; }
    public State CurrentState { get; set; }
    public Pathfinder Pathfinder { get; set; }
    public bool HasPath { get => currentPathEdge != null; }
    public bool HasJob { get => Job != null; }
    private float lerpDuration;
    private float t = 0f;
    private Edge currentPathEdge;
    void Awake()
    {
        AntAnimator = GetComponent<AntAnimator>();
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
        if (Path != null && Path.HasWaypoints)
        {
            SetCurrentPathEdge();
        }
        else
        {
            if (HasJob)
            {
                Job.CancelJob();
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
        SetPath(Pathfinder.RandomWalk(CurrentPosition, 10));
    }

    public void Move(int speed)
    {
        DrawDebugPath();
        if (!currentPathEdge.IsWalkable)
        {
            Rerouting();
        }
        else if (t < lerpDuration)
        {
            var a = (float)Mathf.Min(t / lerpDuration, 1f);
            transform.position = Vector3.Lerp(currentPathEdge.From.Position, currentPathEdge.To.Position, a);
            t += Time.deltaTime * speed;
        }
        else if (Path.HasWaypoints)
        {
            t -= lerpDuration;
            SetCurrentPathEdge();
        }
        else
        {
            currentPathEdge = null;
        }
    }

    private void SetCurrentPathEdge()
    {

        currentPathEdge = Path.WayPoints[0];
        Path.WayPoints.RemoveAt(0);
        lerpDuration = Distance.Manhattan(currentPathEdge.From.Position, currentPathEdge.To.Position);

    }

    void Rerouting()
    {
        var to = Path.HasWaypoints ? Path.WayPoints[Path.WayPoints.Count - 1].To.Position : currentPathEdge.To.Position;
        var path = Pathfinder.FindPath(transform.position, to, HasJob ? true : false);
        SetPath(path);
    }

    void Update()
    {
        CurrentState.Tick();
    }



    void DrawDebugPath()
    {
        if (Path != null && Path.HasWaypoints)
        {
            var path = new List<Edge>() { new Edge() { From = new Vertex("", transform.position, false), To = currentPathEdge.To } };

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

