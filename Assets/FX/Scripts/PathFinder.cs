using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PriorityQueue;


public class Path
{
    public List<Edge> wayPoints = new List<Edge>();
    public float length = 0;
    public bool HasWaypoints { get => wayPoints.Count != 0; }
}

public enum SearchType
{
    NEAREST_CENTRAL_VERTEX, VERTEX, NEAREST_VERTEX
}

public class Pathfinder
{

    private Graph pathGraph { get; set; }

    public Pathfinder(Graph pathGraph)
    {
        this.pathGraph = pathGraph;
    }

    public Path RandomWalk(Vector3 fromVec, int numberOfSteps, int accessMask)
    {
        Path overallPath = new Path();
        Vertex from = pathGraph.GetVertex(fromVec);
        for (int i = 0; i < numberOfSteps; i++)
        {
            var path = PathToSomeNeighbour(from, accessMask);
            if (path != null)
            {
                overallPath.wayPoints.AddRange(path.wayPoints);
                overallPath.length += path.length;
            }
            else
            {
                //TODO teleport
            }
            from = path.wayPoints[path.wayPoints.Count - 1].to;
        }
        return overallPath;
    }

    private Path PathToSomeNeighbour(Vertex from, int accessMask)
    {
        if (from.isCentralVertex)
        {
            var edges = from.edges.OrderBy(e => Guid.NewGuid()).ToList();
            foreach (var edge in edges)
            {
                var edges2 = edge.to.edges.OrderBy(e => Guid.NewGuid()).ToList();
                foreach (var edge2 in edges2)
                {
                    if (edge2.HasAccess(accessMask) && edge2.to.isCentralVertex && from != edge2.to)
                    {
                        return FindPath(from.position, edge2.to.position, accessMask);
                    }
                }
            }
        }
        else
        {
            var edges = from.edges.OrderBy(e => Guid.NewGuid()).ToList();
            foreach (var edge in edges)
            {
                if (edge.HasAccess(accessMask) && edge.to.isCentralVertex)
                {
                    return FindPath(from.position, edge.to.position, accessMask);
                }
            }
        }
        return null;
    }

    public Path FindPath(Vector3 fromVec, Vector3 toVec, int accessMask, SearchType searchType = SearchType.VERTEX)
    {
        Vertex from = pathGraph.GetVertex(fromVec);
        Vertex to = pathGraph.GetVertex(toVec);
        Path path = null;
        if (from.id == to.id) return path;
        if (searchType == SearchType.VERTEX)
        {
            path = Astar(from, to, accessMask);
        }
        else if (searchType == SearchType.NEAREST_CENTRAL_VERTEX)
        {
            var nearestCentralVertex = pathGraph.NearestCentralVertex(from, to, accessMask);
            if (nearestCentralVertex != null)
            {
                path = Astar(from, nearestCentralVertex, accessMask);
            }
        }
        else if (searchType == SearchType.NEAREST_VERTEX)
        {
            var nearestVertex = pathGraph.NearestVertex(from, to, accessMask);
            if (nearestVertex != null)
            {
                path = Astar(from, nearestVertex, accessMask);
            }
        }
        return path;
    }

    private Path Astar(Vertex start, Vertex goal, int accessMask)
    {
        PriorityQueue<Vertex, float> frontier = new PriorityQueue<Vertex, float>(0);
        frontier.Enqueue(start, 0);
        int i = 0;
        var cameFrom = new Dictionary<string, Vertex>();
        var cameThrough = new Dictionary<string, Edge>();
        var costSoFar = new Dictionary<string, float>();

        cameFrom[start.id] = null;
        costSoFar[start.id] = 0;
        var isGoalFound = false;
        while (frontier.Count != 0)
        {
            var current = frontier.Dequeue();
            i++;
            if (i == Settings.Instance.gameSettings.PATHFINDER_MAX_STEPS)
            {
                return null;
            }
            if (current == goal)
            {
                isGoalFound = true;
                break;
            }

            foreach (var nextEdge in current.edges)
            {
                if (!nextEdge.HasAccess(accessMask)) continue;
                var next = nextEdge.to;
                var newCost = costSoFar[current.id] + nextEdge.edgeWeight * nextEdge.edgeMultiplier;
                if (!costSoFar.ContainsKey(next.id) || newCost < costSoFar[next.id])
                {
                    costSoFar[next.id] = newCost;
                    var priority = newCost + GetDistance(next, goal);
                    frontier.Enqueue(next, priority);
                    cameFrom[next.id] = current;
                    cameThrough[nextEdge.id] = nextEdge;
                }
            }
        }
        if (isGoalFound)
        {
            var path = new Path()
            {
                length = costSoFar[goal.id]
            };
            var to = goal;
            var from = cameFrom[to.id];
            while (from != null)
            {
                path.wayPoints.Insert(0, (cameThrough[$"{from.id}{to.id}"]));
                to = from;
                from = cameFrom[from.id];
            }
            path.length = costSoFar[goal.id];
            return path;
        }
        else
        {
            return null;
        }
    }

    private static float GetDistance(Vertex from, Vertex to)
    {
        return Distance.Manhattan(from.position, to.position);
    }

}
