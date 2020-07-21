using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialogue
{
    /// <summary>
    /// The DialogueManager class controls the creation of new dialogue systems.
    /// </summary>
    public class DialogueManager
    {
        private static DialogueManager instance;

        public static DialogueManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DialogueManager();
                }
                return instance;
            }
        }

        // Public Properties
        public DialogueUpdatedEvent ActiveDialogueChanged = new DialogueUpdatedEvent();

        // Private Member Variables
        /// <summary>
        /// List of every dialog in the system
        /// </summary>
        private List<IDialogue> dialogs = new List<IDialogue>();

        /// <summary>
        /// List of all the dialogs that are currently active. Only the most recent dialog will be shown,
        /// but it can use this when that dialog closes to open a new one.
        /// </summary>
        private List<IDialogue> activeDialogs = new List<IDialogue>();

        private DialogueManager() { }

        // Public Methods
        /// <summary>
        /// Creates a new dialog box with the given pages. 
        /// </summary>
        /// <param name="dialoguePages"></param>
        public IDialogue CreateDialogue(IDPage root)
        {
            // The ID of the dialog will be the next index into our dialogs list.
            // Create the dialog for that ID, and add it to the list
            IDialogue dialog = new Dialogue(dialogs.Count, root);
            dialogs.Add(dialog);

            // Hook into the events for updating the active dialog / firing dialog changes
            // When a dialog is opened, set it as the active dialog
            dialog.DialogueOpened.AddListener(() =>
            {
                // If it's already been added, remove the instance already in the system
                // So that we avoid duplicates
                if (activeDialogs.Contains(dialog))
                {
                    activeDialogs.Remove(dialog);
                }

                // Add it in at the end of the list, and set it as active
                activeDialogs.Add(dialog);
                UpdateActiveDialogue();
            });

            // When a dialog is closed, go back to the previous dialog as the active
            dialog.DialogueClosed.AddListener(() =>
            {
                // First, we need to remove it from our active list
                activeDialogs.Remove(dialog);
                UpdateActiveDialogue();
            });

            // When the dialog changes, redirect it to our ActiveDialogChanged event
            dialog.PageUpdated.AddListener(() =>
            {
                UpdateActiveDialogue();
            });

            // Return the new dialog
            return dialog;
        }

        public void SetActive(IDialogue dialog)
        {
            if (!dialogs.Contains(dialog))
            {
                throw new ArgumentException("Dialogue must be created using DialogueManager.Create()");
            }

            activeDialogs.Add(dialog);

            // Update with the new active dialog
            UpdateActiveDialogue();
        }

        /// <summary>
        /// Retrieves the active dialog. This is the last one to be opened, or null if no dialogs are currently active.
        /// </summary>
        /// <returns></returns>
        public IDialogue GetActiveDialogue()
        {
            return activeDialogs.LastOrDefault();
        }

        /// <summary>
        /// Retrieves a Dialogue object, given its ID.
        /// </summary>
        /// <param name="dialogId"></param>
        /// <returns></returns>
        public IDialogue GetDialogue(int dialogId)
        {
            // If the dialog doesn't exist, return null
            if (dialogId >= dialogs.Count)
            {
                return null;
            }

            // Otherwise, return the dialog
            return dialogs[dialogId];
        }

        /// <summary>
        /// Checks if a dialog exists, given its id.
        /// </summary>
        /// <param name="dialogId"></param>
        /// <returns></returns>
        public bool DialogueExists(int dialogId)
        {
            return dialogs.ElementAtOrDefault(dialogId) != null;
        }

        // Private Methods
        /// <summary>
        /// Updates which Dialog is currently active
        /// </summary>
        private void UpdateActiveDialogue()
        {
            ActiveDialogueChanged.Invoke();
        }
    }
}
