using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using SIEvents;
public class Clock : MonoBehaviour
{
    private Text clockText;
    private DateTime currTime = DateTime.Now;
    private DateTime gameTime;

    void Awake()
    {
        clockText = GetComponent <Text>();
        gameTime = new DateTime(2094, currTime.Month, currTime.Day, 0, 0, 0);
        gameTime = gameTime.AddDays(DataTracker.Current.dayCount);
        gameTime = gameTime.AddHours(DataTracker.Current.hourCount);

        EventManager.Instance.OnTimeAdvance.AddListener((int i) =>
        {
            gameTime = gameTime.AddHours(i);
            UpdateText();
        });

        UpdateText();
    }

    void UpdateText() {
        clockText.text = "Date: " + gameTime.ToString("yyyy-mm-dd H:mm");
        //"    Days Passed: " + DataTracker.Current.dayCount.ToString();
    }
}