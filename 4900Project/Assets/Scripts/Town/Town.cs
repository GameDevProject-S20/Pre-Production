using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Town class 
public class Town
{
    public int Id { get; }
    public string Name { get; set; }
    public string Leader { get; set; }
    public string Colour {get; set;} //hex code
    public rarity tier;
    public List<typetag> tags;
    public Sprite Icon;
    public Sprite LeaderPortrait;
    public string Description = "No Description Set";
    public string LeaderBlurb = "No Blurb Set";
    public int leaderDialogueEncounterId = 11;

    public List<int> shops;
    public Dictionary<typetag, float> valueModifiers;

    public Town(int Id, string Name, string Leader, string Colour="#FFFF5E0")
    {
        this.Id = Id;
        this.Name = Name;
        this.Leader = Leader;
        this.Colour = Colour;
        shops = new List<int>();
        tags = new List<typetag>();
        valueModifiers = new Dictionary<typetag, float>();

        // Randomly Select an Icon
        // Do not select the ugly ones
        int iconId = -1;
        bool valid = false;
        while(!valid){
            iconId = Mathf.FloorToInt(Random.Range(0, 31));
            if (iconId != 10
            && iconId != 12
            && iconId != 15
            && iconId != 18
            && iconId != 20) 
            valid = true;
        }
        string path = "Icons/CyberPunk Avatars/" + iconId.ToString("D3");
        LeaderPortrait = Resources.Load<Sprite>(path);
        // select a random image for town
        // TODO: Replace with setting based on town size
        iconId = Mathf.FloorToInt(Random.Range(0, 4));
        string iconName;
        switch (iconId)
        {
            case 1:
                iconName = "Hut";
                break;
            case 2:
                iconName = "Town";
                break;
            case 3:
                iconName = "SmallCity";
                break;
            default:
                iconName = "LargeCity";
                break;
        }
        path = "Icons/Town/" + iconName;
        Icon = Resources.Load<Sprite>(path);
        
    }

    public void AddShop(int i)
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
    }
}
