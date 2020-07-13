using System;
using UnityEngine.Events;
using Quests;

namespace SIEvents
{
    public class StageCompleteCondition : Condition
    {
        private readonly int questId;
        private readonly int stageNum;
        private readonly UnityAction<Quest, Stage> listener;

        public StageCompleteCondition(string _description, int questId, int stageNum)
         : base(_description)
        {
            this.questId = questId;
            this.stageNum = stageNum;
            listener = new UnityAction<Quest, Stage>((Quest quest, Stage stage) => Handler(quest.Id, quest.stages.IndexOf(stage)));
        }

        public override void AllowProgression()
        {
            EventManager.Instance.OnStageComplete.AddListener(listener);
        }

        public override void DisallowProgression()
        {
            EventManager.Instance.OnStageComplete.RemoveListener(listener);
        }

        public void Handler(int qusetId, int stageNum)
        {
            if (questId == this.questId && stageNum == this.stageNum)
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