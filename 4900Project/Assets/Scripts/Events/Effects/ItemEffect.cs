using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class ItemEffect : IEffect
{
    protected readonly string itemName;
    protected readonly int amount;

    private ItemEffect() { }

    public ItemEffect(string itemName, int amount)
    {
        if (!string.IsNullOrEmpty(itemName))
        {
            itemName = itemName.First().ToString().ToUpper() + itemName.Substring(1).ToLower();
        }
        this.itemName = itemName;
        this.amount = amount;
    }

    public abstract bool Apply();
}

public class GiveItem : ItemEffect
{
    public GiveItem(string itemName, int amount) : base(itemName, amount) { }

    public override bool Apply()
    {
        Player.Instance.Inventory.AddItem(itemName, amount);
        return true;
    }

    public override string ToString()
    {
        return string.Format("Give {0}x {1}", amount, itemName);
    }
}

public class TakeItem : ItemEffect
{
    public TakeItem(string itemName, int amount) : base(itemName, amount) { }

    public override bool Apply()
    {
        int numRemoved = Player.Instance.Inventory.RemoveItem(itemName, amount);
        bool success = (numRemoved == amount);
        if (!success)
        {
            Player.Instance.Inventory.AddItem(itemName, numRemoved);
        }

        return success;
    }

    public override string ToString()
    {
        return string.Format("Take {0}x {1}", amount, itemName);
    }
}

