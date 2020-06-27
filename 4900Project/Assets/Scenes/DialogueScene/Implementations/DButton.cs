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
    class DButton : IDButton
    {
        public string Text { get; set; }
        public Action<int> OnButtonClick { get; set; }
    }
}
