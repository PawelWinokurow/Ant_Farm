using System.Collections;
using UnityEngine;

public class Spawn : MonoBehaviour
{

    public void Init(Surface surface, GameManager gm)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3f);

        foreach (Collider collider in colliders)
        {
            Hexagon hex = collider.GetComponent<Hexagon>();
            if (hex)
            {
                surface.AddSpawn(hex.id);
            }
        }

        for (int i = 0; i < 3; i++)
        {
            IAnt ant = Instantiate(gm.antDiggerPrefab, surface.spawnList[UnityEngine.Random.Range(0, surface.spawnList.Count)].transform.position, Quaternion.identity).GetComponent<IAnt>();
            gm.antsList.Add(ant);

            // ant = Instantiate(gm.antBuilderPrefab, surface.spawnList[UnityEngine.Random.Range(0, surface.spawnList.Count)].transform.position, Quaternion.identity).GetComponent<IAnt>();
            // gm.antsList.Add(ant);
        }

    }

}
