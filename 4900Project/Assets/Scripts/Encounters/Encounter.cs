using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using UnityEngine;
using Dialogue;


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
        ///
        /// <summary>
        /// Choice text presented to the player via buttons.
        /// </summary>
        public ReadOnlyCollection<string> ButtonText
        { get; }

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

        protected List<IDPage> dialoguePages;
        protected List<IDButton> dialogueButtons;
        protected int dialogueStage;

        public Encounter(string name, string tag, string bodyText,
                         IEnumerable<string> buttonText, IEnumerable<string> resultText,
                         IEnumerable<Action> effects)
        : this(name, tag, bodyText, buttonText, resultText, effects,
               new Func<bool>[0], new String[0], new Action[0])
        { }

        public Encounter(string name, string tag, string bodyText,
                         IEnumerable<string> buttonText, IEnumerable<string> resultText,
                         IEnumerable<Action> effects, IEnumerable<Func<bool>> conditions,
                         IEnumerable<string> failText, IEnumerable<Action> failEffects)
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
        }

        /// <summary>
        /// Call this to start the encounter, by displaying the dialogue to the player
        /// </summary>
        public void StartDialogue()
        {
            IDialogue dialogue = DialogueManager.CreateDialogue(dialoguePages);
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
