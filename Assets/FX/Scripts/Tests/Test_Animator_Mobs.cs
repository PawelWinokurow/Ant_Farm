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
                GetComponent<Animator_Mobs>().IdleFight();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                GetComponent<Animator_Mobs>().Run();
            }

        }

    }
}

