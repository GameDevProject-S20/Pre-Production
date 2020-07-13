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
    private Encounter enc;

    void Start()
    {
        Dialogue();
    }

    private void Dialogue()
    {
       enc = new Encounter(
            "Tutorial",
            "Tutorial",
            "Welcome to town! I'm the Sheriff of these here parts! You'll be able to find the things you can do in my town on the Town Menu screen. Now, don't be going messing around in my town or you'll be hearing from me and your reputation will suffer. These are hard times and we can't be just letting anyone into our community, if your reputation drops low enough you'd best keep away. I see you're a traveling merchant, I hear the town of York is looking for some medicine. You can get that from the Pharmacy and bring it to them. Good luck!",
            new string[]
            {
                            "Accept Quest"
            },
            new string[]
            {
                            "Purchase some medicine to sell in the town of Riverbed.",
            },
            new Action[]
            {
                            () => {
                                BeginQuest();
                                LoadTown();
                            }
            }
        );

        DataTracker.Current.EncounterManager.AddFixedEncounter(enc);
        DataTracker.Current.EncounterManager.RunFixedEncounter(enc.Id);
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
