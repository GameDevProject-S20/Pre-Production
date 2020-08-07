using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using SIEvents;
using TMPro;

public class StatusBars : MonoBehaviour
{
    [SerializeField]
    private Text healthBarText;
    [SerializeField]
    private Image healthBar;

    [SerializeField]
    private Text fuelBarText;
    [SerializeField]
    private Image fuelBar;

    [SerializeField]
    private Text weightBarText;
    [SerializeField]
    private Image weightBar;

    [SerializeField]
    private TextMeshProUGUI HealthWarning;
    [SerializeField]
    private TextMeshProUGUI FuelWarning;

    Color32 fuel2 = new Color32(14, 27, 221, 255);
    Color32 fuel1 = new Color32(28, 33, 106, 255);

    Color32 health1 = new Color32(106, 28, 32, 255);
    Color32 health2 = new Color32(217, 40, 49, 255);

    void Awake()
    {
        UpdateAmounts();
        EventManager.Instance.OnHealthChange.AddListener(UpdateAmounts);
        InvokeRepeating("WarningCheck", 0.75f, 0.75f);
        HealthWarning.enabled = false;
        FuelWarning.enabled = false;
    }

    private void Update() {
        int count = DataTracker.Current.Player.Inventory.Contains("Fuel");
        fuelBarText.text = count.ToString("0");
        fuelBar.fillAmount = count / 20.0f;
        float temp = DataTracker.Current.Player.Inventory.GetWeightRatio();
        weightBarText.text = ((int)(temp*100)).ToString() + "% Capacity";
        weightBar.fillAmount = temp;
    }

    void UpdateAmounts(int i=0, int j=0, int w=0, string s="")
    {
        healthBarText.text = DataTracker.Current.Player.Health.ToString("0");
        healthBar.fillAmount = DataTracker.Current.Player.Health / (float)DataTracker.Current.Player.HealthCap;


    }

    void WarningCheck()
    {
        int count = DataTracker.Current.Player.Inventory.Contains("Fuel");
        if(count < 24)
        {

            if(fuelBar.color == fuel1)
            {
                fuelBar.color = fuel2;
            }
            else
            {
                fuelBar.color = fuel1;
            }
            FuelWarning.enabled = true;
        }
        else
        {
            fuelBar.color = fuel1;
            FuelWarning.enabled = false;
        }
        count = DataTracker.Current.Player.Health;
        if (count < 30)
        {
            if (healthBar.color == health1)
            {
                healthBar.color = health2;
            }
            else
            {
                healthBar.color = health1;
            }
            HealthWarning.enabled = true;
        }
        else
        {
            healthBar.color = health1;
            HealthWarning.enabled = false;
        }
    }
}
