using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop
{
    public enum ShopTypes {GeneralStore, Mechanic, Pharmacy, None}

    int id;
    public string name;
    public string shortDescription;
    string description;
    public ShopTypes type;
    public Sprite Portrait;

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
        // Randomly Select an Icon
        // Do not select the ugly ones
        int iconId = -1;
        bool valid = false;
        while (!valid)
        {
            iconId = Mathf.FloorToInt(Random.Range(0, 31));
            if (iconId != 10
            && iconId != 12
            && iconId != 15
            && iconId != 18
            && iconId != 20)
                valid = true;
        }
        string path = "Icons/CyberPunk Avatars/" + iconId.ToString("D3");
        Portrait = Resources.Load<Sprite>(path);
    }

    void InitializeInventory(){
        if(type == ShopTypes.GeneralStore)
        {
            inventory.AddItem("Rations", 7);
            inventory.AddItem("Concrete", 3);
            inventory.AddItem("Scrap Metal", 4);
            inventory.AddItem("Wrench", 1);
        }
        else if(type == ShopTypes.Pharmacy)
        {
            inventory.AddItem("Medical Kit", 1);
            inventory.AddItem("Medicine", 1);
        }
    }

}
