using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    private GameManager gm;
    //public GameObject marker;

    IEnumerator Start()
    {
        gm = GameManager.instance;
        yield return null;

        Collider[] colliders=Physics.OverlapSphere(transform.position, 3f);

        foreach(Collider collider in colliders)
        {

            Hexagon hex = collider.GetComponent<Hexagon>();
            if (hex)
            {
                    gm.AddSpawn(hex);
            }
        }
        /*
        for (int i = 0; i < 20; i++)
        {
            IAnt ant = Instantiate(gm.antPrefab, gm.spawnList[Random.Range(0, gm.spawnList.Count)].transform.position, Quaternion.identity);
            gm.antsList.Add(ant);
        }
        */
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
          
            IAnt ant= Instantiate(gm.antPrefab, gm.spawnList[Random.Range(0, gm.spawnList.Count)].transform.position, Quaternion.identity);
            //Instantiate(marker, ant.transform.position, Quaternion.identity);
            gm.antsList.Add(ant);
        }   
    }
}
