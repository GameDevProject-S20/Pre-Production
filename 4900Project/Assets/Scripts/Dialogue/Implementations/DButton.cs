using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialogue
{
    /// <summary>
    /// Implementation of the IDButton interface. This actually implements a dialog button.
    /// </summary>
    public class DButton : IDButton
    {
        public string Text { get; set; }
        public Action OnButtonClick { get; set; }
        public IEnumerable<IPresentCondition> Conditions { get; set; }
        public IEnumerable<IEffect> Effects { get; set; }
        public IDPage NextPage { get; set; }

        public static DButton Exit
        {
            get => new DButton()
            {
                Text = "Done.",
                OnButtonClick = DFunctions.CloseDialogue,
                Conditions = Enumerable.Empty<IPresentCondition>(),
                Effects = Enumerable.Empty<IEffect>(),
                NextPage = null
            };
        }
    }
}
