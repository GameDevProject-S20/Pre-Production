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
                2, new RandomEncounter(
                    "Repair Store",
                    "Shop",
                    "The town mechanic is offering repairs.",
                    new string[]
                    {
                        "Pay with 3 scrap",
                        "Pay with 2 rations",
                        "Leave"
                    },
                    new string[]
                    {
                        "The mechanic repaires your vehicle.",
                        "The mechanic repaires your vehicle.",
                        "You leave the garage."
                    },
                    new Action[]  // successful action
                    {
                        () => {
                            var inventory = DataTracker.Current.Player.Inventory;
                            inventory.RemoveItem("Scrap Metal", 3);  // Scrap Metal
                            DataTracker.Current.Player.addHealth(75);
                            //SceneManager.UnloadSceneAsync("Encounter");
                        },
                        () => {
                            var inventory = DataTracker.Current.Player.Inventory;
                            inventory.RemoveItem("Rations", 2);
                            DataTracker.Current.Player.addHealth(75);
                            //SceneManager.UnloadSceneAsync("Encounter");
                        },
                        () => {

                        }

                    },
                    new Func<bool>[]  // condition (whether the player can take the action or not)
                    {
                        () => {
                            // Always available
                            return DataTracker.Current.Player.Inventory.Contains("Scrap Metal") > 2;
                        },
                        () => {
                            // Only available if the player has 1 rpg
                            return DataTracker.Current.Player.Inventory.Contains("Rations") > 1;
                        },
                        () => {
                            // Always available
                            return true;
                        }
                    },
                    new String[]  // Text to display on failure
                    {
                        "You don't have enough scrap.",
                        "You don't have enough rations.",
                        ""
                    },
                    new Action[]  // Action to take on failure
                    {
                        () => {},
                        () => {},
                        () => {}
                    }
                )
            },
            {
                3, new RandomEncounter(
                    "Town Leader",
                    "Talk",
                    "You attempt to contact the town leader, but they are not available right now.",
                    new string[]
                    {
                        "Leave"
                    },
                    new string[]
                    {
                        "You leave."
                    },
                    new Action[]  // successful action
                    {
                        () => {

                        }

                    },
                    new Func<bool>[]  // condition (whether the player can take the action or not)
                    {
                        () => {
                            // Always available
                            return true;
                        }
                    },
                    new String[]  // Text to display on failure
                    {
                        ""
                    },
                    new Action[]  // Action to take on failure
                    {
                        () => {}
                    }
                )
            }
        };

        public Dictionary<int, Encounter> RandomEncounters = new Dictionary<int, Encounter>()
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
            },
            {
                1, new RandomEncounter(
                    "Farmer's Market",
                    "Loot",
                    "You encounter a quaint farmer's market, one stall selling rare exotic fruits."
                    + "Surely an amicable deal can be struck?",
                    new string[]
                    {
                        "Offer 1 Medicine (+3 Fresh Fruit)",
                        "You don't want any fruit"
                    },
                    new string[]
                    {
                        "3 Fresh Fruit added.",
                        "You never get tired of Rations..."
                    },
                    new Action[]  // successful action
                    {
                        () => {
                            var inventory = DataTracker.Current.Player.Inventory;
                            inventory.AddItem("Fresh Fruit", 3);
                            //SceneManager.UnloadSceneAsync("Encounter");
                        },
                        () => {
                            //SceneManager.UnloadSceneAsync("Encounter");
                        }
                    },
                    new Func<bool>[]  // condition (whether the player can take the action or not)
                    {
                        () => {
                            return DataTracker.Current.Player.Inventory.Contains("Medicine") > 0;
                        },
                        () => true
                    },
                    new String[]  // Text to display on failure
                    {
                        "You don't have any medicine!",
                        ""
                    },
                    new Action[]  // Action to take on failure
                    {
                        () => {
                            //SceneManager.UnloadSceneAsync("Encounter");
                        },
                        () => {}
                    }
                )
            }
        };

        private EncounterCollection()
        { }
    }
}