﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using UnityEngine.XR.WSA.Input;
using SIEvents;
using System;
using UnityEngine.Events;
using DG.Tweening;

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
    AudioClip SuccessfulTrade;
    [SerializeField]
    AudioClip OverGenerous;
    [SerializeField]
    AudioClip RejectedOffer;
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
    [SerializeField]
    GameObject Portrait;
    [SerializeField]
    Sprite IconMissing;
    [SerializeField]
    public Tooltip tooltip;

    bool helpPanelOpened = false;
    [SerializeField]
    GameObject helpPanel;

    [SerializeField]
    TextMeshProUGUI valueText;

    public void Start() {
        shop = ShopManager.Instance.GetShopById(DataTracker.Current.currentShopId);
        copyOfPlayerInventory = new Inventory(DataTracker.Current.Player.Inventory);
        copyOfShopInventory = new Inventory(shop.inventory);
        buildShopList();
        buildPlayerList();
        Name.text = shop.name;
        Portrait.GetComponent<Image>().sprite = shop.Portrait ?? IconMissing;
        float value = DataTracker.Current.Player.Inventory.TotalValue();
        valueText.text = "Approx. Total Value: " + Round((int)value);

        tooltip = GameObject.Find("/Canvas/Panel/Tooltip").GetComponent<Tooltip>();
    }

    /// <summary>
    /// Round to the nearest multiple of a given integer
    /// </summary>
    /// <param name="val">Value to round</param>
    /// <param name="roundTo">Round to the nearest multiple of this integer</param>
    /// <returns></returns>
    int Round(int val, int roundTo=25){
        if (val % roundTo == 0) return val;
        int lower = val - val % roundTo;
        int higher = roundTo - val % roundTo + val;

        int diffLow = val - lower;
        int diffHigh = higher - val;
        return (diffHigh >= diffLow) ? lower : higher;
    }

    //======================================================================//
    //                           UI Functions                               //
    //======================================================================//

    /// <summary>
    /// Generates the list item for a single Item, pushing it into the given parent with the given button handler.
    /// Fetches item details from the ItemManager.
    /// </summary>
    /// <param name="itemName"></param>
    /// <param name="quantity"></param>
    /// <param name="parent"></param>
    /// <param name="onClick"></param>
    void buildListItem(string itemName, int quantity, Transform parent, bool isPlayerItem, UnityAction onClick)
    {
        var item = ItemManager.Current.itemsMaster[itemName];
        var listItem = GameObject.Instantiate(inventoryListItem, Vector3.zero, Quaternion.identity);
        listItem.GetComponentInChildren<TextMeshProUGUI>().text = item.DisplayName + " (" + quantity + ") ";
        Image i = listItem.transform.Find("Text").Find("Rarity").GetComponent<Image>();
        i.sprite = getValueString(item.tier);
        i.preserveAspect = true;
        listItem.transform.Find("Icon").GetComponent<Image>().sprite = item.Icon;
        listItem.transform.SetParent(parent, false);
        listItem.GetComponent<Button>().onClick.AddListener(onClick);
        listItem.name = item.DisplayName + "_button";
        listItem.GetComponent<HoverBehaviour>().tooltip = tooltip;

        // Calculate the total value modifier and add an arrow if the final price is modified
        GameObject PriceIcon = listItem.transform.Find("PriceIcon").gameObject;
        float mod = 1.0f;
        foreach (var tag in item.tags){
            if ( isPlayerItem && shop.playerSellModifiers.ContainsKey(tag)){
                mod *= shop.playerSellModifiers[tag];
            }
            else if ( !isPlayerItem && shop.shopSellModifiers.ContainsKey(tag)){
                mod *= shop.shopSellModifiers[tag];
            }
        }
        if (mod != 1.0f) {
            PriceIcon.SetActive(true);
            if (mod < 1.0f){
                PriceIcon.transform.rotation = Quaternion.Euler(0,0,90);
                if (isPlayerItem) {
                    PriceIcon.GetComponent<Image>().color = new Vector4(213,94,0,255)/255;
                }
                else {
                    PriceIcon.GetComponent<Image>().color = new Vector4(43,159,120,255)/255;
                }
            }
            else {
                PriceIcon.transform.rotation = Quaternion.Euler(0,0,-90);
                if (isPlayerItem) {
                    PriceIcon.GetComponent<Image>().color = new Vector4(43,159,120,255)/255;
                }
                else {
                    PriceIcon.GetComponent<Image>().color = new Vector4(213,94,0,255)/255;
                }
            }
        }

    }

    void buildShopList(){

        foreach(Transform child in shopInventoryObject.transform){
            Destroy(child.gameObject);
        }

        foreach(var item in copyOfShopInventory.Contents){
            buildListItem(item.Key, item.Value, shopInventoryObject, false, () => { addToCart(item.Key); });
        }

    }

    void buildPlayerList(){

        foreach(Transform child in playerInventoryObject.transform){
            Destroy(child.gameObject);
        }

        foreach(var item in copyOfPlayerInventory.Contents){
            buildListItem(item.Key, item.Value, playerInventoryObject, true, () => { addToOffer(item.Key); });
        }
    }


    void buildOfferList(){

        foreach(Transform child in offerListObject.transform){
            Destroy(child.gameObject);
        }

        foreach (var item in offer.Contents) {
            buildListItem(item.Key, item.Value, offerListObject, true, () => { removeFromOffer(item.Key); });
        }
    }

    void buildCartList(){
        foreach(Transform child in cartListObject.transform){
            Destroy(child.gameObject);
            Destroy(child.gameObject);
        }

        foreach(var item in cart.Contents){
            buildListItem(item.Key, item.Value, cartListObject, false, () => { removeFromCart(item.Key); });
        }
    }

    Sprite getValueString(Rarity tier)
    {
        if (tier == Rarity.Abundant)
        {
            return AbundantImage;
        }
        else if (tier == Rarity.Common)
        {
            return CommonImage;
        }
        else if (tier == Rarity.Uncommon)
        {
            return UncommonImage;
        }
        else if (tier == Rarity.Rare)
        {
            return RareImage;
        }
        else
        {
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
        if (tradeFairness != validateTradeAfterModifiers())
        {
            switch (validateTradeAfterModifiers())
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
            AudioSource audioSource = MusicManager.Instance.AudioSource;
            audioSource.PlayOneShot(ScaleChange, 1.0F);
            tradeFairness = validateTradeAfterModifiers();
        }

    }

    public void onTradeButtonClick(){
        if (cart.TotalValueAfterModifiers(shop.shopSellModifiers) == 0 && offer.TotalValueAfterModifiers(shop.playerSellModifiers) == 0)
        {
            offerFeedback.GetComponent<TMPro.TextMeshProUGUI>().text = "";
            return;
        }
        AudioSource audioSource = MusicManager.Instance.AudioSource;
        switch (validateTradeAfterModifiers())
        {
            case 0:
                break;
            case 1:
                offerFeedback.GetComponent<TMPro.TextMeshProUGUI>().text = "My now, what a generous offer!";
                makeTrade();
                audioSource.PlayOneShot(OverGenerous, 2.0F);
                break;
            case 2:
                offerFeedback.GetComponent<TMPro.TextMeshProUGUI>().text = "A fair offer, you got yourself a deal";
                makeTrade();
                audioSource.PlayOneShot(SuccessfulTrade, 2.0F);
                break;
            case 3:
                offerFeedback.GetComponent<TMPro.TextMeshProUGUI>().text = "This ain't a charity, make a real offer would ya";
                audioSource.PlayOneShot(RejectedOffer, 2.0F);
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
            float totalCartValue = cart.TotalValueAfterModifiers(shop.shopSellModifiers);
            float totalOfferValue = offer.TotalValueAfterModifiers(shop.playerSellModifiers);
            float difference = totalOfferValue - totalCartValue;

            Debug.Log($"Offer:{totalOfferValue} vs Cart:{totalCartValue}");
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

        foreach (var item in cart.Contents){

            shop.inventory.RemoveItem(item.Key, item.Value);

            copyOfPlayerInventory.AddItem(item.Key, item.Value);
            DataTracker.Current.Player.Inventory.AddItem(item.Key, item.Value);

            Events.TransactionEvents.Details transactionDetails = new Events.TransactionEvents.Details(item.Key, item.Value, DataTracker.Current.currentShopId, from, to);
            DataTracker.Current.EventManager.OnTransaction.Invoke(transactionDetails);
        }

        from = Events.TransactionEvents.Entity.PLAYER;
        to = Events.TransactionEvents.Entity.SYSTEM;

        foreach (var item in offer.Contents){

            DataTracker.Current.Player.Inventory.RemoveItem(item.Key, item.Value);
            
            shop.inventory.AddItem(item.Key, item.Value);
            copyOfShopInventory.AddItem(item.Key, item.Value);

            Events.TransactionEvents.Details transactionDetails = new Events.TransactionEvents.Details(item.Key, item.Value, DataTracker.Current.currentShopId, from, to);
            DataTracker.Current.EventManager.OnTransaction.Invoke(transactionDetails);
        }

        cart.Contents.Clear();
        offer.Contents.Clear();
        buildPlayerList();
        buildShopList();
        buildOfferList();
        buildCartList();
        float value = DataTracker.Current.Player.Inventory.TotalValue();
        valueText.text = "Approx. Total Value: " + Round((int)value);
    }

    public void leave(){
        //SceneManager.LoadScene("Town");
        SceneManager.UnloadSceneAsync("ShopScene"); 
    }

    public void ToggleHelpPanel(){
        if (helpPanelOpened) {
            helpPanel.GetComponent<RectTransform>().DOScale(0, 0.5f).OnComplete(()=>{helpPanel.SetActive(false);});
            helpPanelOpened = false;
        }
        else {
            helpPanel.SetActive(true);
            helpPanel.GetComponent<RectTransform>().DOScale(1.2f, 0.5f);
            helpPanelOpened = true;
        }
    }

}

