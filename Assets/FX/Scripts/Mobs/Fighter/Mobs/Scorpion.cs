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
            animator.Attack = () => Attack();
            type = MobType.SCORPION;
            health = GetComponent<Health>();
            health.InitHp(mobSettings.HP);
            SetState(new PatrolState(this));
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
            currentPathEdge = path.wayPoints[0];
            path.wayPoints.RemoveAt(0);
            var currentHexNew = currentPathEdge.floorHexagon;
            if ((currentHex == null || currentHexNew.id != currentHex.id))
            {
                if (currentHexNew.type == HexType.SOIL)
                {
                    StartCoroutine(Dig(currentHexNew));
                }
            }
            currentHex = currentHexNew;
            lerpDuration = currentPathEdge.edgeWeight * currentPathEdge.edgeMultiplier;
        }

        public IEnumerator Dig(FloorHexagon hex)
        {
            movementSpeed = 3;
            if (digFX == null) digFX = Instantiate(DigFX, position, Quaternion.identity);
            digFX.transform.position = hex.position;
            digFX.StartFx(hex);
            yield return new WaitForSeconds(5f);
            digFX.StopFx();
            hex.RemoveChildren();
            hex.type = HexType.EMPTY;
            movementSpeed = mobSettings.FOLLOWING_MOVEMENT_SPEED;
        }

        public void RemoveDigFX()
        {
            var digFxOld = currentHex.GetComponentInChildren<Dig_FX>();
            if (digFxOld != null)
            {
                digFxOld.StopFx();
                Destroy(digFxOld);
                currentHex.RemoveChildren();
                currentHex.type = HexType.EMPTY;
            }
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
            if (health.Hit(damage) <= 0)
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
    }
}
