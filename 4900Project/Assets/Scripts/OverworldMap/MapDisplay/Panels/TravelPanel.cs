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

    public void AddTownTags(List<TownTag> tagList)
    {
        List<int> positiveMods = new List<int>();
        List<int> negativeMods = new List<int>();
        int i = 15;
        foreach(TownTag tagtemp in tagList)
        {
            foreach(KeyValuePair<ItemTag,float> mod in tagtemp.playerSellModifiers)
            {
                switch (mod.Key)
                {
                    case ItemTag.General:
                        i = 3;
                        break;
                    case ItemTag.Fuel:
                        i = 2;
                        break;
                    case ItemTag.Useable:
                        i = 10;
                        break;
                    case ItemTag.Food:
                        i = 1;
                        break;
                    case ItemTag.Luxury:
                        i = 4;
                        break;
                    case ItemTag.Medical:
                        i = 5;
                        break;
                    case ItemTag.Building_Materials:
                        i = 13;
                        break;
                    case ItemTag.Tools_And_Parts:
                        i = 8;
                        break;
                    case ItemTag.Combat:
                        i = 14;
                        break;
                    case ItemTag.Scientific:
                        i = 7;
                        break;
                    case ItemTag.Mineral:
                        i = 6;
                        break;
                    case ItemTag.Antique:
                        i = 12;
                        break;
                    case ItemTag.Advanced:
                        i = 11;
                        break;
                }
                if(mod.Value > 1)
                {
                    if(positiveMods.Contains(i) == false && i != 15)
                    {
                        positiveMods.Add(i);
                    }
                }
                else
                {
                    if (negativeMods.Contains(i) == false && i!= 15)
                    {
                        negativeMods.Add(i);
                    }
                }
            }
        }
        if(positiveMods.Count > 0 || negativeMods.Count > 0)
        {
            DetailText.text += "\n Buys:";
        }
        foreach(int a in positiveMods)
        {
            DetailText.text += "<sprite=9><sprite=" + a + ">"; 
        }
        foreach (int a in negativeMods)
        {
            DetailText.text += "<sprite=0><sprite=" + a + ">";
        }
        positiveMods = new List<int>();
        negativeMods = new List<int>();
        i = 15;
        foreach (TownTag tagtemp in tagList)
        {
            foreach (KeyValuePair<ItemTag, float> mod in tagtemp.shopSellModifiers)
            {
                switch (mod.Key)
                {
                    case ItemTag.General:
                        i = 3;
                        break;
                    case ItemTag.Fuel:
                        i = 2;
                        break;
                    case ItemTag.Useable:
                        i = 10;
                        break;
                    case ItemTag.Food:
                        i = 1;
                        break;
                    case ItemTag.Luxury:
                        i = 4;
                        break;
                    case ItemTag.Medical:
                        i = 5;
                        break;
                    case ItemTag.Building_Materials:
                        i = 13;
                        break;
                    case ItemTag.Tools_And_Parts:
                        i = 8;
                        break;
                    case ItemTag.Combat:
                        i = 14;
                        break;
                    case ItemTag.Scientific:
                        i = 7;
                        break;
                    case ItemTag.Mineral:
                        i = 6;
                        break;
                    case ItemTag.Antique:
                        i = 12;
                        break;
                    case ItemTag.Advanced:
                        i = 11;
                        break;
                }
                if (mod.Value > 1)
                {
                    if (positiveMods.Contains(i) == false && i != 15)
                    {
                        positiveMods.Add(i);
                    }
                }
                else
                {
                    if (negativeMods.Contains(i) == false && i != 15)
                    {
                        negativeMods.Add(i);
                    }
                }
            }
        }
        if (positiveMods.Count > 0 || negativeMods.Count > 0)
        {
            DetailText.text += "\n \n Sells: ";
        }
        foreach (int a in positiveMods)
        {
            DetailText.text += "<sprite=9><sprite=" + a + ">";
        }
        foreach (int a in negativeMods)
        {
            DetailText.text += "<sprite=0><sprite=" + a + ">";
        }
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
