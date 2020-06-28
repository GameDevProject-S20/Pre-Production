using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop
{
    public enum ShopTypes {}

    int id;
    string name;
    string shortDescription;
    string description;



    int townID;

    public Dictionary<string, float> fromPlayerModifiers;
    public Dictionary<string, float> toPlayerModifiers;

    public float acceptedPriceDifference = 0.15f;

    public Inventory inventory;
}
