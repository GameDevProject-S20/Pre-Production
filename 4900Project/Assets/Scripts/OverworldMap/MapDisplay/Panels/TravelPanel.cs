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
    TextMeshProUGUI NameText; 
    [SerializeField]
    TextMeshProUGUI DetailText;  

    MapNode Node;

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
        Node = node;
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

    public void SetName(string name){
        NameText.gameObject.SetActive(true);
        NameText.text = name;
    }
    public void SetDetails(string details){
        DetailText.gameObject.SetActive(true);
        DetailText.text = details;
    }



    public void onNodeClick()
    {
        if (MapTravel.Travel(Node)){
            EventManager.Instance.OnTravelStart.Invoke();
        }

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
        if (Node)
        {
            Node.Close();
            Node = null;
        }
        CostInfo.SetActive(false);
        NameText.gameObject.SetActive(false);
        DetailText.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
