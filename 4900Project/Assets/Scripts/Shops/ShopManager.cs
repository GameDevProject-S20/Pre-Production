using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
     //THINGS TO ADD
    //-set up to pull from csv
    //-set up to interact with data tracker

    //ensure only one copy active
    private static ShopManager _current;
    public static ShopManager Current { get { return _current; } }
    Dictionary<int, Shop> shops = new Dictionary<int, Shop>();

    private void Awake()
    {
        if (_current != null && _current != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _current = this;
        }

        Shop testShop1 = new Shop(0, "General Store", "", "", Shop.ShopTypes.GeneralStore);
        Shop testShop2 = new Shop(1, "Pharmacy", "", "", Shop.ShopTypes.Pharmacy);
        shops.Add(0, testShop1);
        shops.Add(1, testShop2);

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
