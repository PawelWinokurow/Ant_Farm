using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WarpTest : MonoBehaviour
{
    private GameManager gm;
    public NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
       if( Input.GetKeyDown(KeyCode.D))
        {
        
            agent.Warp(gm.groundList[Random.Range(0, gm.groundList.Count)].transform.position);
            agent.destination = gm.groundList[Random.Range(0, gm.groundList.Count)].transform.position;
        }
    }
}
