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
        //tooltip = InventoryWindow.Instance.tooltip;
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
            tooltip.GenerateDetailedTooltip(temp);
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
    }
}
