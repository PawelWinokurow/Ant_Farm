public class Edge
{
    public string id;
    public Vertex from;
    public Vertex to;
    public float edgeWeight;
    public int accessMask;
    public bool isWalkable;
    public FloorHexagon floorHexagon;

    public Edge(string id, Vertex from, Vertex to, float edgeWeight, FloorHexagon floorHexagon, int accessMask, bool isWalkable = true)
    {
        this.id = id;
        this.from = from;
        this.to = to;
        this.edgeWeight = edgeWeight;
        this.isWalkable = isWalkable;
        this.floorHexagon = floorHexagon;
        this.accessMask = accessMask;
    }
    public Edge() { }
}