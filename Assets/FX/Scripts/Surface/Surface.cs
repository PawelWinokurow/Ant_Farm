using System.Collections.Generic;
using UnityEngine;


public class Surface : MonoBehaviour
{
    public FloorHexagon hexPrefab;
    public WorkHexagon soilPrefab;
    public WorkHexagon stonePrefab;
    public WorkHexagon spikesPrefab;
    public WorkHexagon turretPrefab;
    public WorkHexagon holePrefab;

    public WorkHexagon soilMountIconPrefab;
    public WorkHexagon stoneMountIconPrefab;
    public WorkHexagon spikesMountIconPrefab;
    public WorkHexagon turretMountIconPrefab;
    public WorkHexagon demountPrefab;

    public BaseHexagon basePrefab;
    public CollectingHexagon foodPrefab;
    private Camera cam;
    public int height;
    public int width;
    public Vector3 ld;
    public Vector3 rd;
    public Vector3 lu;
    public Vector3 ru;
    public float w;
    public float h;

    public FloorHexagon baseHex { get; set; }
    public FloorHexagon[] hexagons;

    public Graph pathGraph;
    private Store store;
    public Dictionary<string, Hexagon> oldHexagons = new Dictionary<string, Hexagon>();
    public Vector3 center;
    private GameSettings gameSettings;
    private PriceSettings priceSettings;
    private ResourcesSettings resourcesSettings;
    public static Surface Instance { get; private set; }
    [HideInInspector] public Vector3[] sideHexagonsPos;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void Init(Graph pathGraph)
    {
        this.pathGraph = pathGraph;
        SetupCamera();
        CalculateSizes();
        Generatehexagons();
        SetCameraPositionToCenter();
        SetbaseHex();
        GetComponent<AddTextureToHex>().Init();
        InitSingletons();
    }

    public void Init(Graph pathGraph, HexagonSerializable[] hexagons)
    {
        this.pathGraph = pathGraph;
        SetupCamera();
        CalculateSizes();
        // Loadhexagons(hexagons);
        SetCameraPositionToCenter();
        SetbaseHex();
        InitSingletons();
    }

    private void InitSingletons()
    {
        gameSettings = Settings.Instance.gameSettings;
        priceSettings = Settings.Instance.priceSettings;
        resourcesSettings = Settings.Instance.resourcesSettings;
        store = Store.Instance;
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
        sideHexagonsPos = new Vector3[(width - 1) * 2 + (height - 1) * 2];
    }

    public void Generatehexagons()
    {
        int n = 0;
        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 hexPosition = new Vector3(w * (x + (z % 2f) / 2f), 0f, z * h);
                FloorHexagon hex = FloorHexagon.CreateHexagon($"{x}_{z}", hexPrefab, hexPosition, transform, HexType.EMPTY, 50f);
                pathGraph.AddHexagonSubGraph(hex, Hexagon.radius, $"{x}_{z}");
                hexagons[z * width + x] = hex;

                if (z == 0 || z == height - 1 || x == 0 || x == width - 1)
                {
                    sideHexagonsPos[n++] = hexPosition;
                }
            }
        }
        pathGraph.SetNeighbours();
    }

    private void SetCameraPositionToCenter()
    {
        center = new Vector3((width - 0.5f) * w / 2f, 0, (height - 1) * h / 2f);
        Camera.main.transform.parent.position = new Vector3((width - 0.5f) * w / 2f, 0, (height - 1) * h / 2f * (1f - 0.09f));
    }

    public void SetbaseHex()
    {
        baseHex = PositionToHex(center);
    }

    // void Loadhexagons(HexagonSerializable[] hexagons)
    // {
    //     for (var i = 0; i < hexagons.Length; i++)
    //     {
    //         var hex = FloorHexagon.CreateHexagon(hexagons[i].id, hexPrefab, VectorTransform.FromSerializable(hexagons[i].position), transform, HexType.EMPTY, 50f);
    //         hexagons[i] = hex;
    //         if (hexagons[i].type == HexType.EMPTY)
    //         {
    //             AddGround(hex);
    //             pathGraph.AllowHexagon(hex.transform.position);

    //         }
    //         else if (hexagons[i].type == HexType.SOIL)
    //         {
    //             AddSoil(hex);
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
            if (edge.HasAccess(Settings.Instance.gameSettings.ACCESS_MASK_FLOOR))
            {
                Debug.DrawLine(edge.from.position, edge.to.position, Color.green);
            }
            else if (edge.HasAccess(Settings.Instance.gameSettings.ACCESS_MASK_SOIL))
            {
                Debug.DrawLine(edge.from.position, edge.to.position, Color.red);
            }
            else if (edge.HasAccess(Settings.Instance.gameSettings.ACCESS_MASK_BASE))
            {
                Debug.DrawLine(edge.from.position, edge.to.position, Color.magenta);
            }
            else if (edge.HasAccess(Settings.Instance.gameSettings.ACCESS_MASK_PROHIBIT))
            {
                Debug.DrawLine(edge.from.position, edge.to.position, Color.black);
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

    public WorkHexagon AddSoil(FloorHexagon hex)
    {
        var block = WorkHexagon.CreateHexagon(hex, soilPrefab);
        block.type = HexType.SOIL;
        block.work = WorkHexagon.MAX_WORK;
        return block;
    }

    public WorkHexagon AddBlock(FloorHexagon hex, HexType type)
    {
        WorkHexagon blockPrefab = GetBlockPrefabByType(type);
        var block = WorkHexagon.CreateHexagon(hex, blockPrefab);
        if (type == HexType.TURRET)
        {
            store.AddAlly((Targetable)block.GetComponent<Trap>());
        }
        block.type = type;
        block.work = WorkHexagon.MAX_WORK;
        return block;
    }

    private WorkHexagon GetBlockPrefabByType(HexType type)
    {
        WorkHexagon blockPrefab = null;
        if (type == HexType.SOIL) blockPrefab = soilPrefab;
        else if (type == HexType.STONE) blockPrefab = stonePrefab;
        else if (type == HexType.SPIKES) blockPrefab = spikesPrefab;
        else if (type == HexType.TURRET) blockPrefab = turretPrefab;
        else if (type == HexType.HOLE) blockPrefab = holePrefab;
        return blockPrefab;
    }

    public void ClearHex(FloorHexagon hex)
    {
        hex.RemoveChildren();
        oldHexagons.Remove(hex.id);
    }

    public void AddGround(FloorHexagon hex)
    {
        hex.type = HexType.EMPTY;
        hex.work = WorkHexagon.MAX_WORK;
    }

    public void AddFood(FloorHexagon hex)
    {
        hex.RemoveChildren();
        var collectingHex = CollectingHexagon.CreateHexagon(hex, foodPrefab, resourcesSettings.FOOD_MAX_AMOUNT);
        collectingHex.type = HexType.FOOD;
    }

    public void AddBase()
    {
        baseHex.vertex.neighbours.ForEach(vertex =>
        {
            vertex.floorHexagon.RemoveChildren();
            vertex.floorHexagon.type = HexType.EMPTY;
            pathGraph.SetAccesabillity(vertex.floorHexagon, gameSettings.ACCESS_MASK_FLOOR, gameSettings.EDGE_WEIGHT_NORMAL);
        });
        baseHex.RemoveChildren();
        BaseHexagon.CreateHexagon(baseHex, basePrefab).type = HexType.BASE;
        pathGraph.SetAccesabillity(baseHex, gameSettings.ACCESS_MASK_PROHIBIT, gameSettings.EDGE_WEIGHT_NORMAL);
    }

    public void PlaceIcon(FloorHexagon hex, SliderValue value)
    {
        if (value == SliderValue.SOIL)
        {
            PlaceMountIcon(hex, soilMountIconPrefab, gameSettings.ACCESS_MASK_SOIL, gameSettings.EDGE_WEIGHT_OBSTACLE);
        }
        else if (value == SliderValue.STONE)
        {
            PlaceMountIcon(hex, stoneMountIconPrefab, gameSettings.ACCESS_MASK_STONE, gameSettings.EDGE_WEIGHT_OBSTACLE);
        }
        else if (value == SliderValue.SPIKES)
        {
            PlaceMountIcon(hex, spikesMountIconPrefab, gameSettings.ACCESS_MASK_FLOOR, gameSettings.EDGE_WEIGHT_NORMAL);
        }
        else if (value == SliderValue.TURRET)
        {
            PlaceMountIcon(hex, turretMountIconPrefab, gameSettings.ACCESS_MASK_STONE, gameSettings.EDGE_WEIGHT_OBSTACLE);
        }
        if (value == SliderValue.DEMOUNT)
        {
            PlaceDemountIcon(hex, demountPrefab);
        }
    }


    private void PlaceMountIcon(FloorHexagon hex, WorkHexagon prefab, int accessMask, int edgeMultiplier)
    {
        var clonedHex = Instantiate(hex).AssignProperties(hex);
        clonedHex.gameObject.SetActive(false);
        oldHexagons.Add(clonedHex.id, clonedHex);
        hex.RemoveChildren();
        pathGraph.SetAccesabillity(hex, accessMask, edgeMultiplier);
        WorkHexagon.CreateHexagon(hex, prefab);
    }

    private void PlaceDemountIcon(FloorHexagon hex, WorkHexagon prefab)
    {
        WorkHexagon clonedHex = WorkHexagon.CreateHexagon(hex, GetBlockPrefabByType(hex.type));
        clonedHex.AssignProperties((WorkHexagon)hex.child);
        oldHexagons.Add(clonedHex.id, clonedHex);
        hex.RemoveChildren();
        WorkHexagon hexNew = WorkHexagon.CreateHexagon(hex, GetBlockPrefabByType(hex.type));
        hexNew.AssignProperties(clonedHex);
        hexNew.transform.localScale = 0.95f * Vector3.one;
        Instantiate(prefab, hexNew.position, Quaternion.identity, hexNew.transform);
        pathGraph.SetAccesabillity(hex, GetAccessMaskByHexType(hex.type), GetEdgeWeightByHexType(hex.type));

        WorkHexagon[] workHexagons = hex.GetComponentsInChildren<WorkHexagon>();
        for (int i = 0; i < workHexagons.Length; i++)
        {
            if (workHexagons[i].isGroundMat == true)
            {
                workHexagons[i].GetComponent<MeshRenderer>().sharedMaterial = hex.mr.sharedMaterial;
            }
        }
    }

    public int GetAccessMaskByHexType(HexType type)
    {
        if (type == HexType.SOIL) return gameSettings.ACCESS_MASK_SOIL;
        else if (type == HexType.STONE) return gameSettings.ACCESS_MASK_STONE;
        else if (type == HexType.SPIKES) return gameSettings.ACCESS_MASK_FLOOR;
        else if (type == HexType.TURRET) return gameSettings.ACCESS_MASK_STONE;
        return gameSettings.ACCESS_MASK_FLOOR;
    }

    public HexType GetHexTypeByIcon(FloorHexagon floorHexagon)
    {
        var tag = ((WorkHexagon)(floorHexagon.child)).tag;
        if (tag == "Soil_Mount_Icon") return HexType.SOIL;
        else if (tag == "Stone_Mount_Icon") return HexType.STONE;
        else if (tag == "Spikes_Mount_Icon") return HexType.SPIKES;
        else if (tag == "Turret_Mount_Icon") return HexType.TURRET;
        return HexType.EMPTY;
    }
    public int GetEdgeWeightByHexType(HexType type)
    {
        if (type == HexType.SOIL) return gameSettings.EDGE_WEIGHT_OBSTACLE;
        else if (type == HexType.STONE) return gameSettings.EDGE_WEIGHT_OBSTACLE;
        else if (type == HexType.SPIKES) return gameSettings.EDGE_WEIGHT_NORMAL;
        else if (type == HexType.TURRET) return gameSettings.EDGE_WEIGHT_OBSTACLE;
        return gameSettings.EDGE_WEIGHT_NORMAL;
    }

    public void RemoveIcon(FloorHexagon hex)
    {
        hex.RemoveChildren();
        var oldIcon = oldHexagons[hex.id];

        if (oldIcon.type == HexType.EMPTY)
        {
            pathGraph.SetAccesabillity(hex, gameSettings.ACCESS_MASK_FLOOR, gameSettings.EDGE_WEIGHT_NORMAL);
            hex.AssignProperties((FloorHexagon)oldIcon);
        }
        else if (oldIcon.type == HexType.SOIL)
        {
            pathGraph.SetAccesabillity(hex, gameSettings.ACCESS_MASK_SOIL, gameSettings.EDGE_WEIGHT_OBSTACLE);
            WorkHexagon.CreateHexagon(hex, soilPrefab).AssignProperties((WorkHexagon)oldIcon);
        }
        else if (oldIcon.type == HexType.STONE)
        {
            pathGraph.SetAccesabillity(hex, gameSettings.ACCESS_MASK_STONE, gameSettings.EDGE_WEIGHT_OBSTACLE);
            WorkHexagon.CreateHexagon(hex, stonePrefab).AssignProperties((WorkHexagon)oldIcon);
        }
        else if (oldIcon.type == HexType.SPIKES)
        {
            pathGraph.SetAccesabillity(hex, gameSettings.ACCESS_MASK_FLOOR, gameSettings.EDGE_WEIGHT_NORMAL);
            WorkHexagon.CreateHexagon(hex, spikesPrefab).AssignProperties((WorkHexagon)oldIcon);
        }
        else if (oldIcon.type == HexType.TURRET)
        {
            pathGraph.SetAccesabillity(hex, gameSettings.ACCESS_MASK_STONE, gameSettings.EDGE_WEIGHT_OBSTACLE);
            WorkHexagon.CreateHexagon(hex, turretPrefab).AssignProperties((WorkHexagon)oldIcon);
        }
        oldHexagons.Remove(hex.id);
        Destroy((FloorHexagon)oldIcon);
    }

    public bool IsInOldHexagons(FloorHexagon hex)
    {
        return oldHexagons.ContainsKey(hex.id);
    }



}