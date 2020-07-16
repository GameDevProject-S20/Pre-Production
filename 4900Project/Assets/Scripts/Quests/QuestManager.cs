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
                if (instance == null)
                {
                    instance = new QuestManager();
                }
                return instance;
            }
        }

        private class QuestContainer
        {
            public enum State
            {
                INACTIVE,
                ACTIVE,
                COMPLETE
            }

            public Quest Quest;
            public State ActiveState;

            public QuestContainer(Quest quest)
            {
                Quest = quest;
                ActiveState = State.INACTIVE;
            }

            public void SetActive()
            {
                ActiveState = State.ACTIVE;
                Quest.AllowProgression();
            }

            public void SetComplete()
            {
                ActiveState = State.COMPLETE;
                Quest.DisallowProgression();
            }
        }

        // Quest lists
        private Dictionary<string, QuestContainer> quests = new Dictionary<string, QuestContainer>();

        private QuestManager() 
        {
            Quest.OnQuestComplete.AddListener((Quest q) => CompleteQuest(q));
        }

        public Quest GetQuest(string name)
        {
            QuestContainer questContainer;
            quests.TryGetValue(name, out questContainer);
            return questContainer.Quest;
        
        }

        // Add new inactive quest
        public void AddQuest(Quest quest)
        {
            string qn = quest.Name;
            quests.Add(qn, new QuestContainer(quest));
            EventManager.Instance.OnQuestManagerUpdated.Invoke();
        }


        // Set inactive quest as active
        public void StartQuest(string questName)
        {
            if (GetQuestState(questName, out QuestContainer questState))
            {
                if (questState.ActiveState == QuestContainer.State.INACTIVE)
                {
                    questState.SetActive();
                    EventManager.Instance.OnQuestManagerUpdated.Invoke();
                }
                else
                {
                    throw new Exception("Quest must be inactive when starting.");
                }
            }
            else
            {
                throw new ArgumentException("Quest Not Found");
            }
        }

        // Complete active quest
        private void CompleteQuest(Quest quest)
        {
            if (GetQuestState(quest.Name, out QuestContainer questState))
            {
                if (questState.ActiveState == QuestContainer.State.ACTIVE)
                {
                    questState.SetComplete();
                    EventManager.Instance.OnQuestComplete.Invoke(quest);
                    EventManager.Instance.OnQuestManagerUpdated.Invoke();
                }
                else
                {
                    throw new Exception("Quest must be active when completing.");
                }
            }
            else
            {
                throw new ArgumentException("Quest Not Found");
            }
        }

        // Get all quests that are either INACTIVE, ACTIVE, or COMPLETE
        private IEnumerable<Quest> GetQuests(QuestContainer.State withState)
        {
            return quests.Values.Where(v => v.ActiveState == withState).Select(qs => qs.Quest);
        }

        public IEnumerable<Quest> GetActiveQuests()
        {
            return GetQuests(QuestContainer.State.ACTIVE);
        }

        public IEnumerable<Quest> GetInctiveQuests()
        {
            return GetQuests(QuestContainer.State.INACTIVE);
        }

        public IEnumerable<Quest> GetCompletedQuests()
        {
            return GetQuests(QuestContainer.State.COMPLETE);
        }

        private bool GetQuestState(string name, out QuestContainer questState)
        {
            return quests.TryGetValue(name, out questState);
        }
    }
}
