using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class GraphTransform
{
    public static GraphSerializable ToSerializable(Graph graph)
    {
        var graphSerializable = new GraphSerializable()
        {
            pathVertices = graph.pathVertices.Select(vertex => ToSerializable(vertex)).ToList(),
            adjacencyList = graph.adjacencyList.Select(edge => ToSerializable(edge)).ToList(),
        };
        return graphSerializable;
    }
    public static Graph FromSerializable(GraphSerializable graphSerializable)
    {
        var graph = new Graph()
        {
            pathVertices = graphSerializable.pathVertices.Select(vertex => FromSerializable(vertex)).ToList(),
            pathVerticesMap = new Dictionary<Vector3, Vertex>()
        };
        graph.pathVertices.ForEach(vertex =>
        {
            graph.pathVerticesMap[vertex.position] = vertex;
        });
        graph.adjacencyList = graphSerializable.adjacencyList.Select(serializedEdge =>
        {

            var edge = new Edge()
            {
                from = graph.pathVerticesMap[VectorTransform.FromSerializable(serializedEdge.fromPosition)],
                to = graph.pathVerticesMap[VectorTransform.FromSerializable(serializedEdge.toPosition)],
                edgeWeight = serializedEdge.edgeWeight,
                // isWalkable = serializedEdge.isWalkable,
            };
            graph.pathVerticesMap[edge.from.position].edges.Add(edge);
            return edge;
        }).ToList();
        for (int i = 0; i < graph.pathVertices.Count; i++)
        {
            graph.pathVertices[i].neighbours.AddRange(graphSerializable.pathVertices[i].neighbours.Select(vertex => graph.pathVerticesMap[VectorTransform.FromSerializable(vertex)]));
        }
        return graph;
    }
    private static EdgeSerializable ToSerializable(Edge edge)
    {
        var from = ToSerializable(edge.from);
        var to = ToSerializable(edge.to);
        var pathEdgeSerializable = new EdgeSerializable(from, to, edge.edgeWeight)
        {
            // isWalkable = edge.isWalkable
        };
        return pathEdgeSerializable;
    }
    private static VertexSerializable ToSerializable(Vertex vertex)
    {
        return new VertexSerializable(vertex.id, VectorTransform.ToSerializable(vertex.position), vertex.neighbours.Select(v => VectorTransform.ToSerializable(v.position)).ToList(), vertex.isCentralVertex);
    }
    private static Vertex FromSerializable(VertexSerializable vertexSerializable)
    {
        return new Vertex(vertexSerializable.id, VectorTransform.FromSerializable(vertexSerializable.position), vertexSerializable.isCentralVertex)
        {
            pathWeight = vertexSerializable.pathWeight
        };
    }

}