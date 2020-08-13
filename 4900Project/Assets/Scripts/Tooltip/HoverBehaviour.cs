using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class HoverBehaviour : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Tooltip tooltip;

    void Awake()
    {
        try
        {
            tooltip = InventoryWindow.Instance.tooltip;
        }
        catch (Exception e)
        { }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            string name = gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
            int index = name.IndexOf("(");
            if (index > 0)
            {
                name = name.Substring(0, index - 1);
            }
            Item temp;
            ItemManager.Current.itemsMaster.TryGetValue(name, out temp);

            if (tooltip.rightClickStage == 0)
            {
                tooltip.GenerateDetailedTooltip(temp);
                tooltip.rightClickStage++;
            }
            else
            {
                if (temp.IsConsumable)
                {
                    int health = temp.GetHealthCured();
                    int maxhealth = temp.GetMaxHealthGiven();
                    Debug.Log($"Adding {health} hp");
                    Debug.Log($"giving {maxhealth} maxhp");
                    Player.Instance.ModifyCap(maxhealth);
                    Player.Instance.AddHealth(health);
                    Player.Instance.Inventory.RemoveItem(temp.DisplayName, 1);
                }
                else
                {
                    tooltip.GenerateTooltip(temp);
                    tooltip.rightClickStage = 0;
                }
            }
        }
        else
        {
            tooltip.gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        string name = gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        int index = name.IndexOf("(");
        if(index > 0)
        {
            name = name.Substring(0, index-1);
        }

        Item temp;
        ItemManager.Current.itemsMaster.TryGetValue(name, out temp);
        tooltip.GenerateTooltip(temp);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.gameObject.SetActive(false);
        tooltip.rightClickStage = 0;
    }
}
