using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth;

    
    public int maxStamina = 100;
    public int currentStamina;
    public int staminaOverTime;


    public ChangeSliderValue changeHealthValue;
    public ChangeSliderValue changeStaminaValue;

    public GameObject healthBar;
    public GameObject staminaBar;

    void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;

        changeHealthValue = healthBar.GetComponent<ChangeSliderValue>();
        changeStaminaValue = staminaBar.GetComponent<ChangeSliderValue>();

        changeHealthValue.SetMaxValue(maxHealth);
        changeStaminaValue.SetMaxValue(maxStamina);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            addHealth(20);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            DamageStamina(20);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            addStamina(20);
        }

    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        changeHealthValue.SetHealth(currentHealth);
    }

    void DamageStamina(int damage)
    {
        currentStamina -= damage;

        changeStaminaValue.SetStamina(currentStamina);
    }

    void addHealth(int heal)
    {
        currentHealth += heal;

        changeHealthValue.SetHealth(currentHealth);
    }

    void addStamina(int heal)
    {
        currentStamina += heal;

        changeStaminaValue.SetStamina(currentStamina);
    }
}
