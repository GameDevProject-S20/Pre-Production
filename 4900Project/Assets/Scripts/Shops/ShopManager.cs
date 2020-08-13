using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager
{
    //THINGS TO ADD
    //-set up to pull from csv
    //-set up to interact with data tracker

    static int id = 0;

    private static ShopManager instance;

    public static ShopManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ShopManager();
            }
            return instance;
        }
    }

    Dictionary<int, Shop> shops = new Dictionary<int, Shop>();

    public Shop CreateYorkShop()
    {
        Shop yorkShop = new Shop(id++, "Marketplace", "Trade Goods", "", Shop.ShopTypes.None);
        yorkShop.inventory.AddItem("Fuel", 30);
        yorkShop.inventory.AddItem("Scrap Metal", 12);
        yorkShop.inventory.AddItem("Hunting Rifle", 1);
        yorkShop.inventory.AddItem("Herbal Medicine", 2);
        yorkShop.inventory.AddItem("Rations", 14);
        yorkShop.inventory.AddItem("Generator", 1);
        yorkShop.inventory.AddItem("Motor", 3);

        return yorkShop;
    }

    public void addShop(Shop s){
        shops.Add(s.id, s);
    }

    public int GetId(){
        return id++;
    }

    //town retrieval
    public Shop GetShopById(int id)
    {
        Shop shop;
        if (shops.TryGetValue(id, out shop))
        {
            return shop;
        }
        else
        {
            return null;
        }
    }
}
