using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return null;


        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.8f);
        foreach(Collider2D collider in colliders)
        {
            if (collider.GetComponent<Hexagon>())
            {
                GameObject.Destroy(collider.gameObject);
            }
        }
    }
}
