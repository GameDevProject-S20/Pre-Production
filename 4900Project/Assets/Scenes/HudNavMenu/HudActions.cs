using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.ExitMenu;
using UnityEngine;
using UnityEngine.SceneManagement;


public class HudActions : MonoBehaviour
{

    public void OnInventoryButtonClick()
    {
        SceneManager.LoadScene("InventoryTestScene", LoadSceneMode.Additive);

    }

    public void OnMenuButtonClick()
    {
        ExitMenuControl.BringUpExitMenu();
    }

    public void OnJournalButtonClick()
    {
       GameObject.Find("Map").GetComponent<OverworldMapUI>().QuestJournalCanvas.SetActive(true); 
    }
}
