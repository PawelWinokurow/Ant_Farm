using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PriorityQueue;
using Unity.Collections;
using Unity.Jobs;

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
        UnityEngine.Random.InitState(Guid.NewGuid().GetHashCode());
        Path overallPath = new Path();
        Vertex from = pathGraph.NearestVertex(fromVec);
        for (int i = 0; i < numberOfSteps; i++)
        {
            var edgesToGo = from.Edges.Where(edge => edge.IsWalkable).ToList();
            var edgeChosen = edgesToGo[UnityEngine.Random.Range(0, edgesToGo.Count)];
            from = edgeChosen.To;
            overallPath.WayPoints.Add(edgeChosen);
            overallPath.Length += edgeChosen.EdgeWeight;
        }
        return overallPath;

    }
    public Path FindPath(Vector3 fromVec, Vector3 toVec, bool findNearest = false)
    {
        Vertex from = pathGraph.NearestVertex(fromVec);
        Vertex to = pathGraph.NearestVertex(toVec);
        if (from.Id == to.Id) return null;
        Path path;
        if (!findNearest)
        {
            return Astar(from, to);
        }
        else
        {
            var paths = to.Neighbours.Select(vertex => Astar(from, vertex)).Where(path => path != null).ToList();
            if (paths.Count() != 0)
            {
                path = paths.Aggregate(paths[0], (acc, p) =>
                {
                    {
                        if (p.Length < acc.Length) return p;
                        else return acc;
                    }

                });
                return path;
            }
            return null;
        }
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
        return Vector3.Distance(from.Position, to.Position);
    }

}
