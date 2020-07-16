using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class InventoryWindow : MonoBehaviour
{

    [SerializeField]
    GameObject inventoryListItem;

    [SerializeField]
    Transform playerInventoryObject;

    private void Start() {
        foreach(var item in DataTracker.Current.Player.Inventory.getContents()){
            var listItem = GameObject.Instantiate(inventoryListItem, Vector3.zero, Quaternion.identity);
            listItem.GetComponentInChildren<TextMeshProUGUI>().text = ItemManager.Current.itemsMaster[item.Key].DisplayName + " (" + item.Value + ") ";
            listItem.transform.SetParent(playerInventoryObject, false);
            listItem.name = ItemManager.Current.itemsMaster[item.Key].DisplayName + "_button";
        }
    }

    public void leave(){
        SceneManager.UnloadSceneAsync("InventoryScene"); 
    }
}
