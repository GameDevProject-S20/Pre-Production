using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// The path to the avatar's image.
        /// </summary>
        string PathToImage { get; set; }
    }
}
