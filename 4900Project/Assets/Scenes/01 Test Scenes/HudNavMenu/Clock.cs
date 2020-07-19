using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Clock : MonoBehaviour
{
    private Text textClock;

    void Awake()
    {
        textClock = GetComponent <Text>();
    }

    void Update()
    {
        DateTime time = DateTime.Now;
        string year = LeadingZero(time.Year);
        string month = LeadingZero(time.Month);
        string day = LeadingZero(time.Day);
        string hour = LeadingZero(time.Hour);
        string minute = LeadingZero(time.Minute);
        string second = LeadingZero(time.Second);

        textClock.text = year + "-" + month + "-" + day + "T"
            + hour + ":" + minute + ":" + second;
    }

    string LeadingZero(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }
}
