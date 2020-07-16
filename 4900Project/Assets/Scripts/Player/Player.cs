using System.Collections;
using System.Collections.Generic;

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

    int health = 100;
    int healthCap = 100;

    private Player() { }

    public void addHealth(int h)
    {
        health += h;
        if (health > healthCap)
        {
            health = healthCap;
        }
    }

}
