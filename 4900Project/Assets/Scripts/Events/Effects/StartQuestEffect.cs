using System.Collections.Generic;
using System;
using System.Linq;

/// <summary>
/// Does not give a lot of information, as Apply always returns true
/// </summary>
public class StartQuestEffect : IEffect
{
    private readonly string questName;

    public StartQuestEffect(string questName)
    {
        this.questName = questName;
    }

    public bool Apply()
    {
        DataTracker.Current.QuestManager.StartQuest(questName);
        return true;
    }
}
