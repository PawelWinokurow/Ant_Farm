using System;
using System.Linq;
using UnityEngine;

namespace FighterNamespace
{
    public class Soldier : Fighter
    {
        void Start()
        {
            gameSettings = Settings.Instance.gameSettings;
            mobSettings = Settings.Instance.soldierSettings;
            animator = GetComponent<MobAnimator>();
            type = MobType.SOLDIER;
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
            return SearchTarget(store.allEnemies, mobSettings.FOLLOWING_ACCESS_MASK);
        }

        public override bool IsTargetInSight()
        {
            return currentHex.vertex.neighbours.Select(vertex => vertex.id).Append(currentHex.id).Contains(target.mob.currentHex.id);
        }
    }
}