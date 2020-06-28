using System;
using System.Collections.Generic;
using UnityEngine;

using Random = System.Random;


namespace Encounters
{
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

        public void NextEncounter()
        {
            Encounter next = randomEncounter();
            Debug.Log(next);
        }

        // Load from csv or wherever in the future...
        private void loadEncounters()
        {
            Encounter enc = new Encounter("Name", "Tag", "BodyText", new string[] {"foo", "bar", "baz"}, new string[] {"fooRes", "barRes", "bazRes"});
            encounters.Add(enc.Id, enc);
        }

        private Encounter randomEncounter()
        {
            int i = random.Next(encounters.Count);
            return encounters[i];
        }
    }
}
