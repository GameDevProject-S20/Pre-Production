using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Town class 
public class Town
{
    public int id { get; }
    public string name { get; set; }
    public string leader { get; set; }
    public List<int> shops;
    public int reputation { get; set; }

    public Town(int a, string b, string c)
    {
        id = a;
        name = b;
        leader = c;
        shops = new List<int>();
        reputation = 50;
    }

    public void addShop(int i)
    {
        shops.Add(i);
    }
    public void removeShop(int i)
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

    public void alterRep(int i)
    {
        reputation += i;
        reputation = Mathf.Clamp(reputation, 0, 100);
    }

}
