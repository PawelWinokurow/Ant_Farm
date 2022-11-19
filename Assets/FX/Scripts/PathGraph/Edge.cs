public class Edge
{
    public string id;
    public Vertex from;
    public Vertex to;
    public int MULTIPLIER = 5;
    public float edgeWeight;
    public float edgeWeightBase;
    public float edgeWeightMod;
    public int accessMask;
    public FloorHexagon floorHexagon;

    public Edge(string id, Vertex from, Vertex to, float edgeWeight, FloorHexagon floorHexagon, int accessMask)
    {
        this.id = id;
        this.from = from;
        this.to = to;
        this.edgeWeight = edgeWeight;
        this.edgeWeightBase = edgeWeight;
        this.edgeWeightMod = edgeWeight * MULTIPLIER;
        this.floorHexagon = floorHexagon;
        this.accessMask = accessMask;
    }

    public Edge() { }

    public bool HasAccess(int accessMask)
    {
        return (this.accessMask & accessMask) == accessMask;
    }
}