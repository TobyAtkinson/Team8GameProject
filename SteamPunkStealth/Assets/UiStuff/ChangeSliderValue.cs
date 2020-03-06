using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSliderValue : MonoBehaviour
{

    public Slider healthBar;
    public Slider staminaBar;

    public void SetMaxValue(int value)
    { 
        healthBar.maxValue = value;
        healthBar.value = value;
        staminaBar.maxValue = value;
        staminaBar.value = value;
    }
    
    public void SetHealth(int value)
    {
        healthBar.value = value;
    }

    public void SetStamina(int value)
    {
        staminaBar.value = value;
    }
}
