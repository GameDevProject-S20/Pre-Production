using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    static Dictionary<string, Item> itemsMaster = new Dictionary<string, Item>();
    Dictionary<string, int> contents = new Dictionary<string, int>();
    public float weightLimit {get; set;}
    float weightOverflowModifier = 1.0f; // for now this does nothing. might be used later if we allow the player to overfill their inventory at a cost

    public Inventory(Inventory sourceInventory){
        contents = sourceInventory.contents;
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
        int capacity =  canFitHowMany(name, amount);
        if (capacity > 0) {
            int currentCount = 0;
            if (contents.TryGetValue(name, out currentCount)){
                contents[name] = currentCount + capacity;
            }
            else {
                contents.Add(name, capacity);
            } 
        }
        return capacity;
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

    public float totalValue(){
        float totalValue = 0;
        foreach (KeyValuePair<string, int> item in contents)
        {
            totalValue += item.Value * itemsMaster[item.Key].value;
        }
        return totalValue;
    }

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

    public int canFitHowMany(string name, int count){
        return Mathf.FloorToInt((weightLimit * weightOverflowModifier - totalWeight()) / itemsMaster[name].weight);
    }

    public bool canFitItems(float weight){
        return weight + totalWeight() <= weightLimit * weightOverflowModifier;
    }
}

public struct Item{
    public string name;
    public string displayName;
    public string tooltip;
    public string description;
    public float value;
    public float weight;
}