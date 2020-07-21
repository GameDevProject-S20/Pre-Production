using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using UnityEngine.XR.WSA.Input;
using SIEvents;

public class Trading : MonoBehaviour
{
    Shop shop;

    Inventory offer = new Inventory();
    Inventory cart  = new Inventory();
    Inventory copyOfPlayerInventory;
    Inventory copyOfShopInventory;
    public int tradeFairness = 4;

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
    [SerializeField]
    Transform offerFeedback;
    [SerializeField]
    TextMeshProUGUI Name;
    [SerializeField]
    GameObject Scale;
    [SerializeField]
    Sprite FairTrade;
    [SerializeField]
    Sprite PlayerTrade;
    [SerializeField]
    Sprite ShopTrade;
    [SerializeField]
    AudioClip ScaleChange;
    [SerializeField]
    Sprite AbundantImage;
    [SerializeField]
    Sprite UncommonImage;
    [SerializeField]
    Sprite CommonImage;
    [SerializeField]
    Sprite RareImage;
    [SerializeField]
    Sprite LegendaryImage;

    public void Start() {
        shop = ShopManager.Instance.GetShopById(DataTracker.Current.currentShopId);
        copyOfPlayerInventory = new Inventory(DataTracker.Current.Player.Inventory);
        copyOfShopInventory = new Inventory(shop.inventory);
        buildShopList();
        buildPlayerList();
        Name.text = shop.name;
    }

    //======================================================================//
    //                           UI Functions                               //
    //======================================================================//

    void buildShopList(){

        foreach(Transform child in shopInventoryObject.transform){
            Destroy(child.gameObject);
        }

        foreach(var item in copyOfShopInventory.getContents()){
            var listItem = GameObject.Instantiate(inventoryListItem, Vector3.zero, Quaternion.identity);
            listItem.GetComponentInChildren<TextMeshProUGUI>().text = ItemManager.Current.itemsMaster[item.Key].DisplayName + " (" + item.Value + ") ";
            listItem.transform.Find("Text").Find("Rarity").GetComponent<Image>().sprite = getValueString(ItemManager.Current.itemsMaster[item.Key].Value);
            listItem.transform.SetParent(shopInventoryObject, false);
            listItem.GetComponent<Button>().onClick.AddListener(() => {addToCart(item.Key);});
            listItem.name = ItemManager.Current.itemsMaster[item.Key].DisplayName + "_button";
        }

    }

    void buildPlayerList(){

        foreach(Transform child in playerInventoryObject.transform){
            Destroy(child.gameObject);
        }

        foreach(var item in copyOfPlayerInventory.getContents()){
            var listItem = GameObject.Instantiate(inventoryListItem, Vector3.zero, Quaternion.identity);
            listItem.GetComponentInChildren<TextMeshProUGUI>().text = ItemManager.Current.itemsMaster[item.Key].DisplayName + " (" + item.Value + ") ";
            listItem.transform.Find("Text").Find("Rarity").GetComponent<Image>().sprite = getValueString(ItemManager.Current.itemsMaster[item.Key].Value);
            listItem.transform.SetParent(playerInventoryObject, false);
            listItem.GetComponent<Button>().onClick.AddListener(() => {addToOffer(item.Key);});
            listItem.name = ItemManager.Current.itemsMaster[item.Key].DisplayName + "_button";
        }
    }


    void buildOfferList(){

        foreach(Transform child in offerListObject.transform){
            Destroy(child.gameObject);
        }
        foreach (var item in offer.getContents()){
            var listItem = GameObject.Instantiate(inventoryListItem, Vector3.zero, Quaternion.identity);
            listItem.GetComponentInChildren<TextMeshProUGUI>().text = ItemManager.Current.itemsMaster[item.Key].DisplayName + " (" + item.Value + ") ";
            listItem.transform.Find("Text").Find("Rarity").GetComponent<Image>().sprite = getValueString(ItemManager.Current.itemsMaster[item.Key].Value);
            listItem.transform.SetParent(offerListObject, false);
            listItem.GetComponent<Button>().onClick.AddListener(() => {removeFromOffer(item.Key);});
            listItem.name = ItemManager.Current.itemsMaster[item.Key].DisplayName + "_button";
        }
    }

    void buildCartList(){
        foreach(Transform child in cartListObject.transform){
            Destroy(child.gameObject);
            Destroy(child.gameObject);
        }
        foreach(var item in cart.getContents()){
            var listItem = GameObject.Instantiate(inventoryListItem, Vector3.zero, Quaternion.identity);
            listItem.GetComponentInChildren<TextMeshProUGUI>().text = ItemManager.Current.itemsMaster[item.Key].DisplayName + " (" + item.Value + ") ";
            listItem.transform.Find("Text").Find("Rarity").GetComponent<Image>().sprite = getValueString(ItemManager.Current.itemsMaster[item.Key].Value);
            listItem.transform.SetParent(cartListObject, false);
            listItem.GetComponent<Button>().onClick.AddListener(() => {removeFromCart(item.Key);});
            listItem.name = ItemManager.Current.itemsMaster[item.Key].DisplayName + "_button";
        }
    }

    Sprite getValueString(float value){
        if (value < 5){
                return AbundantImage;
            }
            else if (value < 15){
                return CommonImage;
            }
            else if (value < 30){
                return UncommonImage;
            }
            else if (value < 50){
                return RareImage;
            }
            else{
                return LegendaryImage;
            }
    }

    void addToCart(string item){
        cart.AddItem(item, 1);
        copyOfShopInventory.RemoveItem(item, 1); 
        buildCartList();
        buildShopList();
        ScaleSwap();
    }

    void addToOffer(string item){
        offer.AddItem(item, 1);
        copyOfPlayerInventory.RemoveItem(item, 1);
        buildOfferList();
        buildPlayerList();
        ScaleSwap();
    }

    void removeFromCart(string item){
        copyOfShopInventory.AddItem(item, 1);
        cart.RemoveItem(item, 1);
        buildCartList();
        buildShopList();
        ScaleSwap();
    }

    void removeFromOffer(string item){
        copyOfPlayerInventory.AddItem(item, 1);
        offer.RemoveItem(item, 1);
        buildPlayerList();
        buildOfferList();
        ScaleSwap();

    }

    public void ScaleSwap()
    {
        if (tradeFairness != validateTrade())
        {
            switch (validateTrade())
            {
                case 0:
                    break;
                case 1:
                    Scale.GetComponent<Image>().sprite = PlayerTrade;
                    break;
                case 2:
                    Scale.GetComponent<Image>().sprite = FairTrade;
                    break;
                case 3:
                    Scale.GetComponent<Image>().sprite = ShopTrade;
                    break;
            }
            AudioSource audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
            audioSource.PlayOneShot(ScaleChange, 1.0F);
            tradeFairness = validateTrade();
        }

    }

    public void onTradeButtonClick(){
        switch (validateTradeAfterModifiers())
        {
            case 0:
                break;
            case 1:
                offerFeedback.GetComponent<TMPro.TextMeshProUGUI>().text = "My now, what a generous offer!";
                makeTrade();
                break;
            case 2:
                offerFeedback.GetComponent<TMPro.TextMeshProUGUI>().text = "A fair offer, you got yourself a deal";
                makeTrade();
                break;
            case 3:
                offerFeedback.GetComponent<TMPro.TextMeshProUGUI>().text = "This ain't a charity, make a real offer would ya";
                break;
        }
    }

    //======================================================================//
    //                         Trading Functions                            //
    //======================================================================// 

    /// <summary>
    /// A trade is valid if the player has enough inventory space to fit all the items they are purchasing
    /// AND the player's offer matches or surpasses the value of their cart.
    /// </summary>
    /// <returns>
    /// 0: Not enough room in inventory (not accepted)
    /// 1: Player offered more than cart is worth (accepted)
    /// 2: Player offered less than cart is worth (accepted)
    /// 3: Player offer is too low (not accepted)
    /// </returns>
    int validateTrade(){
        if (copyOfPlayerInventory.CanFitItems(cart.TotalWeight())){
            float totalCartValue = cart.TotalValue();
            float totalOfferValue = offer.TotalValue();
            float difference =  totalOfferValue - totalCartValue;
            if(difference > totalCartValue * shop.acceptedPriceDifference) {
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

    int validateTradeAfterModifiers()
    {
        if (copyOfPlayerInventory.CanFitItems(cart.TotalWeight()))
        {
            float totalCartValue = cart.TotalValueAfterModifiers(shop.toPlayerModifiers);
            float totalOfferValue = offer.TotalValueAfterModifiers(shop.fromPlayerModifiers);
            float difference = totalOfferValue - totalCartValue;
            if (difference > totalCartValue * shop.acceptedPriceDifference)
            {
                return 1;
            }
            else if (Mathf.Abs(difference) <= totalCartValue * shop.acceptedPriceDifference)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }
        else
        {
            return 0;
        }
    }

    void makeTrade(){

        Events.TransactionEvents.Entity from = Events.TransactionEvents.Entity.SYSTEM;
        Events.TransactionEvents.Entity to = Events.TransactionEvents.Entity.PLAYER;

        foreach (var item in cart.getContents()){

            shop.inventory.RemoveItem(item.Key, item.Value);

            copyOfPlayerInventory.AddItem(item.Key, item.Value);
            DataTracker.Current.Player.Inventory.AddItem(item.Key, item.Value);

            Events.TransactionEvents.Details transactionDetails = new Events.TransactionEvents.Details(item.Key, item.Value, DataTracker.Current.currentShopId, from, to);
            DataTracker.Current.EventManager.OnTransaction.Invoke(transactionDetails);
        }

        from = Events.TransactionEvents.Entity.PLAYER;
        to = Events.TransactionEvents.Entity.SYSTEM;

        foreach (var item in offer.getContents()){

            DataTracker.Current.Player.Inventory.RemoveItem(item.Key, item.Value);
            
            shop.inventory.AddItem(item.Key, item.Value);
            copyOfShopInventory.AddItem(item.Key, item.Value);

            Events.TransactionEvents.Details transactionDetails = new Events.TransactionEvents.Details(item.Key, item.Value, DataTracker.Current.currentShopId, from, to);
            DataTracker.Current.EventManager.OnTransaction.Invoke(transactionDetails);
        }

        cart.getContents().Clear();
        offer.getContents().Clear();
        buildPlayerList();
        buildShopList();
        buildOfferList();
        buildCartList();
    }

    public void leave(){
        //SceneManager.LoadScene("Town");
        SceneManager.UnloadSceneAsync("ShopScene"); 
    }

}

