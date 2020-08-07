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
    
    [Header("GameObjects from Canvas")]
    [SerializeField]
    GameObject startButton;
    [SerializeField]
    GameObject loadingText; 


    void Start()
    {
        EventManager.Instance.onDataTrackerLoad.AddListener(finishLoading);
        SceneManager.LoadScene("DataTracker", LoadSceneMode.Additive);
    }

    IEnumerator loader(){
        yield return new WaitForSeconds(3);
        startButton.SetActive(true);
        loadingText.SetActive(false); 
    }

    public void OnEnterGameClick()
    {
        SceneManager.LoadScene("MapScene", LoadSceneMode.Single);
    }

    void finishLoading(){
        // ARL -- cleanup
        // This is just so I can get something....
        //DataTracker.Current.currentLocationId = 0;
        InitializeEncounters();
        BuildQuest();
        StartCoroutine(loader());
    }

    private void InitializeEncounters()
    {
        foreach (Encounter e in new JsonEncounterDataSource().GetEncounterEnumerator())
        {
            Debug.Log(e.ToString());
            EncounterManager.Instance.AddEncounter(e);
        }

        //EncounterManager.Instance.RunRandomEncounter("tutorial");
    }

    private void BuildQuest()
    {
        // Build Tutorial quest
       Quest quest = new Quest.Builder("Tutorial Quest")
            .SetDescription("Bring Smithsville a new generator.")

            .AddStage(new Stage.Builder("Purchase generator in York.")
                .AddCondition(new TransactionCondition("Purchase 1 generator at the York General Store.", "Generator", 1, TransactionCondition.TransactionTypeEnum.BUY, TownManager.Instance.GetTownByName("York").Id)
                )
            )

            .AddStage(new Stage.Builder("Bring generator to Smithsville.")
                .AddCondition(new EncounterCompleteCondition("Deliver the generator to the sheriff of Smithsville.", 2)
                )
            )

            .Build();

        // Build Follow-Up Quest - Investigate the Bandit Token
        Quest banditQuest = new Quest.Builder("Bandit Quest")
            .SetDescription("Investigate the weird token you found.")
            
            // First step: Travel to Rocket City to investigate the token
            .AddStage(new Stage.Builder("Go ask the town leader of Rocket City about the token.")
                        .AddCondition(new TownEnterCondition("Travel to Rocket City.", 5))
                        .AddCondition(new EncounterCompleteCondition("Talk to the town leader.", 6))
                       )

            // Second step: Use the token to get into the Bandit Camp
            .AddStage(new Stage.Builder("Use the token to get into the bandit camp.")
                        .AddCondition(new TownEnterCondition("Travel to Skull.", 9))
                        .AddCondition(new EncounterCompleteCondition("Talk to the bandit leader.", 7))
                       )

            .Build();
    }


}
