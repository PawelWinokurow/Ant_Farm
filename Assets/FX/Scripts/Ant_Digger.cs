using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
public class Ant_Digger : MonoBehaviour, IAnt
{
    private GameManager gm;
    public List<Hexagon> digList;
    public float damagePeriod=0.3f;
    private NavMeshPath navMeshPath;
    private Vector3 randPos;
    private float speed;
    private Hexagon diggedHex;
    public Transform _transform { get; set; }
    public NavMeshAgent agent { get; set; }

    private void Start()
    {
        _transform = transform;
        navMeshPath = new NavMeshPath();
        gm = GameManager.instance;
        agent = GetComponent<NavMeshAgent>();
        randPos = new Vector3(Random.Range(-100f, 100f), 0, Random.Range(-100f, 100f));
        //speed = Random.Range(3f, 4f);
        speed = Random.Range(10f, 15f);
        agent.avoidancePriority=90;
        digList = new List<Hexagon>();
        DigPath();
    }


    // Update is called once per frame
    void Update()
    {
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(agent.transform.position, out navMeshHit, 1f, NavMesh.AllAreas);
    
      
        if (navMeshHit.mask == 1 << NavMesh.GetAreaFromName("Digged"))
        {
            agent.speed = 0.0f;//если подъехали к стене

        }
        else
        {
            agent.speed = speed;//если едем по полу
      
        }



        if ( agent.velocity.magnitude <0.1f )//если останавливаемся ищем новый путь
        {
            if(navMeshHit.mask == 1 << NavMesh.GetAreaFromName("Digged") )
            {
                if (diggedHex == null)
                {
                    Hexagon hex = test();
                    if (hex != null && hex.isDig)
                    {
                        diggedHex = hex;
                    }
                }
            }
            else
            {
                DigPath();
            }
        }

        if (diggedHex != null)//копаем пока не скопаем
        {
            diggedHex.hp -= Time.deltaTime / damagePeriod;
            //Debug.Log(diggedHex.hp);
            if (diggedHex.hp <= 0 )
            {
                gm.Dig(diggedHex);
            }
        }




    }


    public void DigPath()//находим путь
    {
        
        int i=0;
        if (gm.digList.Count != 0)
        {
            digList = gm.digList.OrderBy(x => Vector3.Distance(randPos, x.transform.position)).ToList();//сортирую массив чтоб каждый муравей в своей стороне работал
            for (i = 0; i < digList.Count; i++)
            {
                agent.CalculatePath(digList[i].transform.position, navMeshPath);//
                if (navMeshPath.status == NavMeshPathStatus.PathComplete)
                {
                    agent.destination = digList[i].transform.position;
                    break;
                }
            }
        }

        if ( gm.digList.Count == 0 || i == digList.Count)//если гексы не выделены /  если выделенные гексы не достижимы
        {
            if(agent.enabled && gm.groundList.Count > 0)
            {
                agent.destination = gm.groundList[Random.Range(0, gm.groundList.Count)].transform.position + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));//рандомно ходим
            }
        }
        
    }


    private Hexagon test()//находим что рядом можно выкопать
    {
        Hexagon hex = null;

        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);

        foreach (Collider collider in colliders)
        {
           Hexagon hexObst = collider.GetComponent<Hexagon>();
            if (hexObst.isDig)
            {
                hex = hexObst;
                break;
            }

        }
        return hex;
    }

}
