using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
            itemName = itemName.First().ToString().ToUpper() + itemName.Substring(1).ToLower();
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
