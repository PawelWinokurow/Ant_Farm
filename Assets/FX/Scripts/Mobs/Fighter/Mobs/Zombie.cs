using System;
using System.Linq;

namespace FighterNamespace
{
    public class Zombie : Fighter
    {
        public Action<SliderValue, FloorHexagon> MutateMob;
        void Start()
        {
            gameSettings = Settings.Instance.gameSettings;
            mobSettings = Settings.Instance.zombieSettings;
            animator = GetComponent<MobAnimator>();
            type = MobType.ZOMBIE;
            health = GetComponent<Health>();
            health.InitHp(mobSettings.HP);
            SetInitialState();
        }

        public void Attack()
        {
            if (UnityEngine.Random.Range(0, 100f) < 5)
            {
                target.mob.Hit(Int32.MaxValue);
                MutateMob(SliderValue.ZOMBIE, target.mob.currentHex);
                CancelJob();
                SetState(new PatrolState(this));
            }
            else if (target.mob.Hit(mobSettings.ATTACK_STRENGTH) <= 0)
            {
                CancelJob();
                SetState(new PatrolState(this));
            }
        }

        public override Target SearchTarget()
        {
            return SearchTarget(store.allAllies, mobSettings.FOLLOWING_ACCESS_MASK);
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