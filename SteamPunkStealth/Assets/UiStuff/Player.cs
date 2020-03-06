using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public float maxHealth = 100f;
    public float currentHealth;

    
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaOverTime = 5f;

    private float StaminaRegenTimer = 0.0f;

    private const float StaminaDecreasePerFrame = 10.0f;
    private const float StaminaIncreasePerFrame = 20.0f;
    private const float StaminaTimeToRegen = 3.0f;

    ChangeSliderValue changeHealthValue;
    ChangeSliderValue changeStaminaValue;

    public GameObject healthBar;
    public GameObject staminaBar;

    void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;

        currentHealth = Mathf.Clamp(currentHealth, 0f, 100f);
        currentStamina = Mathf.Clamp(currentStamina, 0f, 100f);

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

        /*if (Input.GetKeyDown(KeyCode.Z))
        {
            DamageStamina(20);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            addStamina(20);
        }*/

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            currentStamina = Mathf.Clamp(currentStamina - (StaminaDecreasePerFrame * Time.deltaTime), 0.0f, maxStamina);
            StaminaRegenTimer = 0.0f;
        }

       /* else if (currentStamina < maxStamina)
        {
            if (StaminaRegenTimer >= StaminaTimeToRegen)
                currentStamina = Mathf.Clamp(currentStamina + (StaminaIncreasePerFrame * Time.deltaTime), 0.0f, maxStamina);
            else
                StaminaRegenTimer += Time.deltaTime;
        }*/

    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        changeHealthValue.SetHealth(currentHealth);
    }

    void addHealth(int heal)
    {
        currentHealth += heal;

        changeHealthValue.SetHealth(currentHealth);
    }
}
