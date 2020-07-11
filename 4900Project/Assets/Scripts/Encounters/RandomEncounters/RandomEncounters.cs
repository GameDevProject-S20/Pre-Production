using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;


namespace Encounters
{
    public class EncounterCollection
    {
        public EncounterCollection Instance
        {
            get
            {
                if (instance == null) instance = new EncounterCollection();
                return instance;
            }
        }

        public EncounterCollection instance = null;
        public Dictionary<int, Encounter> RandomEncounters = new Dictionary<int, Encounter>()
        {
            {
                0, new Encounter(
                    "title",
                    "tag",
                    "text",
                    new string[]
                    {
                        "btn 1",
                        "btn 2"
                    },
                    new string[]
                    {
                        "res1",
                        "res2"
                    },
                    new Action[]
                    {
                        () => {},
                        () => {}
                    }
                )
            }
        };

        private EncounterCollection()
        { }
    }
}