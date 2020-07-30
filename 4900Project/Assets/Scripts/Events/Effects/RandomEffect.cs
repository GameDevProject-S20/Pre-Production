using System;
using System.Diagnostics;
using UnityEngine;

public class RandomEffect : IEffect
{
    private static System.Random random = new System.Random();
    private IEffect effect;
    private IEffect unusedEffect;
    private Tuple<int, int> chance;

    public RandomEffect(IEffect first, IEffect second, double percentFirst)
    {
        if (percentFirst < 0.0f || percentFirst > 1.0f)
        {
            throw new ArgumentException("percentFirst must be between 0 and 1, inclusive");
        }

        float chanceEffect, chanceUnused;
        if (random.NextDouble() <= percentFirst)
        {
            effect = first;
            unusedEffect = second;

            chanceEffect = (float) percentFirst * 100;
            chanceUnused = (float) 100 - chanceEffect;
        }
        else
        {
            unusedEffect = first;
            effect = second;

            chanceUnused = (float) percentFirst * 100;
            chanceEffect = (float) 100 - chanceUnused;
        }

        chance = new Tuple<int, int>(Mathf.RoundToInt(chanceEffect), Mathf.RoundToInt(chanceUnused));
    }

    public bool Apply()
    {
        return effect.Apply();
    }

    // The chosen effect will always be displayed first
    public override string ToString()
    {
        return string.Format("Random Effect: {0}% ({1}) {2}% ({3})", effect, chance.Item1, unusedEffect, chance.Item2);  
    }
}
