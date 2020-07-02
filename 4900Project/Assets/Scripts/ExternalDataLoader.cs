using FileConstants;
using System;
using System.Collections.Generic;
using System.Linq;

public class ExternalDataLoader : IDataLoader
{
    private class CSVMapNode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
    }

    private class CSVMapEdge
    {
        public int IdA { get; set; }
        public int IdB { get; set; }
    }

    public OverworldMap.LocationGraph LoadMap()
    {
        OverworldMap.LocationGraph graph = new OverworldMap.LocationGraph();

        GameData.LoadCsv(FileConstants.Files.MapNodes, out IEnumerable<CSVMapNode> nodeResult);
        nodeResult.Select(n => graph.AddNode(new OverworldMap.LocationNode(n.Id, n.Name, (OverworldMap.LocationType) Enum.Parse(typeof(OverworldMap.LocationType), n.Type), n.PosX, n.PosY)));

        GameData.LoadCsv(FileConstants.Files.MapEdges, out IEnumerable<CSVMapEdge> edgeResult);
        edgeResult.Select(e => graph.AddEdge(e.IdA, e.IdB));

        return graph;
    }

    public PlayerData LoadPlayer()
    {
        throw new System.NotImplementedException();
    }
}
