using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Ant : MonoBehaviour
{
    private float speed = 5f;

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.forward*Time.deltaTime * speed;
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
        if (Input.GetKeyDown(KeyCode.E))
        {
            GetComponent<AntAnimator>().Idle();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GetComponent<AntAnimator>().Run();
        }
    }
}

