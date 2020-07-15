using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldMapLoader
{
    public static OverworldMap.LocationGraph CreateTestMap(){
        OverworldMap.LocationGraph graph = new OverworldMap.LocationGraph();
        OverworldMap.LocationNode Town1, Town2, Town3, Town4, Town5, Node1, Node2, Node3,
         Node4, Node5, Node6, Node7, Node8, Node9, Node10, Node11, Node12, Node13;

        Town1 = new OverworldMap.LocationNode(0, "Town1", OverworldMap.LocationType.TOWN, 0.03f, 0.068f);
        Town2 = new OverworldMap.LocationNode(1, "Town2", OverworldMap.LocationType.TOWN, -0.327f, 0.367f);
        Town3 = new OverworldMap.LocationNode(2, "Town3", OverworldMap.LocationType.TOWN, -0.12f, -0.42f);
        Town4 = new OverworldMap.LocationNode(3, "Town4", OverworldMap.LocationType.TOWN, 0.67f, 0.10f);
        Town5 = new OverworldMap.LocationNode(4, "Town5", OverworldMap.LocationType.TOWN, 0.12f, 0.65f);
        Node1 = new OverworldMap.LocationNode(-1, "Node1", OverworldMap.LocationType.NONE, -0.24f, 0.2f);

        Node2 = new OverworldMap.LocationNode(-1, "Event1", OverworldMap.LocationType.EVENT, -0.13f, 0.06f);

        Node3 = new OverworldMap.LocationNode(-1, "Node3", OverworldMap.LocationType.NONE, -0.23f, -0.09f);
        Node4 = new OverworldMap.LocationNode(-1, "Node4", OverworldMap.LocationType.NONE, -0.15f, -0.29f);
        Node5 = new OverworldMap.LocationNode(-1, "Node5", OverworldMap.LocationType.NONE, 0f, -0.16f);
        Node6 = new OverworldMap.LocationNode(-1, "Node6", OverworldMap.LocationType.NONE, 0.13f, -0.06f);
        Node7 = new OverworldMap.LocationNode(-1, "Node7", OverworldMap.LocationType.NONE, 0.27f, 0.07f);
        Node8 = new OverworldMap.LocationNode(-1, "Node8", OverworldMap.LocationType.NONE, 0.45f, 0.14f);
        Node9 = new OverworldMap.LocationNode(-1, "Node9", OverworldMap.LocationType.NONE, 0.3f, 0.29f);
        Node10 = new OverworldMap.LocationNode(-1, "Node10", OverworldMap.LocationType.NONE, 0.17f, 0.44f);
        Node11 = new OverworldMap.LocationNode(-1, "Node11", OverworldMap.LocationType.NONE, -0.07f, 0.49f);

        Node12 = new OverworldMap.LocationNode(-1, "Event2", OverworldMap.LocationType.EVENT, 0.125f, 0.233f);

        Node13 = new OverworldMap.LocationNode(-1, "Node13", OverworldMap.LocationType.NONE, -0.09f, 0.28f);


        graph
        .AddNode(Town1).AddNode(Town2).AddNode(Town3).AddNode(Town4).AddNode(Town5).AddNode(Node1)
        .AddNode(Node2).AddNode(Node3).AddNode(Node4).AddNode(Node5).AddNode(Node6).AddNode(Node7)
        .AddNode(Node8).AddNode(Node9).AddNode(Node10).AddNode(Node11).AddNode(Node12).AddNode(Node13)
        .AddEdge(Town1, Node2).AddEdge(Town1, Node6).AddEdge(Town1, Node7).AddEdge(Town1, Node12)
        .AddEdge(Town2, Node1).AddEdge(Town2, Node11).AddEdge(Town2, Node13)
        .AddEdge(Town3, Node4)
        .AddEdge(Town4, Node8)
        .AddEdge(Town5, Node10).AddEdge(Town5, Node11)
        .AddEdge(Node2, Node1).AddEdge(Node2, Node3).AddEdge(Node3, Node4).AddEdge(Node4, Node5)
        .AddEdge(Node5, Node6).AddEdge(Node6, Node7).AddEdge(Node7, Node8).AddEdge(Node8, Node9)
        .AddEdge(Node9, Node10).AddEdge(Node10, Node11).AddEdge(Node1, Node13).AddEdge(Node13, Node12).AddEdge(Node9, Node12);

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
