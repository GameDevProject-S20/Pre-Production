using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTracker : MonoBehaviour
{
    private static DataTracker _current;
    public static DataTracker Current {get {return _current;}}

    public PlayerData Player = new PlayerData();
    public OverworldMap.LocationGraph WorldMap = OverworldMapLoader.CreateTestMap();
    [SerializeField]
    public int currentShop = 0; // Needed if we want store to be their own scene. If we make the store window a prefab, we don't need this.
    public int currentNode = 0;



    private void Awake() {
        if (_current != null && _current != this)
        {
            Destroy(this.gameObject);
        } else {
            _current = this;
        }
    
        Player.Inventory.addItem("item1", 2);
        Player.Inventory.addItem("item2", 8);
        Player.Inventory.addItem("item4", 6);

        DontDestroyOnLoad(gameObject);
    }


    public OverworldMap.LocationNode GetCurrentNode(){
        OverworldMap.LocationNode node;
        if (WorldMap.GetNode(currentNode, out node)){
            return node;
        }
        return null;
    }
}

public class PlayerData {
    public Inventory Inventory = new Inventory();
}
