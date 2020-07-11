using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        // private Dictionary<int, Encounter> randomEncounters;

        private Dictionary<int, Encounter> randomEncounters
        {
            get => EncounterCollection.Instance.RandomEncounters;
        }

        private Queue<Encounter> randomEncounterQueue;

        private EncounterManager()
        {
            random = new Random();
            fixedEncounters = new Dictionary<int, Encounter>();
            //randomEncounters = new Dictionary<int, Encounter>();
            randomEncounterQueue = reloadRandomEncounters();
        }

        /// <summary>
        /// Call this to get a random encounter
        /// </summary>
        public void RunRandomEncounter()
        {
            Encounter next = randomEncounter();
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
        private Queue<Encounter> reloadRandomEncounters()
        {
            // hard coded for now
            var nextEncounters = new List<Encounter>(randomEncounters);

            // Shuffle the list and return as a queue
            return new Queue<Encounter>(nextEncounters.OrderBy(encounterQueue => random.Next()));
        }

        // Return the next random encounter in the shuffled queue
        // If all events have been used, events will be shuffled again.
        private Encounter randomEncounter()
        {
            Encounter enc;
            if (!encounterQueue.TryDequeue(enc))
            {
                encounterQueue = reloadRandomEncounters();
                enc = encounterQueue.Dequeue();
            }
            return enc;
        }
    }
}
