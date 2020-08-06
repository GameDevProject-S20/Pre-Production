using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SIEvents;
public class EncounterNode
{

    int id;
    static Texture2D probMap;
    static bool loaded = false;

    string DefaultProbability;
    public string p { get; set; }
    static Dictionary<string, float> probabilities  = new Dictionary<string, float>(){
        {"Safe", 0.0f},
        {"Low", 0.20f},
        {"Moderate", 0.4f},
        {"High", 0.7f},
        {"Very High", 0.9f}
    };

    static Dictionary<string, int> growthMod  = new Dictionary<string, int>(){
        {"Safe", 1}, // 12
        {"Low", 2}, // 24
        {"Moderate", 3}, // 36
        {"High", 4}, // 48
        {"Very High", 5} // 64
    };

    int growthTime = 0;
    int growthTimeMax = 12;

    public void Init(int id)
    {
        this.id = id;
        if (!loaded)
        {
            probMap = Resources.Load<Texture2D>("Sprites/Map/eventProbabilityTexture");
            loaded = true;
        }
        EventManager.Instance.OnTimeAdvance.AddListener(TimePass);
    }

    void TimePass(int i){
        if (p == DefaultProbability) return;
        growthTime += i * growthMod[DefaultProbability];
        if (growthTime >= growthTimeMax) {
            if (p == "Safe") {
                p = "Low";
            }
            else if (p == "Low") {
                p = "Moderate";
            }
            else if (p == "Moderate") {
                p = "High";
            }
            else if (p == "High") {
                p = "Very High";
            }
            growthTime = growthTime % growthTimeMax;
            EventManager.Instance.OnProbabilityChange.Invoke(id);
        }
    }


    public bool RollEncounter(){
        bool triggered = (Random.value < probabilities[p]);
        if (triggered) {
            p = "Safe";
            EventManager.Instance.OnProbabilityChange.Invoke(id);
        }
        return triggered;
    }

    public void SampleTexture(float x, float y)
    {
        int xMod = Mathf.RoundToInt(Mathf.Lerp(0, 800, (x + 1) / 2.0f));
        int yMod = Mathf.RoundToInt(Mathf.Lerp(0, 600, (y + 1) / 2.0f));
        float val = probMap.GetPixel(xMod, yMod).grayscale;
        if (val == 0f)
        {
            p = "Safe";
        }
        else if (val < 0.25f)
        {
            p = "Low";

        }
        else if (val < 0.50f)
        {
            p = "Moderate";

        }
        else if (val < 0.75f)
        {
            p = "High";

        }
        else
        {
            p = "Very High";
        }
        DefaultProbability = p;

    }

}
