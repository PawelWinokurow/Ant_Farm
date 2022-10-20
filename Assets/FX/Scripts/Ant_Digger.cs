using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
public class Ant_Digger : MonoBehaviour
{
    private GameManager gm;
    private bool isDig;
    public List<Hexagon> hexagonsDig = new List<Hexagon>();
    private NavMeshAgent agent;
    private float t1;
    public float damagePeriod=0.3f;
    private NavMeshPath navMeshPath;
    private Vector3 randPos;
    private float speed;


    private void Start()
    {
        GameManager.rebuildEvent += Dig;
        navMeshPath = new NavMeshPath();
        gm = GameManager.instance;
        agent = GetComponent<NavMeshAgent>();
        randPos = new Vector3(Random.Range(-100f, 100f), 0, Random.Range(-100f, 100f));
        speed = Random.Range(3f, 4f);
    }


    // Update is called once per frame
    void Update()
    {
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(agent.transform.position, out navMeshHit, 1f, NavMesh.AllAreas);

        if (navMeshHit.mask== 1 << NavMesh.GetAreaFromName("WallDig"))
        {
            isDig = true;
            agent.speed= 0.1f;//если подъехали к стене
            t1 += Time.deltaTime/ damagePeriod;
        }
        else
        {
            isDig = false;
            agent.speed = 3.5f;//если едем по полу
            t1 = 0;
        }
     
        
        if (t1 > 1f)
        {
            t1 = 0f;

            RaycastHit hit;

            if (Physics.Raycast(transform.position + Vector3.up * 5, -Vector3.up , out hit, 10f))
            {
                Hexagon hex = hit.transform.GetComponent<Hexagon>();
                if (hex)
                {
                    hex.hp--;
                    if (hex.hp <= 0)
                    {
                        isDig = false;
                        gm.Dig(hex);
                    }
                }
            }
                
        }
     
    }


    public void Dig()
    {
        // Debug.Log("dig");
        if (gm.hexagonsDig.Count != 0 && !isDig)
        {
            hexagonsDig = gm.hexagonsDig.OrderBy(x => Vector3.Distance(randPos, x.transform.position)).ToList();//сортирую массив чтоб каждый муравей в своей стороне работал
            for (int i = 0; i < hexagonsDig.Count; i++)
            {
                agent.CalculatePath(hexagonsDig[i].transform.position, navMeshPath);//
                if (navMeshPath.status == NavMeshPathStatus.PathComplete)
                {
                    agent.destination = hexagonsDig[i].transform.position;
                    isDig = true;
                    break;
                }
            }
        }
    }
}
