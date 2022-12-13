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
            //TODO
            mobSettings = new FighterSettings();
            mobSettings.ATTACK_STRENGTH = Settings.Instance.soldierSettings.ATTACK_STRENGTH;
            mobSettings.FOLLOWING_MOVEMENT_SPEED = Settings.Instance.soldierSettings.FOLLOWING_MOVEMENT_SPEED;
            mobSettings.HP = Settings.Instance.soldierSettings.HP;
            mobSettings.PATROL_MOVEMENT_SPEED = Settings.Instance.soldierSettings.PATROL_MOVEMENT_SPEED;
            animator = GetComponent<MobAnimator>();
            animator.Attack = () => Attack();
            type = MobType.SOLDIER;
            accessMask = gameSettings.ACCESS_MASK_FLOOR + gameSettings.ACCESS_MASK_BASE;
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

        public override Target SearchTarget()
        {
            return SearchTarget(store.allEnemies);
        }

        public override bool IsTargetInSight()
        {
            return currentHex.vertex.neighbours.Select(vertex => vertex.id).Append(currentHex.id).Contains(target.mob.currentHex.id);
        }
    }
}