using System.Collections;
using System.Collections.Generic;

public class HasItemPresentConditon : IPresentCondition
{
    private readonly string itemName;
    private readonly int itemAmount;

    public HasItemPresentConditon(string itemName, int itemAmount)
    {
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
