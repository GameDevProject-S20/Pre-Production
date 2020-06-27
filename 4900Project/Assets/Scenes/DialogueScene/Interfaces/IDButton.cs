using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialogue
{
    /// <summary>
    /// The interface for a single dialog button. Every button has text & a click handler.
    /// </summary>
    public interface IDButton
    {
        /// <summary>
        /// The button's text. This is displayed with the button.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Handler function to be called when the button is clicked. Passes in the ID for the dialogue.
        /// </summary>
        Action OnButtonClick { get; set; }
    }
}
