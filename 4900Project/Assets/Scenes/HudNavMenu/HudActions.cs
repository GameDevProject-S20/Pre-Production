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

    bool JwasKeyDown = false;
    bool IwasKeyDown = false;
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
}
