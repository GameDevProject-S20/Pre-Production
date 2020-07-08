using UnityEngine.Events;
using Quests;

namespace SIEvents
{
    public class Events
    {
        public partial class Transaction
        {
            [System.Serializable]
            public class Event : UnityEvent<Details> { };

            // Currently does not differentiate subsystems (shop v.s. any other NPC transaction entity are all grouped under "System")
            public enum Entity
            {
                PLAYER,
                SYSTEM
            }

            public readonly struct Details
            {
                public string ItemName { get; }
                public int ItemCount { get; }
                public int SystemId { get; } // currently shop id -> can be expanded on if more transaction types happen
                public Entity From { get; }
                public Entity To { get; }

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

        public partial class Town
        {
            [System.Serializable]
            public class EnterEvent : UnityEvent<Town> { };
        }

        public partial class Dialogue
        {
            [System.Serializable]
            public class SelectionEvent : UnityEvent<string> { };
        }

        public partial class Quest
        {
            [System.Serializable]
            public class QuestComplete : UnityEvent<Quests.Quest> { };
            [System.Serializable]
            public class StageComplete : UnityEvent<Quests.Stage> { };
            [System.Serializable]
            public class ConditionComplete : UnityEvent<Quests.Condition> { };
            [System.Serializable]
            public class QuestAdded : UnityEvent<Quests.Quest> { };
        }
    }
}

