using System;
using System.Linq;

namespace FighterNamespace
{
    public class Soldier : Fighter
    {
        void Start()
        {
            gameSettings = Settings.Instance.gameSettings;
            mobSettings = Settings.Instance.soldierSettings;
            animator = GetComponent<MobAnimator>();
            type = ACTOR_TYPE.SOLDIER;
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

        public override Target SearchTarget()
        {
            return SearchTarget(store.allEnemies, mobSettings.FOLLOWING_ACCESS_MASK, EdgeType.SECONDARY, Priorities.ALLIES_TARGET_PRIORITIES);
        }

        public override bool IsTargetInSight()
        {
            if (currentHex != null && target.mob.currentHex != null)
            {
                return currentHex.vertex.neighbours.Select(vertex => vertex.id).Append(currentHex.id).Contains(target.mob.currentHex.id);
            }
            return false;
        }
    }
}