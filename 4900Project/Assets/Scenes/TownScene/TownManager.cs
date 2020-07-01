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

        // Loading in town data from CSV will replace this
        Town dummy = new Town(0, "Smithsville", "Sarif");
        dummy.AlterRep(-10);
        dummy.AddShop(0);
        dummy.AddShop(1);
        towns.Add(dummy.Id, dummy);
        dummy = new Town(1, "Real Town", "Real Leader");
        dummy.AlterRep(10);
        dummy.AddShop(2);
        towns.Add(dummy.Id, dummy);
        dummy = new Town(2, "Test", "Tester");
        dummy.AddShop(3);
        towns.Add(dummy.Id, dummy);
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
