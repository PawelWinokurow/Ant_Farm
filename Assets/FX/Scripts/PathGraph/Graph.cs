using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Graph
{
    public Dictionary<Vector3, Vertex> PathVerticesMap;
    public List<Edge> AdjacencyList { get; set; }
    public List<Vertex> PathVertices { get; set; }
    public Graph()
    {
        AdjacencyList = new List<Edge>();
        PathVertices = new List<Vertex>();
        PathVerticesMap = new Dictionary<Vector3, Vertex>();
    }

    public void ResetPathWeights()
    {
        PathVertices.ForEach(pathVerex => pathVerex.ResetPathWeight());
    }

    public void ResetAllEdgesToWalkable()
    {
        AdjacencyList.ForEach(edge => edge.IsWalkable = true);
    }

    public Vertex GetVertexByPoistion(Vector3 position)
    {
        return PathVerticesMap.GetValueOrDefault(position);
    }

    public Vertex AddVertex(Vertex newVertex)
    {
        int index = PathVertices.FindIndex(vertex => Distance.Manhattan(vertex.Position, newVertex.Position) < 0.1f ? true : false);
        if (index >= 0) return PathVertices[index];
        PathVertices.Add(newVertex);
        PathVerticesMap.Add(newVertex.Position, newVertex);
        return newVertex;
    }

    public void AddEdge(Vertex v1, Vertex v2, float edgeWeight)
    {
        Edge v1Edge = new Edge($"{v1.Id}{v2.Id}", v1, v2, edgeWeight);
        Edge v2Edge = new Edge($"{v2.Id}{v1.Id}", v2, v1, edgeWeight);
        v1.Edges.Add(v1Edge);
        v2.Edges.Add(v2Edge);
        AdjacencyList.Add(v1Edge);
        AdjacencyList.Add(v2Edge);
    }

    public void SetNeighbours()
    {
        PathVertices.Where(vertex => vertex.IsCentralVertex).ToList().ForEach(center =>
        {
            var visited = new Dictionary<string, bool>();
            foreach (var edge1 in center.Edges)
            {
                foreach (var edge2 in edge1.To.Edges)
                {
                    if (edge2.To.IsCentralVertex && !visited.ContainsKey(edge2.To.Id))
                    {
                        visited[edge2.To.Id] = true;
                        center.Neighbours.Add(edge2.To);
                    }
                }
            }
        });
    }

    public void AllowHexagon(FloorHexagon hex)
    {
        GetHexagonEdges(hex).ForEach(edge => edge.IsWalkable = true);
    }
    public void ProhibitHexagon(FloorHexagon hex)
    {

        GetHexagonEdges(hex).ForEach(edge => edge.IsWalkable = false);
    }

    private List<Edge> GetHexagonEdges(FloorHexagon hex)
    {
        Vertex centerVertex = hex.Vertex;
        List<Vertex> hexagonVertices = centerVertex.Edges.Select(edge => edge.To).ToList();
        hexagonVertices.Add(centerVertex);
        var Ids = hexagonVertices.Select(vertex => vertex.Id);
        return hexagonVertices
            .Aggregate(new List<Edge>(), (acc, vertex) => acc.Concat(vertex.Edges).ToList())
            .Where(edge => Ids.Contains(edge.From.Id) && Ids.Contains(edge.To.Id)).ToList();
    }

    public Vertex NearestVertex(Vector3 position)
    {
        if (PathVerticesMap.ContainsKey(position))
        {
            return PathVerticesMap[position];
        }
        return PathVertices.Aggregate(PathVertices[0], (acc, vertex) =>
        {
            if (Distance.Manhattan(vertex.Position, position) <
            Distance.Manhattan(acc.Position, position)) return vertex;
            else return acc;
        });
    }
    public Vertex NearestNeighbour(Vertex from, Vertex to)
    {
        var toNeighbours = to.Neighbours.Where(vertex => vertex.Edges.All(edge => edge.IsWalkable)).ToList();
        if (toNeighbours.Count == 0) return null;
        return toNeighbours.Aggregate(toNeighbours[0], (acc, vertex) =>
        {
            if (Distance.Manhattan(vertex.Position, from.Position) <
            Distance.Manhattan(acc.Position, from.Position)) return vertex;
            else return acc;
        });
    }

    private static float GetDistance(Vertex from, Vertex to)
    {
        return Distance.Manhattan(from.Position, to.Position);
    }

    public Vertex AddHexagonSubGraph(FloorHexagon hex, float R, string id)
    {
        var center = hex.Position;
        float r = (float)Math.Sqrt(3) * R / 2;

        var centerPathPoint = AddVertex(new Vertex($"{id}0", center, true));
        hex.Vertex = centerPathPoint;
        List<Vertex> pathPoints = new List<Vertex>();
        for (int n = 0; n < 6; n++)
        {
            float angle = (float)(Math.PI * (n * 60) / 180.0);
            float x = r * (float)Math.Cos(angle);
            float z = r * (float)Math.Sin(angle);
            pathPoints.Add(AddVertex(new Vertex($"{id}{n + 1}", center + new Vector3(x, 0, z), false)));
        }
        for (int i = 0; i < pathPoints.Count; i++)
        {
            AddEdge(centerPathPoint, pathPoints[i], Distance.Manhattan(centerPathPoint.Position, pathPoints[i].Position));
            AddEdge(pathPoints[i], pathPoints[(i + 2) % pathPoints.Count], Distance.Manhattan(pathPoints[i].Position, pathPoints[(i + 2) % pathPoints.Count].Position));
        }
        return centerPathPoint;
    }

}





