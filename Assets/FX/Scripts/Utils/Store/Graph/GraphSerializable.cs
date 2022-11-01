using System;
using System.Collections.Generic;


[Serializable]
public class GraphSerializable
{
    public Dictionary<Vector3Serializable, PathVertexSerializable> PathVerticesMap;
    public List<PathEdgeSerializable> AdjacencyList { get; set; }
    public List<PathVertexSerializable> PathVertices { get; set; }
}