using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Encounters
{
    public class EncounterCollection
    {
        public static EncounterCollection Instance
        {
            get
            {
                if (instance == null) instance = new EncounterCollection();
                return instance;
            }
        }

        public static EncounterCollection instance = null;

        [SerializeField]
        public Dictionary<int, FixedEncounter> FixedEncounters = new Dictionary<int, FixedEncounter>()
        {
      
                
        };

        public Dictionary<int, RandomEncounter> RandomEncounters = new Dictionary<int, RandomEncounter>()
        {
            
        };

        private EncounterCollection()
        { }
    }
}