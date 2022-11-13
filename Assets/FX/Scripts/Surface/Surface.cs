using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using System.Linq;

public class Surface : MonoBehaviour
{
    public FloorHexagon hexPrefab;
    public WorkHexagon wallPrefab;
    public WorkHexagon wallDigScaled;
    public WorkHexagon wallFillScaled;
    public WorkHexagon digPrefab;
    public WorkHexagon fillPrefab;
    public BaseHexagon basePrefab;
    public CollectingHexagon foodPrefab;
    public CollectingHexagon carryingPrefab;
    private Camera cam;
    private int height;
    private int width;
    public Vector3 ld;
    public Vector3 rd;
    public Vector3 lu;
    public Vector3 ru;
    public float w;
    public float h;

    public FloorHexagon baseHex { get; set; }
    public FloorHexagon[] hexagons;

    public Graph pathGraph;

    public Dictionary<string, Hexagon> oldhexagons = new Dictionary<string, Hexagon>();

    public void Init(Graph pathGraph)
    {
        this.pathGraph = pathGraph;
        SetupCamera();
        CalculateSizes();
        Generatehexagons();
        SetCameraPositionToCenter();
        SetbaseHex();
    }

    public void Init(Graph pathGraph, HexagonSerializable[] hexagons)
    {
        this.pathGraph = pathGraph;
        SetupCamera();
        CalculateSizes();
        // Loadhexagons(hexagons);
        SetCameraPositionToCenter();
        SetbaseHex();
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
        w = Mathf.Sin((60) * Mathf.Deg2Rad) * 2f * Hexagon.radius;//растояние по горизонтали между шестигольниками
        h = 2 * Hexagon.radius * 3f / 4f;//растояние по вертикали между шестигольниками
        height = Mathf.CeilToInt((lu.z - ld.z) / h) - 1;//находим количество шестиугольников в ширину и длину
        width = Mathf.CeilToInt((rd.x - ld.x) / w) - 1;
        hexagons = new FloorHexagon[width * height];
    }

    public void Generatehexagons()
    {
        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 hexPosition = new Vector3(w * (x + (z % 2f) / 2f), 0f, z * h);
                FloorHexagon hex = FloorHexagon.CreateHexagon($"{x}_{z}", hexPrefab, hexPosition, transform, HEX_TYPE.EMPTY, 50f);
                pathGraph.AddHexagonSubGraph(hex, Hexagon.radius, $"{x}_{z}");
                hexagons[z * width + x] = hex;
            }
        }
        pathGraph.SetNeighbours();
    }

    private void SetCameraPositionToCenter()
    {
        Camera.main.transform.parent.position = new Vector3((width - 0.5f) * w / 2f, 0, (height - 1) * h / 2f * (1f - 0.09f));
    }

    public void SetbaseHex()
    {
        baseHex = PositionToHex(Camera.main.transform.parent.position);
    }

    // void Loadhexagons(HexagonSerializable[] hexagons)
    // {
    //     for (var i = 0; i < hexagons.Length; i++)
    //     {
    //         var hex = FloorHexagon.CreateHexagon(hexagons[i].id, hexPrefab, VectorTransform.FromSerializable(hexagons[i].position), transform, HEX_TYPE.EMPTY, 50f);
    //         hexagons[i] = hex;
    //         if (hexagons[i].type == HEX_TYPE.EMPTY)
    //         {
    //             AddGround(hex);
    //             pathGraph.AllowHexagon(hex.transform.position);

    //         }
    //         else if (hexagons[i].type == HEX_TYPE.SOIL)
    //         {
    //             AddBlock(hex);
    //             pathGraph.ProhibitHexagon(hex.transform.position);
    //         }
    //     }
    // }

    void Update()
    {
        if (pathGraph != null)
        {
            DrawDebugSurface();
        }
    }

    private void DrawDebugSurface()
    {
        pathGraph.adjacencyList.ForEach(edge =>
        {
            if (!edge.isWalkable)
            {
                Debug.DrawLine(edge.from.position, edge.to.position, Color.red);
            }
            else
            {
                Debug.DrawLine(edge.from.position, edge.to.position, Color.green);
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
        return hexagons[z * width + x];
    }

    public WorkHexagon AddBlock(FloorHexagon hex)
    {
        var block = WorkHexagon.CreateHexagon(hex, wallPrefab);
        block.type = HEX_TYPE.SOIL;
        block.work = WorkHexagon.MAX_WORK;
        return block;
    }
    public WorkHexagon AddDigScaledBlock(FloorHexagon hex)
    {
        var scaledBlock = WorkHexagon.CreateHexagon(hex, wallDigScaled);
        scaledBlock.type = HEX_TYPE.SOIL;
        scaledBlock.work = WorkHexagon.MAX_WORK;
        return scaledBlock;
    }
    public WorkHexagon AddFillScaledBlock(FloorHexagon hex)
    {
        var scaledBlock = WorkHexagon.CreateHexagon(hex, wallFillScaled);
        scaledBlock.type = HEX_TYPE.SOIL;
        scaledBlock.work = WorkHexagon.MAX_WORK;
        return scaledBlock;
    }

    public void AddGround(FloorHexagon hex)
    {
        hex.RemoveChildren();
        hex.type = HEX_TYPE.EMPTY;
        hex.work = WorkHexagon.MAX_WORK;
    }

    public void AddFood(FloorHexagon hex)
    {
        hex.RemoveChildren();
        var collectingHex = CollectingHexagon.CreateHexagon(hex, foodPrefab);
        collectingHex.type = HEX_TYPE.FOOD;
    }

    public void AddBase()
    {
        baseHex.vertex.neighbours.ForEach(vertex =>
        {
            PositionToHex(vertex.position).RemoveChildren();
            vertex.neighbours.ForEach(vertex =>
            {
                var hex = PositionToHex(vertex.position);
                hex.RemoveChildren();
                pathGraph.AllowHexagon(hex);
            });
        });
        baseHex.RemoveChildren();
        BaseHexagon.CreateHexagon(baseHex, basePrefab).type = HEX_TYPE.BASE;
        pathGraph.ProhibitHexagon(baseHex);
    }


    public void AddIcon(FloorHexagon hex)
    {
        if (hex.type == HEX_TYPE.EMPTY)
        {
            var clonedHex = Instantiate(hex).AssignProperties(hex);
            oldhexagons.Add(clonedHex.id, clonedHex);
            hex.RemoveChildren();
            pathGraph.ProhibitHexagon(hex);
            WorkHexagon.CreateHexagon(hex, fillPrefab);
        }
        else if (hex.type == HEX_TYPE.SOIL)
        {
            WorkHexagon clonedHex = WorkHexagon.CreateHexagon(hex, wallPrefab);
            clonedHex.AssignProperties((WorkHexagon)hex.child);
            oldhexagons.Add(clonedHex.id, clonedHex);
            hex.RemoveChildren();
            WorkHexagon.CreateHexagon(hex, digPrefab);
        }
        else if (hex.type == HEX_TYPE.FOOD)
        {
            CollectingHexagon clonedHex = CollectingHexagon.CreateHexagon(hex, foodPrefab);
            clonedHex.AssignProperties((CollectingHexagon)hex.child);
            oldhexagons.Add(clonedHex.id, clonedHex);
            hex.RemoveChildren();
            CollectingHexagon.CreateHexagon(hex, carryingPrefab);
        };
    }
    public void RemoveIcon(FloorHexagon hex)
    {
        hex.RemoveChildren();
        var oldIcon = oldhexagons[hex.id];
        if (oldIcon.type == HEX_TYPE.EMPTY)
        {
            pathGraph.AllowHexagon(hex);
            hex.AssignProperties((FloorHexagon)oldIcon);
        }
        else if (oldIcon.type == HEX_TYPE.SOIL)
        {
            pathGraph.ProhibitHexagon(hex);
            WorkHexagon.CreateHexagon(hex, wallPrefab).AssignProperties((WorkHexagon)oldIcon);
        }
        else if (oldIcon.type == HEX_TYPE.FOOD)
        {
            CollectingHexagon.CreateHexagon(hex, foodPrefab);
        };
        oldhexagons.Remove(hex.id);
    }



}