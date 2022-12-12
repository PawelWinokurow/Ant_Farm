public class Edge
{
    public string id;
    public Vertex from;
    public Vertex to;
    public float edgeWeight;
    public int edgeMultiplier;
    public int accessMask;
    public FloorHexagon floorHexagon;

    public Edge(string id, Vertex from, Vertex to, float edgeWeight, FloorHexagon floorHexagon, int accessMask, int edgeMultiplier)
    {
        this.id = id;
        this.from = from;
        this.to = to;
        this.edgeWeight = edgeWeight;
        this.edgeMultiplier = edgeMultiplier;
        this.floorHexagon = floorHexagon;
        this.accessMask = accessMask;
    }

    public Edge() { }

    public bool HasAccess(int accessMask)
    {
        return (this.accessMask & accessMask) == this.accessMask;
    }
}