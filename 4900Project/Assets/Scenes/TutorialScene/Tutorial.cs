using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Encounters;

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
            "Welcome to town! I'm the Sheriff of these here parts! You'll be able to find the things you can do in my town on the Town Menu screen. Now, don't be going messing around in my town or you'll be hearing from me and your reputation will suffer. These are hard times and we can't be just letting anyone into our community, if your reputation drops low enough you'd best keep away. I see you're a traveling merchant, I hear the general store is looking for some medicine. You can get that from the Pharmacy and sell it to the general store. Good luck!",
            new string[]
            {
                            "Accept Quest"
            },
            new string[]
            {
                            "Visit the Pharmacy to purchase some Medicine.",
            },
            new Action[]
            {
                            () => {
                                BeginQuest();
                                TempTestQuest();
                                LoadTown();
                            }
            }
        );

        DataTracker.Current.EncounterManager.AddFixedEncounter(enc);
        DataTracker.Current.EncounterManager.RunFixedEncounter(enc.Id);
    }

    private void BeginQuest()
    {
        var quest = new Quest.Builder("Medicine Quest")
            .SetDescription("Purchase one Item1 to sell to the general store.")
            //.SetCorrespondingTownName(null)
            //.SetUsesTurnInButton(true)
            //.SetDisplayName("Medicine Quest")
            .Build();

        {
            QuestStage s = new QuestStage();
            s.Description = "Purchase 1 medicine from the Pharmacy.";
            // TODO: Need the Node ID
            TransactionCondition condition = new LocationSpecificTransactionCondition("Purchase 1 Medicine", "Medicine", 1, TransactionCondition.TranscationTypeEnum.buy, Quest.OnCompletion, 0);
            s.conditions.Add(condition);
            quest.AddStage(s);
        }

        {
            QuestStage s = new QuestStage();
            s.Description = "Sell 1 Medicine to the General Store.";
            // TODO: Need the Node ID
            TransactionCondition condition = new LocationSpecificTransactionCondition("Purchase 1 Medicine", "Medicine", 1, TransactionCondition.TranscationTypeEnum.sell, Quest.OnCompletion, 0);
            s.conditions.Add(condition);
            quest.AddStage(s);
        }

        DataTracker.Current.QuestManager.AddQuest(quest);
        DataTracker.Current.QuestManager.StartQuest("Medicine Quest");

    }

    private void TempTestQuest()
    {
        // Verify that quest progression works
        foreach (var quest in DataTracker.Current.QuestManager.GetQuests())
        {
            Debug.Log("Quest Stage should be 0. Is: " + quest.CurrentStage);
        }

        Item item = new Item("Medicine", "Medicine", "It's medicine.", "Yep, it's medicine.", 15f, 0.4f);
        DataTracker.Current.Player.Inventory.addItem(item.name, 1);

        // Add EventManager to DataTracker
        // Add Event For Transactions
        // Invoke that event here for testing
        // Should be invoked by the store instead once testing confirmed.
        DataTracker.Current.EventManager.Transaction.Invoke();

        foreach (var quest in DataTracker.Current.QuestManager.GetQuests())
        {
            Debug.Log("Quest Stage should be 1. Is: " + quest.CurrentStage);
        }
    }


    private void LoadTown()
    {
        SceneManager.LoadScene("Town", LoadSceneMode.Single);
    }
}
