using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntFarm
{
    public class Test_Soldier : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                GetComponent<SoldierAnimator>().Idle();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                GetComponent<SoldierAnimator>().Run();
            }

        }

    }
}

