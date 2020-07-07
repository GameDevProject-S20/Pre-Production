using Assets.Scripts.Dialogue.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialogue
{
    public class DPage : IDPage
    {
        // Properties
        public string Text { get; set; }
        public IEnumerable<IDButton> Buttons { get; set; }
        public IDAvatar Avatar { get; set; }

        // Public Methods
        /// <summary>
        /// Retrieves a button given its index.
        /// </summary>
        /// <param name="buttonId"></param>
        /// <returns></returns>
        public IDButton GetButton(int buttonIndex)
        {
            return Buttons.ElementAtOrDefault(buttonIndex);
        }
    }
}
