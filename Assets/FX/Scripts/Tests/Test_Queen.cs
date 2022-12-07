using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntFarm
{
    public class Test_Queen : MonoBehaviour
    {
        private Queen queen;
        void Start()
        {
            queen = gameObject.GetComponent<Queen>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
              Vector3 randDir= new Vector3(Random.Range(-1f, 1f),0f, Random.Range(-1f, 1f));
                queen.Fight(transform.position+ randDir);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                queen.Idle();
            }
        }
    }
}
