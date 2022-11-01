using System.Collections.Generic;
using System.Linq;

class StoreService
{

    private static string GRAPH_FILEPATH = "Save/graph";
    private static string HEXAGONS_FILEPATH = "Save/hexagons";

    public static bool DoesGraphExist()
    {
        return FileService.DoesFileExist(GRAPH_FILEPATH);
    }

    public static bool DoHexagonsExist()
    {
        return FileService.DoesFileExist(HEXAGONS_FILEPATH);
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
    public static void SaveHexagons(Hexagon[] hexagons)
    {
        HexagonSerializable[] hexagonsSerializable = hexagons.Select(hex => HexagonTransform.ToSerializable(hex)).ToArray();
        FileService.WriteToBinaryFile<HexagonSerializable[]>(HEXAGONS_FILEPATH, hexagonsSerializable);
    }
    public static HexagonSerializable[] LoadHexagons()
    {
        return FileService.ReadFromBinaryFile<HexagonSerializable[]>(HEXAGONS_FILEPATH);
    }

}
