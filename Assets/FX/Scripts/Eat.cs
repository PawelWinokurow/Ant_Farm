using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eat : MonoBehaviour
{
    private GameManager gm;
    private int downID = 0;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;
        for (int i = 0; i < gm.allHex.Length; i++)
        {
            Hexagon hex = gm.allHex[i];
            if (hex.transform.position.z > gm.creator.ld.z + (gm.creator.lu.z - gm.creator.ld.z) * 0.7f)
                downID = i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            Hexagon hex = gm.allHex[Random.Range(downID, gm.allHex.Length)];
            gm.AddEat(hex.id);

        }
    }
}
