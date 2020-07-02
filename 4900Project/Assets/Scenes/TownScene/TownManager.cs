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
    public static TownManager Current { get { return _current; } }
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


    //! Test Function
    //! Add shops to towns


}
