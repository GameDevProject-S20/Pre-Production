using Encounters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleRandomEncountersEffect : IEffect
{
    private readonly bool on;

    public ToggleRandomEncountersEffect(bool on)
    {
        this.on = on;
    }

    public bool Apply()
    {
        EncounterManager.Instance.ToggleRandomEncounters(on);
        return EncounterManager.Instance.RandomEncountersOn == on;
    }
}
