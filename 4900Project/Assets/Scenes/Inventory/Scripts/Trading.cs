using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Trading : MonoBehaviour
{
    Shop shop;

    Inventory offer = new Inventory();
    Inventory cart  = new Inventory();
    Inventory copyOfPlayerInventory;
    Inventory copyOfShopInventory;

    [SerializeField]
    GameObject inventoryListItem;
    [SerializeField]
    Transform shopInventoryObject;
    [SerializeField]
    Transform playerInventoryObject;
    [SerializeField]
    Transform offerListObject;
    [SerializeField]
    Transform cartListObject;
    public void init(Shop shop_) {
        shop = shop_;
        copyOfPlayerInventory = new Inventory(TradingTest.player.inventory);
        copyOfShopInventory = new Inventory(shop.inventory);
        offer.weightLimit = 10000;
        cart.weightLimit = 10000;
        buildShopList();
        buildPlayerList();

    }


    void buildShopList(){

        foreach(Transform child in shopInventoryObject.transform){
            Destroy(child.gameObject);
        }

        foreach(var item in copyOfShopInventory.getContents()){
            var listItem = GameObject.Instantiate(inventoryListItem, Vector3.zero, Quaternion.identity);
            listItem.GetComponentInChildren<TextMeshProUGUI>().text = Inventory.itemsMaster[item.Key].displayName + " (" + item.Value + ")";
            listItem.transform.SetParent(shopInventoryObject, false);
            listItem.GetComponent<Button>().onClick.AddListener(() => {addToCart(item.Key);});
            listItem.name = Inventory.itemsMaster[item.Key].displayName + "_button";
        }

    }

    void buildPlayerList(){

        foreach(Transform child in playerInventoryObject.transform){
            Destroy(child.gameObject);
        }

        foreach(var item in copyOfPlayerInventory.getContents()){
            var listItem = GameObject.Instantiate(inventoryListItem, Vector3.zero, Quaternion.identity);
            listItem.GetComponentInChildren<TextMeshProUGUI>().text = Inventory.itemsMaster[item.Key].displayName + " (" + item.Value + ")";
            listItem.transform.SetParent(playerInventoryObject, false);
            listItem.GetComponent<Button>().onClick.AddListener(() => {addToOffer(item.Key);});
            listItem.name = Inventory.itemsMaster[item.Key].displayName + "_button";
        }
    }

    void buildOfferList(){

        foreach(Transform child in offerListObject.transform){
            Destroy(child.gameObject);
        }

        foreach(var item in offer.getContents()){
            var listItem = GameObject.Instantiate(inventoryListItem, Vector3.zero, Quaternion.identity);
            listItem.GetComponentInChildren<TextMeshProUGUI>().text = Inventory.itemsMaster[item.Key].displayName + " (" + item.Value + ")";
            listItem.transform.SetParent(offerListObject, false);
            listItem.GetComponent<Button>().onClick.AddListener(() => {removeFromOffer(item.Key);});
            listItem.name = Inventory.itemsMaster[item.Key].displayName + "_button";

        }
    }

    void buildCartList(){

        foreach(Transform child in cartListObject.transform){
            Destroy(child.gameObject);
        }
        foreach(var item in cart.getContents()){
            var listItem = GameObject.Instantiate(inventoryListItem, Vector3.zero, Quaternion.identity);
            listItem.GetComponentInChildren<TextMeshProUGUI>().text = Inventory.itemsMaster[item.Key].displayName + " (" + item.Value + ")";
            listItem.transform.SetParent(cartListObject, false);
            listItem.GetComponent<Button>().onClick.AddListener(() => {removeFromCart(item.Key);});
            listItem.name = Inventory.itemsMaster[item.Key].displayName + "_button";

        }
    }

    void addToCart(string item){
        cart.addItem(item, 1);
        copyOfShopInventory.removeItem(item, 1); 
        buildCartList();
        buildShopList();
    }

    void addToOffer(string item){
        offer.addItem(item, 1);
        copyOfPlayerInventory.removeItem(item, 1);
        buildOfferList();
        buildPlayerList();
    }

    void removeFromCart(string item){
        copyOfShopInventory.addItem(item, 1);
        cart.removeItem(item, 1);
        buildCartList();
        buildShopList();
    }

    void removeFromOffer(string item){
        copyOfPlayerInventory.addItem(item, 1);
        offer.removeItem(item, 1);
        buildPlayerList();
        buildOfferList();

    }

    public void onTradeButtonClick(){
        switch (validateTrade())
        {
            case 0:
            break;
            case 1: makeTrade(); Debug.Log("1");
            break;
            case 2: makeTrade(); Debug.Log("2");
            break;
            case 3: Debug.Log("Too Low!");
            break;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns>
    /// 0: Not enough room in inventory (not accepted)
    /// 1: Player offered more than cart is worth (accepted)
    /// 2: Player offered less than cart is worth (accepted)
    /// 3: Player offer is too low (not accepted)
    /// </returns>
    int validateTrade(){
        if (copyOfPlayerInventory.canFitItems(cart.totalWeight())){
            float totalCartValue = cart.totalValue(shop.toPlayerModifiers);
            float totalOfferValue = offer.totalValue(shop.fromPlayerModifiers);
            float difference =  totalOfferValue - totalCartValue;
            if (difference >= 0){
                return 1;
            }
            else if (Mathf.Abs(difference) <= totalCartValue * shop.acceptedPriceDifference){
                return 2;
            }
            else {
                return 3;
            }
        }
        else{
            return 0;
        }
    }

    void makeTrade(){
        foreach (var item in cart.getContents()){
            TradingTest.player.inventory.addItem(item.Key, item.Value);
            copyOfPlayerInventory.addItem(item.Key, item.Value);
        }
        foreach (var item in offer.getContents()){
            shop.inventory.addItem(item.Key, item.Value);
            copyOfShopInventory.addItem(item.Key, item.Value);
        }

        cart.getContents().Clear();
        offer.getContents().Clear();
        buildPlayerList();
        buildShopList();
        buildOfferList();
        buildCartList();
    }

}

