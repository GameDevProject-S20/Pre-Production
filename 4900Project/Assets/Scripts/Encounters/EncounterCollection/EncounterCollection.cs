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
                        "res2"
                    },
                    new Action[]
                    {
                        () => {
                            var inventory = DataTracker.Current.Player.Inventory;
                            inventory.AddItem("item2", 2);  // Scrap Metal
                            SceneManager.LoadScene("MapScene");
                        },
                        () => {
                            var inventory = DataTracker.Current.Player.Inventory;
                            if (inventory.Contains("item99") > 0)  // Explosive
                            {
                                inventory.AddItem("item3", 1);  // Fusion core
                            }
                            else
                            {
                                // How do we display to the player that they can't do that?
                            }
                            SceneManager.LoadScene("MapScene");
                        }
                    },
                    new Func<bool>[]
                    {
                        () => {
                            return true;
                        },
                        () => {
                            return false;
                        }
                    }
                )
            }
        };

        private EncounterCollection()
        { }
    }
}