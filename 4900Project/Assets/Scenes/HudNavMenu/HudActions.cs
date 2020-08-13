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

    private GameObject escapeMenu;

    void Start(){
        questChangedEvent = DataTracker.Current.EventManager.OnQuestManagerUpdated;
        questChangedEvent.AddListener(new UnityEngine.Events.UnityAction(() => { QuestChangedHandler();}));

        escapeMenu = GameObject.Find("EscapeMenuCanvas");
    }

    bool JwasKeyDown = false;
    bool IwasKeyDown = false;
    bool TwasKeyDown = false;
    bool HwasKeyDown = false; 
    public void Update()
    {

        /*
         * Journal hot key
         */
        var JisDown = Input.GetKey(KeyCode.J);
        if (JisDown != JwasKeyDown && JisDown) 
        {
            JwasKeyDown = JisDown;
            if (GameObject.Find("Map").GetComponent<OverworldMapUI>().QuestJournalCanvas.activeSelf)
            {
                GameObject.Find("QuestUI").GetComponent<QuestJournalWindow>().disableUI();
            }
            else
            {
                OnJournalButtonClick();
            }
        }

        JwasKeyDown = JisDown;

        /*
         * inventory hot key
         */
        var IisDown = Input.GetKey(KeyCode.I);
        if (IisDown != IwasKeyDown && IisDown)
        {
            IwasKeyDown = IisDown;
            if (GameObject.Find("Map").GetComponent<OverworldMapUI>().InventoryCanvas.activeInHierarchy)
            {
                GameObject.FindGameObjectWithTag("InventoryWindow").GetComponent<InventoryWindow>().leave();
            }
            else
            {
                OnInventoryButtonClick();
            }
        }
        IwasKeyDown = IisDown;

        /*
         * toggle map T hot key
         */
        var TisDown = Input.GetKey(KeyCode.T);
        if (TisDown != TwasKeyDown && TisDown)
        {
            TwasKeyDown = TisDown;
            OnToggleView(); 
        }
        TwasKeyDown = TisDown;

        /*
         * toggle map T hot key
         */
        var HisDown = Input.GetKey(KeyCode.H);
        if (HisDown != HwasKeyDown && HisDown)
        {
            HwasKeyDown = HisDown;
            ToggleHelpPanel();
        }
        HwasKeyDown = HisDown;


    }



    public void OnInventoryButtonClick()
    {
        ToggleInterface(GameObject.Find("Map").GetComponent<OverworldMapUI>().InventoryCanvas);
        EventManager.Instance.FreezeMap.Invoke();

    }

    public void OnMenuButtonClick()
    {
        // Store if the interface is active -- have to do this before the Deactivate,
        // because it'll always deactivate after that method call
        var escapeMenuActive = escapeMenu.GetComponent<Canvas>().enabled;

        // Deactivate every interface
        DeactivateInterface();

        // If we were not already active, activate the escape menu
        if (!escapeMenuActive)
        {
            EventManager.Instance.EscapeMenuRequested.Invoke();
        }
        activeObject = null;
    }

    public void OnJournalButtonClick()
    {
        ToggleInterface(GameObject.Find("Map").GetComponent<OverworldMapUI>().QuestJournalCanvas);
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

        EventManager.Instance.ForceUnfreezeMap.Invoke(); 
    }

    /// <summary>
    /// Activates a new interface. Closes the past one if one was open.
    /// </summary>
    /// <param name="interfaceCanvas"></param>
    protected void ToggleInterface(GameObject interfaceCanvas)
    {
        // Store if the interface is already active
        // Do this before deactivating, otherwise it'll always be inactive
        var isActive = interfaceCanvas.activeInHierarchy;

        // Deactivate every interface ...
        DeactivateInterface();

        // If we were already active, we now have every interface closed;
        // Otherwise, we want to open this interface & set it as active
        if (isActive)
        {
            activeObject = null;
        }
        else
        {
            interfaceCanvas.SetActive(true);
            activeObject = interfaceCanvas;
        }
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
