using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialogue {
    /// <summary>
    /// The actual implementation for a Dialogue.
    /// </summary>
    class Dialogue : IDialogue {
        // Properties
        public int Id { get; protected set; }
        protected IEnumerable<IDPage> Pages { get; set; }

        public Dialogue(int id, IEnumerable<IDPage> pages) {
            this.Id = id;
            this.Pages = pages;
        }
    }
}
