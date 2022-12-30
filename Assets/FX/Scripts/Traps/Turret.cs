using System.Collections.Generic;
using System.Linq;
using FighterNamespace;
using UnityEngine;
using DataStructures.ViliWonka.KDTree;

namespace TrapNamespace
{
    public class Turret : Trap
    {
        void Start()
        {
            animator = GetComponent<MobAnimatorTurret>();
            gameSettings = Settings.Instance.gameSettings;
            trapSettings = Settings.Instance.turretSettings;
            type = TrapType.TURRET;
            health = GetComponent<Health>();
            workHexagon = GetComponent<WorkHexagon>();
            currentHex = workHexagon.floorHexagon;
            store = currentHex.store;
            health.InitHp(trapSettings.HP);
            Kill = () =>
            {
                SetState(new DeadState(this));
            };
            SetInitialState();
        }

        public override Target SearchTarget()
        {

            var notDeadMobs = new List<Targetable>(store.allEnemies.Where(mob => mob.currentState != null && mob.currentState.type != STATE.DEAD));
            KDTree mobPositionsTree = new KDTree(notDeadMobs.Select(mob => mob.position).ToArray());
            KDQuery query = new KDQuery();
            List<int> queryResults = new List<int>();
            query.Radius(mobPositionsTree, position, 30f, queryResults);
            if (queryResults.Count == 0) { return null; }
            for (int i = 0; i < queryResults.Count; i++)
            {
                var targetMob = notDeadMobs[queryResults[i]];
                if (IsPositionInSight(targetMob.position))
                {
                    return new Target($"{id}_{targetMob.id}", targetMob);
                }
            }
            return null;
        }

        private bool IsPositionInSight(Vector3 position)
        {
            var targetPosition = position;
            var vec = targetPosition - position;
            var vecLength = Vector3.Magnitude(vec);
            if (vecLength < 30f)
            {
                var vecNorm = Vector3.Normalize(vec);
                var hexagonsOnTrajectory = new List<FloorHexagon>();
                for (int i = 0; i < Mathf.Floor(vecLength); i++)
                {
                    hexagonsOnTrajectory.Add(workHexagon.surfaceOperations.surface.PositionToHex(position + i * vecNorm));
                }
                hexagonsOnTrajectory.Add(workHexagon.surfaceOperations.surface.PositionToHex(targetPosition));
                return hexagonsOnTrajectory.All(hex => hex.type != HexType.SOIL && hex.type != HexType.STONE);
            }
            return false;
        }
        public override bool IsTargetInSight()
        {
            if (target != null)
            {
                return IsPositionInSight(target.mob.position);
            }
            else
            {
                return false;
            }

        }

        public override void Rotation()
        {
            Vector3 forward = Vector3.zero;
            if (currentState.type == STATE.ATTACK && target?.mob != null)
            {
                forward = target.mob.position - transform.position;
            }
            Quaternion rot = Quaternion.LookRotation(angl.up, forward);
            smoothRot = Quaternion.Slerp(smoothRot, rot, Time.deltaTime * 10f);
            body.rotation = smoothRot;
            body.Rotate(new Vector3(-90f, 0f, 180f), Space.Self);
        }
    }


}