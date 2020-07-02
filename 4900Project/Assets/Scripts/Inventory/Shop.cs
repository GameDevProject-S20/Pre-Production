using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop
{
    public enum ShopTypes {}

    int id;
    public string name;
    string shortDescription;
    string description;

    int townID;

    public Dictionary<string, float> fromPlayerModifiers = new Dictionary<string, float>();
    public Dictionary<string, float> toPlayerModifiers = new Dictionary<string, float>();
    public float acceptedPriceDifference = 0.15f;

    public Inventory inventory = new Inventory();

    public Shop(int id_, string name_, string shortDescription_, string description_){
        id = id_;
        name = name_;
        shortDescription = shortDescription_;
        description = description_;
        inventory.weightLimit = 10000;
    }

}
