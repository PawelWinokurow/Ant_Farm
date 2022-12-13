using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures.ViliWonka.KDTree;
using UnityEngine;

namespace GunnerNamespace
{
    public class Gunner : Fighter
    {
        private GunnerSettings gunnerSettings;

        void Start()
        {
            gameSettings = Settings.Instance.gameSettings;
            gunnerSettings = Settings.Instance.gunnerSettings;
            animator = GetComponent<MobAnimator>();
            type = MobType.GUNNER;
            accessMask = gameSettings.ACCESS_MASK_FLOOR + gameSettings.ACCESS_MASK_BASE;
            health = GetComponent<Health>();
            health.InitHp(gunnerSettings.HP);
            SetState(new PatrolState(this));
        }

        public bool IsTargetInSight()
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

        public Target SearchTarget()
        {
            return SearchTarget(store.allEnemies);
        }
    }
}

