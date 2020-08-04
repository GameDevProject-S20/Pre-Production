using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using SIEvents;
using System;
using UnityEngine.Events;
using Assets.Scripts.Town;

public class TownWindow : MonoBehaviour
{
    [SerializeField]
    GameObject ActionPrefab;

    [SerializeField]
    Sprite AbundantIcon;

    [SerializeField]
    Sprite CommonIcon;

    [SerializeField]
    Sprite UncommonIcon;

    [SerializeField]
    Sprite RareIcon;

    [SerializeField]
    Sprite LegendaryIcon;

    [SerializeField]
    Sprite FoodIcon;

    [SerializeField]
    Sprite FuelIcon;

    [SerializeField]
    Sprite MedicineIcon;

    [SerializeField]
    Sprite GeneralStoreIcon;

    [SerializeField]
    Sprite IconMissing;

    [SerializeField]
    Sprite TempTownIconBadCode;

    [SerializeField]
    Sprite TalkIcon;

    [SerializeField]
    Sprite HospitalIcon;

    [SerializeField]
    HospitalData hospitalData;

    Transform ActionMenu;
    // stores the town data for later reference
    private Town townData;

    private void Start()
    {
        UpdatePrefab();
        DataTracker.Current.EventManager.OnTownUpdated.AddListener((town) =>
        {
            // If we're in this town, we want to update the window
            var currentTown = TownManager.Instance.GetCurrentTownData();
            if (currentTown != null && currentTown.Id == town.Id) 
            {
                UpdatePrefab();
            }
        });
    }

    public void UpdatePrefab()
    {
        Debug.Log("Update the prefab for new town");



        townData = TownManager.Instance.GetCurrentTownData();

        ActionMenu = transform.Find("TownBackground").Find("DataBackground").Find("TownActionsBorder").Find("TownActionsBackground").Find("ActionList").Find("ActionGrid");

        // We need to reset the action items for this town since it's changed. 
        foreach (Transform child in ActionMenu)
        {
            GameObject.Destroy(child.gameObject);
        }


        transform.Find("TownBackground").Find("TownName").GetComponent<Text>().text = townData.Name;
        transform.Find("TownBackground").Find("DataBackground").Find("TownDataBackground").Find("TownData").Find("Description").GetComponent<Text>().text = townData.Description;

       { // TODO: disabled for build, and because it isn't working - replace with accepted mockup style 
        //use Rarity tier of town to determine proper icon
        //if (townData.tier != Rarity.None)
        //{
        //    switch (townData.tier)
        //    {
        //        case Rarity.Abundant:
        //            transform.Find("TownBackground").Find("DataBackground").Find("TownDataBackground").Find("TownData").Find("TownImage").Find("RarityIcon").GetComponent<Image>().sprite = AbundantIcon;
        //            break;
        //        case Rarity.Common:
        //            transform.Find("TownBackground").Find("DataBackground").Find("TownDataBackground").Find("TownData").Find("TownImage").Find("RarityIcon").GetComponent<Image>().sprite = CommonIcon;
        //            break;
        //        case Rarity.Uncommon:
        //            transform.Find("TownBackground").Find("DataBackground").Find("TownDataBackground").Find("TownData").Find("TownImage").Find("RarityIcon").GetComponent<Image>().sprite = UncommonIcon;
        //            break;
        //        case Rarity.Rare:
        //            transform.Find("TownBackground").Find("DataBackground").Find("TownDataBackground").Find("TownData").Find("TownImage").Find("RarityIcon").GetComponent<Image>().sprite = RareIcon;
        //            break;
        //        case Rarity.Unique:
        //            transform.Find("TownBackground").Find("DataBackground").Find("TownDataBackground").Find("TownData").Find("TownImage").Find("RarityIcon").GetComponent<Image>().sprite = LegendaryIcon;
        //            break;
        //    }
        //}
        //else
        //{
        //    transform.Find("TownBackground").Find("DataBackground").Find("TownDataBackground").Find("TownData").Find("TownImage").Find("RarityIcon").GetComponent<Image>().sprite = IconMissing;
        //}

        //Post tag images related to town
       /* for (int i = 1; i < 5; i++)
        {
            if (townData.tags.Count >= i)
            {
                switch (townData.tags[i - 1])
                {
                    case ItemTag.Food:
                        transform.Find("TownBackground").Find("DataBackground").Find("TownDataBackground").Find("TownData").Find("TownImage").Find("ResourceIcon" + i.ToString()).GetComponent<Image>().sprite = FoodIcon;
                        break;
                    case ItemTag.Fuel:
                        transform.Find("TownBackground").Find("DataBackground").Find("TownDataBackground").Find("TownData").Find("TownImage").Find("ResourceIcon" + i.ToString()).GetComponent<Image>().sprite = FuelIcon;
                        break;
                    case ItemTag.Medical:
                        transform.Find("TownBackground").Find("DataBackground").Find("TownDataBackground").Find("TownData").Find("TownImage").Find("ResourceIcon" + i.ToString()).GetComponent<Image>().sprite = MedicineIcon;
                        break;
                }
            }
            else
            {
                transform.Find("TownBackground").Find("DataBackground").Find("TownDataBackground").Find("TownData").Find("TownImage").Find("ResourceIcon" + i.ToString()).GetComponent<Image>().enabled = false;
            }
        }*/
       }
        
        string details = "";
            if (townData.Tags.Count > 0)
            {
                foreach (var tag in townData.Tags)
                {
                    if (tag.Name == "Small" || tag.Name == "Medium" || tag.Name == "Large")
                    {
                        continue;
                    }
                    details += "<color=" + tag.Colour+">"+tag.Name + "</color> ";
                }
            }

            switch (townData.Size)
            {
                case Town.Sizes.Small:
                    details += "Hamlet";
                    break;
                case Town.Sizes.Medium:
                    details += "Town";
                    break;
                case Town.Sizes.Large:
                    details += "City";
                    break;
                default:
                    details += "Town";
                    break;
            }
            transform.Find("TownBackground").Find("DataBackground").Find("TownDataBackground").Find("TownData").Find("Tags").GetComponent<TextMeshProUGUI>().text = details;

        //find town image
        if (townData.Icon != null)
        {
            transform.Find("TownBackground").Find("DataBackground").Find("TownDataBackground").Find("TownData").Find("TownImage").GetComponent<Image>().sprite = townData.Icon;
        }
        else
        {
            transform.Find("TownBackground").Find("DataBackground").Find("TownDataBackground").Find("TownData").Find("TownImage").GetComponent<Image>().sprite = IconMissing;
        }

        //time to create the action panels
        //first the leader
        CreateActionButton(townData.LeaderPortrait, TalkIcon, townData.Leader, townData.LeaderBlurb, () =>
        {
            DataTracker.Current.EventManager.OnOpenDialogueClick.Invoke(townData.leaderDialogueEncounterId);
        });

        //need to set talking interaction on button

        //And now every shop
        for (int i = 0; i < townData.shops.Count; i++)
        {
            // Declare all the data for the shop
            var shopId = townData.shops[i];
            var shop = ShopManager.Instance.GetShopById(shopId);
            Sprite actionIcon = null;
            switch (shop.type)
            {
                case Shop.ShopTypes.GeneralStore:
                    actionIcon = GeneralStoreIcon;
                    break;
                case Shop.ShopTypes.Pharmacy:
                    actionIcon = MedicineIcon;
                    break;
                default:
                    // use default
                    break;
            }

            // Create the button
            CreateActionButton(shop.Portrait, actionIcon, shop.name, shop.shortDescription, () =>
            {
                DataTracker.Current.currentShopId = shopId;
                SceneManager.sceneUnloaded += OnSceneUnloaded;
                SceneManager.LoadScene("ShopScene", LoadSceneMode.Additive); // Currently using additive for the shop. 
            });
        }

        // And add the Hospital if the town has one
        if (townData.HasHospital)
        {
            CreateActionButton(HospitalIcon, null, hospitalData.Name, hospitalData.Description, () =>
            {
                DataTracker.Current.EventManager.OnOpenDialogueClick.Invoke(hospitalData.EncounterId);
            });
        }

        EventManager.Instance.OnTownEnter.Invoke(townData);
    }

    private void OnSceneUnloaded(Scene s)
    {
        if (s.name == "ShopScene")
        {
            EventManager.Instance.OnTownEnter.Invoke(townData);
        }

        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    /// <summary>
    /// This should only be used in the Town scene after the tutorial text. 
    /// </summary>
    public void OnClickBringToMap()
    {
        SceneManager.LoadScene("MapScene", LoadSceneMode.Single); 
    }

    /// <summary>
    /// Adds a new Action Button into the window.
    /// </summary>
    /// <param name="icon"></param>
    /// <param name="interactionIcon"></param>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="handler"></param>
    protected void CreateActionButton(Sprite icon, Sprite interactionIcon, string name, string description, UnityAction handler)
    {
        GameObject NewAction = Instantiate(ActionPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        // Default to the IconMissing Sprite if we have no icon
        if (icon == null)
        {
            icon = IconMissing;
        }

        NewAction.transform.parent = ActionMenu;
        NewAction.transform.Find("Portrait").GetComponent<Image>().sprite = icon;
        NewAction.transform.Find("Name").GetComponent<Text>().text = name;
        NewAction.transform.Find("Description").GetComponent<Text>().text = description;
        NewAction.transform.Find("Interaction").GetComponent<Button>().onClick.AddListener(handler);

        // Only update the Interaction icon if we have one to update to
        if (interactionIcon)
        {
            NewAction.transform.Find("Interaction").GetComponent<Image>().sprite = interactionIcon;
        }
    }
}
