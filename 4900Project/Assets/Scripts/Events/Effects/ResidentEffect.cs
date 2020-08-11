using System.Collections.Generic;
using System;
using System.Linq;

/// <summary>
/// Does not give a lot of information, as Apply always returns true
/// </summary>
public class ResidentEffect : IEffect
{
    private readonly string addOrRemove;
    private readonly string residentName;
    private readonly string townName;

    public ResidentEffect(string addOrRemove, string residentName, string townName)
    {
        this.addOrRemove = addOrRemove;
        this.residentName = residentName;
        this.townName = townName;
    }

    public bool Apply()
    {
        Town t = TownManager.Instance.GetTownByName(townName);
        Resident r = TownManager.Instance.GetResident(residentName);
        if (addOrRemove.ToLower() == "add"){
            t.AddResident(r);
        }
        else {
            t.RemoveResident(r); 
        }
        return true;
    }
}
