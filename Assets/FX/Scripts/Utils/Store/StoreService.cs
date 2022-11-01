using System.IO;

class StoreService
{

    private static string GRAPH_FILEPATH = "Save/graph";

    public static bool DoesGraphExist()
    {
        return FileService.DoesFileExist(GRAPH_FILEPATH);
    }
    public static void SaveGraph(Graph graph)
    {
        GraphSerializable graphSerializable = GraphTransform.ToSerializable(graph);
        FileService.WriteToBinaryFile<GraphSerializable>(GRAPH_FILEPATH, graphSerializable);
    }
    public static Graph LoadGraph()
    {
        GraphSerializable graphSerializable = FileService.ReadFromBinaryFile<GraphSerializable>(GRAPH_FILEPATH);
        return GraphTransform.FromSerializable(graphSerializable);
    }

}
