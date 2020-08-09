using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityUtility;

/// <summary>
/// The AddShopEffect is used to add a shop into a town.
/// </summary>
public class AddShopEffect : IEffect
{
    int townId;
    string shopName;
    string shopDescription;
    Shop.ShopTypes shopType;

    public AddShopEffect(int townId, string shopName, string shopDescription, string shopTypeStr)
    {
        this.townId = townId;
        this.shopName = shopName;
        this.shopDescription = shopDescription;
        this.shopType = UnityHelperMethods.ParseEnum<Shop.ShopTypes>(shopTypeStr);
    }

    public bool Apply()
    {
        // Identify the town that the shop should be added to
        var town = DataTracker.Current.TownManager.GetTownById(townId);

        // Add the shop
        town.InitializeShop(shopName, shopDescription, shopType);

        return true;
    }
}