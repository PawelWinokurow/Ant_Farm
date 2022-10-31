using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PriorityQueue;

public class Path
{
    public List<Vector3> WayPoints = new List<Vector3>();
    public float OverallDistance = 0;
}

public class PathFinder
{

    private Graph pathGraph { get; set; }

    public PathFinder(Graph pathGraph)
    {
        this.pathGraph = pathGraph;
    }


    public Path FindPath(Vector3 fromVec, Vector3 toVec, bool findNearest = false)
    {
        PathVertex from = pathGraph.NearestVertex(fromVec);
        PathVertex to = pathGraph.NearestVertex(toVec);
        if (from.Id == to.Id) return null;
        Path path;
        if (!findNearest)
        {
            return Astar(from, to);
        }
        else
        {
            var neighbors = pathGraph.GetNeighbors(to);
            var paths = neighbors.Select(vertex => Astar(from, vertex)).Where(path => path != null).ToList();
            if (paths.Count() != 0)
            {
                path = paths.Aggregate(paths[0], (acc, p) =>
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
    }

    private Path Astar(PathVertex start, PathVertex goal)
    {
        PriorityQueue<PathVertex, float> frontier = new PriorityQueue<PathVertex, float>(0);
        frontier.Enqueue(start, 0);

        var cameFrom = new Dictionary<string, PathVertex>();
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
                path.WayPoints.Add(next.Position);
                next = cameFrom[next.Id];
            }
            path.WayPoints.Reverse();
            path.WayPoints.Add(goal.Position);
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
        return Vector3.Distance(from.Position, to.Position);
    }

}
