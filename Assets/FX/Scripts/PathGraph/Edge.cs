public class Edge
{
    public string Id;
    public Vertex From;
    public Vertex To;
    public float EdgeWeight;
    public bool IsWalkable;

    public Edge(string id, Vertex from, Vertex to, float edgeWeight, bool isWalkable = true)
    {
        Id = id;
        From = from;
        To = to;
        EdgeWeight = edgeWeight;
        IsWalkable = isWalkable;
        IsWalkable = isWalkable;
    }
    public Edge() { }
}