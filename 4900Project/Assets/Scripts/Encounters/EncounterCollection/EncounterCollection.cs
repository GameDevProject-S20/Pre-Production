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

        public Dictionary<int, Encounter> FixedEncounters = new Dictionary<int, Encounter>()
        {
             {
                0, new RandomEncounter(
                    "Broken Gas Station",
                    "Loot",
                    "A lone gas station on the side of the road. The pump is broken."
                    + "The owner says 'I could repair that with some supplies.'",
                    new string[]
                    {
                        "Offer supplies (Requires Wrench, 6 Scrap Metal)",
                        "Leave."
                    },
                    new string[]
                    {
                        "'Come back later and I'll have this fixed!'",
                        "You leave the owner to his problems."
                    },
                    new Action[]  // successful action
                    {
                        () => {
                            var inventory = DataTracker.Current.Player.Inventory;
                            inventory.RemoveItem("Scrap Metal", 6);  // Scrap Metal
                            inventory.RemoveItem("Wrench", 1);  // Scrap Metal
                            OverworldMap.LocationNode node;
                            if (DataTracker.Current.WorldMap.GetNode(9, out node)){
                                node.LocationId = 1;
                            }

                            //SceneManager.UnloadSceneAsync("Encounter");
                        },
                        () => {

                            //SceneManager.UnloadSceneAsync("Encounter");
                        }
                    },
                    new Func<bool>[]  // condition (whether the player can take the action or not)
                    {
                        () => {
                            return DataTracker.Current.Player.Inventory.Contains("Wrench") > 0
                            && DataTracker.Current.Player.Inventory.Contains("Scrap Metal") > 5;
                        },
                        () => {
                            // Always available
                            return true;
                        }
                    },
                    new String[]  // Text to display on failure
                    {
                        "You do not have enough supplies.",
                        ""
                    },
                    new Action[]  // Action to take on failure
                    {
                        () => {},
                        () => {}
                    }
                )
            },

             {
                1, new RandomEncounter(
                    "Gas Station",
                    "Loot",
                    "A lone gas station on the side of the road."
                    + "The owner calls to you. 'Hey there, friend! Good deal on gas, just for you!' ",
                    new string[]
                    {
                        "Purchase 3 gas for 3 scrap metal.",
                        "Leave."
                    },
                    new string[]
                    {
                        "'Come back later!'",
                        "You leave without buying."
                    },
                    new Action[]  // successful action
                    {
                        () => {
                            
                            //SceneManager.UnloadSceneAsync("Encounter");
                        },
                        () => {

                            //SceneManager.UnloadSceneAsync("Encounter");
                        }
                    },
                    new Func<bool>[]  // condition (whether the player can take the action or not)
                    {
                        () => {
                            // Always available
                            return true;
                        },
                        () => {
                            // Always available
                            return true;
                        }
                    },
                    new String[]  // Text to display on failure
                    {
                        "",
                        ""
                    },
                    new Action[]  // Action to take on failure
                    {
                        () => {},
                        () => {}
                    }
                )
            }
        };

        public Dictionary<int, Encounter> RandomEncounters = new Dictionary<int, Encounter>()
        {
        };

        private EncounterCollection()
        { }
    }
}