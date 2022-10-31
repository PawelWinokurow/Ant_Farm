using System.Collections.Generic;
using UnityEngine;


public class Digger : MonoBehaviour, Mob
{
    public List<Vector3> WayPoints { set; get; }
    public Vector3 CurrentPosition { set; get; }
    private Job job;
    public Job Job { get; set; }
    private Path path;
    public Path Path { get; set; }
    public State CurrentState { get; set; }


    private float t = 0f;
    private int i = 0;
    void Start()
    {
        CurrentPosition = transform.position;
        SetState(new IdleState(this));
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

    public bool HasJob
    {
        get => Job != null;
    }


    void DrawDebugPath()
    {
        var path = new List<Vector3>() { CurrentPosition, };

        for (int j = i + 1; j < Path.WayPoints.Count - 1; j++)
        {
            path.Add(Path.WayPoints[j]);
        }
        for (int i = 0; i < path.Count - 1; i++)
        {
            Debug.DrawLine(path[i], path[i + 1], Color.blue);
        }
    }

    public void Move()
    {
        DrawDebugPath();

        if (t < 1f)
        {
            t += Time.deltaTime * 20f;
            transform.position = Vector3.Lerp(Path.WayPoints[i], Path.WayPoints[i + 1], t);
            CurrentPosition = transform.position;
        }
        else
        {
            i++;
            t = 0;
        }
    }

    public void ResetMovement()
    {
        i = 0;
        t = 0;
    }
    public void ResetWaypoints()
    {
        Path = null;
    }

    void Update()
    {
        CurrentState.Tick();
    }

    public bool IsGoalReached
    {
        get => !(Path.WayPoints.Count != 0 && i < Path.WayPoints.Count - 1);
    }
    public bool HasAsignment
    {
        get => HasJob && Job.Assignment != null;
    }

    public void ExecuteAssignment()
    {
        if (Job.Assignment != null)
        {
            Job.Assignment.Execute(this);
            this.Job = null;
        }
        SetState(new IdleState(this));

    }

}

