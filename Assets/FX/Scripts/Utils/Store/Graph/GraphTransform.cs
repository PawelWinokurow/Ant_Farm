using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class GraphTransform
{
    public static GraphSerializable ToSerializable(Graph graph)
    {
        var graphSerializable = new GraphSerializable();
        graphSerializable.PathVertices = graph.PathVertices.Select(vertex => ToSerializable(vertex)).ToList();
        graphSerializable.AdjacencyList = graph.AdjacencyList.Select(edge => ToSerializable(edge)).ToList();
        graphSerializable.PathVerticesMap = new Dictionary<Vector3Serializable, PathVertexSerializable>();
        graphSerializable.PathVertices.ForEach(vertex => graphSerializable.PathVerticesMap[vertex.Position] = vertex);
        return graphSerializable;
    }
    public static Graph FromSerializable(GraphSerializable graphSerializable)
    {
        var graph = new Graph();
        graph.PathVertices = graphSerializable.PathVertices.Select(vertex => FromSerializable(vertex)).ToList();
        graph.AdjacencyList = graphSerializable.AdjacencyList.Select(edge => FromSerializable(edge)).ToList();
        graph.PathVerticesMap = new Dictionary<Vector3, PathVertex>();
        graph.PathVertices.ForEach(vertex => graph.PathVerticesMap[vertex.Position] = vertex);
        return graph;
    }
    private static PathEdgeSerializable ToSerializable(PathEdge edge)
    {
        var from = ToSerializable(edge.From);
        var to = ToSerializable(edge.To);
        var pathEdgeSerializable = new PathEdgeSerializable(from, to, edge.EdgeWeight);
        pathEdgeSerializable.IsWalkable = edge.IsWalkable;
        from.Edges.Add(pathEdgeSerializable);
        return pathEdgeSerializable;
    }
    private static PathEdge FromSerializable(PathEdgeSerializable edgeSerializable)
    {
        var from = FromSerializable(edgeSerializable.From);
        var to = FromSerializable(edgeSerializable.To);
        var pathEdge = new PathEdge(from, to, edgeSerializable.EdgeWeight);
        pathEdge.IsWalkable = edgeSerializable.IsWalkable;
        from.Edges.Add(pathEdge);
        return pathEdge;
    }
    private static PathVertexSerializable ToSerializable(PathVertex vertex)
    {
        var pathVertexSerializable = new PathVertexSerializable(vertex.Id, VectorTransform.ToSerializable(vertex.Position));
        pathVertexSerializable.PathWeight = vertex.PathWeight;
        return pathVertexSerializable;
    }
    private static PathVertex FromSerializable(PathVertexSerializable vertexSerializable)
    {
        var pathVertex = new PathVertex(vertexSerializable.Id, VectorTransform.FromSerializable(vertexSerializable.Position));
        pathVertex.PathWeight = vertexSerializable.PathWeight;
        return pathVertex;
    }

}