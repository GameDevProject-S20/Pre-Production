using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TownManager
{
    //THINGS TO ADD
    //-set up to pull from csv
    //-set up to interact with data tracker

    private static TownManager instance;

    public static TownManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new TownManager();
            }
            return instance;
        }
    }

    private Dictionary<int, Town> towns = new Dictionary<int, Town>();
    private Dictionary<string, TownTag> Tags = new Dictionary<string, TownTag>();

    private TownManager()
    {
        
    }

    public void LoadData()
    {

        CreateTownTags();

        // Load in town data from CSV
        GameData.LoadCsv<TownData>(FileConstants.Files.Town, out IEnumerable<TownData> result);
        var resultString = new System.Text.StringBuilder();
        resultString.AppendLine("Loading in from file:");
        foreach (TownData data in result)
        {
            var town = new Town(data);
            towns.Add(data.Id, town);
            Debug.Log("DATA:" + town.Description);
            resultString.AppendLine("\tCreated town #" + data.Id + ": " + data.Name);
        }
        UnityEngine.Debug.Log(resultString);

    
    }

    public IEnumerable<Town> GetTownEnumerable()
    {
        return towns.Values.AsEnumerable();
    }

    // Get the town at the current node using the Data Tracker
    public Town GetCurrentTownData()
    {
        return GetTownById(DataTracker.Current.GetCurrentNode().LocationId);
    }

    //town retrieval
    public Town GetTownById(int id)
    {
        Town town;
        if (towns.TryGetValue(id, out town))
        {
            return town;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Retrieve town by its name, if it exists
    /// </summary>
    /// <param name="name">Name of Town</param>
    /// <returns>The corresponding town or null if not found</returns>
    public Town GetTownByName(string name)
    {
        foreach (var town in towns.Values)
        {
            if (town.Name == name)
            {
                return town;
            }
        }
        return null;
    }


    void CreateTownTags(){
        
        TownTag tag = new TownTag();
        tag.Name = "Farm";
        tag.Colour = "#078d39";
        tag.Specialization = ItemTag.Food;

        tag.shopSellModifiers.Add(ItemTag.Food, 0.6f);
        tag.shopSellModifiers.Add(ItemTag.Advanced, 0.3f);
        tag.shopSellModifiers.Add(ItemTag.Fuel, 1.2f);
        tag.shopSellModifiers.Add(ItemTag.Building_Materials, 1.5f);
        tag.shopSellModifiers.Add(ItemTag.Tools_And_Parts, 1.5f);
    
        tag.playerSellModifiers.Add(ItemTag.Food, 0.4f);
        tag.playerSellModifiers.Add(ItemTag.Advanced, 0.3f);
        tag.playerSellModifiers.Add(ItemTag.Luxury, 0.25f);
        tag.playerSellModifiers.Add(ItemTag.Building_Materials, 1.5f);
        tag.playerSellModifiers.Add(ItemTag.Tools_And_Parts, 1.5f);
    
        tag.AbundancyModifiers.Add(ItemTag.Food, new List<float>(){1.5f, 1.2f, 1.1f, 1.0f, 1.0f});
        tag.RarityModifers.Add(ItemTag.Food, new List<float>(){1.0f, 1.5f, 1.5f, 2.0f, 3.0f});
        Tags.Add(tag.Name, tag);
        

//=============================================================
        
        tag = new TownTag();
        tag.Name = "Small";
        tag.Colour = "#918b7e";
   
        tag.playerSellModifiers.Add(ItemTag.Advanced, 0.3f);
        tag.playerSellModifiers.Add(ItemTag.Luxury, 0.25f);
    
        tag.BaseAbundancyModifier = new List<float>(){1.0f, 1.0f, 1.0f, 1.0f, 1.0f};
        tag.BaseRarityModifier = new List<float>(){1.0f, 1.2f, 1.2f, 2.0f, 3.0f};

        Tags.Add(tag.Name, tag);
        
//=============================================================
        tag = new TownTag();
        tag.Name = "Medium";
        tag.Colour = "#ccc4b3";
        
        tag.BaseAbundancyModifier = new List<float>(){0.7f, 0.8f, 1.0f, 0.8f, 1.0f};
        tag.BaseRarityModifier = new List<float>(){1.0f, 1.3f, 1.2f, 0.75f, 0.0f};

        Tags.Add(tag.Name, tag);
        
//=============================================================
        
        tag = new TownTag();
        tag.Name = "Large";
        tag.Colour = "#FFF5E0";
        tag.Specialization = ItemTag.Advanced;

        tag.shopSellModifiers.Add(ItemTag.Food, 1.5f);
        tag.shopSellModifiers.Add(ItemTag.Luxury, 1.3f);
        tag.shopSellModifiers.Add(ItemTag.Building_Materials, 1.2f);

        tag.playerSellModifiers.Add(ItemTag.Food, 1.5f);
        tag.playerSellModifiers.Add(ItemTag.Luxury, 1.3f);
        tag.playerSellModifiers.Add(ItemTag.Building_Materials, 1.2f);

        tag.BaseAbundancyModifier = new List<float>(){1.5f, 1.2f, 1.0f, 0.6f, 1.0f};
        tag.BaseRarityModifier = new List<float>(){1.0f, 1.3f, 1.0f, 0.5f, 0.5f};

        Tags.Add(tag.Name, tag);
//=============================================================
        
        tag = new TownTag();
        tag.Name = "Mining";
        tag.Colour = "#b57a74";
        tag.Specialization = ItemTag.Mineral;

        tag.shopSellModifiers.Add(ItemTag.Food, 1.5f);
        tag.shopSellModifiers.Add(ItemTag.Building_Materials, 1.2f);
        tag.shopSellModifiers.Add(ItemTag.Tools_And_Parts, 1.2f);

        tag.playerSellModifiers.Add(ItemTag.Mineral, 0.25f);
        tag.playerSellModifiers.Add(ItemTag.Food, 1.5f);
        tag.playerSellModifiers.Add(ItemTag.Luxury, 0.25f);
        tag.playerSellModifiers.Add(ItemTag.Building_Materials, 1.2f);
        tag.playerSellModifiers.Add(ItemTag.Tools_And_Parts, 1.2f);

        tag.AbundancyModifiers.Add(ItemTag.Mineral, new List<float>(){1.5f, 1.2f, 1.1f, 1.0f, 1.0f});
        tag.RarityModifers.Add(ItemTag.Mineral, new List<float>(){1.0f, 1.5f, 1.5f, 2.0f, 3.0f});

        Tags.Add(tag.Name, tag); 

//=============================================================
        
        tag = new TownTag();
        tag.Name = "Foundry";
        tag.Colour = "#bfb1a3";
        tag.Specialization = ItemTag.Building_Materials;

        tag.playerSellModifiers.Add(ItemTag.Mineral, 1.5f);

        Tags.Add(tag.Name, tag);

//=============================================================
        
        tag = new TownTag();
        tag.Name = "Hospital";
        tag.Colour = "#d9304c";
        tag.Specialization = ItemTag.Medical;

        tag.shopSellModifiers.Add(ItemTag.Food, 1.2f);
        tag.shopSellModifiers.Add(ItemTag.Advanced, 1.5f);

        tag.playerSellModifiers.Add(ItemTag.Advanced, 1.5f);
        tag.playerSellModifiers.Add(ItemTag.Food, 1.2f);
        tag.playerSellModifiers.Add(ItemTag.Luxury, 0.5f);
        tag.playerSellModifiers.Add(ItemTag.Building_Materials, 0.7f);

        tag.AbundancyModifiers.Add(ItemTag.Medical, new List<float>(){1.5f, 1.2f, 1.1f, 1.0f, 1.0f});
        tag.RarityModifers.Add(ItemTag.Medical, new List<float>(){1.0f, 1.5f, 2.0f, 4.0f, 5.0f});

        Tags.Add(tag.Name, tag);   
    }

    public TownTag GetTag(string name){
        TownTag tag;
        if (Tags.TryGetValue(name, out tag)){
            return tag;
        }
        return null;
    }

}

public class TownTag
{
    public string Name;
    public string Colour;
    public ItemTag Specialization = ItemTag.None; // Specialized in these item type
    public Dictionary<ItemTag, float> playerSellModifiers = new Dictionary<ItemTag, float>();
    public Dictionary<ItemTag, float> shopSellModifiers = new Dictionary<ItemTag, float>();
    public List<float> BaseAbundancyModifier = new List<float>(){1.0f, 1.0f, 1.0f, 1.0f, 1.0f};
    public List<float> BaseRarityModifier = new List<float>(){1.0f, 1.0f, 1.0f, 1.0f, 1.0f};
    public Dictionary<ItemTag, List<float>> AbundancyModifiers = new Dictionary<ItemTag, List<float>>();
    public Dictionary<ItemTag, List<float>> RarityModifers = new Dictionary<ItemTag, List<float>>();

    public TownTag(){}
}
