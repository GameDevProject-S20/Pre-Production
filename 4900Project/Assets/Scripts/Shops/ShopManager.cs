using FileConstants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager
{
    //THINGS TO ADD
    //-set up to pull from csv
    //-set up to interact with data tracker

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

    public void LoadData()
    {
        GameData.LoadCsv<ShopData>(Files.Shops, out IEnumerable<ShopData> data);
        foreach (var shopData in data)
        {
            var shop = new Shop(shopData);
            if (shopData.Id == 3)
            {
                shop.inventory.AddItem("Generator", 1);
                shop.fromPlayerModifiers.Add(typetag.Medicine, 0.25f);
            }

            shops.Add(shopData.Id, shop);
        }

        /*Shop testShop1 = new Shop(0, "George's General Store", "General Store", "", Shop.ShopTypes.GeneralStore);
        Shop testShop2 = new Shop(1, "Phil's Pharmacy", "Medical Supplier", "", Shop.ShopTypes.Pharmacy);
        Shop testShop3 = new Shop(2, "Bill's Bulk Goods", "General Store", "", Shop.ShopTypes.GeneralStore);
        testShop3.inventory.AddItem("Generator", 1);
        Shop testShop4 = new Shop(3, "Hal's Hospital", "Medical Supplier", "", Shop.ShopTypes.Pharmacy);
        Shop testShop5 = new Shop(4, "Dan's Depot", "General Store", "", Shop.ShopTypes.GeneralStore);
        Shop testShop6 = new Shop(5, "Carl's Clinic", "Medical Supplier", "", Shop.ShopTypes.Pharmacy);
        Shop testShop7 = new Shop(6, "Sam's Supplies", "General Store", "", Shop.ShopTypes.GeneralStore);
        Shop testShop8 = new Shop(7, "Martha's Medicine", "Medical Supplier", "", Shop.ShopTypes.Pharmacy);
        Shop testShop9 = new Shop(8, "Mike's Marketplace", "General Store", "", Shop.ShopTypes.GeneralStore);
        Shop testShop10 = new Shop(9, "Harry's Healing", "Medical Supplier", "", Shop.ShopTypes.Pharmacy);
        testShop3.fromPlayerModifiers.Add(typetag.Medicine, 0.25f);
        shops.Add(0, testShop1);
        shops.Add(1, testShop2);
        shops.Add(2, testShop3);
        shops.Add(3, testShop4);
        shops.Add(4, testShop5);
        shops.Add(5, testShop6);
        shops.Add(6, testShop7);
        shops.Add(7, testShop8);
        shops.Add(8, testShop9);
        shops.Add(9, testShop10);*/


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
