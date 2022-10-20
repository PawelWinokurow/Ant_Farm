using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    private GameManager gm;

    void Start()
    {
        gm = GameManager.instance;
       // StartCoroutine(Test());
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            EnemyInstantiate();
        }
   
    }

    private IEnumerator Test()
    {
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < 10; i++)
        {
            EnemyInstantiate();
        }
 
    }

    private void EnemyInstantiate()
    {
        RaycastHit hit;

        Vector3 pos = gm.sideList[Random.Range(0, gm.sideList.Count)];
        Debug.DrawRay(pos + Vector3.up * 5, -Vector3.up * 10f, Color.red);

        if (Physics.Raycast(pos + Vector3.up * 5, -Vector3.up * 10f, out hit, 100.0f))
        {
            Hexagon hex = hit.transform.GetComponent<Hexagon>();
            if (!hex.isGround)
            {
                gm.AddGround(hex);
                gm.enemyList.Add(Instantiate(gm.enemyPrefab, hit.transform.position, Quaternion.identity));
            }

        }


    }
}
