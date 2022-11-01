public class PathEdge
{
    public PathVertex From;
    public PathVertex To;
    public float EdgeWeight;
    public bool IsWalkable;

    public PathEdge(PathVertex From, PathVertex To, float EdgeWeight)
    {
        this.From = From;
        this.To = To;
        this.EdgeWeight = EdgeWeight;
        this.IsWalkable = true;
    }
}