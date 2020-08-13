using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using SIEvents;

public class TravelPanel : MonoBehaviour
{


    protected enum States { Closed, Open, Closing }
    protected States state;

    [SerializeField]
    protected RectTransform rt;

    [SerializeField]
    protected RectTransform pointerRt;
    protected Tween t;

    [SerializeField]
    int a;
    [SerializeField]
    int b;

    [SerializeField]
    protected float openTime = 0.2f;

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

    private void Awake()
    {
        Close();
        EventManager.Instance.OnTravelStart.AddListener(() =>
        {
            Close();
        });
    }

    private void Update() {
        rt.ForceUpdateRectTransforms();
    }

    public void SetNode(MapNode node)
    {
        Node = node;
    }

    public void SetTravelInfo(int fuelCost, int travelTime)
    {
        CostInfo.SetActive(true);
        timeText.text = travelTime + " Hour";
        if (travelTime > 1) timeText.text += "s";

        int fuelCount = DataTracker.Current.Player.Inventory.Contains("Fuel");

        if (fuelCount < fuelCost) {
            fuelText.text = "<color=#F91509> On Foot</color>";

        } 
        else if (fuelCount == fuelCost) {
            fuelText.text = "<color=#F91509>" + fuelCost + " Fuel</color>";

        }
        else if (fuelCount <= 5 * fuelCost) {
            fuelText.text = "<color=#b0120a>" + fuelCost + " Fuel</color>";

        }
        else if (fuelCount <= 10 * fuelCost) {
            fuelText.text = "<color=#7c100b>" + fuelCost + " Fuel</color>";
        }
        else {
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
        if (positiveMods.Count > 0 || negativeMods.Count > 0)
        {
            DetailText.text += "\n Buys: \n  ";
            if (positiveMods.Count > 0)
            {
                DetailText.text += "<sprite=9>";
            }
            foreach (int a in positiveMods)
            {
                DetailText.text += "<sprite=" + a + ">";
            }
            if (negativeMods.Count > 0)
            {
                DetailText.text += "  <sprite=0>";
            }
            foreach (int a in negativeMods)
            {
                DetailText.text += "<sprite=" + a + ">";
            }
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
            DetailText.text += "\n Sells: \n  ";
            if (positiveMods.Count > 0)
            {
                DetailText.text += "<sprite=9>";
            }
            foreach (int a in positiveMods)
            {
                DetailText.text += "<sprite=" + a + ">";
            }
            if (negativeMods.Count > 0)
            {
                DetailText.text += "  <sprite=0>";
            }
            foreach (int a in negativeMods)
            {
                DetailText.text += "<sprite=" + a + ">";
            }
        }
    }

    /*public void EnableButton(){
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
    */
    public void onNodeClick()
    {
        if (state == States.Open){
            MapTravel.Travel(Node, () =>
            {
                EventManager.Instance.OnTravelStart.Invoke();
            });
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


    private void Open()
    {
        state = States.Open;
        Sequence s = DOTween.Sequence();
        s.Append(pointerRt.DOScale(1, openTime));
        s.Join(pointerRt.DOLocalMoveY(-13, openTime));
        s.Join(rt.DOLocalMoveY(a, openTime));
        s.Insert(openTime / 2, rt.DOScaleX(1, openTime));
        s.Play();
    }

    private void Close()
    {
        state = States.Closing;
        Sequence s = DOTween.Sequence();
        s.Append(rt.DOScaleX(0, openTime));
        s.Insert(openTime / 2, pointerRt.DOScale(0, openTime));
        s.Join(pointerRt.DOLocalMoveY(-40, openTime));
        s.Join(rt.DOLocalMoveY(b, openTime));
        s.AppendCallback(OnClosed);
        s.Play();

    }

    private void OnClosed(){
        if (Node){
            Node.Close();
            Node = null;
        }
        state = States.Closed;
        CostInfo.SetActive(false);
        NameText.gameObject.SetActive(false);
        DetailText.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public bool IsOpen(){
        return state == States.Open;
    }
}
