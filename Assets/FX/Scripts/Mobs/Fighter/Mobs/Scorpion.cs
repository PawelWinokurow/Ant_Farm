using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace FighterNamespace
{
    public class Scorpion : Fighter
    {
        public Dig_FX DigFX;
        public Dig_FX digFX;
        void Start()
        {
            gameSettings = Settings.Instance.gameSettings;
            mobSettings = Settings.Instance.scorpionSettings;
            animator = GetComponent<MobAnimator>();
            type = MobType.SCORPION;
            health = GetComponent<Health>();
            health.InitHp(mobSettings.HP);
            SetInitialState();
        }

        public void Attack()
        {
            if (target.mob.Hit(mobSettings.ATTACK_STRENGTH) <= 0)
            {
                CancelJob();
                SetState(new PatrolState(this));
            }
        }

        protected override void SetcurrentPathEdge()
        {
            if (path.wayPoints.Count > 1 && path.wayPoints[1].floorHexagon.type == HexType.SOIL)
            {
                var hex = path.wayPoints[1].floorHexagon;
                digJob = new DigJob(hex, hex.transform.position, this);
                StartDigFX(hex);
                SetState(new DigState(this));
            }
            else
            {
                base.SetcurrentPathEdge();
            }
        }

        public void StartDigFX(FloorHexagon hex)
        {
            if (digFX == null) digFX = Instantiate(DigFX, position, Quaternion.identity);
            digFX.transform.position = hex.position;
            digFX.StartFx(hex);
        }

        public void StopDigFX()
        {
            digFX.StopFx();
        }

        public override void SetRandomWalk()
        {
            SetPath(pathfinder.RandomWalk(position, 5, accessMask, EdgeType.PRIMARY));
        }

        public override Target SearchTarget()
        {
            return SearchTarget(store.allAllies, mobSettings.FOLLOWING_ACCESS_MASK, EdgeType.PRIMARY);
        }

        public override bool IsTargetInSight()
        {
            if (currentHex != null && target.mob.currentHex != null)
            {
                return currentHex.vertex.neighbours.Select(vertex => vertex.id).Append(currentHex.id).Contains(target.mob.currentHex.id);
            }
            return false;
        }

        public override float Hit(int damage)
        {
            if (health.hp > 0 && health.Hit(damage) <= 0)
            {
                if (digFX != null)
                {
                    digFX.StopFx();
                    Destroy(digFX, 5f);
                }
                Kill();
            }
            return health.hp;
        }

        protected override void Rotation()
        {
            Vector3 forward = Vector3.zero;
            if (currentState.type == STATE.ATTACK && target?.mob != null)
            {
                forward = target.mob.position - transform.position;
            }
            else if (digJob?.destination != null)
            {
                forward = digJob.destination - transform.position;
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

        public void CancelDigging()
        {
            digJob.Cancel();
            digJob = null;
            StopDigFX();
        }
    }
}
