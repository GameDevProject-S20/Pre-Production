using System;
using System.Diagnostics;

public class RandomEffect : IEffect
{
    private static Random random = new System.Random();
    private IEffect effect;

    public RandomEffect(IEffect first, IEffect second, double percentFirst)
    {
        if (percentFirst < 0.0f || percentFirst > 1.0f)
        {
            throw new ArgumentException("percentFirst must be between 0 and 1, inclusive");
        }

        effect = (random.NextDouble() <= percentFirst) ? first : second;

        UnityEngine.Debug.Log("RandomEffect: " + effect);
    }

    public bool Apply()
    {
        return effect.Apply();
    }
}
