using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extentions;

/// <summary>
/// Satisfied if the player has a specified item on them
/// </summary>
public class HasItemPresentConditon : IPresentCondition
{
    private readonly string itemName;
    private readonly int itemAmount;

    public HasItemPresentConditon(string itemName, int itemAmount)
    {
        if (!string.IsNullOrEmpty(itemName))
        {
            itemName = itemName.ToTitleCase();
        }
        this.itemName = itemName;
        this.itemAmount = itemAmount;
    }

    public bool IsSatisfied()
    {
        if (Player.Instance.Inventory.Contents.TryGetValue(itemName, out int playerAmount))
        { 
            if (playerAmount >= itemAmount)
            {
                return true;
            }
        }
        return false;
    }
}

public class HasItemTagPresentConditon : IPresentCondition
{
    private readonly string tag;
    public HasItemTagPresentConditon(string tag)
    {
        if (!string.IsNullOrEmpty(tag))
        {
            tag = tag.ToTitleCase();
        }
        this.tag = tag;
    }
    public bool IsSatisfied()
    {
        var inventory = Player.Instance.Inventory;
        var validItemNames = ItemManager.Current.GetAllItemsOfType((ItemTag) Enum.Parse(typeof(ItemTag), tag)).Select(i => i.DisplayName);
        return validItemNames.Any(i => inventory.Contains(i) > 0);
    }
}
