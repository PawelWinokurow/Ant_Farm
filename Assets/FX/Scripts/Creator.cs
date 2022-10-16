using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creator : MonoBehaviour
{

    public int higth = 10;
    public int width = 10;
    public Hexagon hexagonPrefab;
    private GameManager gm;

    // Start is called before the first frame update
    void Awake()
    {
        gm = GameManager.instance;

        gm.hexagons = new Hexagon[width, higth];
        int x=0;
        int y = 0;
        for (y = 0; y < higth; y++)
        {
            for (x = 0; x < width; x++)
            {
                gm.hexagons[x,y]= Instantiate(hexagonPrefab, transform.position + new Vector3(x * 1.5f + 1.5f * (y % 2f) / 2f, y * 0.433f, 0f), transform.rotation, transform);
            }
        }
        transform.position = new Vector3(-gm.hexagons[x-1, y-1].mr.transform.position.x / 2f, -gm.hexagons[x-1, y-1].mr.transform.position.y / 2f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
