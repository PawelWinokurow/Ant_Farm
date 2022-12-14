using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Graph
{
    public Dictionary<Vector3, Vertex> pathVerticesMap;
    public List<Edge> adjacencyList { get; set; }
    public List<Vertex> pathVertices { get; set; }
    private GameSettings gameSettings;
    public Graph()
    {
        adjacencyList = new List<Edge>();
        pathVertices = new List<Vertex>();
        pathVerticesMap = new Dictionary<Vector3, Vertex>();
        gameSettings = Settings.Instance.gameSettings;
    }

    public void ResetPathWeights()
    {
        pathVertices.ForEach(pathVerex => pathVerex.ResetPathWeight());
    }

    public void ResetAllEdgesToWalkable()
    {
        adjacencyList.ForEach(edge => edge.edgeMultiplier = gameSettings.EDGE_WEIGHT_NORMAL);
    }

    public Vertex GetVertexByPoistion(Vector3 position)
    {
        return pathVerticesMap.GetValueOrDefault(position);
    }

    public Vertex AddVertex(Vertex newVertex)
    {
        int index = pathVertices.FindIndex(vertex => Distance.Manhattan(vertex.position, newVertex.position) < 0.1f ? true : false);
        if (index >= 0) return pathVertices[index];
        pathVertices.Add(newVertex);
        pathVerticesMap.Add(newVertex.position, newVertex);
        return newVertex;
    }

    public void AddEdge(Vertex v1, Vertex v2, EdgeType type, float edgeWeight, FloorHexagon hex)
    {
        Edge v1Edge = new Edge($"{v1.id}{v2.id}", type, v1, v2, edgeWeight, hex, 1, gameSettings.EDGE_WEIGHT_NORMAL);
        Edge v2Edge = new Edge($"{v2.id}{v1.id}", type, v2, v1, edgeWeight, hex, 1, gameSettings.EDGE_WEIGHT_NORMAL);
        v1.edges.Add(v1Edge);
        v2.edges.Add(v2Edge);
        adjacencyList.Add(v1Edge);
        adjacencyList.Add(v2Edge);
    }

    public void SetNeighbours()
    {
        pathVertices.Where(vertex => vertex.isCentralVertex).ToList().ForEach(center =>
        {
            var visited = new Dictionary<string, bool>();
            visited[center.id] = true;
            foreach (var edge1 in center.edges)
            {

                foreach (var edge2 in edge1.to.edges)
                {
                    if (edge2.to.isCentralVertex && !visited.ContainsKey(edge2.to.id))
                    {
                        visited[edge2.to.id] = true;
                        center.neighbours.Add(edge2.to);
                    }
                }
            }
        });
    }

    public void SetAccesabillity(FloorHexagon hex, int accessMask, int edgeMultiplier)
    {
        GetHexagonEdges(hex).ForEach(edge => { edge.accessMask = accessMask; edge.edgeMultiplier = edgeMultiplier; });
    }

    private List<Edge> GetHexagonEdges(FloorHexagon hex)
    {
        Vertex centerVertex = hex.vertex;
        List<Vertex> hexagonVertices = centerVertex.edges.Select(edge => edge.to).ToList();
        hexagonVertices.Add(centerVertex);
        var Ids = hexagonVertices.Select(vertex => vertex.id);
        return hexagonVertices
            .Aggregate(new List<Edge>(), (acc, vertex) => acc.Concat(vertex.edges).ToList())
            .Where(edge => Ids.Contains(edge.from.id) && Ids.Contains(edge.to.id)).ToList();
    }

    public Vertex GetVertex(Vector3 position)
    {
        if (pathVerticesMap.ContainsKey(position))
        {
            return pathVerticesMap[position];
        }
        return pathVertices.Aggregate(pathVertices[0], (acc, vertex) =>
        {
            if (Distance.Manhattan(vertex.position, position) <
            Distance.Manhattan(acc.position, position)) return vertex;
            else return acc;
        });
    }
    public Vertex NearestCentralVertex(Vertex from, Vertex to, int accessMask, EdgeType edgeType)
    {
        var toNeighbours = to.neighbours.Where(vertex => vertex.edges.Any(edge => edge.HasAccess(accessMask) && (edgeType == EdgeType.SECONDARY || edge.type == EdgeType.PRIMARY))).ToList();
        if (toNeighbours.Count == 0) return null;
        return toNeighbours.Aggregate(toNeighbours[0], (acc, vertex) =>
        {
            if (Distance.Manhattan(vertex.position, from.position) <
            Distance.Manhattan(acc.position, from.position)) return vertex;
            else return acc;
        });
    }
    public Vertex NearestVertex(Vertex from, Vertex to, int accessMask, EdgeType edgeType)
    {
        var toNeighbours = to.edges.Select(e => e.to).Where(vertex => vertex.edges.Any(edge => edge.HasAccess(accessMask) && (edgeType == EdgeType.SECONDARY || edge.type == EdgeType.PRIMARY))).ToList();
        if (toNeighbours.Count == 0) return null;
        return toNeighbours.Aggregate(toNeighbours[0], (acc, vertex) =>
        {
            if (Distance.Manhattan(vertex.position, from.position) <
            Distance.Manhattan(acc.position, from.position)) return vertex;
            else return acc;
        });
    }

    private static float GetDistance(Vertex from, Vertex to)
    {
        return Distance.Manhattan(from.position, to.position);
    }

    public Vertex AddHexagonSubGraph(FloorHexagon hex, float R, string id)
    {
        var center = hex.position;
        float r = (float)Math.Sqrt(3) * R / 2;

        var centerPathPoint = AddVertex(new Vertex($"{id}", center, true, this, hex));
        hex.vertex = centerPathPoint;
        List<Vertex> pathPoints = new List<Vertex>();
        for (int n = 0; n < 6; n++)
        {
            float angle = (float)(Math.PI * (n * 60) / 180.0);
            float x = r * (float)Math.Cos(angle);
            float z = r * (float)Math.Sin(angle);
            pathPoints.Add(AddVertex(new Vertex($"{id}{n}", center + new Vector3(x, 0, z), false, this)));
        }
        for (int i = 0; i < pathPoints.Count; i++)
        {
            AddEdge(centerPathPoint, pathPoints[i], EdgeType.PRIMARY, Vector3.Distance(centerPathPoint.position, pathPoints[i].position), hex);
            AddEdge(pathPoints[i], pathPoints[(i + 2) % pathPoints.Count], EdgeType.SECONDARY, Vector3.Distance(pathPoints[i].position, pathPoints[(i + 2) % pathPoints.Count].position), hex);
        }
        return centerPathPoint;
    }

}





