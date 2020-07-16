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

    public int health {get; set;} = 100 ;
    public int healthCap {get; set;} = 100;

    private Player() { }

    public void addHealth(int h)
    {
        health += h;
        if (health > healthCap)
        {
            health = healthCap;
        }
        EventManager.Instance.OnHealthChange.Invoke(health);
    }

}
