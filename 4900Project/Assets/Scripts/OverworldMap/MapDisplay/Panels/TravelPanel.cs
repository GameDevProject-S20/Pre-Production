using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using SIEvents;

public class TravelPanel : MonoBehaviour
{


    protected enum States { Closed, Open, Selected }
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
            fuelText.text = "<color=#F91509>" + fuelCost + " Fuel</color>";

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

    public void Select(){
        state = States.Selected;
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
