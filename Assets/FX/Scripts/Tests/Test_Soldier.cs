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
                GetComponent<MobAnimator>().Idle();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                GetComponent<MobAnimator>().Run();
            }

        }

    }
}

