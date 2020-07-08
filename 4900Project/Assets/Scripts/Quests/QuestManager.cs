using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using SIEvents;

namespace Quests
{
    public class QuestManager
    {
        private static QuestManager instance;

        public static QuestManager Instance
        {
            get
            {
                if (instance == null) instance = new QuestManager();
                return instance;
            }
        }

        // Quest lists
        private Dictionary<string, Quest> inactiveQuests = new Dictionary<string, Quest>();
        private Quest activeQuest = null;
        private Dictionary<string, Quest> completedQuests = new Dictionary<string, Quest>();
        private Dictionary<string, Quest> allQuests = new Dictionary<string, Quest>();

        private QuestManager() 
        {
            Quest.OnQuestComplete.AddListener((Quest q) => CompleteQuest(q));
        }

        // Add quest to list
        public void AddQuest(Quest quest)
        {
            string qn = quest.Name;
            inactiveQuests.Add(qn, quest);
            allQuests.Add(qn, quest);
            EventManager.Instance.OnQuestAdded.Invoke(quest);
        }


        // Load quest from inactive list to active
        public void StartQuest(string questName)
        {
            if (activeQuest != null)
            {
                // Do we want progress for all quests at once?
                activeQuest.DisallowProgression();
            }

            if (GetQuest(questName, out Quest quest))
            {
                activeQuest = quest;
                inactiveQuests.Remove(questName);
                quest.AllowProgression();
            }
        }


        public Quest GetActiveQuest()
        {
            return activeQuest;
        }


        // I Complete a quest and shift it to the proper spot
        private void CompleteQuest(Quest quest)
        {
            quest.DisallowProgression();
            inactiveQuests.Remove(quest.Name);
            completedQuests.Add(quest.Name, quest);

            if (quest == activeQuest) // check only relevant if can make progress on more than one quest at once
            {
                if (inactiveQuests.Count > 0)
                {
                    activeQuest = inactiveQuests.First().Value;
                }
            }
        }

        //get values
        /*public void GetReadyQuests(out IEnumerable<string> quests)
        {
            quests = readyQuests.Keys;
        }*/

        public bool GetQuest(string questName, out Quest questData)
        {
            return allQuests.TryGetValue(questName, out questData);
        }

        public IEnumerable<Quest> GetQuests()
        {
            return allQuests.Values;
        }

        // Can call this, but shouldn't need to after the journal has been initialized
        public void UpdateJournal()
        {
            //ARL Todo: Fix up
            Dictionary<string, Quest> dict = new Dictionary<string, Quest>();
            dict.Add(activeQuest.Name, activeQuest);
            QuestJournal.Instance.SyncQuests(completedQuests, dict);
        }
    }
}
