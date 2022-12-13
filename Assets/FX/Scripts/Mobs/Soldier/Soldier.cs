using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures.ViliWonka.KDTree;
using UnityEngine;

namespace SoldierNamespace
{
    public class Soldier : Fighter
    {

        private SoldierSettings soldierSettings;

        void Start()
        {
            gameSettings = Settings.Instance.gameSettings;
            soldierSettings = Settings.Instance.soldierSettings;
            animator = GetComponent<MobAnimator>();
            animator.Attack = () => Attack();
            type = MobType.SOLDIER;
            accessMask = gameSettings.ACCESS_MASK_FLOOR + gameSettings.ACCESS_MASK_BASE;
            health = GetComponent<Health>();
            health.InitHp(soldierSettings.HP);
            SetState(new PatrolState(this));
        }

        public void Attack()
        {
            if (target.mob.Hit(soldierSettings.ATTACK_STRENGTH) <= 0)
            {
                CancelJob();
                SetState(new PatrolState(this));
            }
        }

        public Target SearchTarget()
        {
            return SearchTarget(store.allEnemies);
        }

        public bool IsTargetInSight()
        {
            return currentHex.vertex.neighbours.Select(vertex => vertex.id).Append(currentHex.id).Contains(target.mob.currentHex.id);
        }
    }
}

