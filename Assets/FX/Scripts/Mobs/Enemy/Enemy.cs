using System;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour, Mob
{
    public string Id { get; set; }
    public float AttackStrength = 10f;
    public EnemyAnimator EnemyAnimator { get; set; }
    public Action Animation { get; set; }
    public Vector3 Position { get => transform.position; }
    public Path Path { get; set; }
    public State CurrentState { get; set; }
    public Pathfinder Pathfinder { get; set; }
    public SurfaceOperations SurfaceOperations { get; set; }
    public Job Job { get; set; }
    public Action DestroyMob { get; set; }

    public bool HasPath { get => Path != null && CurrentPathEdge != null; }
    public bool HasJob { get => Job != null; }
    private float lerpDuration;
    private float t = 0f;
    public Edge CurrentPathEdge;
    public float Hp { get; set; }

    void Awake()
    {
        EnemyAnimator = GetComponent<EnemyAnimator>();
        EnemyAnimator.enemy = this;
        Hp = 250f;
        SetState(new PatrolState(this));
    }

    public void SetState(State state)
    {
        if (CurrentState != null)
            CurrentState.OnStateExit();

        CurrentState = state;
        gameObject.name = "Enemy - " + state.GetType().Name;

        if (CurrentState != null)
            CurrentState.OnStateEnter();
    }

    public void SetPath(Path path)
    {
        Path = path;
        if (Path != null)
        {
            if (Path.HasWaypoints) SetCurrentPathEdge();
        }
        else
        {
            var pathNew = Pathfinder.FindPath(Position, ((EnemyJob)Job).Target.Position, false);
            if (pathNew != null)
            {
                SetState(new AttackState(this));
            }
            // else
            // {
            //     SetState(new PatrolState(this));
            // }
        }
    }

    public void Hit()
    {
        var target = ((EnemyJob)Job).Target;
        target.Hp -= AttackStrength * Time.deltaTime;
        if (target.Hp <= 0)
        {
            CancelJob();
            target.DestroyMob();
            SetState(new PatrolState(this));
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
        if (t < lerpDuration)
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
        lerpDuration = Vector3.Distance(CurrentPathEdge.From.Position, CurrentPathEdge.To.Position);
    }

    public void CancelJob()
    {
        CurrentState.CancelJob();
    }

    public void Rerouting()
    {
        SetPath(Pathfinder.FindPath(transform.position, ((EnemyJob)Job).Target.Position, true));
    }

    void Update()
    {
        CurrentState.Tick();
    }

    public void SetRunAnimation()
    {
        Animation = EnemyAnimator.Run;
    }
    public void SetRunAtackAnimation()
    {
        Animation = EnemyAnimator.RunFight;
    }
    public void SetIdleAnimation()
    {
        Animation = EnemyAnimator.Idle;
    }
    public void SetIdleFightAnimation()
    {
        Animation = EnemyAnimator.IdleFight;
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

