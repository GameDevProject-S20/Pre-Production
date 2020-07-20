using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemEffect : IEffect
{
    protected readonly string itemName;
    protected readonly int amount;

    private ItemEffect() { }

    public ItemEffect(string itemName, int amount)
    {
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
}

