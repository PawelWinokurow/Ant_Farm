using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FighterNamespace
{
    public class Gunner : Fighter
    {

        void Start()
        {
            gameSettings = Settings.Instance.gameSettings;
            mobSettings = Settings.Instance.gunnerSettings;
            animator = GetComponent<MobAnimator>();
            health = GetComponent<Health>();
            health.InitHp(mobSettings.HP);
            InitSingletons();
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
                    hexagonsOnTrajectory.Add(surface.PositionToHex(position + i * vecNorm));
                }
                hexagonsOnTrajectory.Add(surface.PositionToHex(targetPosition));
                return hexagonsOnTrajectory.All(hex => hex.type != HexType.SOIL && hex.type != HexType.STONE);
            }

            return false;
        }

        public override Target SearchTarget()
        {
            return SearchTarget(store.allEnemies, mobSettings.FOLLOWING_ACCESS_MASK, EdgeType.SECONDARY, Priorities.ALLIES_TARGET_PRIORITIES);
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
