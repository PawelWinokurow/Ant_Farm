using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataStructures.ViliWonka.KDTree;
using UnityEngine;


public class Enemy : MonoBehaviour, Mob
{
    public string id { get; set; }
    public static int ATTACK_STRENGTH = 10;
    public static int ACCESS_MASK = 2;
    public EnemyAnimator animator { get; set; }
    public Action Animation { get; set; }
    public Vector3 position { get => transform.position; }
    public Path path { get; set; }
    public State currentState { get; set; }
    public Pathfinder pathfinder { get; set; }
    public SurfaceOperations surfaceOperations { get; set; }
    public EnemyTarget target { get; set; }
    public Action Kill { get; set; }
    public FloorHexagon currentHex { get; set; }
    public bool HasPath { get => path != null && currentPathEdge != null; }
    private float lerpDuration;
    private float t = 0f;
    public Edge currentPathEdge;
    public float hp { get; set; }
    public Store store;
    public List<Mob> allMobs = new List<Mob>();
    public Dig_FX DigFX;

    void Start()
    {
        animator = GetComponent<EnemyAnimator>();
        animator.enemy = this;
        hp = 250f;
        SetState(new PatrolState(this));
    }

    IEnumerator DistributeJobsCoroutine()
    {
        while (true)
        {
            SearchTarget();
            yield return new WaitForSeconds(0.3f);
        }
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
    }

    public void Hit(int damage)
    {

    }

    public void Attack()
    {
        target.mob.Hit(Enemy.ATTACK_STRENGTH);
        if (target.mob.hp <= 0)
        {
            target.mob.Kill();
            CancelJob();
            SetState(new PatrolState(this));
        }
    }



    public void RemovePath()
    {
        path = null;
    }

    public void SetRandomWalk()
    {
        SetPath(pathfinder.RandomWalk(position, 5, Enemy.ACCESS_MASK));
    }

    public void ExpandRandomWalk()
    {
        var newRandomWalk = pathfinder.RandomWalk(path.wayPoints[path.wayPoints.Count - 1].to.position, 5, Enemy.ACCESS_MASK);
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
        path.wayPoints.RemoveAt(0);
        var currentHexNew = currentPathEdge.floorHexagon;
        if ((currentHex == null || currentHexNew.id != currentHex.id))
        {
            if (currentHexNew.type == HEX_TYPE.SOIL)
            {
                var digFX = Instantiate(DigFX, position, Quaternion.identity, currentHexNew.transform);
                digFX.StartFx(currentHexNew);
            }
            if (currentHex != null && currentHex.GetComponentInChildren<Dig_FX>())
            {
                var digFxOld = currentHex.GetComponentInChildren<Dig_FX>();
                digFxOld.StopFx();
                Destroy(digFxOld);
            }
        }
        currentHex = currentHexNew;
        lerpDuration = Vector3.Distance(currentPathEdge.from.position, currentPathEdge.to.position) * currentPathEdge.edgeWeight / currentPathEdge.edgeWeightBase;

    }

    public void CancelJob()
    {
        currentState.CancelJob();
    }

    public void Rerouting()
    {
        target.hexId = target.mob.currentHex.id;
        if (currentPathEdge != null)
        {
            var pathNew = pathfinder.FindPath(currentPathEdge.to.position, target.mob.currentHex.position, Enemy.ACCESS_MASK, true);
            if (pathNew != null)
            {
                path = pathNew;
            }
        }
        else
        {
            SetPath(pathfinder.FindPath(position, target.mob.currentHex.position, Enemy.ACCESS_MASK, true));
        }
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

    public EnemyTarget SearchTarget()
    {
        var notDeadMobs = new List<Mob>(store.allMobs.Where(mob => mob.currentState.type != STATE.DEAD));
        KDTree mobPositionsTree = new KDTree(notDeadMobs.Select(mob => mob.position).ToArray());
        KDQuery query = new KDQuery();
        List<int> queryResults = new List<int>();
        queryResults.Reverse();
        query.Radius(mobPositionsTree, position, 200f, queryResults);
        if (queryResults.Count == 0) { return null; }
        for (int i = 0; i < queryResults.Count; i++)
        {
            var targetMob = notDeadMobs[queryResults[i]];
            var path = pathfinder.FindPath(position, targetMob.position, Enemy.ACCESS_MASK, true);
            if (path != null)
            {
                var target = new EnemyTarget($"{id}_{targetMob.id}", this, targetMob);
                target.path = path;
                target.mob = targetMob;
                return target;
            }
        }
        return null;
    }

    public void SetTarget(EnemyTarget target)
    {
        this.target = target;
        if (target != null)
        {
            SetPath(target.path);
        }
    }

    public bool IsTargetInNeighbourhood()
    {
        return currentHex.vertex.neighbours.Select(vertex => vertex.id).Append(currentHex.id).Contains(target.mob.currentHex.id);
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

