using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntFarm
{
    public class AntsManager : MonoBehaviour
    {
        public JobScheduler JobScheduler;
        public Digger DiggerPrefab;
        public void AddMob()
        {
            for (int i = 0; i < 1; i++)
            {
                StartCoroutine(SpawnMob());
            }
        }

        IEnumerator SpawnMob()
        {
            JobScheduler.AddMob(Instantiate(DiggerPrefab, new Vector3(30, 0, 30), Quaternion.identity));
            yield return null;
        }
    }
}