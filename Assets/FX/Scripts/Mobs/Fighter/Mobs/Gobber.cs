using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FighterNamespace
{
    public class Gobber : Fighter
    {
        void Start()
        {
            gameSettings = Settings.Instance.gameSettings;
            mobSettings = Settings.Instance.gobberSettings;
            animator = GetComponent<MobAnimator>();
            type = ACTOR_TYPE.GOBBER;
            health = GetComponent<Health>();
            health.InitHp(mobSettings.HP);
            SetInitialState();
        }

        public override bool IsTargetInSight()
        {
            var targetPosition = target.mob.position;
            var vec = targetPosition - position;
            var vecLength = Vector3.Magnitude(vec);
            if (vecLength < 20f)
            {
                var vecNorm = Vector3.Normalize(vec);
                var hexagonsOnTrajectory = new List<FloorHexagon>();
                for (int i = 0; i < Mathf.Floor(vecLength); i++)
                {
                    hexagonsOnTrajectory.Add(surfaceOperations.surface.PositionToHex(position + i * vecNorm));
                }
                hexagonsOnTrajectory.Add(surfaceOperations.surface.PositionToHex(targetPosition));
                return hexagonsOnTrajectory.All(hex => hex.type != HexType.SOIL && hex.type != HexType.STONE);
            }

            return false;
        }

        public override Target SearchTarget()
        {
            return SearchTarget(store.allAllies, mobSettings.FOLLOWING_ACCESS_MASK, EdgeType.SECONDARY, Priorities.ENEMY_TARGET_PRIORITIES);
        }

        public override void Attack()
        {
            target.mob.Hit(mobSettings.ATTACK_STRENGTH);
            if (target.mob.isDead && !isDead)
            {
                CancelJob();
                SetState(new PatrolState(this));
            }
        }

    }
}