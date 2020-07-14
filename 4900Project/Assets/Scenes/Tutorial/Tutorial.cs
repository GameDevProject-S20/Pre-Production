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
                                 EncounterManager.Instance.GetFixedEncounter(1).AllowProgression();
                                 SceneManager.LoadScene("MapScene");
                             }
             }
         );

        Encounter enc2 = new Encounter(
             "Enc 2",
             "Tutorial",
             "Welcome to York!",
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
             1
         );

        Encounter enc3 = new Encounter(
            "Enc 3",
            "Tutorial",
            "Welcome to Big Rock!",
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
            2
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
            2
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
            1
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
    }

    private void LoadTown()
    {
        SceneManager.LoadScene("Town", LoadSceneMode.Single);
    }
}
