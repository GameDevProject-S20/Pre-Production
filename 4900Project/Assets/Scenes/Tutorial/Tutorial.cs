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
             },
             new Action[]
             {
                             () => {
                                 EncounterManager.Instance.GetFixedEncounter(2).AllowProgression();
                                 // Go to the world map
                             }
             },
             new List<Condition>(),
             1
         );

        Encounter enc2 = new Encounter(
             "Enc 2",
             "Tutorial",
             "Welcome to the first town!",
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
                                         EncounterManager.Instance.GetFixedEncounter(3).AllowProgression();
                                     }
             },
             new List<Condition>(),
             2
         );

        Encounter enc3 = new Encounter(
            "Enc 3",
            "Tutorial",
            "Welcome to the second town!",
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
                                                EncounterManager.Instance.GetFixedEncounter(4).AllowProgression();
                                                // Go to shop
                                            }
            },
            new List<Condition>(),
            3
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
                                                        EncounterManager.Instance.GetFixedEncounter(5).AllowProgression();
                                                    }
            },
            new List<Condition>(new OnStageCompleteCondition(/**/)),
            3
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
            new List<Condition>(new OnQuestCompleteCondition(/**/)),
            2
        );

        EncounterManager.Instance.AddFixedEncounter(enc1);
        EncounterManager.Instance.AddFixedEncounter(enc2);
        EncounterManager.Instance.AddFixedEncounter(enc3);
        EncounterManager.Instance.AddFixedEncounter(enc4);
        EncounterManager.Instance.AddFixedEncounter(enc5);

        enc1.AllowProgression();
    }

    private void BeginQuest()
    {
        Quest quest = new Quest.Builder("Medicine Quest")
            .SetDescription("Find medicine and sell it in York.")

            .AddStage(new Stage.Builder("Purchase medicine in Smithsville.")
                .AddCondition(new TransactionCondition("Purchase 1 medicine at the Smithsville Pharmacy", "Medicine", 1, TransactionCondition.TransactionTypeEnum.BUY, TownManager.Instance.GetTownByName("Smithsville").Id)
                )
            )

            .AddStage(new Stage.Builder("Sell Medicine in York.")
                .AddCondition(new TransactionCondition("Sell 1 medicine to the York General Store", "Medicine", 1, TransactionCondition.TransactionTypeEnum.SELL, TownManager.Instance.GetTownByName("York").Id)
                )
            )

            .Build();
    }

    private void LoadTown()
    {
        SceneManager.LoadScene("Town", LoadSceneMode.Single);
    }
}
