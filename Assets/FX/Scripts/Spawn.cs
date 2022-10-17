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

        Collider[] colliders=Physics.OverlapSphere(transform.position, 0.8f);

        foreach(Collider collider in colliders)
        {
            Hexagon hex = collider.GetComponent<Hexagon>();
            if (hex)
            {
                Vector3 pos = hex.transform.position;
                gm.hexagons.Remove(hex);
                GameObject.Destroy(collider.gameObject);
                gm.hexagons.Add(Instantiate(gm.hexagonFloor, pos, Quaternion.identity));
            }
        }
    }
}
