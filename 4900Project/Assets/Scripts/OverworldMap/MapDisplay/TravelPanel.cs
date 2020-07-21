using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using SIEvents;

public class TravelPanel : MonoBehaviour
{

    enum States { Hidden, Hover, Selected }
    States state;

    [SerializeField]
    RectTransform rt;

    [SerializeField]
    RectTransform pointerRt;
    Tween t;

    [SerializeField]
    float openTime = 0.2f;

    [SerializeField]
    GameObject acceptButton;
    [SerializeField]
    GameObject cancelButton;

    [SerializeField]
    TextMeshProUGUI timeText;
    [SerializeField]
    TextMeshProUGUI fuelText;

    MapNode Node;

    private void Awake()
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

    public void SetInfo(int fuelCost, int travelTime)
    {

        timeText.text = travelTime + " Day";
        if (travelTime > 1) timeText.text += "s";

        if (fuelCost > DataTracker.Current.Player.Inventory.Contains("Fuel"))
        {
            fuelText.text = "<color=#F91509>" + fuelCost + " Fuel</color>";
            acceptButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            fuelText.text = fuelCost + " Fuel";
            acceptButton.GetComponent<Button>().interactable = true;
        }
    }

    public void onAcceptButtonClick()
    {
        EventManager.Instance.OnTravelStart.Invoke();
    }

    public void onCancelButtonClick()
    {
        Close();
    }

    /// <summary>
    /// On click, "lock" the panel and display the accept/cancel buttons
    /// </summary>
    public void onNodeClick()
    {
        if (state != States.Hidden){
            state = States.Selected;
            acceptButton.SetActive(true);
            cancelButton.SetActive(true);
            acceptButton.gameObject.GetComponent<RectTransform>().DOLocalMoveY(-30, 0.1f);
            cancelButton.gameObject.GetComponent<RectTransform>().DOLocalMoveY(-30, 0.1f);
        }
    }

    public void onNodeHover()
    {
        if (state != States.Selected)
        {
            Open();
        }
    }

    public void onNodeLeave()
    {
        if (state != States.Selected)
        {
            Close();
        }
    }

    private void Open()
    {
        state = States.Hover;
        Sequence s = DOTween.Sequence();
        s.Append(pointerRt.DOScale(1, openTime));
        s.Join(pointerRt.DOLocalMoveY(-13, openTime));
        s.Join(rt.DOLocalMoveY(0, openTime));
        s.Insert(openTime / 2, rt.DOScaleX(1, openTime));
        s.Play();
    }

    private void Close()
    {

        state = States.Hidden;
        acceptButton.gameObject.GetComponent<RectTransform>().DOLocalMoveY(0, 0.1f).OnComplete(() => { acceptButton.SetActive(false); });
        cancelButton.gameObject.GetComponent<RectTransform>().DOLocalMoveY(0, 0.1f).OnComplete(() => { cancelButton.SetActive(false); });
        Sequence s = DOTween.Sequence();
        s.Append(rt.DOScaleX(0, openTime));
        s.Insert(openTime / 2, pointerRt.DOScale(0, openTime));
        s.Join(pointerRt.DOLocalMoveY(-40, openTime));
        s.Join(rt.DOLocalMoveY(-40, openTime));
        s.AppendCallback(() =>
        {
            if (Node)
            {
                Node.Close();
                Node = null;
            }
            gameObject.SetActive(false);

        });
        s.Play();

    }
}
