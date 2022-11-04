using System;
using System.Collections.Generic;


[Serializable]
public class GraphSerializable
{
    public List<VertexSerializable> PathVertices { get; set; }
    public List<EdgeSerializable> AdjacencyList { get; set; }
}