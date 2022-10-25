using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntFarm
{
    public class Test_BuildWalls : MonoBehaviour
    {
        private Surface2 surf;

        IEnumerator Start()
        {
            surf = Surface2.instance;

            yield return null;
            for (int i = 0; i < surf.allHex.Length; i++)
            {
               if(Random.Range(0, 100f) < 30f)
                {
                    surf.AddWall(i);
                }
            }
        }

       
    }
}
