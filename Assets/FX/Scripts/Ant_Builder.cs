using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
public class Ant_Builder : MonoBehaviour, IAnt
{
    private GameManager gm;
    public List<Hexagon> buildList;
    public float damagePeriod = 0.3f;
    private NavMeshPath navMeshPath;
    private Vector3 randPos;
    private float speed;
    private Hexagon buildHex;
    public Transform Transform { get; set; }
    public NavMeshAgent Agent { get; set; }

    private Job job;

    private void Start()
    {
        Transform = transform;
        navMeshPath = new NavMeshPath();
        gm = GameManager.GetInstance();
        Agent = GetComponent<NavMeshAgent>();
        randPos = new Vector3(Random.Range(-100f, 100f), 0, Random.Range(-100f, 100f));
        //speed = Random.Range(3f, 4f);
        speed = Random.Range(10f, 15f);
        Agent.avoidancePriority = 90;
        buildList = new List<Hexagon>();
        Agent.speed = speed;//если едем по полу
        // BuildPath();
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


    // Update is called once per frame
    void FixedUpdate()
    {
        // if (Agent.remainingDistance == 0f && Agent.velocity.magnitude == 0f && buildHex == null)
        // {

        //     Hexagon hex = test();
        //     if (hex != null && hex.isBuild)
        //     {
        //         buildHex = hex;
        //     }
        //     else
        //     {
        //         BuildPath();
        //     }

        // }


        // if (buildHex != null)
        // {
        //     if (Agent.velocity.magnitude == 0f && Agent.remainingDistance == 0)
        //     {
        //         buildHex.hp += Time.fixedDeltaTime / 0.2f;
        //         if (buildHex.hp >= 1)
        //         {
        //             gm.Evacuate(buildHex.id);
        //             gm.Build(buildHex.id);
        //         }
        //     }
        //     else
        //     {
        //         buildHex = null;
        //     }
        // }

    }



    // public void BuildPath()//находим путь
    // {

    //     int i = 0;
    //     if (gm.buildList.Count != 0)
    //     {
    //         buildList = gm.buildList.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).ToList();//сортирую массив чтоб каждый муравей в своей стороне работал
    //         for (i = 0; i < buildList.Count; i++)
    //         {
    //             Agent.CalculatePath(buildList[i].transform.position, navMeshPath);//
    //             if (navMeshPath.status == NavMeshPathStatus.PathComplete)
    //             {
    //                 Agent.destination = buildList[i].transform.position;
    //                 break;
    //             }
    //         }
    //     }

    //     if (gm.buildList.Count == 0 || i == buildList.Count)//если гексы не выделены /  если выделенные гексы не достижимы
    //     {
    //         if (gm.groundList.Count > 0)
    //         {
    //             Agent.destination = gm.groundList[Random.Range(0, gm.groundList.Count)].transform.position + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));//рандомно ходим
    //         }
    //     }

    // }


    // private Hexagon test()//находим что рядом можно выкопать
    // {
    //     Hexagon hex = null;

    //     Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);

    //     foreach (Collider collider in colliders)
    //     {
    //         Hexagon hexObst = collider.GetComponent<Hexagon>();
    //         if (hexObst.isBuild)
    //         {
    //             hex = hexObst;
    //             break;
    //         }

    //     }
    //     return hex;
    // }

}
