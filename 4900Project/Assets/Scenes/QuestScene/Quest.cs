using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Quest
{
    private static int Ids = 0;
    public int Id { get; }
    public string Name { get; set; }
    protected string Description;

    public int CurrentStage;
    public List<QuestStage> stages = new List<QuestStage>();
    public bool IsCompleted = false;
    public static UnityEvent OnCompletion = new UnityEvent();
    public UnityEvent QuestComplete = new UnityEvent();
    public UnityEvent QuestProgressed = new UnityEvent();

    // Player reference
    //public Trader Player = DataTracker.instance.player;

    public bool TurnInButton;
    public string TownName;

    public Quest()
    {
        Id = Ids++;
        CurrentStage = 0;
        OnCompletion.AddListener(CheckForCompletion);
    }

    public void CheckForCompletion()
    {
        if (IsCompleted)
            return;
        // Check if all conditions in this step have been met
        QuestStage stage = stages[CurrentStage];
        foreach (QuestCondition qc in stage.conditions)
        {
            if (!qc.IsSatisfied)
            {
                return;
            }
        }

        // Apply effects of current stage
        /*Action<Trader> effect;
        if (stageEffects.TryGetValue(stage, out effect))
        {
            effect(player);// Invoking function
        }*/

        // Advance to the next stage
        CurrentStage++;

        // If we are at the last stage, complete the quest
        if (CurrentStage == stages.Count)
        {
            IsCompleted = true;

            if (TurnInButton)
            {
                QuestComplete.Invoke();
            }
            else
            {
                Complete();
            }
        }
    }

    public bool Complete()
    {
        if (CurrentStage != stages.Count)
        {
            return false;
        }

        //QuestManager.current.CompleteQuest(name);
        return true;
    }

    void AdvanceToStage(int stage)
    {
        CurrentStage = stage;
    }

    public void AddStage(QuestStage stage)
    {
        stages.Add(stage);
    }

    public partial class Builder
    {
        string Name;
        string Description;
        bool TurnInButton;
        string TownName;

        public Builder(string name)
        {
            this.Name = name;
        }

        public Builder SetName(string displayName)
        {
            this.Name = displayName;
            return this;
        }

        public Builder SetDescription(string description)
        {
            this.Description = description;
            return this;
        }

        public Builder SetTurnInButton(bool uses)
        {
            this.TurnInButton = uses;
            return this;
        }

        public Builder SetCorrespondingTownName(string name)
        {
            this.TownName = name;
            return this;
        }

        public Quest Build()
        {
            if (Name == null) throw new ArgumentNullException("Name must be set prior to building.");

            Quest q = new Quest();
            q.Name = Name;
            if (Description != null) q.Description = Description;
            if (TownName != null) q.TownName = TownName;
            q.TurnInButton = TurnInButton;
            return q;
        }
    }

}



public class QuestStage
{
    public List<QuestCondition> conditions = new List<QuestCondition>();
    public string Description;
}


// Quest conditions listen for events from the QuestManager
// When they're satisfied, they notify the quest to check for completion
public abstract class QuestCondition
{
    public bool IsSatisfied = false;
    public string Description;
    protected static UnityEvent OnCompletion;

    public QuestCondition(string _description, UnityEvent _onCompletion)
    {
        Description = _description;
        Initialize();
        OnCompletion = _onCompletion;
    }

    public abstract void Initialize();
}


// Generic condition. Satisfied once it hears a certain event.
public class ListenForEventCondition : QuestCondition
{
    string EventName;
    int CurrentCount = 0;
    int RequiredCount = 1;


    public ListenForEventCondition(string _description, string _eventName, int _requiredCount, UnityEvent _onCompletion)
     : base(_description, _onCompletion)
    {
        EventName = _eventName;
        RequiredCount = _requiredCount;
    }

    public override void Initialize()
    {
     /*   QuestManager.current.GenericEvent.AddListener((string name) =>
        {
            if (Name == EventName)
            {
                currentCount += 1;
                isSatisfied = currentCount >= requiredCount;
                if (isSatisfied) onCompletion.Invoke();
            }
        });*/
    }
}

// Transaction condition. Listens for buy / sell events.
// Selling with subtract from buying and vice versa.
public class TransactionCondition : QuestCondition
{
    string ItemName;
    public enum TranscationTypeEnum { buy, sell };
    TranscationTypeEnum TransactionType;
    int RequiredCount = 0;
    int CurrentCount = 0;

    public TransactionCondition(string _description, string _itemName, int _requiredCount, TranscationTypeEnum _transactionType, UnityEvent _onCompletion)
     : base(_description, _onCompletion)
    {
        ItemName = _itemName;
        RequiredCount = _requiredCount;
        TransactionType = _transactionType;
        if (TransactionType == TranscationTypeEnum.sell)
        {
            _requiredCount = -RequiredCount;
        }
    }

    public override void Initialize()
    {
        //add when QM done
        //QuestManager.current.TransactionEvent.AddListener((string item, int count) => DefaultHandler(item, count));
    }

    protected void DefaultHandler(string item, int count)
    {
        if (item == ItemName)
        {
            CurrentCount += count;
            if (TransactionType == TranscationTypeEnum.sell)
            {
                IsSatisfied = CurrentCount <= RequiredCount;
            }
            else
            {
                IsSatisfied = CurrentCount >= RequiredCount;
            }
            if (IsSatisfied) OnCompletion.Invoke();
        }
    }

}


public class LocationSpecificTransactionCondition : TransactionCondition
{

    private int ReqNodeId;

    public LocationSpecificTransactionCondition(string _description, string _itemName, int _requiredCount,
           TranscationTypeEnum _transactionType, UnityEvent _onCompletion, int _nodeId)
           : base(_description, _itemName, _requiredCount, _transactionType, _onCompletion)
    {
        ReqNodeId = _nodeId;
    }

    public override void Initialize()
    {
       /* QuestManager.current.TransactionEvent.AddListener((string item, int count) =>
        {
            if (DataTracker.instance.player.CurrNodeId == reqNodeId)
            {
                // Transaction only counts if completed in required location
                DefaultHandler(item, count);
            }
        });*/
    }
}

public class DialogueCondition : QuestCondition
{
    string ButtonText;

    public DialogueCondition(string _description, string _buttonText, UnityEvent _onCompletion)
     : base(_description, _onCompletion)
    {
        ButtonText = _buttonText;
    }

    public override void Initialize()
    {
        //QuestManager.current.dialogueEvent.AddListener((string buttonText) => DefaultHandler(buttonText));
    }

    protected void DefaultHandler(string buttonText)
    {
        if (buttonText == this.ButtonText)
        {
            IsSatisfied = true;
            OnCompletion.Invoke();
        }
    }
}
