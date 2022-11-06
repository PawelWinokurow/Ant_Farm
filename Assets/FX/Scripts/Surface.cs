using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class Surface : MonoBehaviour
{
    public FloorHexagon hexPrefab;
    public WorkHexagon wallPrefab;
    public WorkHexagon wallPrefabScaled;
    public WorkHexagon digPrefab;
    public WorkHexagon fillPrefab;
    public BaseHexagon basePrefab;
    public FoodHexagon foodPrefab;
    public FoodHexagon carryingPrefab;
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
                PathGraph.AddHexagonSubGraph(hexPosition, Hexagon.Radius, $"x{x}z{z}");
                FloorHexagon hex = FloorHexagon.CreateHexagon($"{x}_{z}", hexPrefab, hexPosition, transform, HEX_TYPE.EMPTY, 50f);
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
    //         var hex = FloorHexagon.CreateHexagon(hexagons[i].Id, hexPrefab, VectorTransform.FromSerializable(hexagons[i].Position), transform, HEX_TYPE.EMPTY, 50f);
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


    public void StartJobExecution(FloorHexagon hex, Mob mob)
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


    public IEnumerator Carry(FloorHexagon hex, Worker worker)
    {
        yield return new WaitForSeconds(2f);
        if (Distance.Manhattan(worker.Job.Destination, BaseHex.Position) < 1)
        {
            //On base
            var baseHex = (BaseHexagon)(BaseHex.Child);
            baseHex.Storage += worker.CarryingWeight;
            Debug.Log("Storage: " + baseHex.Storage);
            worker.CarryingWeight = 0;
            if (hex.Type == HEX_TYPE.FOOD && ((FoodHexagon)(hex.Child)).Food > 0)
            {
                worker.Job.Return();
            }
            else
            {
                worker.Job.CancelJob();
            }

        }
        else
        {
            //On food
            var foodHex = (FoodHexagon)(hex.Child);
            var workerCanCarry = worker.MaxCarryingWeight - worker.CarryingWeight;
            if (workerCanCarry > foodHex.Food)
            {
                worker.CarryingWeight = foodHex.Food;
                foodHex.Food = 0;
                //Cancel all workers
                foodHex.Carriers.ForEach(worker => worker.Job.CancelJob());
            }
            else
            {
                worker.CarryingWeight = workerCanCarry;
                foodHex.Food -= workerCanCarry;
                worker.Job.Return();
            }
            Debug.Log("Food: " + foodHex.Food);
        }
    }
    public IEnumerator Dig(FloorHexagon hex, Worker worker)
    {
        OldHexagons.Remove(hex.Id);
        hex.RemoveChildren();
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

    public IEnumerator Fill(FloorHexagon hex, Worker worker)
    {
        hex.RemoveChildren();
        OldHexagons.Remove(hex.Id);
        AddScaledBlock(hex);
        while (true)
        {
            hex.Work -= worker.ConstructionSpeed;
            hex.transform.GetChild(0).localScale = Vector3.one * (1 - hex.Work / 50f);
            if (hex.Work <= 0)
            {
                hex.RemoveChildren();
                AddBlock(hex);
                worker.Job.CancelJob();
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public FloorHexagon PositionToHex(Vector3 pos)
    {
        int z = (int)Mathf.Round(pos.z / h);
        int x = (int)Mathf.Round((pos.x - (z % 2) * 0.5f * w) / w);
        z = Mathf.Clamp(z, 0, height - 1);
        x = Mathf.Clamp(x, 0, width - 1);
        return Hexagons[z * width + x];
    }

    public void AddBlock(FloorHexagon hex)
    {
        WorkHexagon.CreateHexagon(hex, wallPrefab).Type = HEX_TYPE.SOIL;
    }
    public void AddScaledBlock(FloorHexagon hex)
    {
        WorkHexagon.CreateHexagon(hex, wallPrefabScaled).Type = HEX_TYPE.SOIL;
    }

    public void AddGround(FloorHexagon hex)
    {
        hex.RemoveChildren();
        hex.Type = HEX_TYPE.EMPTY;
        hex.Work = 50f;
    }

    public void AddBase()
    {
        BaseHex.RemoveChildren();
        BaseHexagon.CreateHexagon(BaseHex, basePrefab).Type = HEX_TYPE.BASE;
        PathGraph.ProhibitHexagon(BaseHex.Position);
    }
    public void AddFood(FloorHexagon hex)
    {
        hex.RemoveChildren();
        FoodHexagon.CreateHexagon(hex, foodPrefab).Type = HEX_TYPE.FOOD; ;
        PathGraph.ProhibitHexagon(BaseHex.Position);
    }

    public bool IsInOldHexagons(FloorHexagon hex)
    {
        return OldHexagons.ContainsKey(hex.Id);
    }

    public void AddIcon(FloorHexagon hex)
    {
        if (hex.Type == HEX_TYPE.EMPTY)
        {
            var clonedHex = Instantiate(hex).AssignProperties(hex);
            OldHexagons.Add(clonedHex.Id, clonedHex);
            hex.RemoveChildren();
            PathGraph.ProhibitHexagon(hex.transform.position);
            WorkHexagon.CreateHexagon(hex, fillPrefab);
        }
        else if (hex.Type == HEX_TYPE.SOIL)
        {
            WorkHexagon clonedHex = WorkHexagon.CreateHexagon(hex, wallPrefab);
            clonedHex.AssignProperties((WorkHexagon)hex.Child);
            OldHexagons.Add(clonedHex.Id, clonedHex);
            hex.RemoveChildren();
            WorkHexagon.CreateHexagon(hex, digPrefab);
        }
        else if (hex.Type == HEX_TYPE.FOOD)
        {
            FoodHexagon clonedHex = FoodHexagon.CreateHexagon(hex, foodPrefab);
            clonedHex.AssignProperties((FoodHexagon)hex.Child);
            OldHexagons.Add(clonedHex.Id, clonedHex);
            hex.RemoveChildren();
            FoodHexagon.CreateHexagon(hex, carryingPrefab);
        };
    }
    public void RemoveIcon(FloorHexagon hex)
    {
        hex.RemoveChildren();
        var oldIcon = OldHexagons[hex.Id];
        if (oldIcon.Type == HEX_TYPE.EMPTY)
        {
            PathGraph.AllowHexagon(hex.transform.position);
            hex.AssignProperties((FloorHexagon)oldIcon);
        }
        else if (oldIcon.Type == HEX_TYPE.SOIL)
        {
            PathGraph.ProhibitHexagon(hex.transform.position);
            WorkHexagon.CreateHexagon(hex, wallPrefab).AssignProperties((WorkHexagon)oldIcon);
        }
        else if (oldIcon.Type == HEX_TYPE.FOOD)
        {
            FoodHexagon.CreateHexagon(hex, foodPrefab);
        };
        OldHexagons.Remove(hex.Id);
    }

}