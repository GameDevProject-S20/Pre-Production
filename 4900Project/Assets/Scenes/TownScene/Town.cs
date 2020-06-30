using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Town class 
public class Town
{
    public int Id { get; }
    public string Name { get; set; }
    public string Leader { get; set; }
    public List<int> shops;
    public int Reputation { get; set; }

    public Town(int a, string b, string c)
    {
        Id = a;
        Name = b;
        Leader = c;
        shops = new List<int>();
        Reputation = 50;
    }

    public void AddShop(int i)
    {
        shops.Add(i);
    }

    public void RemoveShopById(int i)
    {
        for(int j = 0; j < shops.Count; j++)
        {
            if(shops[j] == i)
            {
                shops.Remove(j);
                break;
            }
        }
    }

    public void RemoveShop(int i)
    {
        shops.Remove(i);
    }

    public void AlterRep(int i)
    {
        Reputation += i;
        Reputation = Mathf.Clamp(Reputation, 0, 100);
    }

}
