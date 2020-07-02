using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace Dialogue
{
    /// <summary>
    /// Fires whenever the Dialogue has been updated.
    /// Mainly used for reacting to when the Dialogue changes (for example - proceeds to next page, closes, opens, etc.);
    ///  Does not pass through any additional data, but the Dialogue in question can be found using DialogueManager.GetActiveDialog()
    /// </summary>
    public class DialogueUpdatedEvent : UnityEvent
    {
    }
}
