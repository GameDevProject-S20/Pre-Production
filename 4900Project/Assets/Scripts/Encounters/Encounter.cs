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
    public class Encounter
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

        public IDialogue Dialogue { get; }

        /// <summary>
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

        private UnityAction<Condition> onConditionCompleteListener;

        protected List<IDPage> dialoguePages;
        protected List<IDButton> dialogueButtons;
        protected int dialogueStage;

        public Encounter(string name, string tag, string bodyText,
                         IEnumerable<string> buttonText, IEnumerable<string> resultText,
                         IEnumerable<Action> effects, IEnumerable<Condition> conditions = default)
        {
            Id = nextId++;  // static int id for now
            Name = name;
            Tag = tag;
            BodyText = bodyText;

            ButtonText = new ReadOnlyCollection<string>(new List<string>(buttonText));
            ResultText = new ReadOnlyCollection<string>(new List<string>(resultText));
            Effects = new ReadOnlyCollection<Action>(new List<Action>(effects));
            if (ButtonText.Count != ResultText.Count || ButtonText.Count != Effects.Count)
            {
                throw new ArgumentException("buttonText, resultText, and effects must have the same length!");
            }

            dialoguePages = new List<IDPage>();
            dialogueButtons = new List<IDButton>();
            dialogueStage = 0;
            initDialogue();


            // Setup Conditions
            if (Conditions != null)
            {
                Conditions = new ReadOnlyCollection<Condition>(new List<Condition>(conditions));
                onConditionCompleteListener = (Condition c) =>
                {
                    if (conditions.Contains(c)) runIfConditionsSatisfied();
                };

                EventManager.Instance.OnConditionComplete.AddListener(onConditionCompleteListener);

                foreach (var c in Conditions)
                {
                    c.AllowProgression();
                }
            }        
        }

        private bool runIfConditionsSatisfied()
        {
            if (Conditions.All(c => c.IsSatisfied))
            {
                DialogueManager.Instance.StartDialogue(Dialogue);
                return true;
            }
            return false;
        }

        public void Run()
        {
            if (!runIfConditionsSatisfied())
            {
                throw new System.Exception("Conditions not yet satisfied!");
            }
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
                Text = "Done.",
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
    }
}
