using ICSharpCode.NRefactory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTracker : MonoBehaviour
{
    private static DataTracker _current;
    public static DataTracker Current {get {return _current;}}

    private IDataLoader dataLoader;
    public PlayerData Player;
    public OverworldMap.LocationGraph WorldMap;

    private void Awake() {
        if (_current != null && _current != this)
        {
            Destroy(this.gameObject);
        } else {
            _current = this;
        }
    
        DontDestroyOnLoad(gameObject);

        dataLoader = new ExternalDataLoader();
        Player = new PlayerData();
        WorldMap = dataLoader.LoadMap();
        Debug.Log(WorldMap.NodeCount());
    }
}

public class PlayerData {
    Inventory Inventory = new Inventory();
}
