using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

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

        private QuestManager() { }

        //quest lists
        Dictionary<string, Quest> inactiveQuests = new Dictionary<string, Quest>();
        Quest activeQuest = null;
        Dictionary<string, Quest> completedQuests = new Dictionary<string, Quest>();
        Dictionary<string, Quest> allQuests = new Dictionary<string, Quest>();

        //events 
        public EventManager.QuestEvent QuestCompleteEvent = new EventManager.QuestEvent();
        public EventManager.QuestEvent QuestObjectiveCompleted = new EventManager.QuestEvent();
        public EventManager.QuestEvent QuestActivated = new EventManager.QuestEvent();
        public EventManager.QuestEvent QuestProgressed = new EventManager.QuestEvent();

        //add quest to list
        public void AddQuest(Quest quest)
        {
            string qn = quest.Name;
            inactiveQuests.Add(qn, quest);
            allQuests.Add(qn, quest);

            quest.OnQuestComplete.AddListener((Quest q) => CompleteQuest(q.Name));

            quest.OnQuestProgressed.AddListener((Quest q) =>
            {
                QuestProgressed.Invoke(quest);
            });

            if (QuestActivated != null)
            {
                QuestActivated.Invoke(quest);
            }
        }


        // load quest from inactive list to active
        public void StartQuest(string questName)
        {
            if (activeQuest != null)
            {
                //Todo: Set inactive
            }

            if (GetQuest(questName, out Quest quest))
            {
                activeQuest = quest;
                inactiveQuests.Remove(questName);
            }
        }


        public Quest GetActiveQuest()
        {
            return activeQuest;
        }


        //IComplete a quest and shift it to the proper spot
        public void CompleteQuest(string questName)
        {
            GetQuest(questName, out Quest questData);
            completedQuests.Add(questName, questData);

            activeQuest = null;

            QuestCompleteEvent.Invoke(questData);
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
