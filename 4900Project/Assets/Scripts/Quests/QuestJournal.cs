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
                if (instance == null)
                {
                    instance = new QuestJournal();
                }
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
            EventManager.Instance.OnQuestManagerUpdated.AddListener(SyncQuests);

            SyncQuests();
        }

        // On game load or something, make sure the class's collections are up to date
        public void SyncQuests()
        {
            activeQuests = QuestManager.Instance.GetActiveQuests().ToList();
            completedQuests = QuestManager.Instance.GetCompletedQuests().ToList();
        }
    }
}