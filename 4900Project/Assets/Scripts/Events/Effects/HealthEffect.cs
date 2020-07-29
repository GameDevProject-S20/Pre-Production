using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEffect : IEffect
{
    private readonly double percent;

    public HealthEffect(double perc)
    {
        if (perc < -1.0f || perc > 1.0f)
        {
            throw new ArgumentException("perc must be between -1.0 and 1.0, inclusive");
        }else
        {
            percent = perc;
        }
    }

    public bool Apply()
    {
        int maxHealth = Player.Instance.healthCap;
        float delta = maxHealth * (float)percent;
        Player.Instance.addHealth(Mathf.RoundToInt(delta));
        return true;
    }
}
