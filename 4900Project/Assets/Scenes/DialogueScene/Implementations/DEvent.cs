using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace Dialogue {
    /// <summary>
    /// A DEvent is a Dialogue Event that gets activated.
    /// The event will pass through the ID of the Dialogue.
    /// </summary>
    class DEvent : UnityEvent<int> {
    }
}
