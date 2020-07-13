using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialogue
{
    /// <summary>
    /// The DFunctions class provides some utility functions that can be used as button handlers.
    /// This includes things like controlling the dialog & triggering the quest system.
    /// </summary>
    public static class DFunctions
    {
        /// <summary>
        /// Advances the Dialogue system to the next page.
        /// </summary>
        public static readonly Action GoToNextPage = () =>
        {
            DialogueManager.Instance.GetActiveDialogue().GoToNextPage();
        };

        /// <summary>
        /// Closes the Dialogue.
        /// </summary>
        public static readonly Action CloseDialogue = () =>
        {
            DialogueManager.Instance.GetActiveDialogue().Hide();
        };

        /// <summary>
        /// Fires off a notification to our EventManager indicating that the button was pressed.
        /// </summary>
        public static readonly Action NotifyEventManager = () =>
        {
            // TODO
        };
    }
}