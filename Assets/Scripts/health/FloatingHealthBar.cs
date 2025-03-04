using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    public Slider slider;
    public Text healthText;
    // Update is called once per frame
    void Awake()
    {
        slider = GetComponentInChildren<Slider>();
        healthText = GetComponentInChildren<Text>();
    }

    public void DoHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
        healthText.text = currentValue.ToString() + "/" + maxValue.ToString();
    }
}
