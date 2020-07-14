using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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

        ActionMenu = transform.Find("TownBackground").Find("DataBorder").Find("DataBackground").Find("TownActionsBorder").Find("TownActionsBackground").Find("ActionList").Find("ActionGrid");

        // We need to reset the action items for this town since it's changed. 
        foreach (Transform child in ActionMenu)
        {
            GameObject.Destroy(child.gameObject);
        }


        transform.Find("TownBackground").Find("TownName").GetComponent<Text>().text = townData.Name;
        transform.Find("TownBackground").Find("DataBorder").Find("DataBackground").Find("TownDataBackground").Find("TownData").Find("Description").GetComponent<Text>().text = townData.Description;


        //use rarity tier of town to determine proper icon
        if (townData.tier != rarity.None)
        {
            switch (townData.tier)
            {
                case rarity.Abundant:
                    transform.Find("TownBackground").Find("DataBorder").Find("DataBackground").Find("TownDataBackground").Find("TownData").Find("TownImage").Find("RarityIcon").GetComponent<Image>().sprite = AbundantIcon;
                    break;
                case rarity.Common:
                    transform.Find("TownBackground").Find("DataBorder").Find("DataBackground").Find("TownDataBackground").Find("TownData").Find("TownImage").Find("RarityIcon").GetComponent<Image>().sprite = CommonIcon;
                    break;
                case rarity.Uncommon:
                    transform.Find("TownBackground").Find("DataBorder").Find("DataBackground").Find("TownDataBackground").Find("TownData").Find("TownImage").Find("RarityIcon").GetComponent<Image>().sprite = UncommonIcon;
                    break;
                case rarity.Rare:
                    transform.Find("TownBackground").Find("DataBorder").Find("DataBackground").Find("TownDataBackground").Find("TownData").Find("TownImage").Find("RarityIcon").GetComponent<Image>().sprite = RareIcon;
                    break;
                case rarity.Unique:
                    transform.Find("TownBackground").Find("DataBorder").Find("DataBackground").Find("TownDataBackground").Find("TownData").Find("TownImage").Find("RarityIcon").GetComponent<Image>().sprite = LegendaryIcon;
                    break;
            }
        }
        else
        {
            transform.Find("TownBackground").Find("DataBorder").Find("DataBackground").Find("TownDataBackground").Find("TownData").Find("TownImage").Find("RarityIcon").GetComponent<Image>().sprite = IconMissing;
        }

        //Post tag images related to town
        for (int i = 1; i < 5; i++)
        {
            if (townData.tags.Count >= i)
            {
                switch (townData.tags[i - 1])
                {
                    case typetag.Food:
                        transform.Find("TownBackground").Find("DataBorder").Find("DataBackground").Find("TownDataBackground").Find("TownData").Find("TownImage").Find("ResourceIcon" + i.ToString()).GetComponent<Image>().sprite = FoodIcon;
                        break;
                    case typetag.Fuel:
                        transform.Find("TownBackground").Find("DataBorder").Find("DataBackground").Find("TownDataBackground").Find("TownData").Find("TownImage").Find("ResourceIcon" + i.ToString()).GetComponent<Image>().sprite = FuelIcon;
                        break;
                    case typetag.Medicine:
                        transform.Find("TownBackground").Find("DataBorder").Find("DataBackground").Find("TownDataBackground").Find("TownData").Find("TownImage").Find("ResourceIcon" + i.ToString()).GetComponent<Image>().sprite = MedicineIcon;
                        break;
                }
            }
            else
            {
                transform.Find("TownBackground").Find("DataBorder").Find("DataBackground").Find("TownDataBackground").Find("TownData").Find("TownImage").Find("ResourceIcon" + i.ToString()).GetComponent<Image>().enabled = false;
            }
        }

        //find town image
        if (townData.Icon != null)
        {
            transform.Find("TownBackground").Find("DataBorder").Find("DataBackground").Find("TownDataBackground").Find("TownData").Find("TownImage").GetComponent<Image>().sprite = townData.Icon;
        }
        else
        {
            transform.Find("TownBackground").Find("DataBorder").Find("DataBackground").Find("TownDataBackground").Find("TownData").Find("TownImage").GetComponent<Image>().sprite = IconMissing;
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
        NewAction.transform.Find("Name").GetComponent<Text>().text = townData.Leader;
        NewAction.transform.Find("Description").GetComponent<Text>().text = townData.LeaderBlurb;
        NewAction.transform.Find("Interaction").GetComponent<Image>().sprite = IconMissing;
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
                SceneManager.LoadScene("InventoryTestScene", LoadSceneMode.Additive); // Currently using additive for the shop. 
            });
        }
    }


}
