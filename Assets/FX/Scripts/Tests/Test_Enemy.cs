using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntFarm
{
    public class Test_Enemy : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                GetComponent<EnemyAnimator>().Idle();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                GetComponent<EnemyAnimator>().IdleFight();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                GetComponent<EnemyAnimator>().Run();
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                GetComponent<EnemyAnimator>().RunFight();
            }
        }

    }
}

