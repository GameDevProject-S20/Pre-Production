using System.Collections.Generic;
using System;
using System.Linq;

/// <summary>
/// Does not give a lot of information, as Apply always returns true
/// </summary>
public class SetDialogueEncounterEffect : IEffect
{
    private readonly int townId;
    private readonly int encounterId;

    public SetDialogueEncounterEffect(int townId, int encounterId)
    {
        this.townId = townId;
        this.encounterId = encounterId;
    }

    public bool Apply()
    {
        DataTracker.Current.TownManager.GetTownById(townId).leaderDialogueEncounterId = encounterId;
        return true;
    }
}
