using System;
using System.Collections.Generic;


[Serializable]
public class GraphSerializable
{
    public List<PathVertexSerializable> PathVertices { get; set; }
    public List<PathEdgeSerializable> AdjacencyList { get; set; }
}