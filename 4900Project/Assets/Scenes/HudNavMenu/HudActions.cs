using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.ExitMenu;
using UnityEngine;
using UnityEngine.SceneManagement;
using SIEvents;
using DG.Tweening;

public class HudActions : MonoBehaviour
{

    Events.QuestEvents.QuestManagerUpdated questChangedEvent;
    bool helpPanelOpened = false;
    [SerializeField]
    GameObject helpPanel;

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

    public void OnToggleView(){
       GameObject.Find("Map").GetComponent<OverworldMapUI>().ToggleColourMode();
    }

    public void ToggleHelpPanel(){
        if (helpPanelOpened) {
            helpPanel.GetComponent<RectTransform>().DOLocalMoveX(-900, 0.3f);
            helpPanelOpened = false;
        }
        else {
            helpPanel.GetComponent<RectTransform>().DOLocalMoveX(-380, 0.3f);
            helpPanelOpened = true;
        }
    }
}
