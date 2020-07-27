using System.Collections.Generic;
using System.Linq;
using System;
using SIEvents;

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
        EventManager.Instance.OnGivenToPlayer.Invoke(itemName, amount);
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
        if (success)
        {
            EventManager.Instance.OnTakenFromPlayer.Invoke(itemName, amount);
        }
        else
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

public abstract class TaggedItemEffect : IEffect
{
    protected static System.Random rand = new System.Random();
    protected readonly ItemTag itemTag;
    protected readonly int amount;

    private TaggedItemEffect() { }

    public TaggedItemEffect(string itemTag, int amount)
    {
        this.itemTag = (ItemTag) Enum.Parse(typeof(ItemTag), itemTag, true);
        this.amount = amount;
    }

    protected IEnumerable<string> GetOfType()
    {
        return ItemManager.Current.GetItemsByType(new List<ItemTag> { itemTag }).Keys;
    }

    public abstract bool Apply();
}

public class GiveItemWithTag : TaggedItemEffect
{
    public GiveItemWithTag(string itemTag, int amount) : base(itemTag, amount) { }

    public override bool Apply()
    {
        List<string> itemNames = GetOfType().ToList();
        string itemName = itemNames.ElementAt(rand.Next(itemNames.Count));
        Player.Instance.Inventory.AddItem(itemName, amount);
        EventManager.Instance.OnGivenToPlayer.Invoke(itemName, amount);
        return true;
    }

    public override string ToString()
    {
        return string.Format("Give {0}x item with tag '{1}'", amount, itemTag);
    }
}

public class TakeItemWithTag : TaggedItemEffect
{
    public TakeItemWithTag(string itemTag, int amount) : base(itemTag, amount) { }

    public override bool Apply()
    {
        List<string> itemNames = GetOfType().ToList();
        List<string> inInventory = Player.Instance.Inventory.Contents.Keys.Where(i => itemNames.Contains(i)).ToList();//AddItem(itemName, amount);
        string itemName = inInventory.ElementAt(rand.Next(inInventory.Count));


        int numRemoved = Player.Instance.Inventory.RemoveItem(itemName, amount);
        bool success = (numRemoved == amount);
        if (success)
        {
            EventManager.Instance.OnTakenFromPlayer.Invoke(itemName, amount);
        }
        else
        {
            Player.Instance.Inventory.AddItem(itemName, numRemoved);
        }

        return success;
    }

    public override string ToString()
    {
        return string.Format("Take {0}x item with tag '{1}'", amount, itemTag);
    }
}

