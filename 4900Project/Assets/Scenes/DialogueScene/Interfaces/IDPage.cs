using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scenes.DialogueScene.Interfaces {
    /// <summary>
    /// The Dialogue Page interface. Every Dialogue page has text and an array of buttons to be shown.
    /// </summary>
    class IDPage {
        /// <summary>
        /// The text that the page should display.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The list of buttons to show with the page.
        /// </summary>
        public IEnumerable<IDButton> Buttons { get; set; }
    }
}
