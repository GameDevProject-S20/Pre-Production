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
        GameObject.Find("questjournal").GetComponent<UnityEngine.UI.RawImage>().material = default;
        EventManager.Instance.FreezeMap.Invoke();

    }

    public void QuestChangedHandler()
    {
       Material glowing = Resources.Load<Material>("Materials/Glowing");
       GameObject.Find("questjournal").GetComponent<UnityEngine.UI.RawImage>().material = Instantiate(glowing);
    }

    public void OnToggleView(){
        GameObject.Find("Map").GetComponent<OverworldMapUI>().ToggleColourMode();
    }
}
