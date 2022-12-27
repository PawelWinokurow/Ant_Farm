using System.Linq;
using FighterNamespace;
using UnityEngine;

namespace TrapNamespace
{
    public class Spikes : Trap
    {

        void Start()
        {
            animator = GetComponent<MobAnimatorSpikes>();
            gameSettings = Settings.Instance.gameSettings;
            trapSettings = Settings.Instance.spikesSettings;
            type = TrapType.SPIKES;
            health = GetComponent<Health>();
            currentHex = GetComponent<WorkHexagon>().floorHexagon;
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
            var targetMob = store.allEnemies.Where(enemy => enemy.currentHex == currentHex).FirstOrDefault();
            return targetMob != null ? new Target($"{id}_{targetMob.id}", targetMob) : null;
        }

        public override bool IsTargetInSight()
        {
            return store.allEnemies.Where(enemy => enemy.currentHex == currentHex).ToList().Count > 0;
        }

        public void Attack()
        {
            var enemiesToAttack = store.allEnemies.Where(enemy => enemy.currentHex == currentHex).ToList();
            enemiesToAttack.ForEach(enemy => enemy.Hit(trapSettings.ATTACK_STRENGTH));
            Hit(trapSettings.ATTACK_STRENGTH);
        }
    }


}