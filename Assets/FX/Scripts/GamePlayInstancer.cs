using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayInstancer : MonoBehaviour
{
    public GameObject hole;
    public Surface surface;
    public Transform marker;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
           // for (int i = 0; i < 50; i++)
           // {
                Vector3 sidePos = surface.sideHexagonsPos[Random.Range(0, surface.sideHexagonsPos.Length)];
                Vector3 pos = Vector3.Lerp(sidePos, surface.center + (sidePos - surface.center).normalized * 15f, Mathf.Pow(Random.Range(0f, 1f), 3f));
            Hexagon hex = surface.PositionToHex(pos);
                Instantiate(marker, hex.position, Quaternion.identity);
          //  }

        }

    }
}
