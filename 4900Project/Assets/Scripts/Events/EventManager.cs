using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Quests;

public class EventManager
{
    public static EventManager Instance
    {
        get
        {
            if (instance == null) instance = new EventManager();
            return instance;
        }
    }

    private static EventManager instance;

    //=======================================================//

    [System.Serializable]
    public class GenericEvent : UnityEvent<string>{};
    public GenericEvent onEventTrigger;

    public partial class Transaction
    {
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

            public string ToString()
            {
                return string.Format("{0} x{1}, from {2} to {3}", ItemName, ItemCount, From, To);
            }
        }
    }

    //ARL Todo: Return to OnTransaction
    public List<UnityAction<Transaction.Details>> OnTransactionHandlers = new List<UnityAction<Transaction.Details>>();
    public Transaction.Event OnTransaction = new Transaction.Event();

    [System.Serializable]
    public class TownEnterEvent : UnityEvent<int>{};
    public TownEnterEvent onTownEnter;

    [System.Serializable]
    public class DialogueSelectionEvent : UnityEvent<string>{};
    public DialogueSelectionEvent onDialogueSelect;

    [System.Serializable]
    public class QuestEvent : UnityEvent<Quest> { };
    public QuestEvent onQuestUpdate = new QuestEvent();

    public UnityEvent onInventoryChange;

    private EventManager()
    {
        OnTransaction.AddListener((Transaction.Details d) =>
        {
            for (int i = 0; i < OnTransactionHandlers.Count; i++)
            {
                OnTransactionHandlers[i](d);
            }

        });
    }

}
