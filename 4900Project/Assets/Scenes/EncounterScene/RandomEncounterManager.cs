using System;
using System.Collections.Generic;
using UnityEngine;


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

        private RandomEncounterManager()
        {
            Debug.Log("RandomEncounterManager ctor");

            Encounter enc = new Encounter("Name", "Tag", "BodyText", "ResultText",
                    new string[] {"foo", "bar", "baz"});

            Debug.Log(enc.Name);
            Debug.Log(enc.Tag);
            Debug.Log(enc.BodyText);
            Debug.Log(enc.ResultText);
            foreach (string s in enc.ButtonText)
            {
                Debug.Log(s);
            }
        }
    }
}
