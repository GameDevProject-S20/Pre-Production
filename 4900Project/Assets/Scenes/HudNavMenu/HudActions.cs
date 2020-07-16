using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.ExitMenu;
using UnityEngine;
using UnityEngine.SceneManagement;


public class HudActions : MonoBehaviour
{

    public void OnInventoryButtonClick()
    {
        //SceneManager.LoadScene("InventoryTestScene", LoadSceneMode.Additive);
        Debug.Log("Invetory Button Clicked Clicked HUD");
    }

    public void OnJournalButtonClick()
    {
        Debug.Log("Journal Button Clicked Clicked HUD");
    }

    public void OnMenuButtonClick()
    {
        ExitMenuControl.BringUpExitMenu();
    }
}
