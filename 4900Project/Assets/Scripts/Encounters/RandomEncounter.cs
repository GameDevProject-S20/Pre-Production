using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using UnityEngine;
using Dialogue;


namespace Encounters
{
    public class RandomEncounter : Encounter
    {

        /// <summary>
        /// Result text to appear if the action chosen by the player cannot be completed
        /// IE. doesn't have the correct items in inventory
        /// </summary>
        public ReadOnlyCollection<string> FailText
        { get; }

        /// <summary>
        /// Conditions that must be met for an effect to resolve.
        /// Should return a bool.
        /// </summary>
        public ReadOnlyCollection<Func<bool>> Conditions
        { get; }

        /// <summary>
        /// Effects to execute if the player cannot complete an action
        /// </summary>
        public ReadOnlyCollection<Action> FailEffects
        { get; }

        public RandomEncounter(string name, string tag, string bodyText,
                         IEnumerable<string> buttonText, IEnumerable<string> resultText,
                         IEnumerable<Action> effects)
        : this(name, tag, bodyText, buttonText, resultText, effects,
               new Func<bool>[0], new String[0], new Action[0])
        { }


        public RandomEncounter(string name, string tag, string bodyText,
                         IEnumerable<string> buttonText, IEnumerable<string> resultText,
                         IEnumerable<Action> effects, IEnumerable<Func<bool>> conditions,
                         IEnumerable<string> failText, IEnumerable<Action> failEffects)
        : base(name, tag, bodyText, buttonText, resultText, effects)
        {
            // Default unspecified conditions to true
            var conds = new List<Func<bool>>(conditions);
            if (conds.Count < Effects.Count)
            {
                for (int i = conds.Count; i < Effects.Count; i++)
                {
                    conds.Add(() => true);
                }
            }
            Conditions = new ReadOnlyCollection<Func<bool>>(conds);

            // Failure Dialoge
            var failTxt = new List<string>(failText);
            if (failTxt.Count < ResultText.Count)
            {
                for (int i = failTxt.Count; i < ResultText.Count; i++)
                {
                    failTxt.Add("Action failed.");
                }
            }
            FailText = new ReadOnlyCollection<string>(failTxt);

            // Failure effects
            // Default to no effect if unspecified
            var failEffs = new List<Action>(failEffects);
            if (failEffs.Count < Effects.Count)
            {
                for (int i = failEffs.Count; i < Effects.Count; i++)
                {
                    failEffs.Add(() => {});
                }
            }
            FailEffects = new ReadOnlyCollection<Action>(failEffs);

            dialoguePages = new List<IDPage>();
            dialogueButtons = new List<IDButton>();
            dialogueStage = 0;
            initDialogue();
        }

        /// <summary>
        /// Builds the dialogue tree
        /// </summary>
        private void initDialogue()
        {
            Debug.Log("Dialogue init'd");
            // Generic button to end dialogue
            IDButton endBtn = new DButton()
            {
                Text = "Done.",
                OnButtonClick = () => {
                    DFunctions.CloseDialogue();
                    resetEncounter();
                }
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
                        // check conditions
                        if (Conditions[idx]())
                        {
                            dialoguePages[++dialogueStage].Text = ResultText[idx];
                            Effects[idx]();  // call effect function
                        }
                        else
                        {
                            dialoguePages[++dialogueStage].Text = FailText[idx];
                            FailEffects[idx]();
                        }
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

        private void resetEncounter()
        {
            dialogueStage = 0;
        }

    }
}