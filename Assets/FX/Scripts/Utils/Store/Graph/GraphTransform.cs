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
            PathVerticesMap = new Dictionary<Vector3, PathVertex>()
        };
        graph.PathVertices.ForEach(vertex =>
        {
            graph.PathVerticesMap[vertex.Position] = vertex;
        });
        graph.AdjacencyList = graphSerializable.AdjacencyList.Select(serializedEdge =>
        {

            var edge = new PathEdge()
            {
                From = graph.PathVerticesMap[VectorTransform.FromSerializable(serializedEdge.FromPosition)],
                To = graph.PathVerticesMap[VectorTransform.FromSerializable(serializedEdge.ToPosition)],
                EdgeWeight = serializedEdge.EdgeWeight,
                IsWalkable = serializedEdge.IsWalkable,
            };
            graph.PathVerticesMap[edge.From.Position].Edges.Add(edge);
            return edge;
        }).ToList();
        return graph;
    }
    private static PathEdgeSerializable ToSerializable(PathEdge edge)
    {
        var from = ToSerializable(edge.From);
        var to = ToSerializable(edge.To);
        var pathEdgeSerializable = new PathEdgeSerializable(from, to, edge.EdgeWeight)
        {
            IsWalkable = edge.IsWalkable
        };
        return pathEdgeSerializable;
    }
    private static PathVertexSerializable ToSerializable(PathVertex vertex)
    {
        return new PathVertexSerializable(vertex.Id, VectorTransform.ToSerializable(vertex.Position))
        {
            PathWeight = vertex.PathWeight
        };
    }
    private static PathVertex FromSerializable(PathVertexSerializable vertexSerializable)
    {
        return new PathVertex(vertexSerializable.Id, VectorTransform.FromSerializable(vertexSerializable.Position))
        {
            PathWeight = vertexSerializable.PathWeight
        };
    }

}