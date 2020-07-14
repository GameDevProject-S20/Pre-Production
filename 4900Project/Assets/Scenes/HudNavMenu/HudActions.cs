using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class HudActions : MonoBehaviour
{

    public void OnInventoryButtonClick()
        {
            SceneManager.LoadScene("InventoryTestScene");
            Debug.Log("Clicked");
        }

    //public void OnMenuButtonClick()
    //{
    //    TownMenu.SetActive(true);
    //}
}
