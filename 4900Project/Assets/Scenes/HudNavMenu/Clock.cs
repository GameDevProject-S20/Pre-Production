using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Clock : MonoBehaviour
{
    private Text clockText;
    private DateTime currTime = DateTime.Now;
    private DateTime gameTime;

    void Awake()
    {
        clockText = GetComponent <Text>();
    }

    void Update()
    {
        DateTime gameTime = new DateTime(2094, currTime.Month, currTime.Day);
        gameTime = gameTime.AddDays(DataTracker.Current.dayCount);

        clockText.text = "Date: " + gameTime.ToShortDateString() +
            "    Days Passed: " + DataTracker.Current.dayCount.ToString();
    }
}