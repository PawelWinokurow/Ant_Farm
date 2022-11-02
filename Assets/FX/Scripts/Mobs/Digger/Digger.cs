using System.Collections.Generic;
using UnityEngine;


public class Digger : MonoBehaviour, Mob
{
    public List<Vector3> WayPoints { set; get; }
    public Vector3 CurrentPosition { set; get; }
    private Job job;
    public Job Job { get; set; }
    public Path Path { get; set; }
    public State CurrentState { get; set; }
    public Vector3 InitialPosition { get; set; }

    public bool HasAsignment
    {
        get => HasJob && Job.Assignment != null;
    }
    public bool HasPath { get => Path != null; }

    public bool HasJob
    {
        get => Job != null;
    }

    private float lerpDuration;
    private float t = 0f;
    private int i = 0;
    void Awake()
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
        var path = new List<Vector3>() { CurrentPosition };

        for (int j = i + 1; j < Path.WayPoints.Count - 1; j++)
        {
            path.Add(Path.WayPoints[j]);
        }
        for (int i = 0; i < path.Count - 1; i++)
        {
            Debug.DrawLine(path[i], path[i + 1], Color.blue);
        }
    }

    public void Move(int speed)
    {
        if (Path != null)
        {

            // DrawDebugPath();
            if (i < Path.WayPoints.Count - 2)
            {
                if (t < lerpDuration)
                {
                    var a = (float)Mathf.Min(t / lerpDuration, 1f);
                    transform.position = Vector3.Lerp(Path.WayPoints[i], Path.WayPoints[i + 1], a);
                    CurrentPosition = transform.position;
                    t += Time.deltaTime * speed;
                }
                else
                {
                    i++;
                    t -= lerpDuration;
                    if (i < Path.WayPoints.Count - 2) lerpDuration = Vector3.Distance(Path.WayPoints[i], Path.WayPoints[i + 1]);
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
        lerpDuration = Vector3.Distance(Path.WayPoints[0], Path.WayPoints[1]);
    }
    public void RemovePath()
    {
        Path = null;
    }

    void Update()
    {
        CurrentState.Tick();
    }

    public void ExecuteAssignment()
    {
        if (Job.Assignment != null)
        {
            Job.Assignment.Execute(this);
            this.Job = null;
        }
    }

}

