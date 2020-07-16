using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.ExitMenu;
using UnityEngine;
using UnityEngine.SceneManagement;
using SIEvents;


public class HudActions : MonoBehaviour
{

    Events.QuestEvents.QuestManagerUpdated questChangedEvent;

    void Start(){
        questChangedEvent = DataTracker.Current.EventManager.OnQuestManagerUpdated;
        questChangedEvent.AddListener(new UnityEngine.Events.UnityAction(() => { QuestChangedHandler();}));

    }
    public void OnInventoryButtonClick()
    {
        SceneManager.LoadScene("InventoryScene", LoadSceneMode.Additive);

    }

    public void OnMenuButtonClick()
    {
        ExitMenuControl.BringUpExitMenu();
    }

    public void OnJournalButtonClick()
    {
       GameObject.Find("Map").GetComponent<OverworldMapUI>().QuestJournalCanvas.SetActive(true); 
       GameObject.Find("questjournal").GetComponent<UnityEngine.UI.Image>().color = Color.white;
    }

    public void QuestChangedHandler()
    {
       GameObject.Find("questjournal").GetComponent<UnityEngine.UI.Image>().color = Color.red;
    }
}
