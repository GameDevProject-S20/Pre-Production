using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEffect : IEffect
{
    private readonly double percent;

    public HealthEffect(double percent)
    {
        if (percent < -1.0f || percent > 1.0f)
        {
            throw new ArgumentException("percent must be between 0 and 1, inclusive");
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
