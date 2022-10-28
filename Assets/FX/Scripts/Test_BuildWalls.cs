using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntFarm
{
    public class Test_BuildWalls : MonoBehaviour
    {
        private Surface surf;
        [Range(0, 100)]
        public int wallPercentage = 25;
        private int wallPercentageOld;
        void Start()
        {
            surf = Surface.instance;
            CreateWalls();


        }
        private void Update()
        {
            if (wallPercentage != wallPercentageOld)
            {
                CreateWalls();
                wallPercentageOld = wallPercentage;
            }
        }


        void CreateWalls()
        {

            for (int i = 1; i < surf.allHex.Length - 1; i++)
            {
                if (Random.Range(0, 100f) < wallPercentage)
                {
                    surf.AddWall(surf.allHex[i]);
                }
                else
                {
                    surf.AddGround(surf.allHex[i]);
                }
            }
        }
    }
}