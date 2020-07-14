using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Linq;
using SIEvents;

namespace Quests
{
    /// <summary>
    /// A Quest object that can toggle progression on and off
    /// </summary>
    public interface IQuest
    {
        // Set listeners
        //
        // Warning! 
        //  This may cause issues with duplicate listeners if called multiple times with no accompanying DisallowProgression call in between.
        //  UnityEvent is obfuscated, so it is currently impossible to tell without testing
        void AllowProgression();
        
        // Remove listeners
        void DisallowProgression();
    }

    /// <summary>
    /// A series of tasks for the player to complete
    /// </summary>
    public class Quest : IQuest
    {

        private static int Ids = 0;
        public int Id { get; }
        public string Name;
        public string Description;

        public int CurrentStage { get; private set; }
        public List<Stage> stages = new List<Stage>();
        public bool IsCompleted { get; private set; } = false;

        /// <summary>
        /// Intended only to be subscribed to by QuestManager
        /// </summary>
        public static Events.Quest.QuestComplete OnQuestComplete = new Events.Quest.QuestComplete();
        public static Events.Quest.StageComplete OnStageComplete = new Events.Quest.StageComplete();
        public static Events.Quest.ConditionComplete OnConditionComplete = new Events.Quest.ConditionComplete();

        private UnityAction<Stage> OnStageCompleteListener;

        private bool progressionAllowed;

        private Quest()
        {
            Id = Ids++;
            CurrentStage = 0;
            progressionAllowed = false;
        }

        public void AllowProgression()
        {
            if (progressionAllowed) return;

            if (OnStageCompleteListener == null)
            {
                OnStageCompleteListener = (Stage s) =>
                    {
                        if (stages[CurrentStage] == s) OnStageCompleteHandler(s);
                    };
            }
            
            EventManager.Instance.OnStageComplete.AddListener(OnStageCompleteListener);

            // Allow progression on current stage
            stages[CurrentStage].AllowProgression();
        }

        public void DisallowProgression()
        {
            if (!progressionAllowed) return;

            EventManager.Instance.OnStageComplete.RemoveListener(OnStageCompleteListener);

            // Disallow progression on current stage
            stages[CurrentStage].DisallowProgression();
        }

        private void OnStageCompleteHandler(Stage stage)
        {
            if (IsCompleted)
                return;

            //Todo: Move to Stage
            // Apply effects of current stage
            /*Action<Trader> effect;
            if (stageEffects.TryGetValue(stage, out effect))
            {
                effect(player);// Invoking function
            }*/

            OnStageComplete.Invoke(stage);

            // Advance to the next stage
            DisallowProgression();
            CurrentStage++; 

            // If we are at the last stage, complete the quest
            if (CurrentStage == stages.Count)
            {
                IsCompleted = true;
                OnQuestComplete.Invoke(this);
            }
            else
            {
                stages[CurrentStage].AllowProgression();
            }
        }

        public bool IsComplete()
        {
            return IsCompleted;
        }

        public void AddStage(Stage stage)
        {
            stages.Add(stage);
        }

        public override string ToString()
        {
            return string.Format("*** {0} *** {1}\n\n{2}\n\n{3}", Name, IsCompleted ? "[COMPLETE]" : "[IN PROGRESS]", Description,
                string.Join("\n\n", stages.ConvertAll(s => string.Format("[{0}] {1}\n\t{2}", (s.Complete) ? "✓" : " ", s.Description, string.Join("\n\t", s.conditions.ConvertAll(c => string.Format("{0} {1}", (c.IsSatisfied) ? "✓" : " ", c)))))));
        }

        /// <summary>
        /// Builds a new Quest object
        /// </summary>
        public partial class Builder
        {
            private string name;
            private string description;
            private List<Stage.Builder> stageBuilders;

            public Builder(string name)
            {
                this.name = name;
                stageBuilders = new List<Stage.Builder>();
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

            public Quest Build()
            {
                if (stageBuilders.Count == 0) throw new ArgumentException("Quest Requires at least one stage.");

                if (name == null) throw new ArgumentNullException("Name cannot be null.");

                Quest q = new Quest();
                q.Name = name;
                if (description != null) q.Description = description;

                // Add stages
                stageBuilders.ForEach(b => q.AddStage(b.Build()));

                QuestManager.Instance.AddQuest(q);
                QuestManager.Instance.StartQuest(q.Name);

                return q;
            }
        }

    }

    /// <summary>
    /// Subsections of a Quest
    /// </summary>
    public class Stage : IQuest
    {
        public List<Condition> conditions = new List<Condition>();
        public string Description { get; }
        public bool Complete { get; private set; }

        private UnityAction<Condition> OnConditionCompleteListener;

        private Stage() { }
        private Stage(string description)
        {
            Description = description;
            Complete = false;
        }

        public void AllowProgression()
        {
            if (OnConditionCompleteListener == null)
            {
                OnConditionCompleteListener = (Condition c) => 
                    { 
                        if (conditions.Contains(c)) OnConditionCompleteHandler(); 
                    };
            }
            EventManager.Instance.OnConditionComplete.AddListener(OnConditionCompleteListener);
            conditions.ForEach(c => c.AllowProgression());
        }

        public void DisallowProgression()
        {
            EventManager.Instance.OnConditionComplete.RemoveListener(OnConditionCompleteListener);
        }

        public void OnConditionCompleteHandler()
        {
            if (conditions.All(c => c.IsSatisfied))
            {
                Complete = true;
                EventManager.Instance.OnStageComplete.Invoke(this);
            }
        }

        /// <summary>
        /// Builds a new Stage object
        /// </summary>
        public partial class Builder
        {
            private string description;
            private List<Condition> conditions;

            private Builder() { }

            public Builder(string description)
            {
                this.description = description;
                conditions = new List<Condition>();
            }

            public Builder AddCondition(Condition c)
            {
                conditions.Add(c);
                return this;
            }

            public Stage Build()
            {
                if (conditions.Count == 0)
                {
                    throw new Exception("Stage must have at least 1 condition");
                }

                Stage stage = new Stage(description);
                
                //conditions.ForEach(cdn => cdn.OnComplete.AddListener(c => stage.OnConditionCompleteHandler()));
                stage.conditions = conditions;

                return stage;
            }
        }
    }

    /// <summary>
    /// Specific functional conditions that make up a Quest Stage
    /// 
    /// They listen for events from the QuestManager and mark themselves as satisfied accordingly
    /// </summary>
    public abstract class Condition : IQuest
    {
        public bool IsSatisfied { get; private set; } = false;
        public string Description;

        private Condition() { }
        public Condition(string _description)
        {
            Description = _description;
        }
        public abstract void AllowProgression();

        public abstract void DisallowProgression();

        protected void Satisfy()
        {
            IsSatisfied = true;
            DisallowProgression();
            EventManager.Instance.OnConditionComplete.Invoke(this);
        }

        public abstract override string ToString();
    }

    // Transaction condition. Listens for buy / sell events.
    // Selling with subtract from buying and vice versa.
    public class TransactionCondition : Condition
    {
        public enum TransactionTypeEnum { BUY, SELL };

        private readonly string ItemName;
        private readonly TransactionTypeEnum transactionType;
        private readonly int requiredCount;
        private int currentCount;
        private readonly int? ReqLocId;

        protected UnityAction<Events.Transaction.Details> transactionAction;

        public TransactionCondition(string _description, string _itemName, int _requiredCount, TransactionTypeEnum _transactionType, int? requiredLocationId = null)
         : base(_description)
        {
            ItemName = _itemName;
            currentCount = 0;
            requiredCount = _requiredCount;
            transactionType = _transactionType;
            ReqLocId = requiredLocationId;
        }

        public override void AllowProgression()
        {
            if (transactionAction == null)
            {
                // Wanted to add a list of listeners to Condition class, but it involves a lot of type generics that make the code messy
                transactionAction = new UnityAction<Events.Transaction.Details>((Events.Transaction.Details details) => Handler(details));
            }
            EventManager.Instance.OnTransaction.AddListener(transactionAction);
        }

        public override void DisallowProgression()
        {
            EventManager.Instance.OnTransaction.RemoveListener(transactionAction);
        }

        /// <summary>
        /// Used to translate between Transaction Event types defined in the Event System and in this Condition
        /// </summary>
        /// <param name="from">The seller</param>
        /// <param name="to">The buyer</param>
        /// <returns>Either BUY or SELL, from the player's perspective</returns>
        protected TransactionTypeEnum GetTransactionType(Events.Transaction.Entity from, Events.Transaction.Entity to)
        {
            if (from == to) throw new ArgumentException("From and To cannot be the same source");
            return (to == Events.Transaction.Entity.PLAYER) ? TransactionTypeEnum.BUY : TransactionTypeEnum.SELL;
        }

        // As is, you could feasibly immediately buy the item back from the store after completing the quest
        protected virtual void Handler(Events.Transaction.Details details)
        {
            if (details.ItemName == ItemName && GetTransactionType(details.From, details.To) == transactionType)
            {
                currentCount += details.ItemCount;
                if (currentCount >= requiredCount)
                {
                    Satisfy();
                }
            }
        }
       
        public override string ToString()
        {
            return string.Format("{0} {1}{2}: {3}/{4}", (transactionType == TransactionTypeEnum.BUY) ? "Buy" : "Sell", ItemName, (ReqLocId.HasValue) ? " " + TownManager.Instance.GetTownById(ReqLocId.Value).Name : "", currentCount, requiredCount);
        }
    }

    public class VisitCondition : Condition
    {
        //Potentially have a targetType, and use this Condition to visit either any specific town, or any specific type of town.
        OverworldMap.LocationNode target;
        private UnityAction<OverworldMap.LocationNode> visitAction;
        public VisitCondition(string _description, OverworldMap.LocationNode targetNode)
         : base(_description)
        {
            target = targetNode;
        }

        public override void AllowProgression()
        {
            visitAction = new UnityAction<OverworldMap.LocationNode>((OverworldMap.LocationNode target) => Handler(target));
            EventManager.Instance.onNodeVisit.AddListener(visitAction);
        }

        public override void DisallowProgression()
        {
            EventManager.Instance.onNodeVisit.RemoveListener(visitAction);
        }
        protected virtual void Handler(OverworldMap.LocationNode location)
        {
            if (target.Id == location.Id)
            {
                Satisfy();
            }
        }

        public override string ToString()
        {
            return string.Format("Condition: {0}, visit node {1} is {3}.", Description, target, (IsSatisfied ? "satisfied" : "not satisfied"));
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
        public override void AllowProgression()
        {
            throw new NotImplementedException();
        }

        //Todo
        public override void DisallowProgression()
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

        //Todo
        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
