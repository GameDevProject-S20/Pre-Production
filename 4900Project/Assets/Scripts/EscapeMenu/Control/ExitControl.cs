using Dialogue;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ExitMenu
{
    public class ExitControl : MonoBehaviour
    {
        protected static bool promptOpen;
        protected static readonly IDialogue exitDialogue = DialogueManager.Instance.CreateDialogue(QuitPage);

        /// <summary>
        /// Pops up the prompt for quitting the game.
        /// </summary>
        public static void BringUpExitMenu()
        {
            // If the prompt is already open, exit out - don't want multiple prompts
            if (promptOpen)
            {
                return;
            }

            // Set that the prompt is open
            promptOpen = true;

            // Pops up a new Dialogue with the exit prompt
            exitDialogue.Show();
        }

        /// <summary>
        /// Confirms a quit request.
        /// </summary>
        protected static void ConfirmQuit()
        {
            promptOpen = false;
            UnityEngine.Debug.Log("The player has quit the game");


            // If running from the editor: Cancel play mode
            // Otherwise: Quit the game
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        /// <summary>
        /// Cancels a quit request.
        /// </summary>
        protected static void CancelQuit()
        {
            promptOpen = false;
            UnityEngine.Debug.Log("Player has cancelled the quit request");
        }

        protected static IDPage QuitPage
        {
            get => new DPage()
            {
                // No avatar on this one ... unless?
                Avatar = null,

                // Buttons
                Buttons = new List<IDButton>()
                    {
                        // Confirm button
                        new DButton()
                        {
                            Text = "Yes.",
                            Effects = GenericEffect.CreateEnumerable(
                                () => ConfirmQuit()
                            )
                        },

                        // Cancel button
                        new DButton()
                        {
                            Text = "No.",
                            Effects = GenericEffect.CreateEnumerable(
                                () => CancelQuit()
                            )
                        }
                    },

                // Prompt text
                Text = "Would you like to quit the game?"
            };
        }
    }
}
