using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creater : MonoBehaviour
{
    //[System.Serializable]
    public class Hexagon
    {
     public bool isSelected;
     public bool isDigged;
     public MeshRenderer mr;
    }

    public int higth = 10;
    public int width = 10;
    public MeshRenderer hexagonPrefab;
    public Hexagon[,] hexagons;

    // Start is called before the first frame update
    void Start()
    {
        hexagons = new Hexagon[width, higth];
        Debug.Log(hexagons[0,0]);
        int x=0;
        int y = 0;
        for (y = 0; y < higth; y++)
        {
            for (x = 0; x < width; x++)
            {
                Hexagon hex= new Hexagon();
         
                    hex.mr = Instantiate(hexagonPrefab, transform.position + new Vector3(x * 1.5f+ 1.5f*(y%2f)/2f, y * 0.433f, 0f), transform.rotation, transform);

                hexagons[x,y]= hex;

            }
        }
        transform.position = new Vector3(-hexagons[x-1, y-1].mr.transform.position.x / 2f, -hexagons[x-1, y-1].mr.transform.position.y / 2f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
