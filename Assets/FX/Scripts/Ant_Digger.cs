using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
public class Ant_Digger : MonoBehaviour, IAnt
{
    private GameManager gm;
    public float damagePeriod = 0.3f;
    private NavMeshPath navMeshPath;
    private Vector3 randPos;
    private float speed;
    public Transform Transform { get; set; }
    public NavMeshAgent Agent { get; set; }

    private DigJob job;

    private float initialSpeed = 10f;

    private void Start()
    {
        Transform = transform;
        navMeshPath = new NavMeshPath();
        gm = GameManager.GetInstance();
        Agent = GetComponent<NavMeshAgent>();
        randPos = new Vector3(Random.Range(-100f, 100f), 0, Random.Range(-100f, 100f));
        speed = Random.Range(10f, 15f);
    }


    // Update is called once per frame
    void Update()
    {
        if (job != null)
        {
            if (Vector3.Distance(transform.position, job.Destination) < 2f)
            {
                speed = 0;
                var digHex = job.TargetHex;
                digHex.hp -= Time.deltaTime / 0.2f;
                if (digHex.hp <= 0)
                {
                    job.RemoveHexagonFunc(digHex);
                    job = null;
                    speed = initialSpeed;
                }
            }
        }
    }


    public bool HasJob()
    {
        return job != null;
    }

    public void SetPath(NavMeshPath path)
    {
        Agent.SetPath(path);
    }
    public void SetJob(Job job)
    {
        this.job = (DigJob)job;
    }





}
