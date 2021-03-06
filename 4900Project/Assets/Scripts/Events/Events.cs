﻿using System;
using UnityEngine.Events;

namespace SIEvents
{
    /// <summary>
    /// Contains definitions for system-accessable events
    /// </summary>
    public class Events
    {
        public class TransactionEvents
        {
            [System.Serializable]
            public class Event : UnityEvent<Details> { };

            // Currently does not differentiate subsystems (shop v.s. any other NPC transaction entity are all grouped under "System")
            public enum Entity
            {
                PLAYER,
                SYSTEM // Represents any system entity, shop or otherwise
            }

            public readonly struct Details
            {
                public string ItemName { get; }
                public int ItemCount { get; }
                public int SystemId { get; } // currently shop id -> can be expanded on if more transaction types happen
                public Entity From { get; } // The transaction's selling party (e.g. Player)
                public Entity To { get; }   // The transaction's buying party (e.g. Store)

                public Details(string iname, int icount, int sysid, Entity ifrom, Entity ito)
                {
                    ItemName = iname;
                    ItemCount = icount;
                    SystemId = sysid;
                    From = ifrom;
                    To = ito;
                }

                public override string ToString()
                {
                    return string.Format("{0} x{1}, from {2} to {3}", ItemName, ItemCount, From, To);
                }
            }
        }

        public class InventoryEvents
        {
            [System.Serializable]
            public class ChangeEvent : UnityEvent { };
        }

        public class TownEvents
        {
            [System.Serializable]
            public class EnterEvent : UnityEvent<Town> { };
            [System.Serializable]
            public class ClickDialogueEvent : UnityEvent<int> { };
            [System.Serializable]
            public class UpdatedEvent : UnityEvent<Town> { };
        }

        public class EncounterEvents
        {
            [System.Serializable]
            public class EncounterComplete : UnityEvent<Encounters.Encounter> { };
            [System.Serializable]
            public class EncounterTrigger : UnityEvent<int> { };
        }

        public class EffectEvents
        {
            [System.Serializable]
            public class GivenToPlayer : UnityEvent<string, int> { };
            [System.Serializable]
            public class TakenFromPlayer : UnityEvent<string, int> { };
        }

        public class DialogueEvents
        {
            [System.Serializable]
            public class SelectionEvent : UnityEvent<string> { };
        }

        public class QuestEvents
        {
            [System.Serializable]
            public class QuestComplete : UnityEvent<Quests.Quest> { };
            [System.Serializable]
            public class StageComplete : UnityEvent<Quests.Quest, Quests.Stage> { };
            [System.Serializable]
            public class ConditionComplete : UnityEvent<SIEvents.Condition> { };
            [System.Serializable]
            public class QuestAdded : UnityEvent<Quests.Quest> { };
            [System.Serializable]
            public class QuestManagerUpdated : UnityEvent { };
        }

        public partial class MapEvents
        {
            // OverworldMap.LocationNode refers to a node in the data structure
            [System.Serializable]
            public class LocationNodeEvent : UnityEvent<OverworldMap.LocationNode> { };

            // MapNode refers to an object in the Map Scene corresponding to a LocationNode
            [System.Serializable]
            public class MapNodeEvent : UnityEvent<MapNode> { };

            public class LocationIdEvent : UnityEvent<int> { };

            //[System.Serializable]
            //public class NodeMouseDown : UnityEvent<MapNode> { };
        }

        public partial class PlayerEvents
        {
            [System.Serializable]
            public class HealthEvent : UnityEvent<int, int, int, string> { };
            public class TravelTypeChangeEvent : UnityEvent<DataTracker.TravelType> { };

        }

        public class SettingsEvents
        {
            [System.Serializable]
            public class SettingsChangedEvent : UnityEvent<string> { };
        }

        public partial class MiscEvents
        {
            [System.Serializable]
            public class TimeChangedEvent : UnityEvent<TimeSpan> { };
        }
    }
}
