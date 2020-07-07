using Dialogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialogue
{
    /// <summary>
    /// The DHistory class implements the IDHistory interface.
    /// It refers to a single page of history for the Dialogue.
    /// </summary>
    class DHistory : IDHistory
    {
        public IDPage Page { get; set; }
        public List<string> Responses { get; set; }
    }
}
