using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TradingTest : MonoBehaviour
{
    public static Player player;
    public Trading tradingInstance;


    private void Start() {
        player = new Player();
        player.inventory.weightLimit = 10000;
        Shop aShop = new Shop(1, "Test Store", "", "");

        player.inventory.addItem("item1", 4);
        player.inventory.addItem("item4", 3);
        player.inventory.addItem("item5", 1);
        player.inventory.addItem("item7", 6);
        aShop.inventory.addItem("item2", 1);
        aShop.inventory.addItem("item4", 4);
        aShop.inventory.addItem("item6", 3);
        aShop.inventory.addItem("item8", 2);

        tradingInstance.init(aShop);

    }

}

public class Player {
    public Inventory inventory = new Inventory();
}


