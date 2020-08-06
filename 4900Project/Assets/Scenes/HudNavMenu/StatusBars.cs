using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using SIEvents;

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
    private Text HealthWarning;
    [SerializeField]
    private Text FuelWarning;

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

    void UpdateAmounts(int i=0)
    {
        healthBarText.text = DataTracker.Current.Player.Health.ToString("0");
        healthBar.fillAmount = DataTracker.Current.Player.Health / (float)DataTracker.Current.Player.HealthCap;


    }

    void WarningCheck()
    {
        int count = DataTracker.Current.Player.Inventory.Contains("Fuel");
        if(count < 18)
        {
            fuelBar.enabled = !fuelBar.enabled;
            FuelWarning.enabled = true;
        }
        else
        {
            fuelBar.enabled = true;
            FuelWarning.enabled = false;
        }
        count = DataTracker.Current.Player.Health;
        if (count < 30)
        {
            healthBar.enabled = !healthBar.enabled;
            HealthWarning.enabled = true;
        }
        else
        {
            healthBar.enabled = true;
            HealthWarning.enabled = false;
        }
    }
}
