using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Linq;

namespace Quests
{
    public class Quest
    {
        private static int Ids = 0;
        public int Id { get; }
        public string Name;
        public string Description;

        public int CurrentStage { get; private set; }
        public List<Stage> stages = new List<Stage>();
        public bool IsCompleted { get; private set; } = false;
        //public static UnityEvent OnStageCompletion = new UnityEvent();
        public class QuestComplete : UnityEvent<Quest> { }
        public QuestComplete OnQuestComplete;

        // Currently not used
        public class QuestProgressed : UnityEvent<Quest> { }
        public QuestProgressed OnQuestProgressed;

        public struct DebugPipe
        {
            public UnityAction<Quest> OnQuestComplete { get; }
            public UnityAction<Stage> OnStageComplete { get; }
            public UnityAction<Condition> OnConditionComplete { get; }

            public DebugPipe(UnityAction<Quest> onQuestComplete, UnityAction<Stage> onStageComplete, UnityAction<Condition> onConditionComplete)
            {
                OnQuestComplete = onQuestComplete;
                OnStageComplete = onStageComplete;
                OnConditionComplete = onConditionComplete;
            }
        }

        private Quest()
        {
            Id = Ids++;
            CurrentStage = 0;
            OnQuestComplete = new QuestComplete();
            OnQuestProgressed = new QuestProgressed();
            //OnStageCompletion.AddListener(HandleStageComplete);
        }

        public void OnStageCompleteHandler()
        {
            if (IsCompleted)
                return;

            // ALREADY HANDLED IN STAGE
            // Check if all conditions in this step have been met
            /*Stage stage = stages[CurrentStage];
            foreach (Condition qc in stage.conditions)
            {
                if (!qc.IsSatisfied)
                {
                    return;
                }
            }*/

            // Apply effects of current stage
            /*Action<Trader> effect;
            if (stageEffects.TryGetValue(stage, out effect))
            {
                effect(player);// Invoking function
            }*/

            // Call the Check handler unique for each instance of Quest


            // Advance to the next stage
            CurrentStage++;
            stages[CurrentStage-1].SetActive();

            /*if (CurrentStage != stages.Count)
            {
                EventManager.Instance.OnTransaction.RemoveAllListeners();
                foreach (TransactionCondition condition in stages[CurrentStage].conditions)
                {
                    Debug.Log("Adding the next stage listener");
                    EventManager.Instance.OnTransaction.AddListener((EventManager.Transaction.Details details) => condition.DefaultHandler(details.ItemName, details.ItemCount));
                }
            }*/


            // If we are at the last stage, complete the quest
            if (CurrentStage == stages.Count)
            {
                IsCompleted = true;
                OnQuestComplete.Invoke(this);
            }
        }

        public bool IsComplete()
        {
            return IsCompleted;
        }

        /*void AdvanceToStage(int stage)
        {
            CurrentStage = stage;
        }*/

        public void AddStage(Stage stage)
        {
            stages.Add(stage);
            if (stages.Count == 1)
            {
                stages[0].SetActive();
            }
        }

        public override string ToString()
        {
            return string.Join("\n", stages.ConvertAll(s => string.Format("{0} {1}\n\t{2}", (s.Complete) ? "✓" : " ", s.Description, string.Join("\n\t", s.conditions.ConvertAll(c => string.Format("{0} {1}", (c.IsSatisfied) ? "✓" : " ", c))))));
        }

        public partial class Builder
        {
            private string name;
            private string description;
            private List<Stage.Builder> stageBuilders;
            private QuestComplete onComplete;
            //private bool TurnInButton;
            //private string TownName;

            public Builder(string name)
            {
                this.name = name;
                stageBuilders = new List<Stage.Builder>();
                onComplete = new QuestComplete();
            }

            public Builder SetDescription(string description)
            {
                this.description = description;
                return this;
            }

            public Builder AddStage(Stage.Builder stage)
            {
                stageBuilders.Add(stage);
                return this;
            }

            public Builder AddOnCompleteListener(UnityAction<Quest> listener)
            {
                this.onComplete.AddListener(listener);
                return this;
            }

            //Todo: Add stage

            /*public Builder SetTurnInButton(bool uses)
            {
                this.TurnInButton = uses;
                return this;
            }

            public Builder SetCorrespondingTownName(string name)
            {
                this.TownName = name;
                return this;
            }*/

            public Quest Build()
            {
                if (stageBuilders.Count == 0) throw new ArgumentException("Quest Requires at least one stage.");

                if (name == null) throw new ArgumentNullException("Name cannot be null.");

                Quest q = new Quest();
                q.Name = name;
                if (description != null) q.Description = description;

                // Set quest complete listeners
                q.OnQuestComplete = onComplete;

                // Add stage complete handlers
                stageBuilders.ForEach(b => q.AddStage(b.AddOnCompleteListener(s => q.OnStageCompleteHandler()).Build()));

                QuestManager.Instance.AddQuest(q);
                QuestManager.Instance.StartQuest(q.Name);

                //if (TownName != null) q.TownName = TownName;
                //q.TurnInButton = TurnInButton;
                return q;
            }
        }

    }



    public class Stage
    {
        public partial class StageComplete : UnityEvent<Stage> { };
        public StageComplete OnComplete;

        public List<Condition> conditions = new List<Condition>();
        public string Description { get; }
        public bool Complete { get; private set; }

        private Stage(string description)
        {
            Description = description;
            OnComplete = new StageComplete();
            Complete = false;
        }

        public void SetActive()
        {
            conditions.ForEach(c => c.SetActive());
        }

        public void OnConditionCompleteHandler()
        {
            if (conditions.All(c => c.IsSatisfied))
            {
                Complete = true;
                OnComplete.Invoke(this);
            }
        }

        public partial class Builder
        {
            private string description;
            private List<Condition> conditions;
            private StageComplete onComplete;

            private Builder() { }

            public Builder(string description)
            {
                this.description = description;
                conditions = new List<Condition>();
                onComplete = new StageComplete();
            }

            public Builder AddCondition(Condition c)
            {
                conditions.Add(c);
                return this;
            }
            public Builder AddOnCompleteListener(UnityAction<Stage> onComplete)
            {
                this.onComplete.AddListener(onComplete);
                return this;
            }

            public Stage Build()
            {
                if (conditions.Count == 0)
                {
                    throw new Exception("Stage must have at least 1 condition");
                }

                Stage stage = new Stage(description);

                // Set condition complete handlers
                conditions.ForEach(cdn => cdn.OnComplete.AddListener(c => stage.OnConditionCompleteHandler()));
                stage.conditions = conditions;

                // Set listeners
                if (onComplete != null) stage.OnComplete = onComplete;
                return stage;
            }
        }
    }


    // Quest conditions listen for events from the QuestManager
    // When they're satisfied, they notify the quest to check for completion
    public abstract class Condition
    {
        public bool IsSatisfied { get; private set; } = false;
        public string Description;
        public class ConditionComplete : UnityEvent<Condition> { }
        public ConditionComplete OnComplete;

        public Condition(string _description)
        {
            OnComplete = new ConditionComplete();
            Description = _description;
        }

        // Use this to turn listeners on
        public abstract void SetActive();

        protected void Satisfy()
        {
            IsSatisfied = true;
            Cleanup();
            OnComplete.Invoke(this);
        }

        // Only here in order to get around generic complications by creating a builder
        public Condition AddListener(UnityAction<Condition> listener)
        {
            OnComplete.AddListener(listener);
            return this;
        }

        public abstract override string ToString();

        // Used to remove listeners
        public abstract void Cleanup();
    }

    // Transaction condition. Listens for buy / sell events.
    // Selling with subtract from buying and vice versa.
    public class TransactionCondition : Condition
    {
        protected string ItemName;
        public enum TransactionTypeEnum { BUY, SELL };
        protected TransactionTypeEnum TransactionType;
        protected int RequiredCount = 0;
        protected int CurrentCount = 0;
        protected UnityAction<EventManager.Transaction.Details> transactionAction;

        public TransactionCondition(string _description, string _itemName, int _requiredCount, TransactionTypeEnum _transactionType)
         : base(_description)
        {
            ItemName = _itemName;
            RequiredCount = _requiredCount;
            TransactionType = _transactionType;
            if (TransactionType == TransactionTypeEnum.SELL)
            {
                _requiredCount = -RequiredCount;
            }

            // Wanted to add a list of listeners to Condition class, but it involves a lot of type generics that make the code messy
            transactionAction = new UnityAction<EventManager.Transaction.Details>((EventManager.Transaction.Details details) => Handler(details)); 
        }

        public override void SetActive()
        {
            DataTracker.Current.EventManager.OnTransactionHandlers.Add(transactionAction)/*OnTransaction.AddListener(transactionAction)*/;
        }

        protected TransactionTypeEnum GetTransactionType(EventManager.Transaction.Entity from, EventManager.Transaction.Entity to)
        {
            if (from == to) throw new ArgumentException("From and To cannot be the same source");
            return (to == EventManager.Transaction.Entity.PLAYER) ? TransactionTypeEnum.BUY : TransactionTypeEnum.SELL;
        }

        // As is, you could feasibly immediately buy the item back from the store after completing the quest
        protected virtual void Handler(EventManager.Transaction.Details details)
        {
            if (details.ItemName == ItemName && GetTransactionType(details.From, details.To) == TransactionType)
            {
                CurrentCount += details.ItemCount;
                if (CurrentCount == RequiredCount)
                {
                    Satisfy();
                }
            }
        }
        
        public override void Cleanup()
        {
            DataTracker.Current.EventManager.OnTransactionHandlers.Remove(transactionAction);//OnTransaction.RemoveListener(transactionAction);
        }

        public override string ToString()
        {
            return string.Format("{0} {1}: {2}/{3}", (TransactionType == TransactionTypeEnum.BUY) ? "Buy" : "Sell", ItemName, CurrentCount, RequiredCount);
        }
    }


    public class LocationSpecificTransactionCondition : TransactionCondition
    {
        private int ReqLocId;

        public LocationSpecificTransactionCondition(string _description, string _itemName, int _requiredCount,
               TransactionTypeEnum _transactionType, int _locId)
               : base(_description, _itemName, _requiredCount, _transactionType)
        {
            ReqLocId = _locId;
            Debug.Log(string.Format("{0} vs {1}", _locId, ReqLocId));
        }

        protected override void Handler(EventManager.Transaction.Details details)
        {
            if (details.ItemName == ItemName && GetTransactionType(details.From, details.To) == TransactionType)
            {
                Debug.Log(details.ToString());
                Debug.Log(string.Format("current: {0}, expected: {1}", TownManager.Instance.GetTownById(DataTracker.Current.currentLocationId).Name, TownManager.Instance.GetTownById(ReqLocId).Name));
            }
            // Should be a better way to look up currentLocation (should probably be in the player info)
            if (DataTracker.Current.currentLocationId != ReqLocId) return;
            base.Handler(details);
        }

        public override string ToString()
        {
            return string.Format("{0} {1} in [{2}]{3}: {4}/{5}", (TransactionType == TransactionTypeEnum.BUY) ? "Buy" : "Sell", ItemName, ReqLocId, TownManager.Instance.GetTownById(ReqLocId).Name, CurrentCount, RequiredCount);
        }
    }

    public class DialogueCondition : Condition
    {
        string ButtonText;

        public DialogueCondition(string _description, string _buttonText)
         : base(_description)
        {
            ButtonText = _buttonText;

            //Todo: Use EventManager
        }

        //Todo
        public override void SetActive()
        {
            throw new NotImplementedException();
        }

        public void Handler(string buttonText)
        {
            if (buttonText == this.ButtonText)
            {
                Satisfy();
            }
        }

        // Todo
        public override void Cleanup()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
