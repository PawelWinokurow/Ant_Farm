using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

        public override bool IsTargetInSight()
        {
            return store.allEnemies.Any(enemy =>
            {
                var targetPosition = enemy.position;
                var vec = targetPosition - position;
                var vecLength = Vector3.Magnitude(vec);
                if (vecLength < 20f)
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
            });
        }

        public void Attack()
        {
            var enemiesToAttack = store.allEnemies.Where(enemy => enemy.currentHex == currentHex).ToList();
            enemiesToAttack.ForEach(enemy => enemy.Hit(trapSettings.ATTACK_STRENGTH));
        }
    }


}