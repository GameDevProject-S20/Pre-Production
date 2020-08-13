using Extentions;
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

        private List<RandomEncounter> randomEncounterQueue = new List<RandomEncounter>();

        public bool RandomEncountersOn { get; private set; }

        private EncounterManager()
        {
            random = new Random();
            //ReloadRandomEncounterQueue();
            EventManager.Instance.OnEncounterTrigger.AddListener((int id) =>
            {
                if (id == -1 && RandomEncountersOn)
                {
                    RunRandomEncounter();
                }
                else {
                    RunEncounterById(id);
                }
            });
            EventManager.Instance.OnEnterPOIButtonClick.AddListener(RunEncounterById);
            EventManager.Instance.OnOpenDialogueClick.AddListener(RunEncounterById);

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
            if (!RandomEncountersOn)
            {
                Debug.LogWarning("Random Encounters not on, yet an attempt was made to run them.");
                return;
            }

            Encounter next = randomEncounter(tag);
            Debug.Log(next);
            next.RunEncounter();
        }

        public void RunEncounterById(int id){
            Encounter next = GetEncounter(id);
            Debug.Log(next);
            next.RunEncounter();  
        }

        // Load from csv or wherever in the future...
        // For now this demonstrates how to create an encounter object.
        public void ReloadRandomEncounterQueue(string tag = null)
        {
            IEnumerable<RandomEncounter> encounters = randomEncounters.Values;

            if (tag != null)
            {
                encounters = encounters.Where(e => e.Tags.Contains(tag));
            }

            // Reintroduce encounters to queue
            randomEncounterQueue.AddRange(encounters.Except(randomEncounterQueue));

            // Reorder queue
            randomEncounterQueue = randomEncounterQueue.OrderBy(randomEncounterQueue => random.Next()).ToList();
        }

        // Return the next random encounter in the shuffled queue
        // If all events have been used, events will be shuffled again.
        private RandomEncounter randomEncounter(string tag = null)
        {
            RandomEncounter enc = null;

            if (!RandomEncountersOn)
            {
                Debug.LogWarning("Random Encounters not on, yet an attempt was made to fetch one.");
                return enc;
            }

            bool reloaded = false;
            IEnumerable<RandomEncounter> conditionsMetEncounters = randomEncounterQueue.Where(e => e.IsRunnable());

            while (enc == null && reloaded == false)
            {
                IEnumerable<RandomEncounter> valid = randomEncounterQueue.Where(e => e.IsRunnable());

                if (tag != null)
                {
                    valid = randomEncounterQueue.Where(e => e.Tags.Contains(tag));  
                }

                enc = valid.FirstOrDefault();

                // Reload if none found
                if (enc == null)
                {
                    ReloadRandomEncounterQueue(tag);
                    reloaded = true;
                }
            }
            
            if (enc == null)
            {
                throw new Exception("No Random Encounter found");
            }

            randomEncounterQueue.Remove(enc);

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
                if (!RandomEncountersOn)
                {
                    Debug.LogWarning("Random Encounters not on, yet an attempt was made to fetch one.");
                    return null;
                }

                EncounterCollection.Instance.RandomEncounters.TryGetValue(id, out RandomEncounter rvalue);
                return rvalue;
            }
        }

        public void ToggleRandomEncounters(bool on)
        {
            RandomEncountersOn = on;
        }

        public override string ToString()
        {
            return string.Format("Fixed Encounters: {0}\nRandomEncounters: {1}", string.Join(", ", fixedEncounters.Keys), string.Join(", ", randomEncounters.Keys));
        }
    }
}
