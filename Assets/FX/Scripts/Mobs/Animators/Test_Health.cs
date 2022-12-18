using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntFarm
{
    public class Test_Health : MonoBehaviour
    {
        public Health health;

        void Start()
        {
            health =gameObject. GetComponent<Health>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                health.Hit(10);
            }
        }
    }

}
