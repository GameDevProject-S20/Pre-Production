using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialogue {
    /// <summary>
    /// The DFunctions class provides some utility functions that can be used as button handlers.
    /// This includes things like controlling the dialog & triggering the quest system.
    /// </summary>
    static class DFunctions {
        /// <summary>
        /// Advances the Dialogue system to the next page.
        /// </summary>
        public static readonly Action<int> GoToNextPage = (int id) => {
            DialogueManager.GetDialog(id).GoToNextPage();
        };

        /// <summary>
        /// Closes the Dialogue.
        /// </summary>
        public static readonly Action<int> CloseDialogue = (int id) => {
            DialogueManager.GetDialog(id).Hide();
        };

        /// <summary>
        /// Notifies the quest system that a button was pressed.
        /// </summary>
        public static readonly Action<int> NotifyQuestSystem = (int id) => {
            // TODO
        };
    }
}