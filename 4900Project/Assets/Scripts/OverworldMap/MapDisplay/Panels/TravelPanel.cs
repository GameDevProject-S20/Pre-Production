using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using SIEvents;

public class TravelPanel : InfoPanel
{


   // [SerializeField]
   // GameObject acceptButton;
   // [SerializeField]
   // GameObject cancelButton;

    [SerializeField]
    GameObject CostInfo;
    [SerializeField]
    TextMeshProUGUI timeText;
    [SerializeField]
    TextMeshProUGUI fuelText;
    [SerializeField]
    GameObject EnterButton;
    [SerializeField]
    TextMeshProUGUI DetailText;  

    MapNode NodeObj;

    protected override void Awake()
    {
        Close();
        EventManager.Instance.OnTravelStart.AddListener(() =>
        {
            Close();
        });
    }

    public void SetNode(MapNode node)
    {
        NodeObj = node;
    }

    public void SetTravelInfo(int fuelCost, int travelTime)
    {
        CostInfo.SetActive(true);
        timeText.text = travelTime + " Day";
        if (travelTime > 1) timeText.text += "s";

        if (fuelCost > DataTracker.Current.Player.Inventory.Contains("Fuel"))
        {
            fuelText.text = "<color=#F91509>" + fuelCost + " Fuel</color>";
        }
        else
        {
            fuelText.text = fuelCost + " Fuel";
        }
    }

    public void SetDetails(string details){
        DetailText.gameObject.SetActive(true);
        DetailText.text = details;
    }

    public void EnableButton(){
        EnterButton.SetActive(true);
    }

    public void OnButtonClick(){
        OverworldMap.LocationNode node;
        DataTracker.Current.WorldMap.GetNode(NodeObj.NodeId, out node);
        if (node.Type == OverworldMap.LocationType.TOWN){
            EventManager.Instance.OnEnterTownButtonClick.Invoke(node.LocationId);
        }
        else if (node.Type == OverworldMap.LocationType.POI){
            EventManager.Instance.OnEnterPOIButtonClick.Invoke(node.LocationId);
        }
    }

    public void onNodeClick()
    {
        EventManager.Instance.OnTravelStart.Invoke();
    }

    public void onNodeHover()
    {
        if (state != States.Open)
        {
            Open();
        }
    }

    public void onNodeLeave()
    {
        if (state == States.Open)
        {
            Close();
        }
    }

    public void Select(){
        state = States.Selected;
    }

    protected override void OnClosed(){
        if (NodeObj)
        {
            NodeObj.Close();
            NodeObj = null;
        }
        CostInfo.SetActive(false);
        EnterButton.SetActive(false);
        DetailText.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
