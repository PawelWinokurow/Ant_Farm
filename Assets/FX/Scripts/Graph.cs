using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DataStructures.PriorityQueue;

public class PathVertex
{
    public Vector3 GeometricalPoint;
    public string Id;
    public List<Edge> Edges { get; set; }

    public float PathWeight;

    public PathVertex(string Id, Vector3 GeometricalPoint)
    {
        this.GeometricalPoint = GeometricalPoint;
        PathWeight = float.MaxValue;
        Edges = new List<Edge>();
        this.Id = Id;
    }

    public void ResetPathWeight()
    {
        PathWeight = float.MaxValue;
    }

}
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

public class Path
{
    public List<Vector3> WayPoints = new List<Vector3>();
    public float OverallDistance = 0;
}

public class Graph
{
    public Dictionary<Vector3, PathVertex> PathVerticesMap;
    public List<Edge> AdjacencyList { get; set; }
    public List<PathVertex> PathVertices { get; set; }
    public Graph()
    {
        AdjacencyList = new List<Edge>();
        PathVertices = new List<PathVertex>();
        PathVerticesMap = new Dictionary<Vector3, PathVertex>();
    }

    public void ResetPathWeights()
    {
        PathVertices.ForEach(pathVerex => pathVerex.ResetPathWeight());
    }

    public PathVertex GetVertexByPoistion(Vector3 position)
    {
        return PathVerticesMap.GetValueOrDefault(position);
    }

    public PathVertex AddVertex(PathVertex newVertex)
    {
        int index = PathVertices.FindIndex(vertex => Vector3.Distance(vertex.GeometricalPoint, newVertex.GeometricalPoint) < 0.1f ? true : false);
        if (index >= 0) return PathVertices[index];
        PathVertices.Add(newVertex);
        PathVerticesMap.Add(newVertex.GeometricalPoint, newVertex);
        return newVertex;
    }

    public void AddEdge(PathVertex v1, PathVertex v2, float edgeWeight)
    {
        Edge v1Edge = new Edge(v1, v2, edgeWeight);
        Edge v2Edge = new Edge(v2, v1, edgeWeight);
        v1.Edges.Add(v1Edge);
        v2.Edges.Add(v2Edge);
        AdjacencyList.Add(v1Edge);
        AdjacencyList.Add(v2Edge);
    }

    public void RemoveEdge(PathVertex v1, PathVertex v2)
    {
        int index = AdjacencyList.FindIndex(e => e.From.Id == v1.Id && e.To.Id == v2.Id);
        if (index >= 0) AdjacencyList.RemoveAt(index);
        index = AdjacencyList.FindIndex(e => e.From.Id == v2.Id && e.To.Id == v1.Id);
        if (index >= 0) AdjacencyList.RemoveAt(index);

        v1.Edges.FindIndex(e => e.To.Id == v2.Id);
        if (index >= 0) v1.Edges.RemoveAt(index);

        v2.Edges.FindIndex(e => e.To.Id == v1.Id);
        if (index >= 0) v2.Edges.RemoveAt(index);
    }


    public void AllowHexagon(Vector3 hexagonPosition)
    {
        GetHexagonEdges(hexagonPosition).ForEach(edge => edge.IsWalkable = true);
    }
    public void ProhibitHexagon(Vector3 hexagonPosition)
    {
        GetHexagonEdges(hexagonPosition).ForEach(edge => edge.IsWalkable = false);
    }

    private List<Edge> GetHexagonEdges(Vector3 hexagonPosition)
    {
        PathVertex centerVertex = PathVerticesMap[hexagonPosition];
        List<PathVertex> hexagonVertices = centerVertex.Edges.Select(edge => edge.To).ToList();
        hexagonVertices.Add(centerVertex);
        var Ids = hexagonVertices.Select(vertex => vertex.Id);
        return hexagonVertices.Aggregate(new List<Edge>(), (acc, vertex) => acc.Concat(vertex.Edges).ToList()).Where(edge => Ids.Contains(edge.From.Id) && Ids.Contains(edge.To.Id)).ToList();
    }

    public PathVertex NearestVertex(Vector3 position)
    {
        if (PathVerticesMap.ContainsKey(position)) return PathVerticesMap[position];
        return PathVertices.Aggregate(PathVertices[0], (acc, vertex) =>
        {
            if (Vector3.Distance(vertex.GeometricalPoint, position) <
            Vector3.Distance(acc.GeometricalPoint, position)) return vertex;
            else return acc;
        });
    }

    public List<PathVertex> GetNeighbors(PathVertex center)
    {
        return center.Edges.Where(edge => edge.IsWalkable).Select(edge => edge.To).ToList();
    }


    public Path FindPath(Vector3 fromVec, Vector3 toVec)
    {
        PathVertex from = NearestVertex(fromVec);
        PathVertex to = NearestVertex(toVec);
        var neighbors = GetNeighbors(to);
        var paths = neighbors.Select(vertex => Astar(from, vertex)).Where(path => path != null).ToList();
        if (paths.Count() != 0)
        {
            var path = paths.Aggregate(paths[0], (acc, p) =>
            {
                {
                    if (p.OverallDistance < acc.OverallDistance) return p;
                    else return acc;
                }

            });
            return path;
        }
        return null;
    }

    private Path Astar(PathVertex start, PathVertex goal)
    {
        PriorityQueue<PathVertex, float> frontier = new PriorityQueue<PathVertex, float>(0);
        frontier.Insert(start, 0);

        var cameFrom = new Dictionary<string, PathVertex>();
        var costSoFar = new Dictionary<string, float>();

        cameFrom[start.Id] = null;
        costSoFar[start.Id] = 0;
        var isGoalFound = false;
        while (frontier.Top() != null)
        {
            var current = frontier.Pop();
            if (current == goal)
            {
                isGoalFound = true;
                break;
            }

            foreach (var nextEdge in current.Edges)
            {
                if (!nextEdge.IsWalkable) continue;
                var next = nextEdge.To;
                var newCost = costSoFar[current.Id] + nextEdge.EdgeWeight;
                if (!costSoFar.ContainsKey(next.Id) || newCost < costSoFar[next.Id])
                {
                    costSoFar[next.Id] = newCost;
                    var priority = newCost + GetDistance(next, goal);
                    frontier.Insert(next, priority);
                    cameFrom[next.Id] = current;
                }
            }
        }
        if (isGoalFound)
        {
            var path = new Path();
            path.OverallDistance = costSoFar[goal.Id];
            var next = cameFrom[goal.Id];
            while (next != null)
            {
                path.WayPoints.Add(next.GeometricalPoint);
                next = cameFrom[next.Id];
            }
            path.WayPoints.Reverse();
            path.WayPoints = Utils.NormalizePath(path.WayPoints, 1f);
            path.OverallDistance = costSoFar[goal.Id];
            return path;
        }
        else
        {
            return null;
        }
    }

    private static float GetDistance(PathVertex from, PathVertex to)
    {
        return Vector3.Distance(from.GeometricalPoint, to.GeometricalPoint);
    }

}





