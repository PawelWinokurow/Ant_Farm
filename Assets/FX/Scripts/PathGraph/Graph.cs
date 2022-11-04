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
        int index = PathVertices.FindIndex(vertex => Vector3.Distance(vertex.Position, newVertex.Position) < 0.1f ? true : false);
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
        Vertex centerVertex = PathVerticesMap[hexagonPosition];
        List<Vertex> hexagonVertices = centerVertex.Edges.Select(edge => edge.To).ToList();
        hexagonVertices.Add(centerVertex);
        var Ids = hexagonVertices.Select(vertex => vertex.Id);
        return hexagonVertices.Aggregate(new List<Edge>(), (acc, vertex) => acc.Concat(vertex.Edges).ToList()).Where(edge => Ids.Contains(edge.From.Id) && Ids.Contains(edge.To.Id)).ToList();
    }

    public Vertex NearestVertex(Vector3 position)
    {
        if (PathVerticesMap.ContainsKey(position))
        {
            return PathVerticesMap[position];
        }
        return PathVertices.Aggregate(PathVertices[0], (acc, vertex) =>
        {
            if (Vector3.Distance(vertex.Position, position) <
            Vector3.Distance(acc.Position, position)) return vertex;
            else return acc;
        });
    }

    private static float GetDistance(Vertex from, Vertex to)
    {
        return Vector3.Distance(from.Position, to.Position);
    }

    public Vertex AddHexagonSubGraph(Vector3 center, float R, string id)
    {
        float r = (float)Math.Sqrt(3) * R / 2;

        var centerPathPoint = AddVertex(new Vertex($"{id}0", center, true));
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
            AddEdge(centerPathPoint, pathPoints[i], Vector3.Distance(centerPathPoint.Position, pathPoints[i].Position));
            AddEdge(pathPoints[i], pathPoints[(i + 2) % pathPoints.Count], Vector3.Distance(pathPoints[i].Position, pathPoints[(i + 2) % pathPoints.Count].Position));
        }
        return centerPathPoint;
    }

}





