using System.Collections;
using System.Collections.Generic;

namespace SIEvents
{
    /// <summary>
    /// An event listener that can toggle progression on and off
    /// </summary>
    public interface IProgressor
    {
        // Set listeners
        //
        // Warning! 
        //  This may cause issues with duplicate listeners if called multiple times with no accompanying DisallowProgression call in between.
        //  UnityEvent is obfuscated, so it is currently impossible to tell without testing
        void AllowProgression();

        // Remove listeners
        void DisallowProgression();
    }
}