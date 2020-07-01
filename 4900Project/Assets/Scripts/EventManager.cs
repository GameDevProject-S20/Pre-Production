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

    public class GenericEvent : UnityEvent<string>{};
    public GenericEvent onEventTrigger;

    public class TransactionEvent : UnityEvent<string, int>{};
    public TransactionEvent onTransaction;

    public class TownEnterEvent : UnityEvent<int>{};
    public TownEnterEvent onTownEnter;

    public class DialogueSelectionEvent : UnityEvent<string>{};
    public DialogueSelectionEvent onDialogueSelect;

    public UnityEvent onInventoryChange;

}
