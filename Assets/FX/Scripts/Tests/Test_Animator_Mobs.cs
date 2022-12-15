using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntFarm
{
    public class Test_Animator_Mobs : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                GetComponent<IMobAnimator>().IdleFight();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                GetComponent<IMobAnimator>().Run();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                GetComponent<IMobAnimator>().Idle();
            }

        }

    }
}

