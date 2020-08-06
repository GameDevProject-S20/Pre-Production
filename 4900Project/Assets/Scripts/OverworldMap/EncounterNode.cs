using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterNode
{
    static Texture2D probMap;
    static bool loaded = false;

    string DefaultProbability;
    public string p { get; set; }
    Dictionary<string, float> probabilities  = new Dictionary<string, float>(){
        {"Safe", 0.0f},
        {"Low", 0.20f},
        {"Medium", 0.4f},
        {"High", 0.7f},
        {"Very High", 0.9f}
    };

    int growthTime = 2;    

    public void Init()
    {
        if (!loaded)
        {
            probMap = Resources.Load<Texture2D>("Sprites/Map/eventProbabilityTexture");
            loaded = true;
        }
    }


    public bool RollEncounter(){
        bool triggered = (Random.value < probabilities[p]);
        if (triggered) {
            p = "Safe";
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
            p = "Medium";

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
