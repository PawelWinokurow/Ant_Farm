using System;
using System.Collections.Generic;


[Serializable]
public class GraphSerializable
{
    public List<VertexSerializable> pathVertices { get; set; }
    public List<EdgeSerializable> adjacencyList { get; set; }
}