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
        public Dictionary<int, Encounter> FixedEncounters = new Dictionary<int, Encounter>()
        {
      
                
        };

        public Dictionary<int, Encounter> RandomEncounters = new Dictionary<int, Encounter>()
        {
            
        };

        private EncounterCollection()
        { }
    }
}