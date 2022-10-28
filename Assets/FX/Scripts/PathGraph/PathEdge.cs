public class Edge
{
    public PathVertex From;
    public PathVertex To;
    public float EdgeWeight;

    public bool IsWalkable;

    public Edge(PathVertex From, PathVertex To, float EdgeWeight)
    {
        this.From = From;
        this.To = To;
        this.EdgeWeight = EdgeWeight;
        this.IsWalkable = true;
    }
}