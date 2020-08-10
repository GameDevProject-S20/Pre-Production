using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SIEvents;

public class Inventory
{
    public Dictionary<string, int> Contents { get; private set; } = new Dictionary<string, int>();
    
    public float WeightLimit {get; set;}

    float weightOverflowModifier = 1.0f; // for now this does nothing. might be used later if we allow the player to overfill their inventory at a cost

    public Inventory(){
        WeightLimit = 750f;
    }

    // Copy Constructor
    public Inventory(Inventory sourceInventory){
        Contents = new Dictionary<string, int>(sourceInventory.Contents);
        WeightLimit = sourceInventory.WeightLimit;
        weightOverflowModifier = sourceInventory.weightOverflowModifier;
    }

    /// <summary>
    /// Adds an item to the inventory.
    /// Add up to a given amount of items, until the weight limit is reached.
    /// </summary>
    /// <param name="name">Name of the item to add</param>
    /// <param name="amount">Amount to add</param>
    /// <returns>Amount of items added.</returns>
    public int AddItem(string name, int amount){
        int capacity =  CanFitHowMany(name);
        if (capacity > 0) {
            int currentCount = 0;
            if (Contents.TryGetValue(name, out currentCount)){
                Contents[name] = currentCount + Mathf.Min(capacity, amount);
            }
            else {
                Contents.Add(name, Mathf.Min(capacity, amount));
            } 

            EventManager.Instance.OnInventoryChange.Invoke();
        }
        return Mathf.Min(capacity, amount);
    }


    /// <summary>
    /// Returns the weight fill percentage
    /// </summary>
    public float GetWeightRatio() {
        return TotalWeight()/WeightLimit;
    }

    /// <summary>
    /// Removes an item from the inventory
    /// </summary>
    /// <param name="name">Name of the item to remove</param>
    /// <param name="amount">Amount to remove</param>
    /// <returns>Amount of items removed.</returns>
    public int RemoveItem(string name, int amount){
        int currentCount;
        if (Contents.TryGetValue(name, out currentCount)){

            Contents[name] = currentCount - amount;

            if (amount >= currentCount){
                Contents.Remove(name);
                amount = currentCount;
            }

            EventManager.Instance.OnInventoryChange.Invoke();
            return amount;
        }
        return 0;
    }

    /// <summary>
    /// Gets the amount of an item in the inventory
    /// </summary>
    /// <param name="name">Name of item</param>
    /// <returns>Amount of item in inventory</returns>
    public int Contains(string name){
        int amount = 0;
        Contents.TryGetValue(name, out amount);
        return amount;
    }

    /// <summary>
    /// Calculate the total value of all items in the inventory
    /// </summary>
    /// <returns>Total value</returns>
    public float TotalValue(){
        float totalValue = 0;
        foreach (KeyValuePair<string, int> item in Contents)
        {
            totalValue += item.Value * ItemManager.Current.itemsMaster[item.Key].Value;
        }
        return totalValue;
    }

    public float TotalValueAfterModifiers(Dictionary<ItemTag,float> shopVals)
    {
        float totalValue = 0;
        foreach (KeyValuePair<string, int> item in Contents)
        {
            float valueModifier = 1;
            foreach (ItemTag itemTag in ItemManager.Current.itemsMaster[item.Key].tags)
            {
                foreach (KeyValuePair<ItemTag, float> shopMod in shopVals)
                {
                    if (itemTag == shopMod.Key)
                    {
                        valueModifier += shopMod.Value;
                    }
                }
            }
            totalValue += item.Value * ItemManager.Current.itemsMaster[item.Key].Value * valueModifier;
        }
        return totalValue;
    }

    /// <summary>
    /// Calculate the total value of all items in the inventory, after some modifiers have been applied
    /// </summary>
    /// <param name="modifiers">Dictionary of float multipliers.</param>
    /// <returns>Total value after modifiers</returns>
    public float TotalValue(Dictionary<string, float> modifiers){
        float totalValue = 0;
        foreach (KeyValuePair<string, int> item in Contents)
        {
            float mod;
            modifiers.TryGetValue(item.Key, out mod);
            if (mod == 0){
                mod = 1;
            }
            totalValue += item.Value * ItemManager.Current.itemsMaster[item.Key].Value * mod;
        }
        return totalValue;
    }

    /// <summary>
    /// Calculate the total weight of all items in the inventory
    /// </summary>
    /// <returns>Total weight</returns>
    public float TotalWeight(){
        float totalWeight = 0;
        foreach (KeyValuePair<string, int> item in Contents)
        {
            totalWeight += item.Value * ItemManager.Current.itemsMaster[item.Key].Weight;
        }
        return totalWeight;
    }

    /// <summary>
    /// Calculate how many of a given item can fit within the inventory, based on remaining weight.
    /// </summary>
    /// <param name="name">Name of item</param>
    /// <returns>Amount of given item that can fit</returns>
    public int CanFitHowMany(string name){
        ItemManager.Current.itemsMaster.TryGetValue(name, out Item item);
        if (item == null)
        {
            throw new ArgumentException(string.Format("Item {0} not found.\n\nAvailable:\n{1}", name, string.Join("\n", ItemManager.Current.itemsMaster.Keys)));
        }

        return Mathf.FloorToInt((WeightLimit * weightOverflowModifier - TotalWeight()) / item.Weight);
    }

    /// <summary>
    /// Given a certain weight, determine if there is enough room in the inventory to add that weight
    /// </summary>
    /// <param name="weight"></param>
    /// <returns>True if there is enough room in the inventory</returns>
    public bool CanFitItems(float weight){
        return weight + TotalWeight() <= WeightLimit * weightOverflowModifier;
    }

    /// <summary>
    /// Creates a string in which each line corresponds to an item in the inventory.
    /// </summary>
    /// <returns></returns>
    public override string ToString(){
        string output = "";
        foreach (var item in Contents){
            output += ItemManager.Current.itemsMaster[item.Key].DisplayName + " (" + item.Value +")\n";
        }
        return output;
    }
}