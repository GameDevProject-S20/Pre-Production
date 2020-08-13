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
    private Dictionary<string, Resident> Residents = new Dictionary<string, Resident>();
    private TownManager()
    {
        
    }

    public void LoadData()
    {

        CreateTownTags();
        CreateResidents();

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

            // Note: Smithsville's marketplace is only available after completing the Generator quest.
            if (town.Id != 0)
            {
                town.InitializeShop();
            }
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
        tag.Name = "Farming";
        tag.Colour = "#078d39";
        tag.Specialization = ItemTag.Food;
        tag.SpecializationDesc = "food";
        tag.Summary = "- Produces food\n- Values Materials, Parts, Machinery";

        tag.shopSellModifiers.Add(ItemTag.Food, 0.6f);
        tag.shopSellModifiers.Add(ItemTag.Building_Materials, 1.25f);
        tag.shopSellModifiers.Add(ItemTag.Tools_And_Parts, 1.25f);
        tag.shopSellModifiers.Add(ItemTag.Machinery, 1.5f);

        tag.playerSellModifiers.Add(ItemTag.Food, 0.6f);
        tag.playerSellModifiers.Add(ItemTag.Building_Materials, 1.25f);
        tag.playerSellModifiers.Add(ItemTag.Tools_And_Parts, 1.25f);
        tag.playerSellModifiers.Add(ItemTag.Machinery, 1.5f);

        Tags.Add(tag.Name, tag);
        

//=============================================================
        
        tag = new TownTag();
        tag.Name = "Small";
        tag.Colour = "#c2bdb2";
        tag.Summary = "- Specialized in one type of good\n- Doesn't value luxuries.";

        tag.playerSellModifiers.Add(ItemTag.Luxury, 0.5f);
    
        Tags.Add(tag.Name, tag);
        
//=============================================================
        tag = new TownTag();
        tag.Name = "Medium";
        tag.Colour = "#dbd7ce";
        tag.Summary = "- Sells general goods";

        Tags.Add(tag.Name, tag);
        
//=============================================================
        
        tag = new TownTag();
        tag.Name = "Large";
        tag.Colour = "#FFF5E0";
        tag.Summary = "- Sells a high variety of goods\n- Highly values food and luxuries";

        tag.shopSellModifiers.Add(ItemTag.Food, 1.3f);
        tag.shopSellModifiers.Add(ItemTag.Luxury, 1.3f);

        tag.playerSellModifiers.Add(ItemTag.Food, 1.3f);
        tag.playerSellModifiers.Add(ItemTag.Luxury, 1.3f);

        Tags.Add(tag.Name, tag);
//=============================================================
        
        tag = new TownTag();
        tag.Name = "Mining";
        tag.Colour = "#b57a74";
        tag.Specialization = ItemTag.Mineral;
        tag.SpecializationDesc = "minerals";
        tag.Summary = "- Produces ore\n- Values general goods & machinery";

        tag.shopSellModifiers.Add(ItemTag.General, 1.2f);
        tag.shopSellModifiers.Add(ItemTag.Machinery, 1.5f);

        tag.playerSellModifiers.Add(ItemTag.Mineral, 0.25f);
        tag.playerSellModifiers.Add(ItemTag.General, 1.2f);
        tag.playerSellModifiers.Add(ItemTag.Machinery, 1.5f);

        tag.AbundancyModifiers.Add(ItemTag.Mineral, 1.5f);

        Tags.Add(tag.Name, tag); 

//=============================================================
        
        tag = new TownTag();
        tag.Name = "Steel";
        tag.Colour = "#bfb1a3";
        tag.Specialization = ItemTag.Steel;
        tag.SpecializationDesc = "steel";
        tag.Summary = "- Smelts ore into steel";

        tag.playerSellModifiers.Add(ItemTag.Mineral, 3.0f);

        Tags.Add(tag.Name, tag);

//=============================================================
        
        tag = new TownTag();
        tag.Name = "Hospital";
        tag.Colour = "#d9304c";
        tag.Specialization = ItemTag.Medical;
        tag.SpecializationDesc = "medical goods";
        tag.Summary = "- Sells medicine\n- Values food & medical goods";

        tag.shopSellModifiers.Add(ItemTag.Medical, 1.2f);
        tag.shopSellModifiers.Add(ItemTag.Food, 1.2f);

        tag.playerSellModifiers.Add(ItemTag.Food, 1.2f);
        tag.playerSellModifiers.Add(ItemTag.Medical, 1.2f);

        Tags.Add(tag.Name, tag);  

//=============================================================
        
        tag = new TownTag();
        tag.Name = "Bandit";
        tag.Colour = "#c40202";
        tag.Specialization = ItemTag.Combat;
        tag.SpecializationDesc = "weaponry";
        tag.Summary = "- Sells weapons\n- Values basic goods";

        tag.shopSellModifiers.Add(ItemTag.Food, 1.2f);
        tag.shopSellModifiers.Add(ItemTag.Building_Materials, 1.5f);
        tag.shopSellModifiers.Add(ItemTag.Tools_And_Parts, 1.2f);
        tag.shopSellModifiers.Add(ItemTag.Bandit, 10.0f);

        tag.playerSellModifiers.Add(ItemTag.Food, 1.2f);
        tag.playerSellModifiers.Add(ItemTag.Building_Materials, 1.5f);
        tag.playerSellModifiers.Add(ItemTag.Tools_And_Parts, 1.2f);

        tag.RarityModifier = 1.5f;
        Tags.Add(tag.Name, tag);  

//=============================================================
        tag = new TownTag();
        tag.Name = "Factory";
        tag.Colour = "#e66747";
        tag.Specialization = ItemTag.Machinery;
        tag.SpecializationDesc = "machinery";
        tag.Summary = "- Sells machinery\n- Values materials";

        tag.shopSellModifiers.Add(ItemTag.Building_Materials, 1.2f);
        tag.shopSellModifiers.Add(ItemTag.Steel, 1.5f);

        tag.playerSellModifiers.Add(ItemTag.Building_Materials, 1.2f);
        tag.playerSellModifiers.Add(ItemTag.Steel, 1.5f);

        tag.RarityModifier = 1.5f;
        Tags.Add(tag.Name, tag); 

//=============================================================
        tag = new TownTag();
        tag.Name = "Historical";
        tag.Colour = "#d9a223";
        tag.Specialization = ItemTag.Antique;
        tag.SpecializationDesc = "artifacts";
        tag.Summary = "- Values artifacts";

        tag.shopSellModifiers.Add(ItemTag.Antique, 3f);
        tag.playerSellModifiers.Add(ItemTag.Antique, 3f);
        tag.AbundancyModifiers.Add(ItemTag.General,1.5f);
        Tags.Add(tag.Name, tag);  
    }

    public TownTag GetTag(string name){
        TownTag tag;
        if (Tags.TryGetValue(name, out tag)){
            return tag;
        }
        return null;
    }

    void CreateResidents(){
        Residents.Add("hospital", new Resident("hospital", "Hospital", "Replenish your health.", "Icons/ItemIcons/MedicineTier2",12));
        Residents.Add("steelQuest1", new Resident("steelQuest1","Steelmaker's Request", "A message on the job board catches your eye...", "Icons/CyberPunk Avatars/014",210));
        Residents.Add("steelQuest2", new Resident("steelQuest2","Steelmaker", "The Steelmaker awaits your delivery.", "Icons/CyberPunk Avatars/014",211));
        Residents.Add("scienceQuest1", new Resident("scienceQuest1","Researcher", "A researcher has set up a lab in town.", "Icons/CyberPunk Avatars/Scientist1",212));
        Residents.Add("scienceQuest2A", new Resident("scienceQuest2A","Penelope", "A researcher with the Laurentian Institute.", "Icons/CyberPunk Avatars/Scientist1",213));
        Residents.Add("scienceQuest2B", new Resident("scienceQuest2B","Researcher", "A researcher has set up a lab in town.", "Icons/CyberPunk Avatars/Scientist2",213));
        Residents.Add("scienceQuest2C", new Resident("scienceQuest2C","Researcher", "A researcher has set up a lab in town.", "Icons/CyberPunk Avatars/Scientist3",213));
        Residents.Add("scienceQuest3", new Resident("scienceQuest3","Take Soil Sample", "This spot is as good as any.", "Icons/ItemIcons/Shovel",200));

    }

    public Resident GetResident(string name){
        Resident r;
        if (Residents.TryGetValue(name, out r)){
            return r;
        }
        return null;
    }
}

public class TownTag
{
    public string Name;
    public string Colour;
    public string Summary;
    public ItemTag Specialization = ItemTag.None; // Specialized in these item type
    public string SpecializationDesc = "";
    public Dictionary<ItemTag, float> playerSellModifiers = new Dictionary<ItemTag, float>();
    public Dictionary<ItemTag, float> shopSellModifiers = new Dictionary<ItemTag, float>();
    public float RarityModifier = 1.0f;
    public Dictionary<ItemTag, float> AbundancyModifiers = new Dictionary<ItemTag, float>();

    public TownTag(){}
}



public class Resident
{
    public string ShortName;
    public string DisplayName;
    public string Description;
    public string Icon;
    public int EncounterId;

    public Resident(string ShortName, string DisplayName, string Description, string Icon, int EncounterId){
        this.ShortName = ShortName;
        this.DisplayName = DisplayName;
        this.Description = Description;
        this.Icon = Icon;
        this.EncounterId = EncounterId;
    }
}
