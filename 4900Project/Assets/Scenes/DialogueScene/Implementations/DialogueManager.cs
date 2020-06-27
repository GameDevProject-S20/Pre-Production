using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialogue {
    /// <summary>
    /// The DialogueManager class controls the creation of new dialogue systems.
    /// </summary>
    static class DialogueManager {
        private static List<IDialogue> dialogs = new List<IDialogue>();

        /// <summary>
        /// Creates a new dialog box with the given pages. 
        /// </summary>
        /// <param name="dialoguePages"></param>
        public static int CreateDialog(IEnumerable<IDPage> dialoguePages) {
            // The new dialog will have the ID for the next index of our dialogs list
            var index = dialogs.Count;

            // Create the dialog for that ID, and add it to the list
            IDialogue dialog = new Dialogue(index, dialoguePages);
            dialogs.Add(dialog);

            // Return the new id
            return index;
        }

        /// <summary>
        /// Retrieves a Dialogue object, given its ID.
        /// </summary>
        /// <param name="dialogId"></param>
        /// <returns></returns>
        public static IDialogue GetDialog(int dialogId) {
            // If the dialog doesn't exist, return null
            if (dialogId >= dialogs.Count) {
                return null;
            }

            // Otherwise, return the dialog
            return dialogs[dialogId];
        }
    }
}
