using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures.ViliWonka.KDTree;
using UnityEngine;

namespace GunnerNamespace
{
    public class Gunner : MonoBehaviour, Mob
    {
        public string id { get; set; }
        public MobType type { get; set; }
        public MobAnimator animator { get; set; }
        public Health health { get; set; }
        public Action Animation { get; set; }
        public Vector3 position { get => transform.position; }
        public Path path { get; set; }
        public State currentState { get; set; }
        public Pathfinder pathfinder { get; set; }
        public SurfaceOperations surfaceOperations { get; set; }
        public GunnerTarget target { get; set; }
        public FloorHexagon currentHex { get; set; }
        public Action Kill { get; set; }
        public bool HasPath { get => path != null && currentPathEdge != null; }
        private float lerpDuration;
        private float t = 0f;
        public Edge currentPathEdge;
        public Store store;
        public int accessMask { get; set; }
        private GameSettings gameSettings;
        private GunnerSettings gunnerSettings;
        public int movementSpeed { get; set; }
        private Cannon cannon { get; set; }
        public MeshFilter mf;
        public Transform angl;
        private Quaternion smoothRot;

        void Start()
        {
            gameSettings = Settings.Instance.gameSettings;
            gunnerSettings = Settings.Instance.gunnerSettings;
            animator = GetComponent<MobAnimator>();
            animator.Attack = () => Attack();
            cannon = GetComponent<Cannon>();
            type = MobType.GUNNER;
            accessMask = gameSettings.ACCESS_MASK_FLOOR + gameSettings.ACCESS_MASK_BASE;
            health = GetComponent<Health>();
            health.InitHp(gunnerSettings.HP);
            SetState(new PatrolState(this));
        }

        public void SetState(State state)
        {
            if (currentState != null)
                currentState.OnStateExit();

            currentState = state;
            gameObject.name = "Gunner - " + state.GetType().Name;

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
            if (target.mob.Hit(gunnerSettings.ATTACK_STRENGTH) <= 0)
            {
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
            Rotation();
        }

        private void Rotation()
        {
            Vector3 forward = Vector3.zero;
            mf.mesh = animator.current.sequence[animator.f];
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
            mf.transform.rotation = smoothRot;
            mf.transform.Rotate(new Vector3(-90f, 0f, 180f), Space.Self);
        }

        public void SetRunAnimation()
        {
            Animation = animator.Run;
        }
        public void SetIdleAnimation()
        {
            Animation = animator.Idle;
        }
        public void SetIdleFightAnimation()
        {
            Animation = animator.IdleFight;
        }


        public GunnerTarget SearchTarget()
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
                    var target = new GunnerTarget($"{id}_{targetMob.id}", this, targetMob);
                    target.path = path;
                    target.mob = targetMob;
                    return target;
                }
            }
            return null;
        }

        public void SetTarget(GunnerTarget target)
        {
            this.target = target;
            if (target != null)
            {
                SetPath(target.path);
            }
        }

        public bool IsTargetInSight()
        {
            var targetPosition = target.mob.position;
            var vec = targetPosition - position;
            var vecLength = Vector3.Magnitude(vec);
            if (vecLength < 7f)
            {
                var vecNorm = Vector3.Normalize(vec);
                var hexagonsOnTrajectory = new List<FloorHexagon>();
                for (float i = 0; i < Mathf.Floor(vecLength); i += 1f)
                {
                    hexagonsOnTrajectory.Add(surfaceOperations.surface.PositionToHex(position + i * vecNorm));
                }
                hexagonsOnTrajectory.Add(surfaceOperations.surface.PositionToHex(targetPosition));
                return hexagonsOnTrajectory.All(hex => hex.type != HexType.SOIL && hex.type != HexType.STONE);
            }

            return false;
        }

        public float Hit(int damage)
        {
            if (health.Hit(damage) <= 0)
            {
                Kill();
            }
            return health.hp;
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

