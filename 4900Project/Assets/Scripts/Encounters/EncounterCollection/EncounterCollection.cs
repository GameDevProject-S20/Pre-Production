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
        };

        public Dictionary<int, Encounter> RandomEncounters = new Dictionary<int, Encounter>()
        {
            {
                0, new RandomEncounter(
                    "Crashed Ship",
                    "Loot",
                    "You encounter a desolate spacecraft, seemingly crashed here ages ago. "
                    + "There are scraps littering the outside of the shuttle, but the door is jammed closed.",
                    new string[]
                    {
                        "Gather up as much scrap metal as you can carry (+2 Scrap Metal)",
                        "(Requires an RPG) Blast open the door, and loot the ship (+1 Body Armor)"
                    },
                    new string[]
                    {
                        "2 Scrap Metal added.",
                        "1 Explosive removed, 1 Fusion Core added"
                    },
                    new Action[]  // successful action
                    {
                        () => {
                            var inventory = DataTracker.Current.Player.Inventory;
                            inventory.AddItem("Scrap Metal", 2);  // Scrap Metal
                            SceneManager.UnloadSceneAsync("Encounter");
                        },
                        () => {
                            var inventory = DataTracker.Current.Player.Inventory;
                            inventory.AddItem("Body Armor", 1);
                            SceneManager.UnloadSceneAsync("Encounter");
                        }
                    },
                    new Func<bool>[]  // condition (whether the player can take the action or not)
                    {
                        () => {
                            // Always available
                            return true;
                        },
                        () => {
                            // Only available if the player has 1 rpg
                            return DataTracker.Current.Player.Inventory.Contains("RPG") > 0;
                        }
                    },
                    new String[]  // Text to display on failure
                    {
                        "",
                        "You do not have an RPG!"
                    },
                    new Action[]  // Action to take on failure
                    {
                        () => {
                            SceneManager.UnloadSceneAsync("Encounter");
                        },
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
                            SceneManager.UnloadSceneAsync("Encounter");
                        },
                        () => {
                            SceneManager.UnloadSceneAsync("Encounter");
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
                            SceneManager.UnloadSceneAsync("Encounter");
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