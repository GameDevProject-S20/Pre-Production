using System.Collections.Generic;
using System.Linq;
using Dialogue;
using SIEvents;
using UnityEngine.Events;

namespace Encounters
{
    /// <summary>
    /// Encounter data structure
    /// Takes in data and builds a dialogue structure, that can be presented to the player.
    /// </summary>
    public class Encounter
    {
        private static int nextId = 0;

        public int Id { get; }

        public Dialogue.Dialogue Dialogue { get; }

        public List<Condition> Conditions;

        private UnityAction<Condition> onConditionCompleteListener;

        private Encounter(Dialogue.Dialogue dialogue, List<Condition> conditions)
        {
            Id = nextId++;  // static int id for now
            Dialogue = dialogue;
            Conditions = conditions;

            onConditionCompleteListener = (Condition c) =>
            {
                if (conditions.Contains(c)) onConditionCompleteHandler();
            };
            EventManager.Instance.OnConditionComplete.AddListener(onConditionCompleteListener);
            Conditions.ForEach(c => c.AllowProgression());
        }

        private void onConditionCompleteHandler()
        {
            if (Conditions.All(c => c.IsSatisfied))
            {
                DialogueManager.Instance.StartDialogue(dialogue);
            }
        }

        public class Builder
        {
            private Dialogue.Dialogue dialogue;
            private List<Condition> conditions;

            private Builder() { }

            public Builder(Dialogue.Dialogue dialogue)
            {
                this.dialogue = dialogue;
                conditions = new List<Condition>();
            }

            public Builder AddCondition(Condition condition)
            {
                this.conditions.Add(condition);
                return this;
            }
        }
    }
}
