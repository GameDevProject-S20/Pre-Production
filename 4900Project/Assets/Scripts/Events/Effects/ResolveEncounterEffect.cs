using Encounters;

public class ResolveEncounterEffect : IEffect
{
    private readonly int encID;

    private ResolveEncounterEffect() { }

    public ResolveEncounterEffect(int encID)
    {
        this.encID = encID;
    }

    public bool Apply()
    {
        Encounter enc = EncounterManager.Instance.GetEncounter(encID);
        if (enc == null)
        {
            return false;
        }
        else
        {
            // How will this work for RandomEncounters?
            //enc.DisallowProgression();
            FixedEncounter fenc = enc as FixedEncounter;
            if (fenc != null)
            {
                fenc.DisallowProgression();
            }
            return true;
        }
    }
}
