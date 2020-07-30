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
    /// A series of tasks for the player to complete
    /// </summary>
    public class Quest : IProgressor
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
        public static Events.QuestEvents.QuestComplete OnQuestComplete = new Events.QuestEvents.QuestComplete();
        public static Events.QuestEvents.StageComplete OnStageComplete = new Events.QuestEvents.StageComplete();
        public static Events.QuestEvents.ConditionComplete OnConditionComplete = new Events.QuestEvents.ConditionComplete();

        private UnityAction<Quest, Stage> OnStageCompleteListener;

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
                OnStageCompleteListener = (Quest _, Stage s) =>
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

            OnStageComplete.Invoke(this, stage);

            // Advance to the next stage
            DisallowProgression();
            CurrentStage++; 

            // If we are at the last stage, complete the quest
            if (CurrentStage == stages.Count)
            {
                IsCompleted = true;
                Debug.Log("Completed Quest: " + this.Name);
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

                // Link stages to quest and add them
                stageBuilders.ForEach(b =>
                {
                    q.AddStage(b.SetParentQuest(q).Build());
                });

                QuestManager.Instance.AddQuest(q);
                //QuestManager.Instance.StartQuest(q.Name);

                return q;
            }
        }

    }

    /// <summary>
    /// Subsections of a Quest
    /// </summary>
    public class Stage : IProgressor
    {
        public List<Condition> conditions = new List<Condition>();
        public string Description { get; }
        public bool Complete { get; private set; }

        private Quest parentQuest;

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
                EventManager.Instance.OnStageComplete.Invoke(parentQuest, this);
            }
        }

        /// <summary>
        /// Builds a new Stage object
        /// </summary>
        public partial class Builder
        {
            private string description;
            private List<Condition> conditions;
            private Quest parentQuest;

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

            public Builder SetParentQuest(Quest q)
            {
                parentQuest = q;
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

                stage.parentQuest = parentQuest;

                return stage;
            }
        }
    }
}
