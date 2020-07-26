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
        inventory.weightLimit = 10000;
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

        // The BASELINE probablity of seeing items of a certain rarity
        List<float> RarityProbability = new List<float>(){ 1.0f, 0.75f, 0.4f, 0.1f, 0.5f};

        // The BASELINE amount of items of a certain rarity
        List<int> RarityAmount = new List<int>(){ 15, 6, 3, 1, 1};

        // The probablity of seeing items of a certain rarity PER ITEM TYPE
        Dictionary<ItemTag, List<float>> categoryProb = new Dictionary<ItemTag, List<float>>();
        // The amount of seeing items of a certain rarity PER ITEM TYPE
        Dictionary<ItemTag, List<float>> categoryQuant  = new Dictionary<ItemTag, List<float>>();

        // Step 1: Apply modifiers
        foreach (TownTag tag in town.Tags){

            // Apply price modifiers
            AddModifers(shopSellModifiers, tag.shopSellModifiers);
            AddModifers(playerSellModifiers, tag.playerSellModifiers);

            // Modify the BASELINE probablity and amounts
            for (int i = 0; i < 5; i++)
            {
                RarityProbability[i] *= tag.BaseRarityModifier[i];
                RarityAmount[i] =  Mathf.FloorToInt(RarityAmount[i] * tag.BaseAbundancyModifier[i]);
            }

            // Modify the PER ITEM TYPE probablity 
            foreach (var type in tag.RarityModifers){
                List<float> l;
                if (categoryProb.TryGetValue(type.Key, out l)){
                    for (int i = 0; i < 5; i++)
                    {
                        l[i] *= type.Value[i];
                    }
                }
                else {
                    categoryProb.Add(type.Key, type.Value);
                }
            }

            // Modify the PER ITEM TYPE amounts
            foreach (var type in tag.AbundancyModifiers){
                List<float> l;
                if (categoryQuant.TryGetValue(type.Key, out l)){
                    for (int i = 0; i < 5; i++)
                    {
                        l[i] *= type.Value[i];
                    }
                }
                else {
                    categoryQuant.Add(type.Key, type.Value);
                }
            }
        }

        // Step 2: Populate inventory
        // A small town will only stock goods they specialize in
        // A medium town will stock a small variety of goods
        // A large town will stock a large amount of goods
        foreach (var tag in town.Tags){
            List<Item> items = ItemManager.Current.GetAllItemsOfType(tag.Specialization);
            List<float> probMod;
            if (!categoryProb.TryGetValue(tag.Specialization, out probMod)){
                probMod = new List<float>(){1.0f, 1.0f, 1.0f, 1.0f, 1.0f};
            }
            List<float> amountMod;
            if (!categoryQuant.TryGetValue(tag.Specialization, out amountMod)){
                amountMod = new List<float>(){1.0f, 1.0f, 1.0f, 1.0f, 1.0f}; 
            }

            foreach(var item in items){
                int tier;
                if (item.tier == Rarity.Abundant){
                    tier = 0;
                }
                else if (item.tier == Rarity.Common){
                    tier = 1;
                }
                else if (item.tier == Rarity.Uncommon){
                    tier = 2;
                }
                else if (item.tier == Rarity.Rare){
                    tier = 3;
                }
                else{
                    tier = 4;
                }

                if (Random.value < RarityProbability[tier] * probMod[tier]){
                    inventory.AddItem(item.DisplayName, Mathf.FloorToInt(RarityAmount[tier] * amountMod[tier]));
                }
            }
        }

        // Fill medium and large towns with some general items
        if (town.Size != Town.Sizes.Small){
            List<Item> items = ItemManager.Current.GetAllItemsOfType(ItemTag.General);
            foreach(var item in items){
                int tier;
                if (item.tier == Rarity.Abundant){
                    tier = 0;
                }
                else if (item.tier == Rarity.Common){
                    tier = 1;
                }
                else if (item.tier == Rarity.Uncommon){
                    tier = 2;
                }
                else if (item.tier == Rarity.Rare){
                    tier = 3;
                }
                else{
                    tier = 4;
                }
                if (Random.value < RarityProbability[tier]){
                    inventory.AddItem(item.DisplayName, Mathf.FloorToInt(RarityAmount[tier]));
                }
            }
        }

    }
}
