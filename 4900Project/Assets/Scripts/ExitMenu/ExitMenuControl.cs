using Dialogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ExitMenu
{
    public class ExitMenuControl
    {
        protected static bool promptOpen;

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
            var menu = GetDialoguePages();
            DialogueManager.Instance.CreateDialogue(menu);
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

        /// <summary>
        /// Constructs the main prompt for the Exit Menu
        /// </summary>
        /// <returns></returns>
        protected static IEnumerable<IDPage> GetDialoguePages()
        {
            return new List<IDPage>()
            {
                new DPage()
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
                            OnButtonClick = () => {
                                ConfirmQuit();
                                DFunctions.CloseDialogue();
                            }
                        },

                        // Cancel button
                        new DButton()
                        {
                            Text = "No.",
                            OnButtonClick = () =>
                            {
                                CancelQuit();
                                DFunctions.CloseDialogue();
                            }
                        }
                    },

                    // Prompt text
                    Text = "Would you like to quit the game?"
                }
            };
        }
    }
}
