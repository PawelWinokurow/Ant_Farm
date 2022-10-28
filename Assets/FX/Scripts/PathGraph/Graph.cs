using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


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

    public void ResetAllEdgesToWalkable()
    {
        AdjacencyList.ForEach(edge => edge.IsWalkable = true);
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
        if (PathVerticesMap.ContainsKey(position))
        {
            return PathVerticesMap[position];
        }
        return PathVertices.Aggregate(PathVertices[0], (acc, vertex) =>
        {
            if (Vector3.Distance(vertex.GeometricalPoint, position) <
            Vector3.Distance(acc.GeometricalPoint, position)) return vertex;
            else return acc;
        });
    }

    public List<PathVertex> GetNeighbors(PathVertex center)
    {
        return center.Edges.Select(edge => edge.To).ToList();
    }

    private static float GetDistance(PathVertex from, PathVertex to)
    {
        return Vector3.Distance(from.GeometricalPoint, to.GeometricalPoint);
    }

    public void AddHexagonSubGraph(Vector3 center, float R, string id)
    {
        float r = (float)Math.Sqrt(3) * R / 2;

        var centerPathPoint = AddVertex(new PathVertex($"{id}0", center));
        List<PathVertex> pathPoints = new List<PathVertex>();
        for (int n = 0; n < 6; n++)
        {
            float angle = (float)(Math.PI * (n * 60) / 180.0);
            float x = r * (float)Math.Cos(angle);
            float z = r * (float)Math.Sin(angle);
            pathPoints.Add(AddVertex(new PathVertex($"{id}{n + 1}", center + new Vector3(x, 0, z))));
        }
        for (int i = 0; i < pathPoints.Count; i++)
        {
            AddEdge(centerPathPoint, pathPoints[i], Vector3.Distance(centerPathPoint.GeometricalPoint, pathPoints[i].GeometricalPoint));
            AddEdge(pathPoints[i], pathPoints[(i + 2) % pathPoints.Count], Vector3.Distance(pathPoints[i].GeometricalPoint, pathPoints[(i + 2) % pathPoints.Count].GeometricalPoint));
        }
    }

}





