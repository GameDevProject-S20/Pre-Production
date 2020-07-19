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
        DateTime gameTime = new DateTime(2094, currTime.Month, currTime.Day + DataTracker.Current.dayCount);
        //string year = LeadingZero(gameTime.Year);
        //string month = LeadingZero(gameTime.Month);
        //string day = LeadingZero(gameTime.Day);

        clockText.text = "Date: " + gameTime.ToShortDateString() +
            "    Days Passed: " + DataTracker.Current.dayCount.ToString();
    }

        string LeadingZero(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }
}