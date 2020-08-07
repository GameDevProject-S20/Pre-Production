using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.ExitMenu;
using UnityEngine;
using UnityEngine.SceneManagement;
using SIEvents;
using DG.Tweening;

public class HudActions : MonoBehaviour
{
    protected GameObject activeObject;

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
        ActivateInterface(GameObject.Find("Map").GetComponent<OverworldMapUI>().InventoryCanvas);
        EventManager.Instance.FreezeMap.Invoke();

    }

    public void OnMenuButtonClick()
    {
        DeactivateInterface();
        EventManager.Instance.EscapeMenuRequested.Invoke();
    }

    public void OnJournalButtonClick()
    {
        ActivateInterface(GameObject.Find("Map").GetComponent<OverworldMapUI>().QuestJournalCanvas);
        GameObject.Find("questjournal").GetComponent<UnityEngine.UI.RawImage>().material = default;
        EventManager.Instance.FreezeMap.Invoke();
    }

    public void QuestChangedHandler()
    {
       Material glowing = Resources.Load<Material>("Materials/Glowing");
       GameObject.Find("questjournal").GetComponent<UnityEngine.UI.RawImage>().material = Instantiate(glowing);
    }

    /// <summary>
    /// Deactivates all opened interfaces.
    /// </summary>
    protected void DeactivateInterface()
    {
        if (activeObject != null)
        {
            activeObject.SetActive(false);
        }

        // Tell the Escape Menu to close
        DataTracker.Current.EventManager.EscapeMenuCloseRequested.Invoke();
    }

    /// <summary>
    /// Activates a new interface. Closes the past one if one was open.
    /// </summary>
    /// <param name="interfaceCanvas"></param>
    protected void ActivateInterface(GameObject interfaceCanvas)
    {
        DeactivateInterface();

        interfaceCanvas.SetActive(true);
        activeObject = interfaceCanvas;
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
