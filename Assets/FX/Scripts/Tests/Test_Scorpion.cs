using UnityEngine;

namespace AntFarm
{
    public class Test_Scorpion : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                GetComponent<MobAnimator>().Idle();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                GetComponent<MobAnimator>().IdleFight();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                GetComponent<MobAnimator>().Run();
            }
        }

    }
}

