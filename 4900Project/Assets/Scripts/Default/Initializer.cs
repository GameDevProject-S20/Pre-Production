using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Encounters;
using Quests;
using SIEvents;


public class Initializer : MonoBehaviour
{
    void Start()
    {
        EventManager.Instance.onDataTrackerLoad.AddListener(finishLoading);
        SceneManager.LoadScene("DataTracker", LoadSceneMode.Additive);
    }

    IEnumerator loader(){
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("MapScene", LoadSceneMode.Single);
    }

    void finishLoading(){
        InitializeTutorial();
        InitializeEncounters();
        StartCoroutine(loader());
    }

    void InitializeTutorial()
    {
        // id = 0
        Encounter tutorialStage1 = new Encounter(
             "Tutorial Part 1",
             "Tutorial",
             "You notice you are running low on gas.",
             new string[]
             {
                             "[Next]"
             },
             new string[]
             {
                            "Looks like there is a town up ahead. Maybe you can get fuel there."
             },
             new Action[]
             {
                             () => {
                                 //Debug.Log(EncounterManager.Instance);
                                //EncounterManager.Instance.GetFixedEncounter(1).AllowProgression();
                                TownManager.Instance.GetTownByName("Smithsville").leaderDialogueEncounterId = 1;
                                OverworldMap.LocationNode node;
                                if (DataTracker.Current.WorldMap.GetNode(12, out node)){
                                    node.LocationId = -1;
                                }

                             }
             }
         );

        //id = 1
        Encounter tutorialStage2 = new Encounter(
             "Tutorial Part 2",
             "Tutorial",
             "Welcome to Smithsville! Our <color=#2675AD>generator</color> is out and we can't repair it. I will give you fuel if you help us.",
             new string[]
             {
                                     "Accept"
             },
             new string[]
             {
                                     "Great! Take this medicine and trade for a new <color=#2675AD>generator</color> in York.",
             },
             new Action[]
             {
                                     () => {
                                        DataTracker.Current.Player.Inventory.AddItem("Fuel", 60);
                                         DataTracker.Current.Player.Inventory.AddItem("Medicine", 8);
                                         BeginQuest();
                                         EncounterManager.Instance.GetFixedEncounter(3).AllowProgression();
                                         TownManager.Instance.GetTownByName("Smithsville").leaderDialogueEncounterId = 2;
                                     }
             }
         );

        //id = 2
        Encounter tutorialStage2_2 = new Encounter(
             "Tutorial Part 2",
             "Tutorial",
             "Thank you for agreeing to help us.",
             new string[]
             {
                                     "[Next]"
             },
             new string[]
             {
                                     "Please bring the new <color=#2675AD>generator</color> soon.",
             },
             new Action[]
             {
                                     () => {
                                     }
             }
         );

        //id = 3
        Encounter tutorialStage3 = new Encounter(
            "Tutorial Part 3",
            "Tutorial",
            "You have arrived in York.",
            new string[]
            {
                                            "[Next]"
            },
            new string[]
            {
                                            "Exchange the medicine for a new <color=#2675AD>generator</color> at the local store.",
            },
            new Action[]
            {
                                            () => {
                                                EncounterManager.Instance.GetFixedEncounter(4).AllowProgression();
                                                TownManager.Instance.GetTownByName("Smithsville").leaderDialogueEncounterId = 5;
                                                // Go to shop
                                            }
            },
            null,
            TownManager.Instance.GetTownByName("York").Id
        );

        //id = 4
        Encounter tutorialStage4 = new Encounter(
            "Tutorial Part 4",
            "Tutorial",
            "Thanks for selling that medicine!",
            new string[]
            {
                                                    "[Next]"
            },
            new string[]
            {
                                                    "Return to the sheriff of Smithsville.",
            },
            new Action[]
            {
                                                    () => {
                                                        //EncounterManager.Instance.GetFixedEncounter(4).AllowProgression();
                                                    }
            },
            new List<Condition> { new StageCompleteCondition("", 0, 0) },
            TownManager.Instance.GetTownByName("York").Id
        );

        //id = 5
        Encounter tutorialStage5 = new RandomEncounter(
                    "Tutorial Part 5",
                    "Tutorial",
                    "Do you have a replacement <color=#2675AD>generator</color>?",
                    new string[]
                    {
                        "Yes. (requires generator)",
                        "No."
                    },
                    new string[]
                    {
                        "Thanks! Smithsville should be up and running again. Have some fuel.",
                        "Very well. Please bring a <color=#2675AD>generator</color> soon."
                    },
                    new Action[]  // successful action
                    {
                        () => {
                            var inventory = DataTracker.Current.Player.Inventory;
                            inventory.RemoveItem("Generator", 1);  // Scrap Metal
                            DataTracker.Current.Player.Inventory.AddItem("Fuel", 80);
                            TownManager.Instance.GetTownByName("Smithsville").leaderDialogueEncounterId = 11;
                            EventManager.Instance.onDialogueSelected.Invoke("TutorialPart5GiveGenerator");
                            DataTracker.Current.WorldMap.AddEdge(3, 15);
                            EventManager.Instance.RequestRedraw.Invoke();
                            TownManager.Instance.GetTownByName("Smithsville").AddShop(0);
                            TownManager.Instance.GetTownByName("Smithsville").AddShop(1);
                        },
                        () => {

                        }
                    },
                    new Func<bool>[]  // condition (whether the player can take the action or not)
                    {
                        () => {
                            return DataTracker.Current.Player.Inventory.Contains("Generator") > 0;
                        },
                        () => {
                            // Always available
                            return true;
                        }
                    },
                    new String[]  // Text to display on failure
                    {
                        "You do not have a generator.",
                        ""
                    },
                    new Action[]  // Action to take on failure
                    {
                        () => {},
                        () => {}
                    }
                );

        EncounterManager.Instance.AddFixedEncounter(tutorialStage1);
        EncounterManager.Instance.AddFixedEncounter(tutorialStage2);
        EncounterManager.Instance.AddFixedEncounter(tutorialStage2_2);
        EncounterManager.Instance.AddFixedEncounter(tutorialStage3);
        EncounterManager.Instance.AddFixedEncounter(tutorialStage4);
        EncounterManager.Instance.AddFixedEncounter(tutorialStage5);

        //enc1.AllowProgression();

        

        
        

    }

    private void InitializeEncounters(){


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

               RandomEncounter renc8 = new RandomEncounter(
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
                                node.LocationId = 9;
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
                );
    

             RandomEncounter renc9 = new RandomEncounter(
                    "Gas Station",
                    "Loot",
                    "A lone gas station on the side of the road."
                    + "The owner calls to you. 'Hey there, friend! Good deal on gas, just for you!' ",
                    new string[]
                    {
                        "Purchase 30 gas for 3 scrap metal.",
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
                            
                            DataTracker.Current.Player.Inventory.AddItem("Fuel", 30);
                        },
                        () => {

                            //SceneManager.UnloadSceneAsync("Encounter");
                        }
                    },
                    new Func<bool>[]  // condition (whether the player can take the action or not)
                    {
                        () => {
                            // Always available
                            return DataTracker.Current.Player.Inventory.Contains("Scrap Metal") > 2;
                        },
                        () => {
                            // Always available
                            return true;
                        }
                    },
                    new String[]  // Text to display on failure
                    {
                        "You don't have enough scrap.",
                        ""
                    },
                    new Action[]  // Action to take on failure
                    {
                        () => {},
                        () => {}
                    }
                );
            
                RandomEncounter renc10 = new RandomEncounter(
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
                );

                  RandomEncounter renc11 = new RandomEncounter(
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
                );

        EncounterManager.Instance.AddRandomEncounter(renc6);
        EncounterManager.Instance.AddRandomEncounter(renc7);
        EncounterManager.Instance.AddFixedEncounter(renc8);
        EncounterManager.Instance.AddFixedEncounter(renc9);
        EncounterManager.Instance.AddFixedEncounter(renc10);
        EncounterManager.Instance.AddFixedEncounter(renc11);
    }

    private void BeginQuest()
    {
        Quest quest = new Quest.Builder("Tutorial Quest")
            .SetDescription("Bring Smithsville a new generator.")

            .AddStage(new Stage.Builder("Purchase generator in York.")
                .AddCondition(new TransactionCondition("Purchase 1 generator at the York General Store.", "Generator", 1, TransactionCondition.TransactionTypeEnum.BUY, TownManager.Instance.GetTownByName("York").Id)
                )
            )

            .AddStage(new Stage.Builder("Bring generator to Smithsville.")
                .AddCondition(new DialogueCondition("Deliver the generator to the sheriff of Smithsville.", "TutorialPart5GiveGenerator")
                )
            )

            .Build();
    }


}
