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
    public class Encounter : IProgressor
    {
        /// <summary>
        /// Static id counter
        /// </summary>
        [SerializeField]
        public int Id;

        [SerializeField]
        public IDialogue Dialogue = null;

        /// <summary>
        /// Conditions that must be satisfied before the encounter will occur
        /// Intended for Fixed Encounters only
        /// </summary>
        public List<Condition> Conditions = null;

        /// <summary>
        /// The town to be entered in order to trigger the encounter
        /// Intended for Fixed Encounters only
        /// </summary>
        public int? FixedEncounterTownId = null;

        private UnityAction<Condition> onConditionCompleteListener;
        private UnityAction<Town> onTownEnterListener;

        /// <summary>
        /// Used to indicate whether the encounter has passed its conditions or not
        /// </summary>
        private bool ready;

        public Encounter() { }

        /*public Encounter(IEnumerable<Condition> encounterRunConditions = default, int? fixedEncounterTownId = null)
        {
            Effects = new ReadOnlyCollection<Action>(new List<Action>(effects));

            if (encounterRunConditions != null)
            {
                Conditions = new ReadOnlyCollection<Condition>(new List<Condition>(encounterRunConditions));
            }

            if (fixedEncounterTownId.HasValue)
            {
                this.fixedEncounterTownId = fixedEncounterTownId;
            }
        }*/


        public void RunEncounter()
        {
            DialogueManager.Instance.SetActive(Dialogue);
        }

        public void AllowProgression()
        {
            // Add town enter listener
            if (FixedEncounterTownId.HasValue)
            {
                if (onTownEnterListener == null)
                {
                    onTownEnterListener = (Town t) =>
                    {
                        Debug.Log(string.Format("On Town Enter: {0} caught by encounter {1}", t.Name, Id));
                        if (t.Id == FixedEncounterTownId.Value && ready)
                        {
                            RunEncounter();
                        }
                    };

                    EventManager.Instance.OnTownEnter.AddListener(onTownEnterListener);
                }
            }

            // Add condition listener
            if (Conditions != null && Conditions.Count > 0)
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
                // IF there are no conditions nor towns required, this will run as soon as AllowProgression is called
                ready = true;
                if (!FixedEncounterTownId.HasValue)
                {
                    RunEncounter();
                }
            }
        }

        public void DisallowProgression()
        {
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
            return string.Format("Encounter {0}\n\nTown: {1}\nConditions: [\n{2}\n]\nDialogue: {{\n{3}}}", Id, FixedEncounterTownId.Value, string.Join(",\n", Conditions.Select(c => string.Format("{{\n{0}\n}}", c))), Dialogue);
        }
    }
}
