using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Encounters;
using Quests;
using System.Linq;

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
            "Welcome to town! I'm the Sheriff of these here parts! You'll be able to find the things you can do in my town on the Town Menu screen. Now, don't be going messing around in my town or you'll be hearing from me and your reputation will suffer. These are hard times and we can't be just letting anyone into our community, if your reputation drops low enough you'd best keep away. I see you're a traveling merchant, I hear the town of Riverbed is looking for some medicine. You can get that from the Pharmacy and bring it to them. Good luck!",
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
                                //TempTestQuest();
                                LoadTown();
                            }
            }
        );

        DataTracker.Current.EncounterManager.AddFixedEncounter(enc);
        DataTracker.Current.EncounterManager.RunFixedEncounter(enc.Id);
    }

    private void BeginQuest()
    {
        Quest.DebugPipe debugPipe = new Quest.DebugPipe(
            (Quest q) => Debug.Log(string.Format("Completed Quest: {0}", q.Name)), 
            (Stage s) => Debug.Log(string.Format("Completed Stage: {0}", s.Description)), 
            (Condition c) => Debug.Log(string.Format("Completed Condition: {0}", c.Description))
        );

        Debug.Log(string.Join("\n", TownManager.Instance.GetTownEnumerable().Select(t => string.Format("{0}: {1}", t.Id, t.Name))));

        Quest quest = new Quest.Builder("Medicine Quest")
            .SetDescription("Find medicine and sell it in York.")

            .AddStage(new Stage.Builder("Purchase medicine in Smithsville.")
                .AddOnCompleteListener(debugPipe.OnStageComplete)
                .AddCondition(new LocationSpecificTransactionCondition("Purchase 1 medicine at the Smithsville Pharmacy", "Medicine", 1, TransactionCondition.TransactionTypeEnum.BUY, TownManager.Instance.GetTownByName("Smithsville").Id)
                    .AddListener(debugPipe.OnConditionComplete)
                )
            )    
            
            .AddStage(new Stage.Builder("Sell Medicine in York.")
                .AddOnCompleteListener(debugPipe.OnStageComplete)
                .AddCondition(new LocationSpecificTransactionCondition("Sell 1 medicine to the York General Store", "Medicine", 1, TransactionCondition.TransactionTypeEnum.SELL, TownManager.Instance.GetTownByName("York").Id)
                    .AddListener(debugPipe.OnConditionComplete)
                )
            )

            //.AddOnCompleteListener(debugPipe.OnQuestComplete)
            .Build();

        /*DataTracker.Current.QuestManager.AddQuest(quest);
        DataTracker.Current.QuestManager.StartQuest("Medicine Quest");*/

    }

    private void ConditionDebugger()
    {

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
        //DataTracker.Current.EventManager.Transaction.Invoke();

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
