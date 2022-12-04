using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntFarm
{
    public class Test_Ant : MonoBehaviour
    {
        // private float speed = 5f;

        void Update()
        {
            /*
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += Vector3.forward * Time.deltaTime * speed;
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.position += -Vector3.forward * Time.deltaTime * speed;
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.position += -Vector3.right * Time.deltaTime * speed;
            }

            if (Input.GetKey(KeyCode.D))

            {
                transform.position += Vector3.right * Time.deltaTime * speed;
            }
            */
            if (Input.GetKeyDown(KeyCode.E))
            {
                GetComponent<MobAnimator>().Idle();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                GetComponent<MobAnimator>().Run();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                GetComponent<MobAnimator>().RunFood();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                gameObject.GetComponent<Health>().Restart();
            }


            if (Input.GetKeyDown(KeyCode.S))
            {
                gameObject.GetComponent<Health>().Hit(10);
            }

        }
    }
}

