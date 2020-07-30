using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEffect : IEffect
{
    private readonly int mod;

    public HealthEffect(int mod)
    {
        this.mod = mod;
    }

    public bool Apply()
    {
        int prevHealth = Player.Instance.Health;
        Player.Instance.ModifyCap(mod);
        return (prevHealth + mod != Player.Instance.Health);
    }
}

public class HealthPercentEffect : IEffect
{
    private readonly float percent;

    public HealthPercentEffect(double perc)
    {
        if (perc < -1.0f || perc > 1.0f)
        {
            throw new ArgumentException("perc must be between -1.0 and 1.0, inclusive");
        }
        else
        {
            percent = (float)perc;
        }
    }

    public bool Apply()
    {
        int mod = Mathf.RoundToInt(Player.Instance.Health * percent);
        return new HealthEffect(mod).Apply();
    }
}

public class MaxHealthEffect : IEffect
{
    private readonly int mod;

    public MaxHealthEffect(int mod)
    {
        this.mod = mod;
    }

    public bool Apply()
    {
        int prevMaxHealth = Player.Instance.HealthCap;
        Player.Instance.ModifyCap(mod);
        return (prevMaxHealth + mod != Player.Instance.HealthCap);
    }
}

public class MaxHealthPercentEffect : IEffect
{
    private readonly float percent;

    public MaxHealthPercentEffect(double perc)
    {
        if (perc < -1.0f || perc > 1.0f)
        {
            throw new ArgumentException("perc must be between -1.0 and 1.0, inclusive");
        }
        else
        {
            percent = (float)perc;
        }
    }

    public bool Apply()
    {
        int mod = Mathf.RoundToInt(Player.Instance.HealthCap * percent);
        return new MaxHealthEffect(mod).Apply();
    }
}

