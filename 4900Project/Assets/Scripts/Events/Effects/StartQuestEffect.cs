using System.Collections.Generic;
using System;
using System.Linq;
using Extentions;

/// <summary>
/// Does not give a lot of information, as Apply always returns true
/// </summary>
public class StartQuestEffect : IEffect
{
    private readonly string questName;

    public StartQuestEffect(string questName)
    {
        if (!string.IsNullOrEmpty(questName))
        {
            questName = questName.ToTitleCase();
        }

        this.questName = questName;
    }

    public bool Apply()
    {
        DataTracker.Current.QuestManager.StartQuest(questName);
        return true;
    }
}
