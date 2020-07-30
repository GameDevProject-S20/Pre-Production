using System.Collections.Generic;

namespace Encounters
{
    /// <summary>
    /// Serves encounters
    /// </summary>
    public interface IEncounterDataSource
    {
        IEnumerable<Encounter> GetEncounterEnumerator();
    }
}