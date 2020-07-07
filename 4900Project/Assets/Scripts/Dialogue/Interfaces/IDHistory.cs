using Dialogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialogue
{
    /// <summary>
    /// The IDHistory class is used to store the History of pages that the Dialog has gone through.
    /// Every IDHistory refers to one single page, along with the response that was selected for that page.
    /// </summary>
    interface IDHistory
    {
        /// <summary>
        /// This refers to the page of the history.
        /// </summary>
        IDPage Page { get; set; }

        /// <summary>
        /// The response that the user gave to that page.
        /// </summary>
        List<string> Responses { get; set; }
    }
}
