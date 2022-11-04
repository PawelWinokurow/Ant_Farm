using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MobFactory : MonoBehaviour
{
    public JobScheduler JobScheduler;
    public Digger DiggerPrefab;
    public void AddMob()
    {
        Debug.Log("In");
        for (int i = 0; i < 10; i++)
        {
            StartCoroutine(SpawnDigger());
        }
    }

    IEnumerator SpawnDigger()
    {
        JobScheduler.AddMob(Instantiate(DiggerPrefab, new Vector3(30, 0, 30), Quaternion.identity));
        yield break;
    }
}
