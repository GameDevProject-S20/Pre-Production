using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop
{
    public enum ShopTypes {GeneralStore, Mechanic, Pharmacy, None}

    int id;
    public string name;
    public string shortDescription;
    string description;
    ShopTypes type;

    int townID;

    public Dictionary<string, float> fromPlayerModifiers = new Dictionary<string, float>();
    public Dictionary<string, float> toPlayerModifiers = new Dictionary<string, float>();
    public float acceptedPriceDifference = 0.2f;

    public Inventory inventory = new Inventory();

    public Shop(int id_, string name_, string shortDescription_, string description_, ShopTypes type_)
    {
        id = id_;
        name = name_;
        shortDescription = shortDescription_;
        description = description_;
        type = type_;
        inventory.weightLimit = 10000;
        InitializeInventory();
    }

    void InitializeInventory(){
        if(type == ShopTypes.GeneralStore)
        {
            inventory.addItem("Rations", 7);
            inventory.addItem("Concrete", 3);
            inventory.addItem("Scrap Metal", 4);
            inventory.addItem("Wrench", 1);
        }
        else if(type == ShopTypes.Pharmacy)
        {
            inventory.addItem("Medical Kit", 1);
            inventory.addItem("Medicine", 1);
        }
    }

}
