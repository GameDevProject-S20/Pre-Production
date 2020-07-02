using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    // Move this to Item Manager
    public static Dictionary<string, Item> itemsMaster = new Dictionary<string, Item>();
    Dictionary<string, int> contents = new Dictionary<string, int>();
    public float weightLimit {get; set;}
    float weightOverflowModifier = 1.0f; // for now this does nothing. might be used later if we allow the player to overfill their inventory at a cost

    public Inventory(){
        weightLimit = 100000;
        if (itemsMaster.Count == 0){
            Item item1 = new Item("item1", "Item 1", "", "", 1, 1);
            Item item2 = new Item("item2", "Item 2", "", "", 2, 2);
            Item item3 = new Item("item3", "Item 3", "", "", 3, 3);
            Item item4 = new Item("item4", "Item 4", "", "", 5, 5);
            Item item5 = new Item("item5", "Item 5", "", "", 8, 8);
            Item item6 = new Item("item6", "Item 6", "", "", 13, 13);
            Item item7 = new Item("item7", "Item 7", "", "", 21, 21);
            Item item8 = new Item("item8", "Item 8", "", "", 34, 34);
            Item medicine = new Item("Medicine", "Medicine", "", "", 9, 9);

            itemsMaster.Add(item1.name, item1);
            itemsMaster.Add(item2.name, item2);
            itemsMaster.Add(item3.name, item3);
            itemsMaster.Add(item4.name, item4);
            itemsMaster.Add(item5.name, item5);
            itemsMaster.Add(item6.name, item6);
            itemsMaster.Add(item7.name, item7);
            itemsMaster.Add(item8.name, item8);
            itemsMaster.Add(medicine.name, medicine);
        }
    }

    // Copy Constructor
    public Inventory(Inventory sourceInventory){
        contents = new Dictionary<string, int>(sourceInventory.contents);
        weightLimit = sourceInventory.weightLimit;
        weightOverflowModifier = sourceInventory.weightOverflowModifier;
    }

    /// <summary>
    /// Adds an item to the inventory.
    /// Add up to a given amount of items, until the weight limit is reached.
    /// </summary>
    /// <param name="name">Name of the item to add</param>
    /// <param name="amount">Amount to add</param>
    /// <returns>Amount of items added.</returns>
    public int addItem(string name, int amount){
        int capacity =  canFitHowMany(name);
        if (capacity > 0) {
            int currentCount = 0;
            if (contents.TryGetValue(name, out currentCount)){
                contents[name] = currentCount + Mathf.Min(capacity, amount);
            }
            else {
                contents.Add(name, Mathf.Min(capacity, amount));
            } 
        }
        return Mathf.Min(capacity, amount);
    }
    

    /// <summary>
    /// Removes an item from the inventory
    /// </summary>
    /// <param name="name">Name of the item to remove</param>
    /// <param name="amount">Amount to remove</param>
    /// <returns>Amount of items removed.</returns>
    public int removeItem(string name, int amount){
        int currentCount;
        if (contents.TryGetValue(name, out currentCount)){
            contents[name] = currentCount - amount;
            if (amount >= currentCount){
                contents.Remove(name);
                return currentCount;
            }
            return amount;
        }
        return 0;
    }

    /// <summary>
    /// Gets the amount of an item in the inventory
    /// </summary>
    /// <param name="name">Name of item</param>
    /// <returns>Amount of item in inventory</returns>
    public int contains(string name){
        int amount = 0;
        contents.TryGetValue(name, out amount);
        return amount;
    }

    /// <summary>
    /// Calculate the total value of all items in the inventory
    /// </summary>
    /// <returns>Total value</returns>
    public float totalValue(){
        float totalValue = 0;
        foreach (KeyValuePair<string, int> item in contents)
        {
            totalValue += item.Value * itemsMaster[item.Key].value;
        }
        return totalValue;
    }

    /// <summary>
    /// Calculate the total value of all items in the inventory, after some modifiers have been applied
    /// </summary>
    /// <param name="modifiers">Dictionary of float multipliers.</param>
    /// <returns>Total value after modifiers</returns>
    public float totalValue(Dictionary<string, float> modifiers){
        float totalValue = 0;
        foreach (KeyValuePair<string, int> item in contents)
        {
            float mod;
            modifiers.TryGetValue(item.Key, out mod);
            if (mod == 0){
                mod = 1;
            }
            totalValue += item.Value * itemsMaster[item.Key].value * mod;
        }
        return totalValue;
    }

    /// <summary>
    /// Calculate the total weight of all items in the inventory
    /// </summary>
    /// <returns>Total weight</returns>
    public float totalWeight(){
        float totalWeight = 0;
        foreach (KeyValuePair<string, int> item in contents)
        {
            totalWeight += item.Value * itemsMaster[item.Key].weight;
        }
        return totalWeight;
    }

    public Dictionary<string, int> getContents(){
        return contents;
    }

    /// <summary>
    /// Calculate how many of a given item can fit within the inventory, based on remaining weight.
    /// </summary>
    /// <param name="name">Name of item</param>
    /// <returns>Amount of given item that can fit</returns>
    public int canFitHowMany(string name){
        return Mathf.FloorToInt((weightLimit * weightOverflowModifier - totalWeight()) / itemsMaster[name].weight);
    }

    /// <summary>
    /// Given a certain weight, determine if there is enough room in the inventory to add that weight
    /// </summary>
    /// <param name="weight"></param>
    /// <returns>True if there is enough room in the inventory</returns>
    public bool canFitItems(float weight){
        return weight + totalWeight() <= weightLimit * weightOverflowModifier;
    }

    /// <summary>
    /// Creates a string in which each line corresponds to an item in the inventory.
    /// </summary>
    /// <returns></returns>
    public override string ToString(){
        string output = "";
        foreach (var item in contents){
            output += itemsMaster[item.Key].displayName + " (" + item.Value +")\n";
        }
        return output;
    }
}

public struct Item{
    public string name;
    public string displayName;
    public string tooltip;
    public string description;
    public float value;
    public float weight;

    public Item(string name_, string displayName_, string tooltip_, string description_, float value_, float weight_){
        name = name_;
        displayName = displayName_;
        tooltip = tooltip_;
        description = description_;
        value = value_;
        weight = weight_;
    }
}