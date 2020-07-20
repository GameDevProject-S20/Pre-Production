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
        //EncounterManager.Instance.
        throw new System.NotImplementedException();
    }
}
