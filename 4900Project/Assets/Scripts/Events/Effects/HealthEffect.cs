using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEffect : IEffect
{
    private readonly double percent;

    public HealthEffect(double percent)
    {
        this.percent = percent;
    }

    public bool Apply()
    {
        int currHealth = Player.Instance.health;
        float toAdd = currHealth * (float)percent;
        Player.Instance.addHealth(Mathf.RoundToInt(toAdd));
        return true;
    }
}
