using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used for reading town data from a CSV
/// </summary>
public class TownData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Leader { get; set; }
    public string Colour { get; set; } //hex code
    public string Size { get; set; } //hex code
}



//Town class 
public class Town
{
    public enum Sizes {Small, Medium, Large}

    public int Id { get; set; }
    public string Name { get; set; }
    public string Leader { get; set; }
    public string Colour { get; set; }
    public Sizes Size { get; set; }
    public List<TownTag> Tags;
    public Sprite Icon;
    public Sprite LeaderPortrait;

    public string Description = "No Description Set";
    public string LeaderBlurb = "No Blurb Set";
    public int leaderDialogueEncounterId = 11;
    public List<int> shops;
        
    /// <summary>
    /// Constructor for loading in from a TownData class
    /// </summary>
    /// <param name="data"></param>
    public Town(TownData data) : this(data.Id, data.Name, data.Leader, data.Colour, data.Size)
    {

    }

    /// <summary>
    /// Main constructor. Populates from provided data values
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Name"></param>
    /// <param name="Leader"></param>
    /// <param name="Colour"></param>

    public Town(int Id, string Name, string Leader, string Colour="#FFFF5E0", string Size="Medium")
    {
        this.Id = Id;
        this.Name = Name;
        this.Leader = Leader;
        this.Colour = Colour;
        this.Size = (Sizes)System.Enum.Parse(typeof(Sizes), Size);

        shops = new List<int>();
        Tags = new List<TownTag>();

        SetDescription();
        SetLeaderBlurb();

        // Fetch town leader avatar
        {
            string path = $"Icons/CyberPunk Avatars/TownLeaders/{Leader}";
            LeaderPortrait = Resources.Load<Sprite>(path);
        }

        {
            string iconName;
            switch (this.Size)
            {
                case Sizes.Small:
                    iconName = "Town";
                    Tags.Add(TownManager.Instance.GetTag("Small"));
                    break;
                case Sizes.Medium:
                    iconName = "SmallCity";
                    Tags.Add(TownManager.Instance.GetTag("Medium"));
                    break;
                case Sizes.Large:
                    iconName = "LargeCity";
                    Tags.Add(TownManager.Instance.GetTag("Large"));
                    break;
                default:
                    iconName = "Town";
                    break;
            }
            string path = "Icons/Town/" + iconName;
            Icon = Resources.Load<Sprite>(path);
        }

    }

    public void InitializeShop(){
        // Create a store and populate it based on the town's tags
        Shop shop = new Shop(ShopManager.Instance.GetId(), "Marketplace", "Trade Goods", "", Shop.ShopTypes.None);
        shop.InitializeInventory(this);
        ShopManager.Instance.addShop(shop);
        shops.Add(shop.id);
    }

   /* public void AddShop(int i)
    {
        shops.Add(i);
    }

    public void RemoveShop(int i)
    {
        for(int j = 0; j < shops.Count; j++)
        {
            if(shops[j] == i)
            {
                shops.Remove(j);
                break;
            }
        }
    }*/

    public void AddTag(TownTag tag){
        Tags.Add(tag);
    }

    private void SetDescription()
    {
        this.Description = $@"{this.Name} is a {getWord("size")} situated in {getWord("region")} nearby a {getWord("adj")} {getWord("noun")}.

They are lead by {this.Leader} and known for having lots of {getWord("resource")}. They will pay handsomely for {getWord("resource")}.

The inhabitants are often found {getWord("verb")} and are {getWord("verb2")} when it comes to meeting new people.";  
    }

    private void SetLeaderBlurb()
    {
        this.LeaderBlurb = $@"A {getWord("adj")} individual who looks like they know a thing or two about {getWord("verb")}";
    }

    private string getWord(string type)
    {
        int randNum = Mathf.FloorToInt(Random.Range(0, 7));
        string[] region = new string[] { "Moorswood", "Gothic Gourge", "New Asia", "Tempest Region", "Maroon Territory", "Broken Vale", "Cinder Country" };
        string[] adj = new string[] { "bright", "dark", "gloomy", "desolate", "tough", "unusual", "hostile" };
        string[] verb = new string[] { "fighting", "vanishing", "crafting", "story telling", "cooking", "coding", "hunting"};
        string[] verb2 = new string[] { "wary", "welcoming", "aggresive", "curious", "stand offish", "dismissive", "enthusatic" };
        string[] noun = new string[] { "river", "forest", "mountain", "swamp", "cave", "ruin", "field"};
        string[] resource = new string[] { "wood", "food", "metal", "medicine", "weapons", "jewlery", "armour" };
        string[] size = new string[] { "small group of huts", "town", "small city", "large city", "base", "tent city", "empire" };
        switch (type) {
            case "adj":
                return adj[randNum];
            case "verb":
                return verb[randNum];
            case "verb2":
                return verb2[randNum];
            case "noun":
                return noun[randNum];
            case "resource":
                return resource[randNum];
            case "size":
                return size[randNum];
            case "region":
                return region[randNum];
            default:
                return "word";
        }
    }
}

