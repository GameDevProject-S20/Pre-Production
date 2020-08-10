using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop
{
    public enum ShopTypes {GeneralStore, Mechanic, Pharmacy, None}

    public int id { get; set; }
    public string name;
    public string shortDescription;
    string description;
    public ShopTypes type;
    public Sprite Portrait;

    int townID;

    public Dictionary<ItemTag, float> shopSellModifiers = new Dictionary<ItemTag, float>();
    public Dictionary<ItemTag, float> playerSellModifiers = new Dictionary<ItemTag, float>();

    public float basePriceModifier = 1.0f;

    public float acceptedPriceDifference = 0.2f;





    public Inventory inventory = new Inventory();

    public Shop(int id_, string name_, string shortDescription_, string description_, ShopTypes type_)
    {
        id = id_;
        name = name_;
        shortDescription = shortDescription_;
        description = description_;
        type = type_;
        inventory.WeightLimit = 10000;
        //InitializeInventory();
        // Randomly Select an Icon
        // Do not select the ugly ones
        int iconId = -1;
        bool valid = false;
        while (!valid)
        {
            iconId = Mathf.FloorToInt(Random.Range(0, 31));
            if (iconId != 10
            && iconId != 12
            && iconId != 15
            && iconId != 18
            && iconId != 20)
                valid = true;
        }
        string path = "Icons/CyberPunk Avatars/" + iconId.ToString("D3");
        Portrait = Resources.Load<Sprite>(path);
    }

    public void Restock(Town t){
        inventory.Contents.Clear();
        InitializeInventory(t);
    }

    /// <summary>
    /// Multiplicatively add modifiers
    /// </summary>
    public void AddModifers(Dictionary<ItemTag, float> baseValues, Dictionary<ItemTag, float> mod){
        foreach(var tag in mod){
            float val;
            if (baseValues.TryGetValue(tag.Key, out val)){
                val *= tag.Value;
            }
            else {
                baseValues.Add(tag.Key, tag.Value);
            }
        }
    }

    public void InitializeInventory(Town town){

        // Base chance of seeing rare items
        float RareItemChance = 0.50f;

        // Base min & max quantity of items per tier
        Dictionary<Rarity,(int,int)> ItemRanges = new Dictionary<Rarity, (int, int)>(){
            {Rarity.Abundant, (15, 30)},
            {Rarity.Common, (8, 18)},
            {Rarity.Uncommon, (6, 12)},
            {Rarity.Rare, (1,4)},
            {Rarity.Unique, (1,1)}
        };

        Dictionary<Rarity,(int,int)> GeneralItemRanges = new Dictionary<Rarity, (int, int)>(){
            {Rarity.Abundant, (10, 20)},
            {Rarity.Common, (5, 9)},
            {Rarity.Uncommon, (2, 4)},
            {Rarity.Rare, (1,1)},
            {Rarity.Unique, (0,0)}
        };

        // Modifiers to above quantities per item type
        Dictionary<ItemTag, float> QuantityModifiers  = new Dictionary<ItemTag, float>();

        // Step 1: Apply modifiers
        foreach (TownTag tag in town.Tags){

            // Apply price modifiers
            AddModifers(shopSellModifiers, tag.shopSellModifiers);
            AddModifers(playerSellModifiers, tag.playerSellModifiers);

            RareItemChance *= tag.RarityModifier;

            // Modify the per type amounts
            foreach (var type in tag.AbundancyModifiers){
                float mod;
                if (QuantityModifiers.TryGetValue(type.Key, out mod)){
                    mod *=  type.Value;
                }
                else {
                    QuantityModifiers.Add(type.Key, type.Value);
                }
            }
        }

        // Step 2: Populate inventory with specialized goods
        foreach (var tag in town.Tags){
            if (tag.Specialization == ItemTag.None) continue;
            List<Item> itemsOfType = ItemManager.Current.GetAllItemsOfType(tag.Specialization);
            foreach (Item item in itemsOfType) {
                if (item.tags.Contains(ItemTag.Unique)) continue;
                float mod;
                if (!QuantityModifiers.TryGetValue(tag.Specialization, out mod)) mod = 1;
                    

                int amount = Mathf.RoundToInt(Mathf.Round(Random.Range(ItemRanges[item.tier].Item1, ItemRanges[item.tier].Item2)) * mod);
                inventory.AddItem(item.DisplayName, amount);

            }
        }

        // Step 2: Populate inventory with general goods
        // Small Towns can have abundant and common items
        // Medium & Large Towns can have any general item
        List<Item> generalItems = ItemManager.Current.GetAllItemsOfType(ItemTag.General);
        float SmallTownMod = 0.4f;
        float MediumTownMod = 1.0f;
        float LargeTownMod = 1.0f;

        List<ItemTag> largeTownItems = new List<ItemTag>(){
            ItemTag.Combat,
            ItemTag.Machinery,
            ItemTag.Tools_And_Parts,
            ItemTag.Medical,
            ItemTag.Building_Materials,
            ItemTag.Luxury
        };

        if (town.Size == Town.Sizes.Small){
            foreach(Item item in generalItems) {
                if (item.tier != Rarity.Abundant && item.tier != Rarity.Common) continue;
                int amount = Mathf.RoundToInt(Random.Range(GeneralItemRanges[item.tier].Item1, GeneralItemRanges[item.tier].Item2) * SmallTownMod);
                inventory.AddItem(item.DisplayName, amount);
            }
        }
        else if (town.Size == Town.Sizes.Medium){
            foreach(Item item in generalItems) {
                int amount = Mathf.RoundToInt(Random.Range(GeneralItemRanges[item.tier].Item1, GeneralItemRanges[item.tier].Item2) * MediumTownMod);
                inventory.AddItem(item.DisplayName, amount);
            }
        }

        else if (town.Size == Town.Sizes.Large){
            foreach(Item item in generalItems) {
                int amount = Mathf.RoundToInt(Random.Range(GeneralItemRanges[item.tier].Item1, GeneralItemRanges[item.tier].Item2) * LargeTownMod);
                inventory.AddItem(item.DisplayName, amount);
            }

            // Large towns can have items from a variety of tags
            // Combat, Building Materials, Tools & Parts, Medical, Luxury
            Dictionary<string, Item> variedGoods = ItemManager.Current.GetItemsByType(largeTownItems);
            Dictionary<Rarity, float> probabilities = new Dictionary<Rarity, float>(){
                {Rarity.Abundant, 1.0f},
                {Rarity.Common, 0.7f},
                {Rarity.Uncommon, 0.6f},
                {Rarity.Rare, RareItemChance / 2.0f},
                {Rarity.Unique, 0.0f}
            };
            foreach(Item item in variedGoods.Values){
                if (item.tags.Contains(ItemTag.Unique) || Random.value > probabilities[item.tier] || item.tags.Contains(ItemTag.Food) ||inventory.Contains(item.DisplayName) > 0) continue;
                int amount = Mathf.CeilToInt(Random.Range(GeneralItemRanges[item.tier].Item1, GeneralItemRanges[item.tier].Item2));
                inventory.AddItem(item.DisplayName, amount);
            }

        }

    }
}
