using SIEvents;
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
    [System.Serializable]
    public class EncounterManager
    {
        [SerializeField]
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

        [SerializeField]
        private Dictionary<int, FixedEncounter> fixedEncounters
        {
            get => EncounterCollection.Instance.FixedEncounters;
        }

        [SerializeField]
        private Dictionary<int, RandomEncounter> randomEncounters
        {
            get => EncounterCollection.Instance.RandomEncounters;
        }

        private Queue<RandomEncounter> randomEncounterQueue;

        private EncounterManager()
        {
            random = new Random();
            randomEncounterQueue = reloadRandomEncounters();
            EventManager.Instance.OnNodeEnter.AddListener((OverworldMap.LocationNode node) =>
            {
                if (node.Type == OverworldMap.LocationType.EVENT)
                {
                    RunRandomEncounter();
                }
            });
        }

        public void AddEncounter(Encounter enc)
        {
            FixedEncounter fenc = enc as FixedEncounter;
            if (fenc != null)
            {
                fixedEncounters.Add(fenc.Id, fenc);
                fenc.AllowProgression();
            }
            else
            {
                RandomEncounter renc = enc as RandomEncounter;
                randomEncounters.Add(renc.Id, renc);
            }
        }

        public void RemoveFixedEncounter(FixedEncounter enc)
        {
            fixedEncounters.Remove(enc.Id);
            enc.DisallowProgression();
        }

        /// <summary>
        /// Call this to get a random encounter
        /// </summary>
        public void RunRandomEncounter(string tag = null)
        {
            Encounter next = randomEncounter(tag);
            Debug.Log(next);
            next.RunEncounter();
        }

        // Load from csv or wherever in the future...
        // For now this demonstrates how to create an encounter object.
        private Queue<RandomEncounter> reloadRandomEncounters()
        {
            // hard coded for now
            var nextEncounters = new List<RandomEncounter>(randomEncounters.Values);

            // Shuffle the list and return as a queue
            return new Queue<RandomEncounter>(nextEncounters.OrderBy(randomEncounterQueue => random.Next()));
        }

        // Return the next random encounter in the shuffled queue
        // If all events have been used, events will be shuffled again.
        private Encounter randomEncounter(string tag = null)
        {
            Encounter enc;
            if (tag != null)
            {
                List<RandomEncounter> tagged = randomEncounterQueue.Where(e => e.Tags.Contains(tag)).ToList();
                if (tagged.Count == 0)
                {
                    tagged = randomEncounters.Values.Where(e => e.Tags.Contains(tag)).ToList();
                    if (tagged.Count > 0)
                    {
                        enc = tagged[random.Next(tagged.Count)];
                    }
                    else
                    {
                        throw new Exception(string.Format("Encounter with tag '{0}' not found", tag));
                    }
                }
            }
            {
                if (randomEncounterQueue.Count > 0)
                {
                    enc = randomEncounterQueue.Dequeue();
                }
                else
                {
                    randomEncounterQueue = reloadRandomEncounters();
                    enc = randomEncounterQueue.Dequeue();
                }
            }

            return enc;
        }

        public Encounter GetEncounter(int id)
        {
            EncounterCollection.Instance.FixedEncounters.TryGetValue(id, out FixedEncounter fvalue);
            
            if (fvalue != null)
            {
                return fvalue;
            }
            else
            {
                EncounterCollection.Instance.RandomEncounters.TryGetValue(id, out RandomEncounter rvalue);
                return rvalue;
            }
        }

        public override string ToString()
        {
            return string.Format("Fixed Encounters: {0}\nRandomEncounters: {1}", string.Join(", ", fixedEncounters.Keys), string.Join(", ", randomEncounters.Keys));
        }
    }
}
