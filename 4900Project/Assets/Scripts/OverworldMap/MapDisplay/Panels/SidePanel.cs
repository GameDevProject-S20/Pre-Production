using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SIEvents;
using DG.Tweening;

public class SidePanel : MonoBehaviour
{
    [SerializeField]
    GameObject TitlePanel;
    [SerializeField]
    GameObject Buffer;

    [SerializeField]
    TextMeshProUGUI TitleText;
    [SerializeField]
    TextMeshProUGUI SubtitleText;
    [SerializeField]
    TextMeshProUGUI DetailsText;

    bool isTown;
    int id;
    int locId;
    bool isOpen = false; 

    private void Start() {
        EventManager.Instance.OnTravelStart.AddListener(Close);
        Close();
    }

    bool EwasKeyDown = false; 
    private void Update()
    {
        /*
         * Enter hot key
         */
        var EisDown = Input.GetKey(KeyCode.E);
        if (EisDown != EwasKeyDown && EisDown && isOpen)
        {
            EwasKeyDown = EisDown;
            OnButtonClick();
        }
        EwasKeyDown = EisDown;

    }

    public void OpenTown(int id){
        this.id = id;
        OverworldMap.LocationNode node;
        DataTracker.Current.WorldMap.GetNode(id, out node);
        locId = node.LocationId;
        isTown = true;
        Town t = DataTracker.Current.TownManager.GetTownById(locId);
        TitlePanel.SetActive(true);
        Buffer.SetActive(true);

        TitleText.text = t.Name;

        string subtitle = "";
        string details = "";

        if (t.Tags.Count > 0)
        {
            foreach (var tag in t.Tags)
            {

                details += "<color=" + tag.Colour + ">" + tag.Name + "</color>:\n" + tag.Summary + "\n";

                if (tag.Name == "Small" || tag.Name == "Medium" || tag.Name == "Large")
                {
                    continue;
                }
                subtitle += tag.Name + " ";
            }
        }

        switch (t.Size)
        {
            case Town.Sizes.Small:
                subtitle += "Hamlet";
                break;
            case Town.Sizes.Medium:
                subtitle += "Town";
                break;
            case Town.Sizes.Large:
                subtitle += "City";
                break;
            default:
                subtitle += "Town";
                break;
        }
        SubtitleText.text = subtitle;
        DetailsText.text = details;
        Open();
    }

    public void OpenPOI(int id){
        this.id = id;
        OverworldMap.LocationNode node;
        DataTracker.Current.WorldMap.GetNode(id, out node);
        locId = node.LocationId;
        isTown = false;
        TitlePanel.SetActive(false);
        Buffer.SetActive(false);
        SubtitleText.text = "Point of Interest";
        DetailsText.text = "A minor location which you may enter.";
        Open();
    }

    public void Open(){
        isOpen = true;
        gameObject.GetComponent<RectTransform>().DOLocalMoveX(500, 0.5f);
    }

    public void Close(){
        isOpen = false; 
        gameObject.GetComponent<RectTransform>().DOLocalMoveX(800, 0.5f);
    }

    public void OnButtonClick(){
        if (isTown){
            EventManager.Instance.OnEnterTownButtonClick.Invoke(locId);
        }
        else {
            EventManager.Instance.OnEnterPOIButtonClick.Invoke(locId);
        }
        Close();

    }
}
