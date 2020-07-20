using System.Collections.Generic;

namespace Encounters
{
    public interface IEncounterDataSource
    {
        IEnumerator<Encounter> GetEncounterEnumerator();
    }
}