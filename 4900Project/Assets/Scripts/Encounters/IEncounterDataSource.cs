using System.Collections.Generic;

namespace Encounters
{
    public interface IEncounterDataSource
    {
        IEnumerable<Encounter> GetEncounterEnumerator();
    }
}