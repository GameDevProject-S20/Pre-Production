using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using SIEvents;

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



    Transform ActionMenu;
    // stores the town data for later reference
    private Town townData;

    private void Start()
    {
        UpdatePrefab();

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

        // TODO: disabled for build, and because it isn't working - replace with accepted mockup style 
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
        
        TextMeshProUGUI t = transform.Find("TownBackground").Find("DataBackground").Find("TownDataBackground").Find("TownData").Find("Tags").GetComponent<TextMeshProUGUI>();
        t.text = "";
        foreach(var tag in townData.Tags){
            t.text += "<color="+ tag.Colour +">" + tag.Name +"</color>; ";
        }
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
        GameObject NewAction = Instantiate(ActionPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        NewAction.transform.parent = ActionMenu;
        if (townData.LeaderPortrait != null)
        {
            NewAction.transform.Find("Portrait").GetComponent<Image>().sprite = townData.LeaderPortrait;
        }
        else
        {
            NewAction.transform.Find("Portrait").GetComponent<Image>().sprite = IconMissing;
        }
                    NewAction.transform.Find("Portrait").GetComponent<Image>().sprite = townData.LeaderPortrait;

        NewAction.transform.Find("Name").GetComponent<Text>().text = townData.Leader;
        NewAction.transform.Find("Description").GetComponent<Text>().text = townData.LeaderBlurb;
        NewAction.transform.Find("Interaction").GetComponent<Image>().sprite = TalkIcon;
        NewAction.transform.Find("Interaction").GetComponent<Button>().onClick.AddListener(() => 
        {
            DataTracker.Current.EventManager.TriggerEncounter.Invoke(townData.leaderDialogueEncounterId);
        });
        //need to set talking interaction on button

        //And now every shop
        for (int i = 0; i < townData.shops.Count; i++)
        {
            NewAction = Instantiate(ActionPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            NewAction.transform.parent = ActionMenu;
            if (ShopManager.Instance.GetShopById(townData.shops[i]).Portrait != null)
            {
                NewAction.transform.Find("Portrait").GetComponent<Image>().sprite = ShopManager.Instance.GetShopById(townData.shops[i]).Portrait;
            }
            else
            {
                NewAction.transform.Find("Portrait").GetComponent<Image>().sprite = IconMissing;
            }

            NewAction.transform.Find("Name").GetComponent<Text>().text = ShopManager.Instance.GetShopById(townData.shops[i]).name;
            NewAction.transform.Find("Description").GetComponent<Text>().text = ShopManager.Instance.GetShopById(townData.shops[i]).shortDescription;
            
            if (ShopManager.Instance.GetShopById(townData.shops[i]).type == Shop.ShopTypes.GeneralStore)
            {
                NewAction.transform.Find("Interaction").GetComponent<Image>().sprite = GeneralStoreIcon;
            }
            else if (ShopManager.Instance.GetShopById(townData.shops[i]).type == Shop.ShopTypes.Pharmacy)
            {
                NewAction.transform.Find("Interaction").GetComponent<Image>().sprite = MedicineIcon;
            }


            //on click, need to tell the DataTracker what shop I really want. MUST use a new int. 
            int x = townData.shops[i];
            NewAction.transform.Find("Interaction").GetComponent<Button>().onClick.AddListener(() =>
            {
                DataTracker.Current.currentShopId = x;
                SceneManager.sceneUnloaded += OnSceneUnloaded;
                SceneManager.LoadScene("ShopScene", LoadSceneMode.Additive); // Currently using additive for the shop. 
            });

        // ! I am hardcoding this as a test
        // ! Add a mechanic to Smithsville
        if (townData.Id == 3){
            NewAction = Instantiate(ActionPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            NewAction.transform.parent = ActionMenu;
            NewAction.transform.Find("Portrait").GetComponent<Image>().sprite = IconMissing;
            NewAction.transform.Find("Name").GetComponent<Text>().text = "Rob's Repairs";
            NewAction.transform.Find("Description").GetComponent<Text>().text = "Local Mechanic";
            NewAction.transform.Find("Interaction").GetComponent<Image>().sprite = IconMissing;
            NewAction.transform.Find("Interaction").GetComponent<Button>().onClick.AddListener(() => 
            {
                DataTracker.Current.EventManager.TriggerEncounter.Invoke(10);
            });
        }
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

}
