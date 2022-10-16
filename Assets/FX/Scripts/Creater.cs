using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creater : MonoBehaviour
{
    public class Hexagon
    {
     public bool isSelected;
     public bool isDigged;
     public MeshRenderer mr;
    }

    public int higth = 10;
    public int width = 10;
    public GameObject hexagonPrefab;
    public Hexagon[,] hexagons;

    // Start is called before the first frame update
    void Start()
    {
        hexagons = new Hexagon[width, higth];
        for (int y = 0; y < higth; y++)
        {
            for (int x = 0; x < width; x++)
            {
                hexagons[x,y].mr = Instantiate(hexagonPrefab, transform.position+new Vector3(x*0.5f,y*0.5f,0f), transform.rotation,transform).GetComponent<MeshRenderer>();

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
