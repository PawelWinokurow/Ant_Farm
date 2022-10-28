using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Surface : MonoBehaviour
{
    public Hexagon hexPrefab;
    public GameObject wallPrefab;
    public GameObject digPrefab;
    public GameObject buildPrefab;
    private Camera cam;
    public int height = 10;
    public int width = 10;
    public Vector3 ld;
    public Vector3 rd;
    public Vector3 lu;
    public Vector3 ru;
    public float w;
    public float h;

    private Hexagon[,] allHexXZ;//это для счета к какому хексу мышка наиближе
    public Hexagon[] allHex;//это хексы логики

    public Graph PathGraph;

    public List<Hexagon> digList;
    public List<Hexagon> buildList;



    public void Init(Graph PathGraph)
    {
        digList = new List<Hexagon>();
        buildList = new List<Hexagon>();
        this.PathGraph = PathGraph;
        cam = Camera.main;

        ld = cam.ScreenToWorldPoint(new Vector3(0, Screen.height * 0.09f, 1f));

        rd = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height * 0.09f, 1f));

        lu = cam.ScreenToWorldPoint(new Vector3(0, Screen.height, 1f));

        ru = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 1f));

        float radius = 2f;
        w = Mathf.Sin((60) * Mathf.Deg2Rad) * 2f * radius;//растояние по горизонтали между шестигольниками
        h = 2 * radius * 3f / 4f;//растояние по вертикали между шестигольниками

        height = Mathf.CeilToInt((lu.z - ld.z) / h) - 1;//находим количество шестиугольников в ширину и длину
        width = Mathf.CeilToInt((rd.x - ld.x) / w) - 1;

        allHexXZ = new Hexagon[width, height];
        allHex = new Hexagon[width * height];

        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 hexPosition = new Vector3(w * (x + (z % 2f) / 2f), 0f, z * h);
                PathGraph.AddHexagonSubGraph(hexPosition, radius, $"x{x}z{z}");

                Hexagon hex = Instantiate(hexPrefab, new Vector3(w * (x + (z % 2f) / 2f), 0f, z * h), Quaternion.identity, transform);
                allHexXZ[x, z] = hex;//двумерный массив
                allHex[z * width + x] = hex;

            }
        }

        Camera.main.transform.parent.position = new Vector3((width - 0.5f) * w / 2f, 0, (height - 1) * h / 2f * (1f - 0.09f));


        for (int i = 0; i < allHex.Length; i++)//записываем соседей
        {
            allHex[i].neighbors = new List<int>();
            for (int j = 0; j < allHex.Length; j++)
            {
                if (Vector3.Distance(allHex[i].transform.position, allHex[j].transform.position) < 4f)
                {
                    if (allHex[i] != allHex[j])
                    {
                        allHex[i].neighbors.Add(j);
                    }
                }
            }
        }
    }

    void Update()
    {
        if (PathGraph != null)
        {

            PathGraph.AdjacencyList.ForEach(edge =>
            {
                if (!edge.IsWalkable)
                {
                    Debug.DrawLine(edge.From.GeometricalPoint, edge.To.GeometricalPoint, Color.red);
                }
                else
                {
                    Debug.DrawLine(edge.From.GeometricalPoint, edge.To.GeometricalPoint, Color.green);
                }

            }
            );
        }
    }

    public Hexagon PositionToHex(Vector3 pos)
    {
        int z = (int)Mathf.Round(pos.z / h);
        int x = (int)Mathf.Round((pos.x - (z % 2) * 0.5f * w) / w);
        z = Mathf.Clamp(z, 0, allHexXZ.GetLength(1) - 1);
        x = Mathf.Clamp(x, 0, allHexXZ.GetLength(0) - 1);
        return allHexXZ[x, z];
    }


    public void AddGround(Hexagon hex)
    {
        HexagonClear(hex);
        hex.isGround = true;
        hex.cost = 1;
    }


    public void AddWall(Hexagon hex)
    {
        HexagonClear(hex);
        hex.isWall = true;
        hex.cost = -1;
        PathGraph.ProhibitHexagon(hex.transform.position);
        Instantiate(wallPrefab, hex.transform.position, Quaternion.identity, hex.transform);
    }

    public void AddDig(Hexagon hex)
    {
        HexagonClear(hex);
        hex.isDig = true;
        hex.cost = -1;
        digList.Add(hex);
        //PathGraph.ProhibitHexagon(hex.transform.position);
        Instantiate(digPrefab, hex.transform.position, Quaternion.identity, hex.transform);
    }
    public void AddBuild(Hexagon hex)
    {
        HexagonClear(hex);
        hex.isBuild = true;
        hex.cost = 1;
        buildList.Add(hex);
        // PathGraph.ProhibitHexagon(hex.transform.position);
        Instantiate(buildPrefab, hex.transform.position, Quaternion.identity, hex.transform);
    }

    private void HexagonClear(Hexagon hex)
    {
        hex.isWall = false;
        hex.isDig = false;
        hex.isGround = false;
        hex.isBuild = false;
        hex.isSpawn = false;
        digList.Remove(hex);
        buildList.Remove(hex);
        for (int i = 0; i < hex.transform.childCount; i++)//удаляет чайлды старой графики
        {
            GameObject.Destroy(hex.transform.GetChild(i).gameObject);
        }
    }

}