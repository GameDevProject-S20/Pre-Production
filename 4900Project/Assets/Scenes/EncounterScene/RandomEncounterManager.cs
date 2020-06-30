using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Random = System.Random;


namespace Encounters
{

    /// <summary>
    /// Manager class to randomly spawn encounters
    /// Uses the singleton pattern - get from RandomEncounterManager.Instance
    /// </summary>
    public class RandomEncounterManager
    {
        public static RandomEncounterManager Instance
        {
            get
            {
                if (instance == null) instance = new RandomEncounterManager();
                return instance;
            }
        }

        private static RandomEncounterManager instance;
        private Random random;
        private Dictionary<int, Encounter> encounters;

        private RandomEncounterManager()
        {
            random = new Random();
            encounters = new Dictionary<int, Encounter>();
            loadEncounters();
        }

        /// <summary>
        /// Call this to get a random encounter
        /// </summary>
        public void NextEncounter()
        {
            Encounter next = randomEncounter();
            Debug.Log(next);
            next.StartDialogue();
        }

        // Load from csv or wherever in the future...
        // For now this demonstrates how to create an encounter object.
        private void loadEncounters()
        {
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
            encounters.Add(enc.Id, enc);
        }

        private Encounter randomEncounter()
        {
            int i = random.Next(encounters.Count);
            return encounters[i];
        }
    }
}
