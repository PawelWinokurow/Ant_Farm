using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures.ViliWonka.KDTree;
using UnityEngine;
using FighterNamespace;

public class Fighter : MonoBehaviour, Targetable
{
    public string id { get; set; }
    public ACTOR_TYPE type { get; set; }
    public IMobAnimator animator { get; set; }
    public Health health { get; set; }
    public Vector3 position { get => transform.position; }
    public Path path { get; set; }
    public State currentState { get; set; }
    public Pathfinder pathfinder { get; set; }
    protected SurfaceOperations surfaceOperations { get; set; }
    protected Surface surface { get; set; }
    protected Store store;
    public Target target { get; set; }
    public FloorHexagon currentHex { get; set; }
    public Action Kill { get; set; }
    public bool HasPath { get => path != null && currentPathEdge != null; }
    protected float lerpDuration;
    protected float t = 0f;
    public Edge currentPathEdge;
    public int accessMask { get; set; }
    public GameSettings gameSettings;
    public FighterSettings mobSettings;
    public int movementSpeed { get; set; }
    public Transform body;
    public Transform angl;
    protected Quaternion smoothRot;
    public DigJob digJob;
    public bool isDead { get => health != null && health.isDead; }
    public GameObject obj { get => gameObject; }

    protected void InitSingletons()
    {
        surface = Surface.Instance;
        surfaceOperations = SurfaceOperations.Instance;
        store = Store.Instance;
    }
    public void SetInitialState()
    {
        var target = SearchTarget();
        if (target != null)
        {
            SetTarget(target);
            SetState(new FollowingState(this));
        }
        else
        {
            SetState(new PatrolState(this));
        }
    }

    public void SetState(State state)
    {
        if (currentState != null)
            currentState.OnStateExit();

        currentState = state;
        gameObject.name = type.ToString() + " - " + state.GetType().Name;

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
    }

    public void RemovePath()
    {
        path = null;
    }

    public virtual void SetRandomWalk()
    {
        SetPath(pathfinder.RandomWalk(position, 5, accessMask));
    }

    public void ExpandRandomWalk()
    {
        var newRandomWalk = pathfinder.RandomWalk(path.wayPoints[path.wayPoints.Count - 1].to.position, 100, accessMask);
        path.length += newRandomWalk.length;
        path.wayPoints.AddRange(newRandomWalk.wayPoints);
    }

    public void Move()
    {
        DrawDebugPath();
        if (t < lerpDuration)
        {
            var a = (float)Mathf.Min(t / lerpDuration, 1f);
            transform.position = Vector3.Lerp(currentPathEdge.from.position, currentPathEdge.to.position, a);
            t += Time.deltaTime * movementSpeed;
        }
        else if (path.HasWaypoints)
        {
            t -= lerpDuration;
            SetcurrentPathEdge();
        }
        else
        {
            ResetCurrentPathEdge();
        }
    }

    public void ResetCurrentPathEdge()
    {
        currentPathEdge = null;
    }

    protected virtual void SetcurrentPathEdge()
    {
        currentPathEdge = path.wayPoints[0];
        path.wayPoints.RemoveAt(0);
        var currentHexNew = currentPathEdge.floorHexagon;
        currentHex = currentHexNew;
        lerpDuration = currentPathEdge.edgeWeight * currentPathEdge.edgeMultiplier;
    }

    public void CancelJob()
    {
        currentState.CancelJob();
    }

    public void Rerouting()
    {
        target.hex = target.mob.currentHex;
        if (currentPathEdge != null)
        {
            var pathNew = pathfinder.FindPath(currentPathEdge.to.position, target.mob.currentHex.position, accessMask, SearchType.NEAREST_VERTEX);
            if (pathNew != null)
            {
                path = pathNew;
            }
        }
        else
        {
            SetPath(pathfinder.FindPath(position, target.mob.currentHex.position, accessMask, SearchType.NEAREST_VERTEX));
        }
    }

    void Update()
    {
        currentState.Tick();
        Rotation();
    }

    protected virtual void Rotation()
    {
        Vector3 forward = Vector3.zero;
        if (currentState.type == STATE.ATTACK && target?.mob != null)
        {
            forward = target.mob.position - transform.position;
        }
        else if (currentPathEdge != null)
        {
            forward = currentPathEdge.to.position - transform.position;
        }
        Quaternion rot = Quaternion.LookRotation(angl.up, forward);
        smoothRot = Quaternion.Slerp(smoothRot, rot, Time.deltaTime * 10f);
        body.rotation = smoothRot;
        body.Rotate(new Vector3(-90f, 0f, 180f), Space.Self);
    }

    public void SetRunAnimation()
    {
        animator.Run();
    }
    public void SetIdleAnimation()
    {
        animator.Idle();
    }
    public void SetIdleFightAnimation()
    {
        animator.IdleFight();
    }


    public Target SearchTarget(List<Targetable> mobs, int accessMask, EdgeType edgeType, Dictionary<ACTOR_TYPE, int> priorities)
    {
        var notDeadMobGroups = mobs.Where(mob => mob.currentState?.type != STATE.DEAD).GroupBy(mob => priorities[mob.type]).OrderBy(group => group.Key);
        foreach (var notDeadMobGroup in notDeadMobGroups)
        {
            KDTree mobPositionsTree = new KDTree(notDeadMobGroup.Select(mob => mob.position).ToArray());
            KDQuery query = new KDQuery();
            List<int> queryResults = new List<int>();
            List<float> queryDistances = new List<float>();
            query.KNearest(mobPositionsTree, position, notDeadMobGroup.ToList().Count, queryResults, queryDistances);
            queryResults.Reverse();
            queryDistances.Reverse();
            if (queryResults.Count == 0 || queryDistances[0] > 10000f) { return null; }
            for (int i = 0; i < queryResults.Count; i++)
            {
                var targetMob = notDeadMobGroup.ToList()[queryResults[i]];
                if (targetMob.currentHex == null)
                {
                    continue;
                }
                var path = pathfinder.FindPath(position, targetMob.currentHex.position, accessMask, SearchType.NEAREST_VERTEX, edgeType);
                if (path != null)
                {
                    var target = new Target($"{id}_{targetMob.id}", targetMob);
                    target.path = path;
                    target.mob = targetMob;
                    return target;
                }
            }
        }
        return null;
    }

    public void SetTarget(Target target)
    {
        this.target = target;
        if (target != null)
        {
            SetPath(target.path);
        }
    }

    public virtual void Hit(int damage)
    {
        health.Hit(damage);
        if (isDead)
        {
            Kill();
        }
    }

    public virtual bool IsTargetInSight()
    {
        return false;
    }

    public virtual Target SearchTarget()
    {
        return null;
    }

    public virtual void Attack()
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


