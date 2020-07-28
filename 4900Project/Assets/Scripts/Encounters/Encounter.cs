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

        public void RunEncounter()
        {
            if (!ready) throw new Exception("Encounter not ready.");
            Dialogue.Show();
        }

        public void AllowProgression()
        {
            bool isTownSpecificEncounter = FixedEncounterTownId.HasValue;
            bool hasConditions = Conditions != null && Conditions.Count > 0;

            // Add town enter listener
            if (isTownSpecificEncounter)
            {
                if (onTownEnterListener == null)
                {
                    onTownEnterListener = (Town t) =>
                    {
                        Debug.Log(string.Format("On Town Enter: {0} caught by encounter {1}", t.Name, Id));
                        if (t.Id == FixedEncounterTownId.Value)
                        {
                            RunEncounter();
                        }
                    };

                    EventManager.Instance.OnTownEnter.AddListener(onTownEnterListener);
                }
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
            
            if (!hasConditions)
            {
                ready = true;

                // IF there are no conditions nor towns required, this will run as soon as AllowProgression is called
                if (!isTownSpecificEncounter)
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
