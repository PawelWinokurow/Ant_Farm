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
    public float OverallDistance = 0;

}

public struct PathfinderJob : IJobParallelFor
{
    public ManagedObjectRef<Pathfinder> Pathfinder;
    public NativeArray<Vector3> From;
    public NativeArray<Vector3> To;
    public NativeArray<ManagedObjectRef<Path>> Result;

    public void Execute(int i)
    {
        Path path = ManagedObjectWorld.Get(Pathfinder).FindPath(From[i], To[i], true);
        Result[i] = ManagedObjectWorld.Add(path);
    }
}

public class Pathfinder
{

    private Graph pathGraph { get; set; }

    public Pathfinder(Graph pathGraph)
    {
        this.pathGraph = pathGraph;
    }

    public Path RandomWalk(Vector3 fromVec, Vector3 initialPosition, int numberOfSteps)
    {
        UnityEngine.Random.InitState(Guid.NewGuid().GetHashCode());
        Path overallPath = new Path();
        Vertex from = pathGraph.NearestVertex(fromVec);
        for (int i = 0; i < numberOfSteps; i++)
        {
            Path path = null;
            Vertex to = null;
            do
            {
                double angle = UnityEngine.Random.Range(0, (float)(2 * Math.PI));
                float x = initialPosition.x + 10 * (float)Math.Cos(angle);
                float z = initialPosition.x + 10 * (float)Math.Sin(angle);
                to = pathGraph.NearestVertex(new Vector3(x, 0, z));
                path = Astar(from, to);
            } while (path == null);
            from = to;
            overallPath.WayPoints.AddRange(path.WayPoints);
            overallPath.OverallDistance += path.OverallDistance;
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
                OverallDistance = costSoFar[goal.Id]
            };
            var to = goal;
            var from = cameFrom[to.Id];
            while (from != null)
            {
                path.WayPoints.Add(cameThrough[$"{from.Id}{to.Id}"]);
                to = from;
                from = cameFrom[from.Id];
            }
            path.WayPoints.Reverse();
            path.OverallDistance = costSoFar[goal.Id];
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
