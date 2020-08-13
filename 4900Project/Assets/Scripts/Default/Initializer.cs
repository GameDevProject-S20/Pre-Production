using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Encounters;
using Quests;
using SIEvents;
using System.Xml.Serialization;

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
        SetStartingInventory();
        InitializeEncounters();
        BuildQuest();
        StartCoroutine(loader());
    }

    private void SetStartingInventory()
    {
        Player.Instance.Inventory.AddItem("Hunting Rifle", 1);
    }

    private void InitializeEncounters()
    {
        foreach (Encounter e in new JsonEncounterDataSource().GetEncounterEnumerator())
        {
            Debug.Log(e.ToString());
            EncounterManager.Instance.AddEncounter(e);
        }


        EncounterManager.Instance.ToggleRandomEncounters(false);
        EncounterManager.Instance.ReloadRandomEncounterQueue();
    }

    private void BuildQuest()
    {
       Quest quest = new Quest.Builder("Tutorial Quest")
            .SetDescription("Bring Smithsville a new generator.")

            .AddStage(new Stage.Builder("Purchase generator in York.")
                .AddCondition(new TransactionCondition("Purchase 1 generator at the York General Store.", "Generator", 1, TransactionCondition.TransactionTypeEnum.BUY, TownManager.Instance.GetTownByName("York").Id)
                )
            )

            .AddStage(new Stage.Builder("Bring generator to Smithsville.")
                .AddCondition(new EncounterCompleteCondition("Talk to the sheriff of Smithsville.", 2)
                )
            )

            .Build();

        quest = new Quest.Builder("Machine Merchant")
            .SetDescription("Steelton needs a replacement for some machinery.")

            .AddStage(new Stage.Builder("Purchase heavy machinery in Frakton.")
                .AddCondition(new TransactionCondition("Purchase heavy machinery.", "Heavy Machinery", 1, TransactionCondition.TransactionTypeEnum.BUY, TownManager.Instance.GetTownByName("Frakton").Id)
                )
            )

            .AddStage(new Stage.Builder("Bring the machinery to Steelton.")
                .AddCondition(new EncounterCompleteCondition("Talk to the Steelmaker in Steelton.", 211)
                )
            )

            .Build();
    }


}
