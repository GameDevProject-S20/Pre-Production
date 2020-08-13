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

        //=== Inventory Change =======================================//
        public Events.InventoryEvents.ChangeEvent OnInventoryChange = new Events.InventoryEvents.ChangeEvent();


        //=== Town ===================================================//

        public Events.TownEvents.EnterEvent OnTownEnter = new Events.TownEvents.EnterEvent();
        public Events.TownEvents.ClickDialogueEvent OnOpenDialogueClick = new Events.TownEvents.ClickDialogueEvent();
        public Events.TownEvents.UpdatedEvent OnTownUpdated = new Events.TownEvents.UpdatedEvent();

        //=== Encounters ============================================//

        public Events.EncounterEvents.EncounterComplete OnEncounterComplete = new Events.EncounterEvents.EncounterComplete();
        public Events.EncounterEvents.EncounterTrigger OnEncounterTrigger = new Events.EncounterEvents.EncounterTrigger();

        //=== Effects ===============================================//

        public Events.EffectEvents.GivenToPlayer OnGivenToPlayer = new Events.EffectEvents.GivenToPlayer();
        public Events.EffectEvents.TakenFromPlayer OnTakenFromPlayer = new Events.EffectEvents.TakenFromPlayer();

        //=== Quest ==================================================//

        public Events.QuestEvents.QuestAdded OnQuestAdded = new Events.QuestEvents.QuestAdded();
        public Events.QuestEvents.QuestComplete OnQuestComplete = new Events.QuestEvents.QuestComplete();
        public Events.QuestEvents.StageComplete OnStageComplete = new Events.QuestEvents.StageComplete();
        public Events.QuestEvents.ConditionComplete OnConditionComplete = new Events.QuestEvents.ConditionComplete();
        public Events.QuestEvents.QuestManagerUpdated OnQuestManagerUpdated = new Events.QuestEvents.QuestManagerUpdated();

        //=== Dialogue ===============================================//

        public Events.DialogueEvents.SelectionEvent OnDialogueSelected = new Events.DialogueEvents.SelectionEvent();
        public UnityEvent OnDialogueEnd = new UnityEvent();

        //=== Map ====================================================//

        public Events.MapEvents.LocationNodeEvent OnNodeArrive = new Events.MapEvents.LocationNodeEvent();
        public UnityEvent RequestRedraw = new UnityEvent();
        public Events.MapEvents.MapNodeEvent OnNodeMouseEnter = new Events.MapEvents.MapNodeEvent();
        public Events.MapEvents.MapNodeEvent OnNodeMouseDown = new Events.MapEvents.MapNodeEvent();
        public UnityEvent OnTravelStart = new UnityEvent();
        public Events.MapEvents.LocationIdEvent OnEnterTownButtonClick = new Events.MapEvents.LocationIdEvent();
        public Events.MapEvents.LocationIdEvent OnEnterPOIButtonClick = new Events.MapEvents.LocationIdEvent();
        public UnityEvent FreezeMap = new UnityEvent();
        public UnityEvent UnfreezeMap = new UnityEvent();
        public UnityEvent ForceUnfreezeMap = new UnityEvent(); 
        public Events.MapEvents.LocationIdEvent OnProbabilityChange = new Events.MapEvents.LocationIdEvent();
        public UnityEvent SetViewDefault = new UnityEvent();
        public UnityEvent SetViewProbability = new UnityEvent();
        public UnityEvent OnColourChange = new UnityEvent();

        //=== Player =================================================//

        public Events.PlayerEvents.HealthEvent OnHealthChange = new Events.PlayerEvents.HealthEvent();
        public Events.PlayerEvents.TravelTypeChangeEvent OnTravelTypeChanged = new Events.PlayerEvents.TravelTypeChangeEvent();

        //=== Escape Menu ===========================================//

        public UnityEvent EscapeMenuRequested = new UnityEvent();
        public UnityEvent EscapeMenuCloseRequested = new UnityEvent();
        public UnityEvent OnCreditsButtonClicked = new UnityEvent();

        //=== Campfire ==============================================//

        public UnityEvent OnCampfireStarted = new UnityEvent();
        public UnityEvent OnCampfireEnded = new UnityEvent();

        //=== Settings ==============================================//

        public Events.SettingsEvents.SettingsChangedEvent OnSettingsChanged = new Events.SettingsEvents.SettingsChangedEvent();

        //=== Time ==================================================//

        public Events.MiscEvents.TimeChangedEvent OnTimeChanged = new Events.MiscEvents.TimeChangedEvent();

        //=== Misc ===================================================//
        public UnityEvent onDataTrackerLoad = new UnityEvent();
    }
}
