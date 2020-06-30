using System.Collections;
using UnityEngine;
using Encounters;


// Just a class for demo purposes.
public class EncounterInvoker : MonoBehaviour
{
    void Start()
    {
        var mgr = RandomEncounterManager.Instance;
        mgr.NextEncounter();
    }
}
