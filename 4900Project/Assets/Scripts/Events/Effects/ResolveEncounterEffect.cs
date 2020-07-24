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
        Encounter enc = EncounterManager.Instance.GetFixedEncounter(encID);
        if (enc == null)
        {
            return false;
        }
        else
        {
            enc.DisallowProgression();
            return true;
        }
    }
}
