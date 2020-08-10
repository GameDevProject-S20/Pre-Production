using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CapacityHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Tooltip tooltip;

    void Start()
    {
        tooltip = transform.parent.parent.Find("Tooltip").gameObject.GetComponent<Tooltip>();
    }

    void Awake()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.GenerateTooltip("Inventory Capacity","Current inventory space used. Higher weights require more fuel to transport. Max is 750 units.");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.gameObject.SetActive(false);
    }
}