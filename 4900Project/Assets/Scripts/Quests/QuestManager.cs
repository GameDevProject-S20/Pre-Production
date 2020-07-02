using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;


public class QuestManager
{
    private static QuestManager instance;

    public static QuestManager Instance
    {
        get
        {
            if (instance == null) instance = new QuestManager();
            return instance;
        }
    }

    private QuestManager() {}

    //quest lists
    Dictionary<string, Quest> inactiveQuests = new Dictionary<string, Quest>();
    Dictionary<string, Quest> activeQuests = new Dictionary<string, Quest>();
    Dictionary<string, Quest> readyQuests = new Dictionary<string, Quest>();
    Dictionary<string, Quest> completedQuests = new Dictionary<string, Quest>();
    Dictionary<string, Quest> allQuests = new Dictionary<string, Quest>();

    //events 
    public EventManager.TransactionEvent transactionEvent;
    public EventManager.DialogueSelectionEvent dialogueEvent;
    public EventManager.QuestEvent QuestCompleteEvent = new EventManager.QuestEvent();
    public EventManager.QuestEvent QuestObjectiveCompleted = new EventManager.QuestEvent();
    public EventManager.QuestEvent QuestActivated = new EventManager.QuestEvent();
    public EventManager.QuestEvent QuestProgressed = new EventManager.QuestEvent();
    
    void Awake(){
        transactionEvent = EventManager.Current.onTransaction; 
    }

    //add quest to list
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

        if (QuestActivated != null)
        {
            QuestActivated.Invoke(quest);
        }
    }


    // load quest from inactive list to active
    public void StartQuest(string questName)
    {
        if (inactiveQuests.ContainsKey(questName))
        {
            activeQuests.Add(questName, inactiveQuests[questName]);
            inactiveQuests.Remove(questName);
        }
    }


    //IComplete a quest and shift it to the proper spot
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

    //get values
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
