using FileConstants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownManager : MonoBehaviour
{
    //THINGS TO ADD
    //-set up to pull from csv
    //-set up to interact with data tracker

    //ensure only one copy active
    private static TownManager _current;
    public static TownManager current { get { return _current; } }
    Dictionary<int, Town> towns = new Dictionary<int, Town>();

    private void Awake()
    {
        if (_current != null && _current != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _current = this;
        }
        int ids = 0;
        // Loading in town data from CSV
        GameData.LoadCsv<TownTemp>(Files.Town, out IEnumerable<TownTemp> result);
        Town dummy;
        foreach (TownTemp data in result)
        {
            dummy = new Town(ids, data.Name, data.Leader);
            towns.Add(ids, dummy);
            ids++;
        }
    }


    // Get the town at the current node using the Data Tracker
   /* public Town GetCurrentTownData()
    {
        return GetTownById(DataTracker.instance.GetCurrentNode().IdOfLandmarkAtNode);
    }*/

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
}

public class TownTemp
{
    public int Id { get; }
    public string Name { get; set; }
    public string Leader { get; set; }
    public string Colour { get; set; }
}
