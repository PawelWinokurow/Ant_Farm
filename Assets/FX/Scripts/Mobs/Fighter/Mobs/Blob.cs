using System;
using System.Collections.Generic;
using System.Linq;

namespace FighterNamespace
{
    public class Blob : Fighter
    {
        void Start()
        {
            gameSettings = Settings.Instance.gameSettings;
            mobSettings = Settings.Instance.blobSettings;
            animator = GetComponent<MobAnimatorBlob>();
            health = GetComponent<Health>();
            health.InitHp(mobSettings.HP);
            InitSingletons();
            SetInitialState();
        }

        public override void Attack()
        {
            var neighbours = currentHex.vertex.neighbours
                .Select(neighbour => neighbour.floorHexagon)
                .ToList()
                .Append(currentHex);
            new List<Targetable>(store.allAllies).ForEach(ally =>
            {
                if (neighbours.Contains(ally.currentHex))
                {
                    ally.Hit(mobSettings.ATTACK_STRENGTH);
                    if ((target == null || target.Equals(null)) && !isDead)
                    {
                        CancelJob();
                        SetState(new PatrolState(this));
                    }
                }
            });
        }

        public override Target SearchTarget()
        {
            return SearchTarget(store.allAllies, mobSettings.FOLLOWING_ACCESS_MASK, EdgeType.SECONDARY, Priorities.ENEMY_TARGET_PRIORITIES);
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