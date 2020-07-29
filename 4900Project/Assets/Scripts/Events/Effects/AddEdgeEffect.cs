using System.Collections.Generic;
using System;
using System.Linq;

/// <summary>
/// Does not give a lot of information, as Apply always returns true
/// </summary>
public class AddEdgeEffect : IEffect
{
    private readonly int node1;
    private readonly int node2;

    public AddEdgeEffect(int node1, int node2)
    {
        this.node1 = node1;
        this.node2 = node2;
    }

    public bool Apply()
    {
        OverworldMap.LocationNode nodeA;
        OverworldMap.LocationNode nodeB;
        DataTracker.Current.WorldMap.GetNode(node1, out nodeA);
        DataTracker.Current.WorldMap.GetNode(node2, out nodeB);
        DataTracker.Current.WorldMap.AddEdge(nodeA, nodeB);
        return true;
    }
}
