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
    public class EncounterManager
    {
        public static EncounterManager Instance
        {
            get
            {
                if (instance == null) instance = new EncounterManager();
                return instance;
            }
        }

        private static EncounterManager instance;
        private Random random;
        private Dictionary<int, Encounter> fixedEncounters;
        private Dictionary<int, Encounter> randomEncounters;


        private EncounterManager()
        {
            random = new Random();
            fixedEncounters = new Dictionary<int, Encounter>();
            randomEncounters = new Dictionary<int, Encounter>();
            //loadEncounters();
        }

        public void AddFixedEncounter(Encounter encounter)
        {
            fixedEncounters.Add(encounter.Id, encounter);
        }
        public void AddRandomEncounter(Encounter encounter)
        {
            randomEncounters.Add(encounter.Id, encounter);
        }

        /// <summary>
        /// Call this to get a random encounter
        /// </summary>
        public void RunRandomEncounter()
        {
            Encounter next = randomEncounter();
            Debug.Log(next);
            next.StartDialogue();
        }

        public void RunFixedEncounter(int id)
        {
            fixedEncounters.TryGetValue(id, out Encounter encounter);
            if (encounter != null)
            {
                encounter.StartDialogue();
            }
            Debug.Log(encounter.Id + " : " + ((encounter != null) ? "Found" : "Not Found"));
        }

        // Load from csv or wherever in the future...
        // For now this demonstrates how to create an encounter object.
        private void loadEncounters()
        {
        }

        private Encounter randomEncounter()
        {
            int i = random.Next(randomEncounters.Count);
            return randomEncounters[i];
        }
    }
}
