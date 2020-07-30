using System.Collections.Generic;
using System;
using System.Linq;
using SIEvents;

/// <summary>
/// Does not give a lot of information, as Apply always returns true
/// </summary>
public class TriggerEventEffect : IEffect
{
    private readonly string name;

    public TriggerEventEffect(string name)
    {
        this.name = name;
    }

    public bool Apply()
    {
        EventManager.Instance.OnDialogueSelected.Invoke(name);
        return true;
    }
}
