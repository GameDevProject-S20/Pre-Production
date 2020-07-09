using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Dialogue
{
    /// <summary>
    /// Describes an Avatar. The Avatar will be shown on the page with a name and a character image.
    /// </summary>
    public interface IDAvatar
    {
        /// <summary>
        /// The name of the character.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The avatar's icon.
        /// </summary>
        Sprite Icon { get; set; }
    }
}
