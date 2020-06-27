using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scenes.DialogueScene.Interfaces {
    /// <summary>
    /// The interface for a single dialog button. Every button has text & a click handler.
    /// </summary>
    interface IDButton {
        // The button's text. This is what gets displayed for the button.
        public string Text { get; set; }

        // The click handler. This will be called when the button gets clicked.
        public Action OnButtonClick { get; set; }
    }
}
