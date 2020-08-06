using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEffect : IEffect
{
    private readonly int mod;

    public HealthEffect(int mod)
    {
        if (mod == 0)
        {
            //throw new Exception("Useless Effect");
        }

        this.mod = mod;
    }

    public bool Apply()
    {
        int prevHealth = Player.Instance.Health;
        Player.Instance.AddHealth(mod);
        return (prevHealth + mod != Player.Instance.Health);
    }
}

public class HealthPercentEffect : IEffect
{
    private readonly int percent;

    public HealthPercentEffect(int perc)
    {
        if (perc == 0)
        {
            //throw new ArgumentException("Useless Effect");
        }
        else if (perc < -100 || perc > 100)
        {
            throw new ArgumentException("perc must be between -100 and 100, inclusive");
        }

        percent = perc;
    }

    public bool Apply()
    {
        int mod = Mathf.RoundToInt(((float)Player.Instance.HealthCap) * (percent/100.0f));
        mod = Mathf.Max(mod, 1-Player.Instance.HealthCap); //serves to not kill the player if they have full hp
        return new HealthEffect(mod).Apply();
    }
}

public class MaxHealthEffect : IEffect
{
    private readonly int mod;

    public MaxHealthEffect(int mod)
    {
        if (mod == 0)
        {
            //throw new Exception("Useless Effect");
        }

        this.mod = mod;
    }

    public bool Apply()
    {
        int prevMaxHealth = Player.Instance.HealthCap;
        Player.Instance.ModifyCap(mod);
        int changeHealth = 0;
        if(mod<0){
          if(prevMaxHealth+mod<Player.Instance.Health)
          {
            changeHealth = prevMaxHealth-Player.Instance.Health+mod;
          }
        }
        else
        {
          changeHealth = mod;
        }
        Player.Instance.AddHealth(changeHealth);
        return (prevMaxHealth + mod != Player.Instance.HealthCap);
    }
}

public class MaxHealthPercentEffect : IEffect
{
    private readonly int percent;

    public MaxHealthPercentEffect(int perc)
    {
        if (perc == 0)
        {
            //throw new ArgumentException("Useless Effect");
        }
        else if (perc < -100 || perc > 100)
        {
            throw new ArgumentException("perc must be between -100 and 100, inclusive");
        }
        else
        {
            percent = perc;
        }
    }

    public bool Apply()
    {
        int mod = Mathf.RoundToInt(((float)Player.Instance.HealthCap) * (percent/100.0f));
        return new MaxHealthEffect(mod).Apply();
    }
}
