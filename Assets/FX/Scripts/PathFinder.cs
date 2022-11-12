using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PriorityQueue;


public class Path
{
    public List<Edge> WayPoints = new List<Edge>();
    public float Length = 0;
    public bool HasWaypoints { get => WayPoints.Count != 0; }

}

public class Pathfinder
{

    private Graph pathGraph { get; set; }

    public Pathfinder(Graph pathGraph)
    {
        this.pathGraph = pathGraph;
    }

    public Path RandomWalk(Vector3 fromVec, int numberOfSteps)
    {
        Path overallPath = new Path();
        Vertex from = pathGraph.NearestVertex(fromVec);
        for (int i = 0; i < numberOfSteps; i++)
        {
            var path = PathToSomeNeighbour(from);
            if (path != null)
            {
                overallPath.WayPoints.AddRange(path.WayPoints);
                overallPath.Length += path.Length;
            }
            else
            {
                //TODO teleport
            }
            from = path.WayPoints[path.WayPoints.Count - 1].To;
        }
        return overallPath;
    }

    private Path PathToSomeNeighbour(Vertex from)
    {
        var neighbours = from.Neighbours.OrderBy(a => Guid.NewGuid()).ToList();
        foreach (var to in neighbours)
        {
            if (to.Edges[0].IsWalkable)
            {
                return FindPath(from.Position, to.Position);
            }
        }
        return null;
    }

    public Path FindPath(Vector3 fromVec, Vector3 toVec, bool findNearest = false)
    {
        Vertex from = pathGraph.NearestVertex(fromVec);
        Vertex to = pathGraph.NearestVertex(toVec);
        Path path = null;
        if (from.Id == to.Id) return path;
        if (!findNearest)
        {
            path = Astar(from, to);
        }
        else
        {
            var nearestNeighbour = pathGraph.NearestNeighbour(from, to);
            if (nearestNeighbour != null)
            {
                path = Astar(from, nearestNeighbour);
            }
        }
        return path;
    }

    private Path Astar(Vertex start, Vertex goal)
    {
        PriorityQueue<Vertex, float> frontier = new PriorityQueue<Vertex, float>(0);
        frontier.Enqueue(start, 0);

        var cameFrom = new Dictionary<string, Vertex>();
        var cameThrough = new Dictionary<string, Edge>();
        var costSoFar = new Dictionary<string, float>();

        cameFrom[start.Id] = null;
        costSoFar[start.Id] = 0;
        var isGoalFound = false;
        while (frontier.Count != 0)
        {
            var current = frontier.Dequeue();
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
                    frontier.Enqueue(next, priority);
                    cameFrom[next.Id] = current;
                    cameThrough[nextEdge.Id] = nextEdge;
                }
            }
        }
        if (isGoalFound)
        {
            var path = new Path()
            {
                Length = costSoFar[goal.Id]
            };
            var to = goal;
            var from = cameFrom[to.Id];
            while (from != null)
            {
                path.WayPoints.Insert(0, (cameThrough[$"{from.Id}{to.Id}"]));
                to = from;
                from = cameFrom[from.Id];
            }
            path.Length = costSoFar[goal.Id];
            return path;
        }
        else
        {
            return null;
        }
    }

    private static float GetDistance(Vertex from, Vertex to)
    {
        return Distance.Manhattan(from.Position, to.Position);
    }

}
