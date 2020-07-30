﻿using FileConstants;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityUtility;

public enum Rarity {None, Abundant, Common, Uncommon, Rare, Unique}
public enum ItemTag {None,General,Fuel,Useable,Food,Luxury,Medical,Building_Materials,Tools_And_Parts,Combat,Scientific,Mineral,Antique,Advanced}

/// <summary>
/// Intermediate class for handling loading items from a CSV file.
/// This should be the class that gets generated, then each should be fed into the Item class.
/// </summary>
[Serializable]
public struct ItemCsvData
{
    public string DisplayName { get; set; }
    public string Tooltip { get; set; }
    public string Description { get; set; }
    public float Value { get; set; }
    public float Weight { get; set; }
    public string Tags { get; set; }
    public string IconName { get; set; }
}

public class Item 
{
    public string DisplayName;
    public string Tooltip;
    public string Description;
    public float Value;
    public float Weight;
    public Rarity tier;
    public List<ItemTag> tags;
    public Sprite Icon;

    protected static Rarity CalculateTier(float Value)
    {
        //create Rarity based on value
        if (Value <= 2)
        {
            return Rarity.Abundant;
        }
        else if (Value <= 10)
        {
            return Rarity.Common;
        }
        else if (Value <= 30)
        {
            return Rarity.Uncommon;
        }
        else if (Value <= 60)
        {
            return Rarity.Rare;
        }
        else if (Value > 60)
        {
            return Rarity.Unique;
        }
        return Rarity.None;
    }

    /// <summary>
    /// Creates an item given the values to set.
    /// This can be used for hardcoding items.
    /// </summary>
    /// <param name="name_"></param>
    /// <param name="tooltip_"></param>
    /// <param name="description_"></param>
    /// <param name="value_"></param>
    /// <param name="weight_"></param>
    /// <param name="_tags"></param>
    public Item(string name_, string tooltip_, string description_, float value_, float weight_, List<ItemTag> _tags)
    {
        DisplayName = name_;
        Tooltip = tooltip_;
        Description = description_;
        Value = value_;
        Weight = weight_;
        tags = _tags;
        tier = CalculateTier(Value);
    }

    /// <summary>
    /// Creates an Item out of an ItemCsvData class.
    /// This can be used for generating items from CSV.
    /// </summary>
    /// <param name="itemData"></param>
    public Item(ItemCsvData itemData)
    {
        DisplayName = itemData.DisplayName;
        Tooltip = itemData.Tooltip;
        Description = itemData.Description;
        Value = itemData.Value;
        Weight = itemData.Weight;
        tags = UnityHelperMethods.ParseCommaSeparatedList<ItemTag>(itemData.Tags, UnityHelperMethods.ParseEnum<ItemTag>);
        tier = CalculateTier(Value);
        Icon = Resources.Load<Sprite>($"Icons/ItemIcons/{itemData.IconName}".Replace(".png", ""));
    }

}

public class ItemManager : MonoBehaviour
{
    private static ItemManager _current;
    public static ItemManager Current { get { return _current; } }
    public  Dictionary<string, Item> itemsMaster = new Dictionary<string, Item>();
    public  Dictionary<ItemTag, List<Item>> itemsByCategory = new Dictionary<ItemTag, List<Item>>();


    void Awake()
    {
        if (_current != null && _current != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _current = this;
        }

        //create item list
        GameData.LoadCsv(Files.Items, out IEnumerable<ItemCsvData> itemsData);
        foreach (var itemData in itemsData)
        {
            var item = new Item(itemData);
            itemsMaster.Add(item.DisplayName, item);
            foreach (var tag in item.tags){
                List<Item> category;
                if (itemsByCategory.TryGetValue(tag, out category)){
                    category.Add(item);
                }
                else{
                    itemsByCategory.Add(tag, new List<Item>(){item});
                }
            }

        }
    }

    public Dictionary<string, Item> GetItemsByType(List<ItemTag> types)
    {
        Dictionary<string, Item> values = new Dictionary<string, Item>();
        foreach (ItemTag targettype in types)
        {
            foreach(KeyValuePair<string,Item> viewed in itemsMaster)
            {
                if(viewed.Value.tags.Contains(targettype) && values.ContainsKey(viewed.Key) == false)
                {
                    values.Add(viewed.Key,viewed.Value);
                }
            }
        }
        return values;
    }

    public List<Item> GetAllItemsOfType(ItemTag type){
        List<Item> l;
        if (itemsByCategory.TryGetValue(type, out l)){
            return l;
        }
        return null;
    }



}
