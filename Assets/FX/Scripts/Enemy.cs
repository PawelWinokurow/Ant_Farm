using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IAnt
{
    private GameManager gm;
    private Vector3 target;
    public List<IAnt> antsList;
    private Hexagon diggedHex;
    private Vector3 center;

    private float speed;
    public Transform Transform { get; set; }
    public NavMeshAgent Agent { get; set; }


    private Job job;

    void Start()
    {
        gm = GameManager.GetInstance();
        Agent = GetComponent<NavMeshAgent>();
        Transform = transform;
        center = gm.spawn.transform.position;
        center = new Vector3(center.x, 0, center.z);
        Agent.destination = center;
        speed = Random.Range(10f, 15f);
    }



    void FixedUpdate()
    {
        Agent.speed = 15;

        antsList = gm.antsList.OrderBy(x => Vector3.Distance(transform.position, x.Transform.position)).ToList();
        if (antsList.Count > 0 && Vector3.Distance(transform.position, antsList[0].Transform.position) < 10)
        {
            target = antsList[0].Transform.position;
        }
        else
        {
            target = gm.spawn.transform.position;
        }

        Agent.destination = target;

        if (Vector3.Distance(transform.position, target) > 0.1f)
        {

            if (Agent.remainingDistance == 0f && Agent.velocity.magnitude == 0f)
            {

                Hexagon hex = test();
                if (hex != null)
                {
                    diggedHex = hex;
                }

            }


            if (diggedHex != null)
            {
                if (Agent.velocity.magnitude == 0f && Agent.remainingDistance == 0)
                {
                    diggedHex.hp -= Time.fixedDeltaTime / 0.1f;
                    if (diggedHex.hp <= 0)
                    {

                        // gm.Dig(diggedHex.id);
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

    public bool HasJob()
    {
        return job != null;
    }

    public void SetJob(Job job)
    {
        this.job = job;
    }


    public void SetPath(NavMeshPath path)
    {
        Agent.SetPath(path);
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
