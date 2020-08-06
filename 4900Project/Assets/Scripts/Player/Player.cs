using System;
using System.Collections;
using System.Collections.Generic;
using SIEvents;

public class Player
{
    private static Player instance;

    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Player();
            }
            return instance;
        }
    }

    public Inventory Inventory = new Inventory();

    public int Health {get; private set;} = 100;
    public int HealthCap {get; private set;} = 100;

    private Player() { }

    public void AddHealth(int h)
    {
        if (h == 0) return;
        Health += h;
        Health = UnityEngine.Mathf.Clamp(Health, 0, HealthCap);
        EventManager.Instance.OnHealthChange.Invoke(h, Health, HealthCap, "");
    }

    public void ModifyCap(int mod)
    {
        if (mod == 0) return;
        HealthCap += mod;
        HealthCap = UnityEngine.Mathf.Max(HealthCap, 0);
        EventManager.Instance.OnHealthChange.Invoke(mod, Health, HealthCap, "max");
    }
}
