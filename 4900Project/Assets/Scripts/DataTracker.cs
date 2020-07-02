using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTracker : MonoBehaviour
{
    private static DataTracker _current;
    public static DataTracker Current {get {return _current;}}

    private void Awake() {
        if (_current != null && _current != this)
        {
            Destroy(this.gameObject);
        } else {
            _current = this;
        }
    
        DontDestroyOnLoad(gameObject);
    }

    public PlayerData player = new PlayerData();


}

public class PlayerData {
    Inventory inventory = new Inventory();
}
