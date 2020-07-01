﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class QuestManager : MonoBehaviour
{
    private static QuestManager _current;
    public static QuestManager current { get { return _current; } }

    public static UnityEvent Loaded = new UnityEvent();

    public void Awake()
    {
        if (_current != null && _current != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _current = this;
            Loaded.Invoke();
        }
    }

    Dictionary<string, Quest> inactiveQuests = new Dictionary<string, Quest>();
    Dictionary<string, Quest> activeQuests = new Dictionary<string, Quest>();
    Dictionary<string, Quest> readyQuests = new Dictionary<string, Quest>();
    Dictionary<string, Quest> completedQuests = new Dictionary<string, Quest>();
    Dictionary<string, Quest> allQuests = new Dictionary<string, Quest>();

    [System.Serializable]
    public class TransactionEvent : UnityEvent<string, int> { };
    public TransactionEvent transactionEvent;

    [System.Serializable]
    public class DialogueEvent : UnityEvent<string> { };
    public DialogueEvent dialogueEvent;

    [System.Serializable]
    public class QuestEvent : UnityEvent<string> { };
    public QuestEvent questCompleteEvent;

    public QuestEvent questObjectiveCompleted;

    public QuestEvent questActivated;
    public QuestEvent questProgressed;

    public void AddQuest(Quest quest)
    {
        string qn = quest.Name;
        inactiveQuests.Add(qn, quest);
        allQuests.Add(qn, quest);
    }
}