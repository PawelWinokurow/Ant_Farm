using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class GraphTransform
{
    public static GraphSerializable ToSerializable(Graph graph)
    {
        var graphSerializable = new GraphSerializable()
        {
            PathVertices = graph.PathVertices.Select(vertex => ToSerializable(vertex)).ToList(),
            AdjacencyList = graph.AdjacencyList.Select(edge => ToSerializable(edge)).ToList(),
        };
        return graphSerializable;
    }
    public static Graph FromSerializable(GraphSerializable graphSerializable)
    {
        var graph = new Graph()
        {
            PathVertices = graphSerializable.PathVertices.Select(vertex => FromSerializable(vertex)).ToList(),
            PathVerticesMap = new Dictionary<Vector3, Vertex>()
        };
        graph.PathVertices.ForEach(vertex =>
        {
            graph.PathVerticesMap[vertex.Position] = vertex;
        });
        graph.AdjacencyList = graphSerializable.AdjacencyList.Select(serializedEdge =>
        {

            var edge = new Edge()
            {
                From = graph.PathVerticesMap[VectorTransform.FromSerializable(serializedEdge.FromPosition)],
                To = graph.PathVerticesMap[VectorTransform.FromSerializable(serializedEdge.ToPosition)],
                EdgeWeight = serializedEdge.EdgeWeight,
                IsWalkable = serializedEdge.IsWalkable,
            };
            graph.PathVerticesMap[edge.From.Position].Edges.Add(edge);
            return edge;
        }).ToList();
        for (int i = 0; i < graph.PathVertices.Count; i++)
        {
            graph.PathVertices[i].Neighbours.AddRange(graphSerializable.PathVertices[i].Neighbours.Select(vertex => graph.PathVerticesMap[VectorTransform.FromSerializable(vertex)]));
        }
        return graph;
    }
    private static EdgeSerializable ToSerializable(Edge edge)
    {
        var from = ToSerializable(edge.From);
        var to = ToSerializable(edge.To);
        var pathEdgeSerializable = new EdgeSerializable(from, to, edge.EdgeWeight)
        {
            IsWalkable = edge.IsWalkable
        };
        return pathEdgeSerializable;
    }
    private static VertexSerializable ToSerializable(Vertex vertex)
    {
        return new VertexSerializable(vertex.Id, VectorTransform.ToSerializable(vertex.Position), vertex.Neighbours.Select(v => VectorTransform.ToSerializable(v.Position)).ToList(), vertex.IsCentralVertex);
    }
    private static Vertex FromSerializable(VertexSerializable vertexSerializable)
    {
        return new Vertex(vertexSerializable.Id, VectorTransform.FromSerializable(vertexSerializable.Position), vertexSerializable.IsCentralVertex)
        {
            PathWeight = vertexSerializable.PathWeight
        };
    }

}