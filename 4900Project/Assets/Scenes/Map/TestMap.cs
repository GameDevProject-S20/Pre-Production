using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMap : MonoBehaviour
{
    public OverworldMapManager manager;

    private bool drawn;

    // Start is called before the first frame update
    void Start()
    {
        OverworldMap.LocationNode Town1, Town2, Town3, Town4, Town5, Node1, Node2, Node3, Node4;

        Town1 = new OverworldMap.LocationNode(1, "Town1", OverworldMap.LocationType.TOWN, 0f, 0f);
        Town2 = new OverworldMap.LocationNode(2, "Town2", OverworldMap.LocationType.TOWN, -0.5f, 0.9f);
        Town3 = new OverworldMap.LocationNode(3, "Town3", OverworldMap.LocationType.TOWN, 0.2f, 0.5f);
        Town4 = new OverworldMap.LocationNode(4, "Town4", OverworldMap.LocationType.TOWN, 0.8f, 0.5f);
        Town5 = new OverworldMap.LocationNode(5, "Town5", OverworldMap.LocationType.TOWN, -0.9f, -0.7f);
        Node1 = new OverworldMap.LocationNode(6, "Node1", OverworldMap.LocationType.NONE, -0.6f, -0.3f);
        Node2 = new OverworldMap.LocationNode(7, "Node2", OverworldMap.LocationType.NONE, -0.5f, 0f);
        Node3 = new OverworldMap.LocationNode(8, "Node3", OverworldMap.LocationType.NONE, -0.35f, 0.3f);
        Node4 = new OverworldMap.LocationNode(9, "Node4", OverworldMap.LocationType.NONE, 0.6f, 0.4f);


        manager.Graph
        .AddNode(Town1).AddNode(Town2).AddNode(Town3).AddNode(Town4).AddNode(Town5)
        .AddNode(Node1).AddNode(Node2).AddNode(Node3).AddNode(Node4).AddEdge(Town1, Node1).AddEdge(Town1, Node2)
        .AddEdge(Town1, Node3).AddEdge(Town5, Node1).AddEdge(Node1, Node2).AddEdge(Node2, Node3).AddEdge(Node3, Town2)
        .AddEdge(Node3, Town3).AddEdge(Town3, Node4).AddEdge(Node4, Town4);
    }

    void Update()
    {
        if (!drawn)
        {
            manager.RequestDraw();
            drawn = true;
        }
    }
}
