using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTravel : MonoBehaviour
{
    static int baseFuelRate = 5;

    static Dictionary<float, float> weightThresholds = new Dictionary<float, float>(){
        {0.0f, 0.6f},
        {0.1f, 0.8f},
        {0.3f, 1.0f},
        {0.6f, 1.2f},
        {0.95f, 1.4f},
        {1.01f, 2.0f},
        {1.25f, 2.6f},
        {1.45f, 3.0f}
    };

    public static int timeRate { get; set; } = 1;


    public static int GetFuelCost(MapNode destination){
        float fill = DataTracker.Current.Player.Inventory.GetWeightRatio();
        float weightMod = 1.0f;
        foreach (var t in weightThresholds)
        {
            if (t.Key >= fill){
                break;
            }
            else{
                weightMod = t.Value;
            }
        }
        return Mathf.RoundToInt(baseFuelRate * weightMod);
    }

    public static bool Travel(MapNode destination){
        int cost = GetFuelCost(destination);
        if (DataTracker.Current.Player.Inventory.Contains("Fuel") >= cost) {
            DataTracker.Current.Player.Inventory.RemoveItem("Fuel", cost);
            return true;
        }
        return false;
    }

}
