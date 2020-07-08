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
            if (instance == null) instance = new TownManager();
            return instance;
        }
    }

    private Dictionary<int, Town> towns = new Dictionary<int, Town>();

    private TownManager()
    {
        
    }

    public void LoadData()
    {
        // Load in town data from CSV
        GameData.LoadCsv<Town>(FileConstants.Files.Town, out IEnumerable<Town> result);
        var resultString = new System.Text.StringBuilder();
        resultString.AppendLine("Loading in from file:");
        foreach (Town data in result)
        {
            towns.Add(data.Id, data);
            resultString.AppendLine("\tCreated town #" + data.Id + ": " + data.Name);
        }
        UnityEngine.Debug.Log(resultString);


        towns[0].AddShop(0);
        towns[0].AddShop(1);
        towns[1].AddShop(2);
        towns[1].AddShop(3);
        towns[2].AddShop(4);
        towns[2].AddShop(5);
        towns[3].AddShop(6);
        towns[3].AddShop(7);
        towns[4].AddShop(8);
        towns[4].AddShop(9);
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

    //! Test Function
    //! Add shops to towns


}
