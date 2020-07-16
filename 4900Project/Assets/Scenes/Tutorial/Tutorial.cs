using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Encounters;
using Quests;
using SIEvents;

public class Tutorial : MonoBehaviour
{
    void Start()
    {
        init();
        // Set player to node 1
    }

    private void init()
    {
        Encounter enc1 = new Encounter(
             "Enc 1",
             "Tutorial",
             "The cart's gas is running so low...\nI need to get into town as soon as possible.\nLooks like there's one not too far ahead...",
             new string[]
             {
                             "Travel."
             },
             new string[]
             {
                            ""
             },
             new Action[]
             {
                             () => {
                                 Debug.Log(EncounterManager.Instance);
                                 EncounterManager.Instance.GetFixedEncounter(1).AllowProgression();
                                 SceneManager.LoadScene("MapScene");
                             }
             }
         );

        Encounter enc2 = new Encounter(
             "Enc 2",
             "Tutorial",
             "Welcome to Smithsville!",
             new string[]
             {
                                     "Take items."
             },
             new string[]
             {
                                     "Received Gas and Medicine!",
             },
             new Action[]
             {
                                     () => {
                                         // Give gas
                                         // Give medicine
                                         BeginQuest();
                                         EncounterManager.Instance.GetFixedEncounter(2).AllowProgression();
                                     }
             },
             null,
             TownManager.Instance.GetTownByName("Smithsville").Id
         );

        Encounter enc3 = new Encounter(
            "Enc 3",
            "Tutorial",
            "Welcome to York!",
            new string[]
            {
                                            "Done."
            },
            new string[]
            {
                                            "Exchange the medicine for wood at the local store.",
            },
            new Action[]
            {
                                            () => {
                                                EncounterManager.Instance.GetFixedEncounter(3).AllowProgression();
                                                // Go to shop
                                            }
            },
            null,
            TownManager.Instance.GetTownByName("York").Id
        );

        Encounter enc4 = new Encounter(
            "Enc 4",
            "Tutorial",
            "Thanks for selling that medicine! Here's some gas for your travel back to town",
            new string[]
            {
                                                    "Done."
            },
            new string[]
            {
                                                    "",
            },
            new Action[]
            {
                                                    () => {
                                                        EncounterManager.Instance.GetFixedEncounter(4).AllowProgression();
                                                    }
            },
            new List<Condition> { new QuestCompleteCondition("", 0) },
            TownManager.Instance.GetTownByName("York").Id
        );

        Encounter enc5 = new Encounter(
            "Enc 5",
            "Tutorial",
            "Thanks completing that tutorial!",
            new string[]
            {
                                                            "Done."
            },
            new string[]
            {
                                                            "",
            },
            new Action[]
            {
                                                            () => {
                                                            }
            },
            new List<Condition> { new QuestCompleteCondition("", 0) },
            TownManager.Instance.GetTownByName("Smithsville").Id
        );

        EncounterManager.Instance.AddFixedEncounter(enc1);
        EncounterManager.Instance.AddFixedEncounter(enc2);
        EncounterManager.Instance.AddFixedEncounter(enc3);
        EncounterManager.Instance.AddFixedEncounter(enc4);
        EncounterManager.Instance.AddFixedEncounter(enc5);

        enc1.AllowProgression();

        RandomEncounter renc6 = new RandomEncounter(
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
                        //SceneManager.UnloadSceneAsync("Encounter");
                    },
                    () => {
                        var inventory = DataTracker.Current.Player.Inventory;
                        inventory.AddItem("Body Armor", 1);
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
                    () => {},
                    () => {}
            }
        );

        RandomEncounter renc7 = new RandomEncounter(
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
        );
        
        EncounterManager.Instance.AddRandomEncounter(renc6);
        EncounterManager.Instance.AddRandomEncounter(renc7);
    }

    private void BeginQuest()
    {
        Quest quest = new Quest.Builder("Medicine Quest")
            .SetDescription("Find medicine and sell it in Big Rock.")

            .AddStage(new Stage.Builder("Purchase medicine in York.")
                .AddCondition(new TransactionCondition("Purchase 1 medicine at the York Pharmacy", "Medicine", 1, TransactionCondition.TransactionTypeEnum.BUY, TownManager.Instance.GetTownByName("York").Id)
                )
            )

            .AddStage(new Stage.Builder("Sell Medicine in Big Rock.")
                .AddCondition(new TransactionCondition("Sell 1 medicine to the Big Rock General Store", "Medicine", 1, TransactionCondition.TransactionTypeEnum.SELL, TownManager.Instance.GetTownByName("Big Rock").Id)
                )
            )

            .Build();

        Quest secondQuest = new Quest.Builder("Testing Second Quest")
            .AddStage(new Stage.Builder("Purchase a Wrench in Smithsville.")
                .AddCondition(new TransactionCondition("Purchase 1 Wrench at the General Store", "Wrench", 1, TransactionCondition.TransactionTypeEnum.BUY, TownManager.Instance.GetTownByName("York").Id)
                )
            )
            .Build();
    }

    private void LoadTown()
    {
        SceneManager.LoadScene("Town", LoadSceneMode.Single);
    }
}
