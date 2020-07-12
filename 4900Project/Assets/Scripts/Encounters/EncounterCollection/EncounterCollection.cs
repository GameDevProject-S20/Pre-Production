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
                0, new Encounter(
                    "Crashed Ship",
                    "Loot",
                    "You encounter a desolate spacecraft, seemingly crashed here ages ago. Whatever happened to the crew, they're long gone. "
                    + "There are scraps littering the outside of the shuttle, but the door is jammed closed.",
                    new string[]
                    {
                        "[Take] Gather up as much scrap metal as you can carry, and return to your ship. (+2 scrap metal)",
                        "[Search] (Requires 1 explosive) Blast open the door, and loot the inside of the ship (+1 fusion core)"
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
                            inventory.AddItem("item2", 2);  // Scrap Metal
                            SceneManager.LoadScene("MapScene");
                        },
                        () => {
                            var inventory = DataTracker.Current.Player.Inventory;
                            inventory.RemoveItem("item99", 1);
                            inventory.AddItem("item3", 1);  // Fusion core
                            SceneManager.LoadScene("MapScene");
                        }
                    },
                    new Func<bool>[]  // condition (whether the player can take the action or not)
                    {
                        () => {
                            // Always available
                            return true;
                        },
                        () => {
                            // Only available if the player has 1 explosive
                            return DataTracker.Current.Player.Inventory.Contains("item99") > 0;
                        }
                    },
                    new String[]  // Text to display on failure
                    {
                        "",
                        "You do not have enough explosives!"
                    },
                    new Action[]  // Action to take on failure
                    {
                        () => {},
                        () => {
                            // Restart encounter or.....?
                        }
                    }
                )
            }
        };

        private EncounterCollection()
        { }
    }
}