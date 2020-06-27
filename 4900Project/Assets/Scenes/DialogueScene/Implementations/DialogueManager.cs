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
    public static class DialogueManager
    {
        private static List<IDialogue> dialogs = new List<IDialogue>();

        /// <summary>
        /// Creates a new dialog box with the given pages. 
        /// </summary>
        /// <param name="dialoguePages"></param>
        public static IDialogue CreateDialog(IEnumerable<IDPage> dialoguePages)
        {
            // The ID of the dialog will be the next index into our dialogs list.
            // Create the dialog for that ID, and add it to the list
            IDialogue dialog = new Dialogue(dialogs.Count, dialoguePages);
            dialogs.Add(dialog);

            // Return the new dialog
            return dialog;
        }

        /// <summary>
        /// Retrieves a Dialogue object, given its ID.
        /// </summary>
        /// <param name="dialogId"></param>
        /// <returns></returns>
        public static IDialogue GetDialog(int dialogId)
        {
            // If the dialog doesn't exist, return null
            if (dialogId >= dialogs.Count)
            {
                return null;
            }

            // Otherwise, return the dialog
            return dialogs[dialogId];
        }

        public static bool DialogExists(int dialogId)
        {
            return dialogs.ElementAtOrDefault(dialogId) != null;
        }
    }
}
