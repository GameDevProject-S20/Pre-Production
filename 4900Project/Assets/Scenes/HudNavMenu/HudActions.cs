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
        GameObject.Find("Map").GetComponent<OverworldMapUI>().InventoryCanvas.SetActive(true);
        EventManager.Instance.FreezeMap.Invoke();

    }

    public void OnMenuButtonClick()
    {
        EventManager.Instance.EscapeMenuRequested.Invoke();
    }

    public void OnJournalButtonClick()
    {
        GameObject.Find("Map").GetComponent<OverworldMapUI>().QuestJournalCanvas.SetActive(true); 
        GameObject.Find("questjournal").GetComponent<UnityEngine.UI.Image>().color = Color.white;
        EventManager.Instance.FreezeMap.Invoke();

    }

    public void QuestChangedHandler()
    {
       GameObject.Find("questjournal").GetComponent<UnityEngine.UI.Image>().color = Color.red;
    }
}
