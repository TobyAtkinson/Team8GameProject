using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSliderValue : MonoBehaviour
{

    public Slider healthBar;
    public Slider staminaBar;

    public void SetMaxValue(float value)
    { 
        healthBar.maxValue = value;
        staminaBar.maxValue = value;
    }
    
    public void SetHealth(float value)
    {
        healthBar.value = value;
    }

    public void SetStamina(float value)
    {
        staminaBar.value = value;
    }
}
