using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using SIEvents;

public class InventoryWindow : MonoBehaviour
{
    public static InventoryWindow Instance { get => instance; }

    private static InventoryWindow instance;

    [SerializeField]
    GameObject inventoryListItem;

    [SerializeField]
    Transform playerInventoryObject;

    [SerializeField]
    public Tooltip tooltip;

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

    List<GameObject> itemObjects;
    int count = 50;
    void Start()
    {
        if (instance != null) Destroy(instance);
        instance = this;
        tooltip = GameObject.Find("Tooltip").GetComponent<Tooltip>();
        itemObjects = new List<GameObject>();
        Populate();

        EventManager.Instance.OnInventoryChange.AddListener(() => {
            Clear();
            Populate();
        });
    }

    public void leave(){
        transform.parent.gameObject.SetActive(false);
    }

    // Add item sprites
    void Populate()
    {
        foreach(var item in DataTracker.Current.Player.Inventory.Contents){
            var listItem = GameObject.Instantiate(inventoryListItem, Vector3.zero, Quaternion.identity);
            listItem.GetComponentInChildren<TextMeshProUGUI>().text = ItemManager.Current.itemsMaster[item.Key].DisplayName + " (" + item.Value + ") ";
            listItem.transform.Find("Text").Find("Rarity").GetComponent<Image>().sprite = getValueString(ItemManager.Current.itemsMaster[item.Key].Value);
            listItem.transform.Find("Icon").GetComponent<Image>().sprite = ItemManager.Current.itemsMaster[item.Key].Icon;
            listItem.transform.SetParent(playerInventoryObject, false);
            listItem.name = ItemManager.Current.itemsMaster[item.Key].DisplayName + "_button";
            listItem.GetComponent<HoverBehaviour>().tooltip = tooltip;
            itemObjects.Add(listItem);
        }
    }

    // Clear item sprites
    void Clear()
    {
        foreach (var item in itemObjects)
        {
            Destroy(item);
        }
        itemObjects.Clear();
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
