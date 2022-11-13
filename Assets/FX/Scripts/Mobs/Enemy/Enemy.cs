using System;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour, Mob
{
    public string id { get; set; }
    public float ATTACK_STRENGTH = 10f;
    public EnemyAnimator animator { get; set; }
    public Action Animation { get; set; }
    public Vector3 position { get => transform.position; }
    public Path path { get; set; }
    public State currentState { get; set; }
    public Pathfinder pathfinder { get; set; }
    public SurfaceOperations surfaceOperations { get; set; }
    public Job job { get; set; }
    public Action Kill { get; set; }
    public FloorHexagon currentHex { get; set; }
    public bool HasPath { get => path != null && currentPathEdge != null; }
    public bool HasJob { get => job != null; }
    private float lerpDuration;
    private float t = 0f;
    public Edge currentPathEdge;
    public float hp { get; set; }
    public List<Mob> allMobs = new List<Mob>();
    void Awake()
    {
        animator = GetComponent<EnemyAnimator>();
        animator.enemy = this;
        hp = 250f;
        SetState(new PatrolState(this));
    }

    public void SetState(State state)
    {
        if (currentState != null)
            currentState.OnStateExit();

        currentState = state;
        gameObject.name = "Enemy - " + state.GetType().Name;

        if (currentState != null)
            currentState.OnStateEnter();
    }

    public void SetPath(Path path)
    {
        this.path = path;
        if (path != null)
        {
            if (path.HasWaypoints) SetcurrentPathEdge();
        }
        else
        {
            var pathNew = pathfinder.FindPath(position, ((EnemyJob)job).target.position, false);
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
        var target = ((EnemyJob)job).target;
        target.hp -= ATTACK_STRENGTH * Time.deltaTime;
        if (target.hp <= 0)
        {
            CancelJob();
            target.Kill();
            SetState(new PatrolState(this));
        }
    }

    public void RemovePath()
    {
        path = null;
    }

    public void SetRandomWalk()
    {
        SetPath(pathfinder.RandomWalk(position, 5));
    }

    public void ExpandRandomWalk()
    {
        var newRandomWalk = pathfinder.RandomWalk(path.wayPoints[path.wayPoints.Count - 1].to.position, 5);
        path.length += newRandomWalk.length;
        path.wayPoints.AddRange(newRandomWalk.wayPoints);
    }

    public void Move(int speed)
    {
        DrawDebugPath();
        if (t < lerpDuration)
        {
            var a = (float)Mathf.Min(t / lerpDuration, 1f);
            transform.position = Vector3.Lerp(currentPathEdge.from.position, currentPathEdge.to.position, a);
            t += Time.deltaTime * speed;
        }
        else if (path.HasWaypoints)
        {
            t -= lerpDuration;
            SetcurrentPathEdge();
        }
        else
        {
            currentPathEdge = null;
        }
    }

    private void SetcurrentPathEdge()
    {
        currentPathEdge = path.wayPoints[0];
        currentHex = currentPathEdge.floorHexagon;
        path.wayPoints.RemoveAt(0);
        lerpDuration = Vector3.Distance(currentPathEdge.from.position, currentPathEdge.to.position);
    }

    public void CancelJob()
    {
        currentState.CancelJob();
    }

    public void Rerouting()
    {
        SetPath(pathfinder.FindPath(transform.position, ((EnemyJob)job).target.position, true));
    }

    void Update()
    {
        currentState.Tick();
    }

    public void SetRunAnimation()
    {
        Animation = animator.Run;
    }
    public void SetRunAtackAnimation()
    {
        Animation = animator.RunFight;
    }
    public void SetIdleAnimation()
    {
        Animation = animator.Idle;
    }
    public void SetIdleFightAnimation()
    {
        Animation = animator.IdleFight;
    }

    public void SearchTarget()
    {

    }

    void DrawDebugPath()
    {
        if (path != null && path.HasWaypoints)
        {
            var pathDebug = new List<Edge>() { new Edge() { from = new Vertex("", transform.position, false), to = currentPathEdge.to } };

            for (int j = 1; j < path.wayPoints.Count; j++)
            {
                pathDebug.Add(path.wayPoints[j]);
            }
            for (int i = 0; i < pathDebug.Count; i++)
            {
                Debug.DrawLine(pathDebug[i].from.position, pathDebug[i].to.position, Color.blue);
            }
        }
    }

}

