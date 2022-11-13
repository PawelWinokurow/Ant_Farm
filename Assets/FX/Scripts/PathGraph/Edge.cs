public class Edge
{
    public string id;
    public Vertex from;
    public Vertex to;
    public float edgeWeight;
    public bool isWalkable;
    public FloorHexagon floorHexagon;

    public Edge(string id, Vertex from, Vertex to, float edgeWeight, FloorHexagon hex, bool isWalkable = true)
    {
        this.id = id;
        this.from = from;
        this.to = to;
        this.edgeWeight = edgeWeight;
        this.isWalkable = isWalkable;
        this.floorHexagon = hex;
    }
    public Edge() { }
}