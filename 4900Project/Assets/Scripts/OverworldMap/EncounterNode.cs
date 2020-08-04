using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterNode
{
    static Texture2D probMap;
    static bool loaded = false;
    public float probability;
    public string probabilityCategory;

    public void Init()
    {
        if (!loaded)
        {
            probMap = Resources.Load<Texture2D>("Sprites/Map/eventProbabilityTexture");
            loaded = true;
        }
    }

    public void SampleTexture(float x, float y)
    {
        int xMod = Mathf.RoundToInt(Mathf.Lerp(0, 800, (x + 1) / 2.0f));
        int yMod = Mathf.RoundToInt(Mathf.Lerp(0, 600, (y + 1) / 2.0f));
        probability = probMap.GetPixel(xMod, yMod).grayscale;
        if (probability == 0f)
        {
            probabilityCategory = "Safe";
        }
        else if (probability < 0.25f)
        {
            probabilityCategory = "Low";

        }
        else if (probability < 0.50f)
        {
            probabilityCategory = "Medium";

        }
        else if (probability < 0.75f)
        {
            probabilityCategory = "High";

        }
        else
        {
            probabilityCategory = "Very High";
        }

    }

}
