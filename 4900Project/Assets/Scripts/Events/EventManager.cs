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

        public Events.TransactionEvents.Event OnTransaction = new Events.TransactionEvents.Event();

        //=== Town ===================================================//

        public Events.TownEvents.EnterEvent OnTownEnter = new Events.TownEvents.EnterEvent();

        //=== Encounters ============================================//

        //public Events.EncounterLocation.EncounterEvent OnEncounterEnter = new Events.EncounterLocation.EncounterEvent();
        public Events.EncounterEvents.TriggerEncounterEvent TriggerEncounter = new Events.EncounterEvents.TriggerEncounterEvent();

        //=== Quest ==================================================//

        public Events.QuestEvents.QuestAdded OnQuestAdded = new Events.QuestEvents.QuestAdded();
        public Events.QuestEvents.QuestComplete OnQuestComplete = new Events.QuestEvents.QuestComplete();
        public Events.QuestEvents.StageComplete OnStageComplete = new Events.QuestEvents.StageComplete();
        public Events.QuestEvents.ConditionComplete OnConditionComplete = new Events.QuestEvents.ConditionComplete();
        public Events.QuestEvents.QuestManagerUpdated OnQuestManagerUpdated = new Events.QuestEvents.QuestManagerUpdated();

        //=== Dialogue ===============================================//

        public Events.DialogueEvents.SelectionEvent onDialogueSelected = new Events.DialogueEvents.SelectionEvent();

        //=== Map ====================================================//

        public Events.MapEvents.NodeEvent OnNodeEnter = new Events.MapEvents.NodeEvent();
        public UnityEvent RequestRedraw = new UnityEvent();


        //=== Misc ===================================================//
        public UnityEvent onDataTrackerLoad = new UnityEvent();
    }
}
