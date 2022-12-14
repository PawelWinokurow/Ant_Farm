using System;
using System.Collections.Generic;
using UnityEngine;

namespace WorkerNamespace
{

    public class Worker : MonoBehaviour, Mob
    {
        public string id { get; set; }
        public MobType type { get; set; }
        public float carryingWeight = 0;
        public MobAnimator animator { get; set; }
        public Health health { get; set; }
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
        public int accessMask { get; set; }
        private GameSettings gameSettings;
        private WorkerSettings workerSettings;
        public int movementSpeed { get; set; }
        public Transform body;
        public Transform angl;
        private Quaternion smoothRot;
        public Store store;


        void Start()
        {
            gameSettings = Settings.Instance.gameSettings;
            workerSettings = Settings.Instance.workerSettings;
            animator = GetComponent<MobAnimator>();
            health = GetComponent<Health>();
            health.InitHp(workerSettings.HP);
            type = MobType.WORKER;
            accessMask = gameSettings.ACCESS_MASK_FLOOR + gameSettings.ACCESS_MASK_BASE;
            SetState(new IdleState(this));
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
            Rotation();
        }

        public void SetRunAnimation()
        {
            animator.Run();
        }
        public void SetRunFoodAnimation()
        {
            animator.RunFood();
        }
        public void SetIdleAnimation()
        {
            animator.Idle();
        }

        public float Hit(int damage)
        {
            if (health.hp > 0 && health.Hit(damage) <= 0)
            {
                Kill();
            }
            return health.hp;
        }

        private void Rotation()
        {
            Vector3 forward = Vector3.zero;
            if (currentPathEdge != null)
            {
                forward = currentPathEdge.to.position - transform.position;
            }
            else if (job?.destination != null)
            {
                forward = job.destination - transform.position;
            }
            Quaternion rot = Quaternion.LookRotation(angl.up, forward);
            smoothRot = Quaternion.Slerp(smoothRot, rot, Time.deltaTime * 10f);
            body.rotation = smoothRot;
            body.Rotate(new Vector3(-90f, 0f, 180f), Space.Self);
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

