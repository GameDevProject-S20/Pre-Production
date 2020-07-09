using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Quests;

namespace SIEvents
{
    /// <summary>
    ///  Maintains all system-accessible events as defined in Events.cs
    /// </summary>
    public class EventManager
    {
        public static EventManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EventManager();
                }
                return instance;
            }
        }

        private static EventManager instance;

        //=== Transaction ============================================//

        public Events.Transaction.Event OnTransaction = new Events.Transaction.Event();

        //=== Town ===================================================//

        public Events.Town.EnterEvent OnTownEnter = new Events.Town.EnterEvent();

        //=== Quest ==================================================//

        public Events.Quest.QuestComplete OnQuestComplete = new Events.Quest.QuestComplete();
        public Events.Quest.StageComplete OnStageComplete = new Events.Quest.StageComplete();
        public Events.Quest.ConditionComplete OnConditionComplete = new Events.Quest.ConditionComplete();
        public Events.Quest.QuestAdded OnQuestAdded = new Events.Quest.QuestAdded();
    }
}
