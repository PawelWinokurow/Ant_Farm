using System;
using System.Collections.Generic;
using UnityEngine;

namespace WorkerNamespace
{

    public class Worker : MonoBehaviour, Mob
    {
        public string id { get; set; }
        public MobType type { get; set; }
        public static float CONSTRUCTION_SPEED = 2f;
        public static int LOADING_SPEED = 50;
        public static int MAX_CARRYING_WEIGHT = 100;
        public float carryingWeight = 0;
        public AntAnimator animator { get; set; }
        public Health health { get; set; }
        public Action Animation { get; set; }
        public Vector3 position { get => transform.position; }
        public WorkerJob job { get; set; }
        public Path path { get; set; }
        public State currentState { get; set; }
        public Pathfinder pathfinder { get; set; }
        public SurfaceOperations surfaceOperations { get; set; }
        public bool HasPath { get => path != null && currentPathEdge != null; }
        public bool HasJob { get => job != null; }
        private float lerpDuration;
        private float t = 0f;
        public Edge currentPathEdge;
        public Action Kill { get; set; }
        public FloorHexagon currentHex { get; set; }
        public float hp { get; set; }
        public int accessMask { get; set; }

        void Start()
        {
            hp = 100f;
            animator = GetComponent<AntAnimator>();
            animator.worker = this;
            health = GetComponent<Health>();
            health.MAX_HP = Settings.Instance.gameSettings.WORKER_HP;
            type = MobType.WORKER;
            accessMask = Settings.Instance.gameSettings.ACCESS_MASK_FLOOR;
            SetState(new IdleState(this));
        }

        public void SetState(State state)
        {
            if (currentState != null)
                currentState.OnStateExit();

            currentState = state;
            gameObject.name = "Worker - " + state.GetType().Name;

            if (currentState != null)
                currentState.OnStateEnter();
        }

        public void SetPath(Path path)
        {
            this.path = path;
            if (path != null)
            {
                if (path.HasWaypoints) SetcurrentPathEdge();
                else currentState.OnStateEnter();
            }
            else
            {
                if (HasJob)
                {
                    job.Cancel();
                }
                else
                {
                    SetState(new IdleState(this));
                }
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
            var newRandomWalk = pathfinder.RandomWalk(path.wayPoints[path.wayPoints.Count - 1].to.position, 5, accessMask);
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
            if (HasJob)
            {
                var pathNew = pathfinder.FindPath(position, job.destination, accessMask, SearchType.NEAREST_CENTRAL_VERTEX);
                SetPath(pathNew);
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
        public void SetRunFoodAnimation()
        {
            Animation = animator.RunFood;
        }
        public void SetIdleAnimation()
        {
            Animation = animator.Idle;
        }

        public void Hit(int damage)
        {
            hp -= damage;
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

