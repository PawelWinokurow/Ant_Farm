using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures.ViliWonka.KDTree;
using UnityEngine;

namespace SoldierNamespace
{
    public class Soldier : MonoBehaviour, Mob
    {
        public string id { get; set; }
        public MobType type { get; set; }
        public SoldierAnimator animator { get; set; }
        public Health health { get; set; }
        public Action Animation { get; set; }
        public Vector3 position { get => transform.position; }
        public Path path { get; set; }
        public State currentState { get; set; }
        public Pathfinder pathfinder { get; set; }
        public SurfaceOperations surfaceOperations { get; set; }
        public SoldierTarget target { get; set; }
        public Action Kill { get; set; }
        public FloorHexagon currentHex { get; set; }
        public bool HasPath { get => path != null && currentPathEdge != null; }
        private float lerpDuration;
        private float t = 0f;
        public Edge currentPathEdge;
        public Store store;
        public int accessMask { get; set; }
        private GameSettings gameSettings;
        private SoldierSettings soldierSettings;
        public int movementSpeed { get; set; }

        void Start()
        {
            gameSettings = Settings.Instance.gameSettings;
            soldierSettings = Settings.Instance.soldierSettings;
            Kill = () => SetState(new DeadState(this));
            animator = GetComponent<SoldierAnimator>();
            animator.soldier = this;
            type = MobType.SOLDIER;
            accessMask = gameSettings.ACCESS_MASK_FLOOR + gameSettings.ACCESS_MASK_BASE;
            health = GetComponent<Health>();
            health.MAX_HP = soldierSettings.HP;
            SetState(new PatrolState(this));
        }

        public void SetState(State state)
        {
            if (currentState != null)
                currentState.OnStateExit();

            currentState = state;
            gameObject.name = "Soldier - " + state.GetType().Name;

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

        public void Attack()
        {
            target.mob.Hit(soldierSettings.ATTACK_STRENGTH);
            if (target.mob.health.hp <= 0)
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

        private void SetcurrentPathEdge()
        {
            currentPathEdge = path.wayPoints[0];
            path.wayPoints.RemoveAt(0);
            var currentHexNew = currentPathEdge.floorHexagon;
            currentHex = currentHexNew;
            lerpDuration = Vector3.Distance(currentPathEdge.from.position, currentPathEdge.to.position) * currentPathEdge.edgeWeight / currentPathEdge.edgeWeightBase;
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
        }

        public void SetRunAnimation()
        {
            Animation = animator.Run;
        }
        public void SetIdleAnimation()
        {
            Animation = animator.Idle;
        }

        public SoldierTarget SearchTarget()
        {
            var notDeadMobs = new List<Mob>(store.allEnemies.Where(mob => mob.currentState?.type != STATE.DEAD));
            KDTree mobPositionsTree = new KDTree(notDeadMobs.Select(mob => mob.position).ToArray());
            KDQuery query = new KDQuery();
            List<int> queryResults = new List<int>();
            queryResults.Reverse();
            query.Radius(mobPositionsTree, position, 200f, queryResults);
            if (queryResults.Count == 0) { return null; }
            for (int i = 0; i < queryResults.Count; i++)
            {
                var targetMob = notDeadMobs[queryResults[i]];
                if (targetMob.currentHex == null)
                {
                    continue;
                }
                var path = pathfinder.FindPath(position, targetMob.currentHex.position, accessMask, SearchType.NEAREST_VERTEX);
                if (path != null)
                {
                    var target = new SoldierTarget($"{id}_{targetMob.id}", this, targetMob);
                    target.path = path;
                    target.mob = targetMob;
                    return target;
                }
            }
            return null;
        }

        public void SetTarget(SoldierTarget target)
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

        public void Hit(int damage)
        {
            health.Hit(damage);
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
}

