using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using UnityEngine;
using Dialogue;
using SIEvents;
using UnityEngine.Events;

namespace Encounters
{
    /// <summary>
    /// Encounter data structure
    /// Takes in data and builds a dialogue structure, that can be presented to the player.
    /// </summary>
    [System.Serializable]
    public abstract class Encounter 
    {
        /// <summary>
        /// Static id counter
        /// </summary>
        [SerializeField]
        public int Id;

        [SerializeField]
        public IDialogue Dialogue = null;

        public abstract void RunEncounter();
    }

    /// <summary>
    /// Encounters to be activated outside of Event nodes.
    /// </summary>
    public class FixedEncounter : Encounter, IProgressor
    {
        /// <summary>
        /// The town to be entered in order to trigger the encounter
        /// Intended for Fixed Encounters only
        /// </summary>
        public int FixedEncounterTownId;

        /// <summary>
        /// Conditions that must be satisfied before the encounter can occur
        /// </summary>
        public List<Condition> Conditions = null;

        private UnityAction<Condition> onConditionCompleteListener;
        private UnityAction<Town> onTownEnterListener;

        /// <summary>
        /// Used to indicate whether the encounter has passed its conditions or not
        /// </summary>
        private bool ready;

        /// <summary>
        /// Set this as the active dialogue.
        /// </summary>
        public override void RunEncounter()
        {
            if (!ready) throw new Exception(string.Format("Encounter {0} not ready.", Id));
            Dialogue.Show();
        }

        public void AllowProgression()
        {
            bool hasConditions = Conditions != null && Conditions.Count > 0;

            // Add town enter listener
            if (onTownEnterListener == null && FixedEncounterTownId != -1)
            {
                onTownEnterListener = (Town t) =>
                {
                    Debug.Log(string.Format("On Town Enter: {0} caught by encounter {1}", t.Name, Id));
                    if (t.Id == FixedEncounterTownId && ready)
                    {
                        RunEncounter();
                    }
                };

                EventManager.Instance.OnTownEnter.AddListener(onTownEnterListener);
            }       

            // Add condition listener
            if (hasConditions)
            {
                if (onConditionCompleteListener == null)
                {
                    // Add listener
                    onConditionCompleteListener = (Condition c) =>
                    {
                        if (!Conditions.Contains(c)) return;

                        if (Conditions.All(cond => cond.IsSatisfied))
                        {
                            ready = true;
                        }
                    };

                    EventManager.Instance.OnConditionComplete.AddListener(onConditionCompleteListener);

                    // Enable conditions
                    foreach (var c in Conditions)
                    {
                        c.AllowProgression();
                    }
                }
            }
            else
            {
                ready = true;
            }
        }

        public void DisallowProgression()
        {
            ready = false;

            if (onConditionCompleteListener != null)
            {
                EventManager.Instance.OnConditionComplete.RemoveListener(onConditionCompleteListener);
            }

            if (onTownEnterListener != null)
            {
                EventManager.Instance.OnTownEnter.RemoveListener(onTownEnterListener);
            }

            // ARL -- should this be elsewhere? Look how Quest handles this
            EventManager.Instance.OnEncounterComplete.Invoke(this);
        }

        public override string ToString()
        {
            return string.Format("Encounter {0}\n\nTown: {1}\nConditions: [\n{2}\n]\nDialogue: {{\n{3}}}", Id, FixedEncounterTownId, string.Join(",\n", Conditions.Select(c => string.Format("{{\n{0}\n}}", c))), Dialogue);
        }
    }

    /// <summary>
    /// Encounters that belong in a bag, to be selected from using some randomness.
    /// </summary>
    public class RandomEncounter : Encounter
    {
        /// <summary>
        /// This encounter should only be run if all conditions are met
        /// </summary>
        public List<IPresentCondition> Conditions = null;

        public string[] Tags;

        public bool IsRunnable()
        {
            return Conditions == null || Conditions.All(c => c.IsSatisfied());
        }

        public override void RunEncounter()
        {
            if (!IsRunnable())
            {
                throw new Exception(string.Format("Random Encounter with id {0} is not ready to run.", Id));
            }
            Dialogue.Show();
        }
    }
}
