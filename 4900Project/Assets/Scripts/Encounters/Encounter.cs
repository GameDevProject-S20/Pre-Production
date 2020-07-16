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
    public class Encounter : IProgressor
    {
        private static int nextId = 0;

        /// <summary>
        /// Static id counter
        /// </summary>
        public int Id
        { get; }

        /// <summary>
        /// Name...
        /// </summary>
        public string Name
        { get; }

        /// <summary>
        /// String tag for categorizing encounters
        /// </summary>
        public string Tag
        { get; }

        /// <summary>
        /// Initial text presented to the player.
        /// </summary>
        public string BodyText
        { get; }

        /// <summary>
        /// Choice text presented to the player via buttons.
        /// </summary>
        public ReadOnlyCollection<string> ButtonText
        { get; }

        /// Result text dependent on the action selected by the player.
        /// Indexes correspond to ButtonText.
        /// </summary>
        public ReadOnlyCollection<string> ResultText
        { get; }

        /// <summary>
        /// Effects that should map to each dialogue choice.
        /// Indexes correspond to ButtonText.
        /// </summary>
        public ReadOnlyCollection<Action> Effects
        { get; }


        private ReadOnlyCollection<Condition> Conditions;
        
        /// <summary>
        /// Conditions that must be satisfied before the encounter will occur
        /// Intended for Fixed Encounters only
        /// </summary>
        public ReadOnlyCollection<Condition> EncounterRunConditions
        { get; }

        /// <summary>
        /// The town to be entered in order to trigger the encounter
        /// Intended for Fixed Encounters only
        /// </summary>
        private int? fixedEncounterTownId;

        private UnityAction<Condition> onConditionCompleteListener;
        private UnityAction<Town> onTownEnterListener;

        /// <summary>
        /// Used to indicate whether the encounter has passed its conditions or not
        /// </summary>
        private bool ready;

        protected List<IDPage> dialoguePages;
        protected List<IDButton> dialogueButtons;
        protected int dialogueStage;

        public Encounter(string name, string tag, string bodyText,
                         IEnumerable<string> buttonText, IEnumerable<string> resultText,
                         IEnumerable<Action> effects, IEnumerable<Condition> encounterRunConditions = default, int? fixedEncounterTownId = null)
        {
            Id = nextId++;  // static int id for now
            Name = name;
            Tag = tag;
            BodyText = bodyText;

            ButtonText = new ReadOnlyCollection<string>(new List<string>(buttonText));
            ResultText = new ReadOnlyCollection<string>(new List<string>(resultText));
            Effects = new ReadOnlyCollection<Action>(new List<Action>(effects));

            if (encounterRunConditions != null)
            {
                EncounterRunConditions = new ReadOnlyCollection<Condition>(new List<Condition>(encounterRunConditions));
            }

            if (fixedEncounterTownId.HasValue)
            {
                this.fixedEncounterTownId = fixedEncounterTownId;
            }


            if (ButtonText.Count != ResultText.Count || ButtonText.Count != Effects.Count)
            {
                throw new ArgumentException("buttonText, resultText, and effects must have the same length!");
            }

            dialoguePages = new List<IDPage>();
            dialogueButtons = new List<IDButton>();
            dialogueStage = 0;   

            initDialogue();
        }


        public void RunEncounter()
        {
            DisallowProgression();
            //EncounterManager.Instance.RemoveFixedEncounter(this);
            DialogueManager.Instance.CreateDialogue(dialoguePages);
        }

        // Debug purposes
        public override string ToString()
        {
            var sb = new StringBuilder("Encounter {\n");
            sb.AppendLine($"\tId = {Id}");
            sb.AppendLine($"\tName = {Name}");
            sb.AppendLine($"\tTag = {Tag}");
            sb.AppendLine($"\tBodyText = {BodyText}");

            for (int i = 0; i < ButtonText.Count; i++)
            { sb.AppendLine($"\tButton {i} = {ButtonText[i]}"); }

            for (int i = 0; i < ResultText.Count; i++)
            { sb.AppendLine($"\tResult {i} = {ResultText[i]}"); }

            sb.AppendLine("}");

            return sb.ToString();
        }

        /// <summary>
        /// Builds the dialogue tree
        /// </summary>
        private void initDialogue()
        {

            // Generic button to end dialogue
            IDButton endBtn = new DButton()
            {
                Text = "[Close]",
                OnButtonClick = DFunctions.CloseDialogue
            };

            // This loop builds buttons by adding text,
            // then dynamically sets the text of the next page (to allow branching.)
            // Also invokes the associated lambda function.
            for (int i = 0; i < ButtonText.Count; i++)
            {
                // Need to copy local variable for the closure to work
                // Otherwise throws index out of bounds
                int idx = i;
                dialogueButtons.Add(new DButton()
                {
                    Text = ButtonText[idx],
                    OnButtonClick = () =>
                    {
                        dialoguePages[++dialogueStage].Text = ResultText[idx];
                        Effects[idx]();  // call effect function
                        DFunctions.GoToNextPage();
                    }
                });
            }

            // Initial page
            dialoguePages.Add(new DPage()
            {
                Text = $"{Name}\n\n{BodyText}",
                Buttons = dialogueButtons
            });

            // Resolution page
            dialoguePages.Add(new DPage()
            {
                Text = "",
                Buttons = new List<IDButton>()
                {
                    endBtn
                }
            });
        }

        public void AllowProgression()
        {
            // Add town enter listener
            if (fixedEncounterTownId.HasValue)
            {
                if (onTownEnterListener == null)
                {
                    onTownEnterListener = (Town t) =>
                    {
                        Debug.Log(string.Format("On Town Enter: {0} caught by encounter {1}", t.Name, Name));
                        if (t.Id == fixedEncounterTownId.Value && ready)
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
                    onConditionCompleteListener = (Condition _) =>
                    {
                        if (Conditions.All(c => c.IsSatisfied))
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
                if (!fixedEncounterTownId.HasValue)
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
        }
    }
}
