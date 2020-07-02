using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    private static EventManager _current;
    public static EventManager Current {get {return _current;}}

    private void Awake() {
        if (_current != null && _current != this)
        {
            Destroy(this.gameObject);
        } else {
            _current = this;
        }
    }

    //=======================================================//

    [System.Serializable]
    public class GenericEvent : UnityEvent<string>{};
    public GenericEvent onEventTrigger;

    [System.Serializable]
    public class TransactionEvent : UnityEvent<string, int>{};
    public TransactionEvent onTransaction;

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

}
