using System.Collections.Generic;
using System;
using System.Linq;

/// <summary>
/// Does not give a lot of information, as Apply always returns true
/// </summary>
public class SetNodeEncounterEffect : IEffect
{
    private readonly int nodeId;
    private readonly int encounterId;

    public SetNodeEncounterEffect(int nodeId, int encounterId)
    {
        this.nodeId = nodeId;
        this.encounterId = encounterId;

    }

    public bool Apply()
    {
        OverworldMap.LocationNode node;
        if (DataTracker.Current.WorldMap.GetNode(nodeId, out node)){
            node.LocationId = encounterId;
            return true;
        }
        return false;
    }
}
