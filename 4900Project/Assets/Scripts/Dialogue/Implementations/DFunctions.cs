using SIEvents;
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
            DialogueManager.GetActiveDialogue().GoToNextPage();
        };

        /// <summary>
        /// Closes the Dialogue.
        /// </summary>
        public static readonly Action CloseDialogue = () =>
        {
            DialogueManager.GetActiveDialogue().Hide();
        };

        /// <summary>
        /// Invokes the onDialogueSelect event of the EventManager.
        /// Accepts one string argument which will be passed through to the onDialogueSelect event.
        /// </summary>
        /// <param name="argument">The argument to pass through. The onDialogueSelect event will be fired with the value of this parameter.</param>
        /// <returns>An Action that the OnButtonClick handler should be hooked up to.</returns>
        public static Action NotifyEventManager(string argument)
        {
            return () =>
            {
                EventManager.Instance.onDialogueSelected.Invoke(argument);
            };
        }
    }
}