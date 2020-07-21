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

    [SerializeField]
    Sprite AbundantImage;
    [SerializeField]
    Sprite UncommonImage;
    [SerializeField]
    Sprite CommonImage;
    [SerializeField]
    Sprite RareImage;
    [SerializeField]
    Sprite LegendaryImage;

    private void Start() {
        foreach(var item in DataTracker.Current.Player.Inventory.Contents){
            var listItem = GameObject.Instantiate(inventoryListItem, Vector3.zero, Quaternion.identity);
            listItem.GetComponentInChildren<TextMeshProUGUI>().text = ItemManager.Current.itemsMaster[item.Key].DisplayName + " (" + item.Value + ") ";
            listItem.transform.Find("Text").Find("Rarity").GetComponent<Image>().sprite = getValueString(ItemManager.Current.itemsMaster[item.Key].Value);
            listItem.transform.Find("Icon").GetComponent<Image>().sprite = ItemManager.Current.itemsMaster[item.Key].Icon;
            listItem.transform.SetParent(playerInventoryObject, false);
            listItem.name = ItemManager.Current.itemsMaster[item.Key].DisplayName + "_button";
        }
    }

    public void leave(){
        transform.parent.gameObject.SetActive(false);
    }

    Sprite getValueString(float value)
    {
        if (value < 5)
        {
            return AbundantImage;
        }
        else if (value < 15)
        {
            return CommonImage;
        }
        else if (value < 30)
        {
            return UncommonImage;
        }
        else if (value < 50)
        {
            return RareImage;
        }
        else
        {
            return LegendaryImage;
        }
    }
}
