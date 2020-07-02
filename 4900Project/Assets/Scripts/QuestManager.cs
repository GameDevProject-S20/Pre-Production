using System;
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

    public EventManager.TransactionEvent transactionEvent;


    public EventManager.DialogueSelectionEvent dialogueEvent;

 
    public EventManager.QuestEvent QuestCompleteEvent;

    public EventManager.QuestEvent QuestObjectiveCompleted;

    public EventManager.QuestEvent QuestActivated;
    public EventManager.QuestEvent QuestProgressed;
    
    public void AddQuest(Quest quest)
    {
        string qn = quest.Name;
        inactiveQuests.Add(qn, quest);
        allQuests.Add(qn, quest);

        quest.QuestComplete.AddListener(() =>
        {
            activeQuests.Remove(qn);
            readyQuests.Add(qn, quest);
            QuestObjectiveCompleted.Invoke(quest);
        });

        quest.QuestProgressed.AddListener(() =>
        {
            QuestProgressed.Invoke(quest);
        });

        QuestActivated.Invoke(quest);

    }

    public void StartQuest(string questName)
    {
        if (inactiveQuests.ContainsKey(questName))
        {
            activeQuests.Add(questName, inactiveQuests[questName]);
            inactiveQuests.Remove(questName);
        }
    }

    public void CompleteQuest(string questName)
    {
        GetQuest(questName, out Quest questData);
        completedQuests.Add(questName, questData);
        if(questData.TurnInButton)
        {
            readyQuests.Remove(questName);
        }
        else
        {
            activeQuests.Remove(questName);
        }

        QuestCompleteEvent.Invoke(questData);
    }

    public void GetReadyQuests(out IEnumerable<string> quests)
    {
        quests = readyQuests.Keys;
    }

    public bool GetQuest(string questName,out Quest questData)
    {
        return allQuests.TryGetValue(questName, out questData);
    }

    public IEnumerable<Quest> GetQuests()
    {
        return allQuests.Values;
    }



    // Can call this, but shouldn't need to after the journal has been initialized
    public void UpdateJournal()
    {
        QuestJournal.Instance.SyncQuests(completedQuests, activeQuests);
    }
}
