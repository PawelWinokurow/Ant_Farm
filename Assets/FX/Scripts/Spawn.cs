using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return null;


        Collider[] colliders=Physics.OverlapSphere(transform.position, 0.8f);

        foreach(Collider collider in colliders)
        {
            Debug.Log(collider);
            if (collider.GetComponent<Hexagon>())
            {
                GameObject.Destroy(collider.gameObject);
            }
        }
    }
}
