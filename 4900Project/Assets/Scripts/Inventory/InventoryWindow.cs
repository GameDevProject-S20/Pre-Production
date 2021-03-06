﻿using System.Collections;
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

        tooltip = transform.parent.Find("InventoryWindow/Tooltip").gameObject.GetComponent<Tooltip>();

        itemObjects = new List<GameObject>();
        Populate();

        EventManager.Instance.OnInventoryChange.AddListener(() => {
            Clear();
            Populate();
        });
    }

    public void leave(){
        transform.parent.gameObject.SetActive(false);
        EventManager.Instance.UnfreezeMap.Invoke();

    }

    // Add item sprites
    void Populate()
    {
        foreach(var item in DataTracker.Current.Player.Inventory.Contents){
            var listItem = GameObject.Instantiate(inventoryListItem, Vector3.zero, Quaternion.identity);
            listItem.GetComponentInChildren<TextMeshProUGUI>().text = ItemManager.Current.itemsMaster[item.Key].DisplayName + " (" + item.Value + ") ";
            Image i = listItem.transform.Find("Text").Find("Rarity").GetComponent<Image>();
            i.sprite = getValueString(ItemManager.Current.itemsMaster[item.Key].tier);
            i.preserveAspect = true;
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

    Sprite getValueString(Rarity tier)
    {
        if (tier == Rarity.Abundant)
        {
            return AbundantImage;
        }
        else if (tier == Rarity.Common)
        {
            return CommonImage;
        }
        else if (tier == Rarity.Uncommon)
        {
            return UncommonImage;
        }
        else if (tier == Rarity.Rare)
        {
            return RareImage;
        }
        else
        {
            return LegendaryImage;
        }
    }
}
