using System.Collections;
using UnityEngine;
using Encounters;
using System;


// Just a class for demo purposes.
public class EncounterInvoker : MonoBehaviour
{
    void Start()
    {
        var mgr = EncounterManager.Instance;

        Encounter enc = new Encounter(
            "Uranium Deposit Discovered!",
            "Scavenging",
            "You find a deposit of uranium laying out in the open. Something tells you it's too good to be true...",
            new string[]
            {
                            "[Take] Claim the uranium for yourself",
                            "[Leave] Leave the resource alone",
                            "[Attack] Destroy the resource"
            },
            new string[]
            {
                            "It was a trap! You're surrounded by an army of Klingon-droid hybrids!",
                            "Aren't you quite the 'adventurer'...",
                            "If you can't have it, nobody should."
            },
            new Action[]
            {
                            () => Debug.Log("Effect 1 invoked -> *Decrease ship health*"),
                            () => {},  // nothing happens
                            () => Debug.Log("Effect 3 invoked -> *Decrease karma*"),
            }
        );

        mgr.AddFixedEncounter(enc);
        mgr.RunFixedEncounter(enc.Id);
    }
}
