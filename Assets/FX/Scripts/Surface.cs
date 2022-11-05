using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class Surface : MonoBehaviour
{
    public Hexagon hexPrefab;
    public Hexagon wallPrefab;
    public Hexagon wallPrefabScaled;
    public Hexagon digPrefab;
    public Hexagon fillPrefab;
    public Hexagon basePrefab;
    public Hexagon foodPrefab;
    private Camera cam;
    private int height;
    private int width;
    public Vector3 ld;
    public Vector3 rd;
    public Vector3 lu;
    public Vector3 ru;
    public float w;
    public float h;

    public Hexagon BaseHex { get; set; }
    public Hexagon[] Hexagons;

    public Graph PathGraph;

    public Dictionary<string, Hexagon> OldHexagons = new Dictionary<string, Hexagon>();

    public void Init(Graph PathGraph)
    {
        this.PathGraph = PathGraph;
        SetupCamera();
        CalculateSizes();
        GenerateHexagons();
        SetCameraPositionToCenter();
        SetBaseHex();
    }

    public void Init(Graph PathGraph, HexagonSerializable[] hexagons)
    {
        this.PathGraph = PathGraph;
        SetupCamera();
        CalculateSizes();
        LoadHexagons(hexagons);
        SetCameraPositionToCenter();
        SetBaseHex();
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
        PathGraph.SetNeighbours();
    }

    private void SetCameraPositionToCenter()
    {
        Camera.main.transform.parent.position = new Vector3((width - 0.5f) * w / 2f, 0, (height - 1) * h / 2f * (1f - 0.09f));
    }

    private void SetBaseHex()
    {
        BaseHex = PositionToHex(Camera.main.transform.parent.position);
        BaseHex.ClearHexagon();
    }

    void LoadHexagons(HexagonSerializable[] hexagons)
    {
        for (var i = 0; i < hexagons.Length; i++)
        {
            var hex = Hexagon.CreateHexagon(hexagons[i].Id, hexPrefab, VectorTransform.FromSerializable(hexagons[i].Position), transform, HEX_TYPE.EMPTY);
            Hexagons[i] = hex;
            if (hexagons[i].HexType == HEX_TYPE.EMPTY)
            {
                AddGround(hex);
                PathGraph.AllowHexagon(hex.transform.position);

            }
            else if (hexagons[i].HexType == HEX_TYPE.SOIL)
            {
                AddBlock(hex);
                PathGraph.ProhibitHexagon(hex.transform.position);
            }
        }
    }

    void Update()
    {
        if (PathGraph != null)
        {
            DrawDebugSurface();
        }
    }

    private void DrawDebugSurface()
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


    public void StartJobExecution(Hexagon hex, Mob mob)
    {
        switch (mob.Job.Type)
        {
            case JobType.DIG:
                StartCoroutine(Dig(hex, (Worker)mob));
                break;
            case JobType.FILL:
                StartCoroutine(Fill(hex, (Worker)mob));
                break;
            case JobType.CARRYING:
                StartCoroutine(Carry(hex, (Worker)mob));
                break;
        }
    }


    public IEnumerator Carry(Hexagon hex, Worker worker)
    {
        yield return new WaitForSeconds(5f);
    }
    public IEnumerator Dig(Hexagon hex, Worker worker)
    {
        OldHexagons.Remove(hex.Id);
        hex.ClearHexagon();
        AddBlock(hex);
        while (true)
        {
            hex.Work -= worker.ConstructionSpeed;
            hex.transform.GetChild(0).localScale = Vector3.one * hex.Work / 50;
            if (hex.Work <= 0)
            {
                AddGround(hex);
                PathGraph.AllowHexagon(hex.transform.position);
                worker.Job.CancelJob();
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator Fill(Hexagon hex, Worker worker)
    {
        hex.ClearHexagon();
        OldHexagons.Remove(hex.Id);
        AddScaledBlock(hex);
        while (true)
        {
            hex.Work -= worker.ConstructionSpeed;
            hex.transform.GetChild(0).localScale = Vector3.one * (1 - hex.Work / 50f);
            if (hex.Work <= 0)
            {
                hex.ClearHexagon();
                AddBlock(hex);
                worker.Job.CancelJob();
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
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
        hex.Work = 50f;
        Instantiate(wallPrefab, hex.transform.position, Quaternion.identity, hex.transform);
    }
    public void AddScaledBlock(Hexagon hex)
    {
        hex.HexType = HEX_TYPE.SOIL;
        hex.Work = 50f;
        Instantiate(wallPrefabScaled, hex.transform.position, Quaternion.identity, hex.transform);
    }

    public void AddGround(Hexagon hex)
    {
        hex.HexType = HEX_TYPE.EMPTY;
        hex.Work = 50f;
        hex.ClearHexagon();
    }

    public bool IsInOldHexagons(Hexagon hex)
    {
        return OldHexagons.ContainsKey(hex.Id);
    }

    public void AddIcon(Hexagon hex)
    {
        var clonedHex = Instantiate(hex).AssignProperties(hex);
        OldHexagons.Add(clonedHex.Id, clonedHex);
        Hexagon prefab = null;
        if (hex.IsEmpty)
        {
            PathGraph.ProhibitHexagon(hex.transform.position);
            prefab = fillPrefab;
        }
        else if (hex.IsSoil)
        {
            prefab = digPrefab;
        }
        hex.ClearHexagon();
        Instantiate(prefab, hex.transform.position, Quaternion.identity, hex.transform).AssignProperties(clonedHex);
    }
    public void RemoveIcon(Hexagon hex)
    {
        var oldIcon = OldHexagons[hex.Id];
        if (oldIcon.IsEmpty)
        {
            PathGraph.AllowHexagon(hex.transform.position);
        }
        else if (oldIcon.IsSoil)
        {
            PathGraph.ProhibitHexagon(hex.transform.position);
        }
        hex.ClearHexagon();
        OldHexagons.Remove(hex.Id);
        Instantiate(oldIcon, hex.transform.position, Quaternion.identity, hex.transform);
    }

}