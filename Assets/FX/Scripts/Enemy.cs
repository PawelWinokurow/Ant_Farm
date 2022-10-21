using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private GameManager gm;
    private Vector3 target;
    public List<IAnt> antsList;
    private NavMeshAgent agent;
    private Hexagon diggedHex;
    private Vector3 center;

    private float speed;


    void Start()
    {
        gm = GameManager.instance;
        agent = GetComponent<NavMeshAgent>();
        center = Camera.main.transform.position;
        center = new Vector3(center.x, 0, center.z);
        agent.destination = center;
        speed = Random.Range(10f, 15f);
    }


    void FixedUpdate()
    {


        agent.speed = 15;


        antsList = gm.antsList.OrderBy(x => Vector3.Distance(transform.position, x.position)).ToList();
        if (antsList.Count>0 &&  Vector3.Distance(transform.position, antsList[0].position) < 10)
        {
            target = antsList[0].position;
        }
        else
        {
            target = gm.spawn.transform .position;
        }

        agent.destination = target;







        if (Vector3.Distance(transform.position, target) > 0.1f)
        {

            if (agent.remainingDistance == 0f && agent.velocity.magnitude == 0f)
            {

                Hexagon hex = test();
                if (hex != null)
                {
                    diggedHex = hex;
                }

            }


            if (diggedHex != null)
            {
                if (agent.velocity.magnitude == 0f && agent.remainingDistance == 0)
                {
                    diggedHex.hp -= Time.fixedDeltaTime / 0.1f;
                    if (diggedHex.hp <= 0)
                    {

                        gm.Dig(diggedHex);
                        diggedHex = null;
                    }
                }
                else
                {
                    diggedHex = null;
                }
            }

        }
    }



    private Hexagon test()//находим что рядом можно выкопать
    {
        Hexagon hex = null;

        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);

        float minDist = 1000000;

        foreach (Collider collider in colliders)
        {
            Hexagon hexObst = collider.GetComponent<Hexagon>();
            if (hexObst.isWall)
            {
                float dist = Vector3.Distance(target, transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    hex = hexObst;
                }
            }

        }
        return hex;
    }
}
