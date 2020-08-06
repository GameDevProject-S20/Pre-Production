using SIEvents;
using System;
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

    /// <summary>
    /// Travels. Passes through an action to be called when travel is ready.
    /// This allows it to delay until an encounter completes, if the player runs out of gas.
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="onTravelReady"></param>
    public static void Travel(MapNode destination, Action onTravelReady){
        int cost = GetFuelCost(destination);
        int currentFuel = DataTracker.Current.Player.Inventory.Contains("Fuel");

        // If the player has enough fuel to travel: Go ahead & travel
        if (currentFuel >= cost) { 
            DataTracker.Current.Player.Inventory.RemoveItem("Fuel", cost);
            DataTracker.Current.dayCount += dayRate;
            onTravelReady();
        }
        else
        {
            // Otherwise, we need to run a LowFuel encounter
            DataTracker.Current.EncounterManager.RunRandomEncounter("LowFuel");

            // Delay the progression of travel until they complete the encounter
            EventManager.Instance.OnDialogueEnd.AddListener(() =>
            {
                DataTracker.Current.dayCount += dayRate;
                onTravelReady();
            });
        }
    }

}
