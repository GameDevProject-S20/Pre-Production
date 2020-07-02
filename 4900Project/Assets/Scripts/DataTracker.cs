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



    private void Awake() {
        if (_current != null && _current != this)
        {
            Destroy(this.gameObject);
        } else {
            _current = this;
        }
    
        Player.Inventory.addItem("item1", 4);
        Player.Inventory.addItem("item4", 3);
        Player.Inventory.addItem("item5", 1);
        Player.Inventory.addItem("item7", 6);

        DontDestroyOnLoad(gameObject);
    }

   
}

public class PlayerData {
    public Inventory Inventory = new Inventory();
}
