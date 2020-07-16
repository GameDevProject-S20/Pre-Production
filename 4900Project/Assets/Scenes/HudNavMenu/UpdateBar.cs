using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UpdateBar : MonoBehaviour
{
    
    private Text barText;
    private Image bar;

    void Awake()
    {
        barText = GetComponentInChildren<Text>();
        bar = GetComponent<Image>();
 
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: When we have player health and fuel in the DataTracker
        // we can update the fill and the text with those numbers
        barText.text = (bar.fillAmount * 100).ToString("0");
    }
}
