using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Surface : MonoBehaviour
{
    public Hexagon hexPrefab;
    public Hexagon wallPrefab;
    public GameObject digPrefab;
    public GameObject fillPrefab;
    private Camera cam;
    private int height;
    private int width;
    public Vector3 ld;
    public Vector3 rd;
    public Vector3 lu;
    public Vector3 ru;
    public float w;
    public float h;

    public Hexagon[] Hexagons;

    public Graph PathGraph;

    public Dictionary<string, GameObject> Icons = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> OldBlocks = new Dictionary<string, GameObject>();

    public void Init(Graph PathGraph)
    {
        this.PathGraph = PathGraph;
        SetupCamera();
        CalculateSizes();
        GenerateHexagons();
        Camera.main.transform.parent.position = new Vector3((width - 0.5f) * w / 2f, 0, (height - 1) * h / 2f * (1f - 0.09f));
    }

    public void Init(Graph PathGraph, HexagonSerializable[] hexagons)
    {
        this.PathGraph = PathGraph;
        SetupCamera();
        CalculateSizes();
        LoadHexagons(hexagons);
        Camera.main.transform.parent.position = new Vector3((width - 0.5f) * w / 2f, 0, (height - 1) * h / 2f * (1f - 0.09f));
    }

    public void SetupCamera()
    {
        cam = Camera.main;

        ld = cam.ScreenToWorldPoint(new Vector3(0, Screen.height * 0.09f, 1f));

        rd = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height * 0.09f, 1f));

        lu = cam.ScreenToWorldPoint(new Vector3(0, Screen.height, 1f));

        ru = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 1f));
    }

    public void CalculateSizes()
    {
        w = Mathf.Sin((60) * Mathf.Deg2Rad) * 2f * Hexagon.Radius;//растояние по горизонтали между шестигольниками
        h = 2 * Hexagon.Radius * 3f / 4f;//растояние по вертикали между шестигольниками

        height = Mathf.CeilToInt((lu.z - ld.z) / h) - 1;//находим количество шестиугольников в ширину и длину
        width = Mathf.CeilToInt((rd.x - ld.x) / w) - 1;
        Hexagons = new Hexagon[width * height];
    }

    public void GenerateHexagons()
    {
        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 hexPosition = new Vector3(w * (x + (z % 2f) / 2f), 0f, z * h);
                PathGraph.AddHexagonSubGraph(hexPosition, Hexagon.Radius, $"x{x}z{z}");
                Hexagon hex = Hexagon.CreateHexagon($"{x}_{z}", hexPrefab, hexPosition, transform, HEX_TYPE.EMPTY);
                Hexagons[z * width + x] = hex;
            }
        }
    }

    void LoadHexagons(HexagonSerializable[] hexagons)
    {
        foreach (var hexSerializable in hexagons)
        {
            var hex = Hexagon.CreateHexagon(hexSerializable.Id, hexPrefab, VectorTransform.FromSerializable(hexSerializable.Position), transform, HEX_TYPE.EMPTY);
            Hexagons.Append(hex);
            if (hexSerializable.HexType == HEX_TYPE.EMPTY)
            {
                AddGround(hex);

            }
            else if (hexSerializable.HexType == HEX_TYPE.SOIL)
            {
                AddBlock(hex);
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
                    Debug.DrawLine(edge.From.Position, edge.To.Position, Color.red);
                }
                else
                {
                    Debug.DrawLine(edge.From.Position, edge.To.Position, Color.green);
                }
            }
            );
        }
    }

    public Hexagon PositionToHex(Vector3 pos)
    {
        int z = (int)Mathf.Round(pos.z / h);
        int x = (int)Mathf.Round((pos.x - (z % 2) * 0.5f * w) / w);
        z = Mathf.Clamp(z, 0, height - 1);
        x = Mathf.Clamp(x, 0, width - 1);
        return Hexagons[z * width + x];
    }

    public void AddBlock(Hexagon hex)
    {
        hex.HexType = HEX_TYPE.SOIL;
        PathGraph.ProhibitHexagon(hex.transform.position);
        Instantiate(wallPrefab, hex.transform.position, Quaternion.identity, hex.transform);
    }

    public void AddGround(Hexagon hex)
    {
        hex.HexType = HEX_TYPE.EMPTY;
        ClearHexagon(hex);
    }

    public void AddIcon(Hexagon hex)
    {
        if (Icons.ContainsKey(hex.Id))
        {
            var oldIcon = Icons[hex.Id];
            GameObject.Destroy(oldIcon);
            Icons.Remove(hex.Id);
            Instantiate(OldBlocks[hex.Id], hex.transform.position, Quaternion.identity, hex.transform);
            OldBlocks.Remove(hex.Id);
        }
        else
        {
            OldBlocks.Add(hex.Id, Instantiate(hex.gameObject));
            ClearHexagon(hex);
            if (hex.IsEmpty)
            {
                Icons.Add(hex.Id, Instantiate(fillPrefab, hex.transform.position, Quaternion.identity, hex.transform));
            }
            else if (hex.IsSoil)
            {
                Icons.Add(hex.Id, Instantiate(digPrefab, hex.transform.position, Quaternion.identity, hex.transform));
            }
        }
    }

    private void ClearHexagon(Hexagon hex)
    {
        for (int i = 0; i < hex.transform.childCount; i++)//удаляет чайлды старой графики
        {
            GameObject.Destroy(hex.transform.GetChild(i).gameObject);
        }
    }

}