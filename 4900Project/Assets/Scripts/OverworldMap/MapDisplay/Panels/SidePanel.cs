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

    private void Start() {
        EventManager.Instance.OnTravelStart.AddListener(Close);
        Close();
    }

    public void OpenTown(int id){
        this.id = id;
        isTown = true;
        Town t = DataTracker.Current.TownManager.GetTownById(id);
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
        isTown = false;
        TitlePanel.SetActive(false);
        Buffer.SetActive(false);
        SubtitleText.text = "Point of Interest";
        DetailsText.text = "A minor location which you may enter.";
        Open();
    }

    public void Open(){
        gameObject.GetComponent<RectTransform>().DOLocalMoveX(500, 0.5f);
    }

    public void Close(){
        gameObject.GetComponent<RectTransform>().DOLocalMoveX(800, 0.5f);
    }

    public void OnButtonClick(){
        if (isTown){
            EventManager.Instance.OnEnterTownButtonClick.Invoke(id);
        }
        else {
            EventManager.Instance.OnEnterPOIButtonClick.Invoke(id);
        }
        Close();

    }
}
