using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trading : MonoBehaviour
{
    Shop shop;
    Player player;

    Inventory offer;
    Inventory cart;
    Inventory copyOfPlayerInventory;
    Inventory copyOfShopInventory;


    private void Start() {
        copyOfPlayerInventory = new Inventory(player.inventory);
        copyOfPlayerInventory.weightLimit = 100000;
        copyOfShopInventory = new Inventory(shop.inventory);
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
            else if (difference <= totalCartValue * shop.acceptedPriceDifference){
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
            player.inventory.addItem(item.Key, item.Value);
            copyOfPlayerInventory.addItem(item.Key, item.Value);
        }
        foreach (var item in offer.getContents()){
            shop.inventory.addItem(item.Key, item.Value);
            copyOfShopInventory.addItem(item.Key, item.Value);

        }

        cart.getContents().Clear();
        offer.getContents().Clear();
    }
}

public class Player {
    public Inventory inventory;
}