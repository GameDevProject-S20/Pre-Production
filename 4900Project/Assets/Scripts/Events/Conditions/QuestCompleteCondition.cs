using System;
using UnityEngine.Events;
using Quests;

namespace SIEvents
{
    public class QuestCompleteCondition : Condition
    {
        private readonly int questId;
        private readonly UnityAction<Quest> listener;

        public QuestCompleteCondition(string _description, int questId)
         : base(_description)
        {
            this.questId = questId;
            listener = new UnityAction<Quest>((Quest quest) => Handler(quest.Id));
        }

        public override void AllowProgression()
        {
            EventManager.Instance.OnQuestComplete.AddListener(listener);
        }

        public override void DisallowProgression()
        {
            EventManager.Instance.OnQuestComplete.RemoveListener(listener);
        }

        public void Handler(int questId)
        {
            if (questId == this.questId)
            {
                Satisfy();
            }
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}