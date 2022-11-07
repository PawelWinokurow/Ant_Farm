using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using System.Linq;

public class Surface : MonoBehaviour
{
    public FloorHexagon HexPrefab;
    public WorkHexagon WallPrefab;
    public WorkHexagon WallDigScaled;
    public WorkHexagon WallFillScaled;
    public WorkHexagon DigPrefab;
    public WorkHexagon FillPrefab;
    public BaseHexagon BasePrefab;
    public CollectingHexagon FoodPrefab;
    public CollectingHexagon CarryingPrefab;
    private Camera cam;
    private int height;
    private int width;
    public Vector3 ld;
    public Vector3 rd;
    public Vector3 lu;
    public Vector3 ru;
    public float w;
    public float h;

    public FloorHexagon BaseHex { get; set; }
    public FloorHexagon[] Hexagons;

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
        // LoadHexagons(hexagons);
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
        Hexagons = new FloorHexagon[width * height];
    }

    public void GenerateHexagons()
    {
        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 hexPosition = new Vector3(w * (x + (z % 2f) / 2f), 0f, z * h);
                FloorHexagon hex = FloorHexagon.CreateHexagon($"{x}_{z}", HexPrefab, hexPosition, transform, HEX_TYPE.EMPTY, 50f);
                PathGraph.AddHexagonSubGraph(hex, Hexagon.Radius, $"x{x}z{z}");
                Hexagons[z * width + x] = hex;
            }
        }
        PathGraph.SetNeighbours();
    }

    private void SetCameraPositionToCenter()
    {
        Camera.main.transform.parent.position = new Vector3((width - 0.5f) * w / 2f, 0, (height - 1) * h / 2f * (1f - 0.09f));
    }

    public void SetBaseHex()
    {
        BaseHex = PositionToHex(Camera.main.transform.parent.position);
    }

    // void LoadHexagons(HexagonSerializable[] hexagons)
    // {
    //     for (var i = 0; i < hexagons.Length; i++)
    //     {
    //         var hex = FloorHexagon.CreateHexagon(hexagons[i].Id, HexPrefab, VectorTransform.FromSerializable(hexagons[i].Position), transform, HEX_TYPE.EMPTY, 50f);
    //         Hexagons[i] = hex;
    //         if (hexagons[i].Type == HEX_TYPE.EMPTY)
    //         {
    //             AddGround(hex);
    //             PathGraph.AllowHexagon(hex.transform.position);

    //         }
    //         else if (hexagons[i].Type == HEX_TYPE.SOIL)
    //         {
    //             AddBlock(hex);
    //             PathGraph.ProhibitHexagon(hex.transform.position);
    //         }
    //     }
    // }

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

    public FloorHexagon PositionToHex(Vector3 pos)
    {
        int z = (int)Mathf.Round(pos.z / h);
        int x = (int)Mathf.Round((pos.x - (z % 2) * 0.5f * w) / w);
        z = Mathf.Clamp(z, 0, height - 1);
        x = Mathf.Clamp(x, 0, width - 1);
        return Hexagons[z * width + x];
    }

    public WorkHexagon AddBlock(FloorHexagon hex)
    {
        var block = WorkHexagon.CreateHexagon(hex, WallPrefab);
        block.Type = HEX_TYPE.SOIL;
        block.Work = WorkHexagon.MaxWork;
        return block;
    }
    public WorkHexagon AddDigScaledBlock(FloorHexagon hex)
    {
        var scaledBlock = WorkHexagon.CreateHexagon(hex, WallDigScaled);
        scaledBlock.Type = HEX_TYPE.SOIL;
        scaledBlock.Work = WorkHexagon.MaxWork;
        return scaledBlock;
    }
    public WorkHexagon AddFillScaledBlock(FloorHexagon hex)
    {
        var scaledBlock = WorkHexagon.CreateHexagon(hex, WallFillScaled);
        scaledBlock.Type = HEX_TYPE.SOIL;
        scaledBlock.Work = WorkHexagon.MaxWork;
        return scaledBlock;
    }

    public void AddGround(FloorHexagon hex)
    {
        hex.RemoveChildren();
        hex.Type = HEX_TYPE.EMPTY;
        hex.Work = WorkHexagon.MaxWork;
    }

    public void AddFood(FloorHexagon hex)
    {
        hex.RemoveChildren();
        var collectingHex = CollectingHexagon.CreateHexagon(hex, FoodPrefab);
        collectingHex.Type = HEX_TYPE.FOOD;
    }

    public void AddBase()
    {
        BaseHex.Vertex.Neighbours.ForEach(vertex =>
        {
            PositionToHex(vertex.Position).RemoveChildren();
            vertex.Neighbours.ForEach(vertex =>
            {
                var hex = PositionToHex(vertex.Position);
                hex.RemoveChildren();
                PathGraph.AllowHexagon(hex);
            });
        });
        BaseHex.RemoveChildren();
        BaseHexagon.CreateHexagon(BaseHex, BasePrefab).Type = HEX_TYPE.BASE;
        PathGraph.ProhibitHexagon(BaseHex);
    }


    public void AddIcon(FloorHexagon hex)
    {
        if (hex.Type == HEX_TYPE.EMPTY)
        {
            var clonedHex = Instantiate(hex).AssignProperties(hex);
            OldHexagons.Add(clonedHex.Id, clonedHex);
            hex.RemoveChildren();
            PathGraph.ProhibitHexagon(hex);
            WorkHexagon.CreateHexagon(hex, FillPrefab);
        }
        else if (hex.Type == HEX_TYPE.SOIL)
        {
            WorkHexagon clonedHex = WorkHexagon.CreateHexagon(hex, WallPrefab);
            clonedHex.AssignProperties((WorkHexagon)hex.Child);
            OldHexagons.Add(clonedHex.Id, clonedHex);
            hex.RemoveChildren();
            WorkHexagon.CreateHexagon(hex, DigPrefab);
        }
        else if (hex.Type == HEX_TYPE.FOOD)
        {
            CollectingHexagon clonedHex = CollectingHexagon.CreateHexagon(hex, FoodPrefab);
            clonedHex.AssignProperties((CollectingHexagon)hex.Child);
            OldHexagons.Add(clonedHex.Id, clonedHex);
            hex.RemoveChildren();
            CollectingHexagon.CreateHexagon(hex, CarryingPrefab);
        };
    }
    public void RemoveIcon(FloorHexagon hex)
    {
        hex.RemoveChildren();
        var oldIcon = OldHexagons[hex.Id];
        if (oldIcon.Type == HEX_TYPE.EMPTY)
        {
            PathGraph.AllowHexagon(hex);
            hex.AssignProperties((FloorHexagon)oldIcon);
        }
        else if (oldIcon.Type == HEX_TYPE.SOIL)
        {
            PathGraph.ProhibitHexagon(hex);
            WorkHexagon.CreateHexagon(hex, WallPrefab).AssignProperties((WorkHexagon)oldIcon);
        }
        else if (oldIcon.Type == HEX_TYPE.FOOD)
        {
            CollectingHexagon.CreateHexagon(hex, FoodPrefab);
        };
        OldHexagons.Remove(hex.Id);
    }



}