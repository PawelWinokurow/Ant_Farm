using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    private GameManager gm;

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
                gm.AddFloor(hex);

            }
        }

        gm.navSurface.BuildNavMesh();
    }
}
