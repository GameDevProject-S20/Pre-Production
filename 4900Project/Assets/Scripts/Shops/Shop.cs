using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityUtility;

/// <summary>
/// Houses the CSV Data for the shop.
/// </summary>
public struct ShopData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public string ShopType { get; set; }
    public string Owner { get; set; }
}

public class Shop
{
    public enum ShopTypes {GeneralStore, Mechanic, Pharmacy, None}

    int id;
    public string name;
    string owner;
    public string shortDescription;
    string description;
    public ShopTypes type;
    public Sprite Portrait;

    int townID;

    public Dictionary<typetag, float> fromPlayerModifiers = new Dictionary<typetag, float>();
    public Dictionary<typetag, float> toPlayerModifiers = new Dictionary<typetag, float>();
    public float acceptedPriceDifference = 0.2f;

    public Inventory inventory = new Inventory();
    
    /// <summary>
    /// Constructs a Shop given the CSV data. Used for dynamically fetching from our data files.
    /// </summary>
    /// <param name="data"></param>
    public Shop(ShopData data) : this(data.Id, data.Name, data.Owner, data.ShortDescription, data.Description, UnityHelperMethods.ParseEnum<ShopTypes>(data.ShopType))
    {
    }

    /// <summary>
    /// Constructs a shop given the values for each field. Can be used for hardcoding.
    /// </summary>
    /// <param name="id_"></param>
    /// <param name="name_"></param>
    /// <param name="owner_"></param>
    /// <param name="shortDescription_"></param>
    /// <param name="description_"></param>
    /// <param name="type_"></param>
    public Shop(int id_, string name_, string owner_, string shortDescription_, string description_, ShopTypes type_)
    {
        id = id_;
        name = name_;
        owner = owner_;
        shortDescription = shortDescription_;
        description = description_;
        type = type_;
        inventory.weightLimit = 10000;
        InitializeInventory();

        Portrait = Resources.Load<Sprite>($"Icons/CyberPunk Avatars/ShopOwners/{owner_}");
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
