using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldMapLoader
{
    public static OverworldMap.LocationGraph CreateTestMap(){
        OverworldMap.LocationGraph graph = new OverworldMap.LocationGraph();
        OverworldMap.LocationNode Town0, Town1, Town2;

        Town0 = new OverworldMap.LocationNode(0, "Town0", OverworldMap.LocationType.TOWN, -0.2f, -0.068f);
        Town1 = new OverworldMap.LocationNode(1, "Town1", OverworldMap.LocationType.TOWN, 0.00f, 0.00f);
        Town2 = new OverworldMap.LocationNode(2, "Town2", OverworldMap.LocationType.TOWN, 0.2327f, 0.2367f);


        graph
        .AddNode(Town0)
        .AddNode(Town1)
        .AddNode(Town2)
        .AddEdge(Town0, Town1)
        .AddEdge(Town1, Town2);

        return graph;
    }

    public static OverworldMap.LocationGraph LoadMap(){
        OverworldMap.LocationGraph graph = new OverworldMap.LocationGraph();
        // Load in node data from CSV
        GameData.LoadCsv<Node>(FileConstants.Files.MapNodes, out IEnumerable<Node> resultN);
        var resultString = new System.Text.StringBuilder();
        resultString.AppendLine("Loading in from file:");
        foreach (Node data in resultN)
        {
            graph.AddNode(new OverworldMap.LocationNode(data.LocationId, data.Id, data.Name, (OverworldMap.LocationType)System.Enum.Parse(typeof(OverworldMap.LocationType), data.Type), data.PosX / DataTracker.Current.mapScale, data.PosY / DataTracker.Current.mapScale));
            resultString.AppendLine("Added Node #" + data.Id + ": " + data.Name);
        }
        UnityEngine.Debug.Log(resultString);

        // Load in edge data from CSV
        GameData.LoadCsv<Edge>(FileConstants.Files.MapEdges, out IEnumerable<Edge> resultE);
        resultString = new System.Text.StringBuilder();
        resultString.AppendLine("Loading in from file:");
        foreach (Edge data in resultE)
        {
            OverworldMap.LocationNode A;
            OverworldMap.LocationNode B;
            bool aFound = graph.GetNode(data.idOfSource, out A);
            bool bFound = graph.GetNode(data.idOfTarget, out B);
            if (aFound && bFound){
                graph.AddEdge(A, B);
                resultString.AppendLine("Added Edge from node #" + data.idOfSource + " to node # " + data.idOfTarget);
            }
        }
        UnityEngine.Debug.Log(resultString);

        return graph;
    }

    // Temporary node for loading data
    // To be used until we figure out how we want to handle LocationID
    public class Node {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public int LocationId {get; set;}
    }

    public class Edge{
        public int idOfSource { get; set; }
        public int idOfTarget { get; set; }
    }



}
