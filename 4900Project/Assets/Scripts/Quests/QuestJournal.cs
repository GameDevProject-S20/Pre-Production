using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using SIEvents;

namespace Quests
{
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

        private QuestJournal()
        {
            activeQuests = new List<Quest>();
            completedQuests = new List<Quest>();

            // Set up event listeners
            EventManager.Instance.OnQuestComplete.AddListener(onQuestComplete);
            EventManager.Instance.OnQuestAdded.AddListener(onQuestAdded);

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
            DataTracker.Current.QuestManager.UpdateJournal();
        }

        // Remove it from the list
        private void onQuestComplete(Quest quest)
        {
            if (quest.IsCompleted)
            {
                if (!completedQuests.Contains(quest))
                {
                    completedQuests.Add(quest);
                }
                activeQuests.Remove(quest);
            }
        }

        // Add it to the list
        private void onQuestAdded(Quest quest)
        {
            if (!activeQuests.Contains(quest))
            {
                activeQuests.Add(quest);
            }
        }
    }
}