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

    private Path PathToSomeNeighbour(Vertex from)
    {
        if (from.neighbours.Count != 0)
        {

            var neighbours = from.neighbours.OrderBy(a => Guid.NewGuid()).ToList();
            foreach (var to in neighbours)
            {
                if (to.edges[0].isWalkable)
                {
                    return FindPath(from.position, to.position);
                }
            }
        }
        else
        {
            var edges = from.edges.OrderBy(a => Guid.NewGuid()).ToList();
            foreach (var edge in edges)
            {
                if (edge.to.isCentralVertex)
                {
                    return FindPath(from.position, edge.to.position);
                }
            }
        }
        return null;
    }

    public Path FindPath(Vector3 fromVec, Vector3 toVec, bool findNearest = false)
    {
        Vertex from = pathGraph.NearestVertex(fromVec);
        Vertex to = pathGraph.NearestVertex(toVec);
        Path path = null;
        if (from.id == to.id) return path;
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

        cameFrom[start.id] = null;
        costSoFar[start.id] = 0;
        var isGoalFound = false;
        while (frontier.Count != 0)
        {
            var current = frontier.Dequeue();
            if (current == goal)
            {
                isGoalFound = true;
                break;
            }

            foreach (var nextEdge in current.edges)
            {
                if (!nextEdge.isWalkable) continue;
                var next = nextEdge.to;
                var newCost = costSoFar[current.id] + nextEdge.edgeWeight;
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
