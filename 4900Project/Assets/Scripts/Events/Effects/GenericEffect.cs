using System.Collections.Generic;
using System;
using System.Linq;

/// <summary>
/// Does not give a lot of information, as Apply always returns true
/// </summary>
public class GenericEffect : IEffect
{
    private Action effect;

    public GenericEffect(Action effect)
    {
        this.effect = effect;
    }

    public bool Apply()
    {
        effect.Invoke();
        return true;
    }

    public static IEnumerable<GenericEffect> CreateEnumerable(params Action[] actions)
    {
        return actions.Select(a => new GenericEffect(a));
    }
}
