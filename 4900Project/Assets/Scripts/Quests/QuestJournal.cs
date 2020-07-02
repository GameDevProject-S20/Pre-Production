using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;


public class QuestJournal
{
    public static QuestJournal Instance
    {
        get
        {
            if (instance == null) instance = new QuestJournal();
            return instance;
        }
    }

    public ReadOnlyCollection<Quest> ActiveQuests
    {
        get => activeQuests.AsReadOnly();
    }

    public ReadOnlyCollection<Quest> CompletedQuests
    {
        get => completedQuests.AsReadOnly();
    }

    private static QuestJournal instance = null;
    private List<Quest> activeQuests;
    private List<Quest> completedQuests;

    // Make sure has the most current QuestManager
    private QuestManager mgr
    {
        get => QuestManager.current;
    }

    private QuestJournal()
    {
        activeQuests = new List<Quest>();
        completedQuests = new List<Quest>();

        // Set up event listeners
        EventManager.Current.onQuestUpdate.AddListener(onQuestUpdate);

        requestQuests();
    }

    // On game load or something, make sure the class's collections are up to date
    public void SyncQuests(Dictionary<string, Quest> compQuests, Dictionary<string, Quest> actQuests)
    {
        completedQuests = compQuests.Values.ToList();
        activeQuests = actQuests.Values.ToList();
    }

    // Get the quest manager to send the completed/active quests
    private void requestQuests()
    {
        mgr.UpdateJournal();
    }

    // Add or remove it from the list
    private void onQuestUpdate(Quest quest)
    {
        if (quest.IsCompleted)
        {
            if (!completedQuests.Contains(quest))
            {
                completedQuests.Add(quest);
            }
            activeQuests.Remove(quest);
        }
        else
        {
            if (!activeQuests.Contains(quest))
            {
                activeQuests.Add(quest);
            }
        }
    }
}