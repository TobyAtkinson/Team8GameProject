using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiFader : MonoBehaviour
{

    public CanvasGroup hpBar;
    public CanvasGroup staminaBar;
    public CanvasGroup itemCooldown;

    bool hpFaded;
    bool staminaFaded;
    bool cooldownFaded;

    Player playerScript;

    ItemCooldown cooldownScript;



    void Start()
    {
        playerScript = GameObject.FindWithTag("Player").GetComponent<Player>();
        //cooldownScript = GetComponent<ItemCooldown>();



    }

    void Update()
    {
        if (playerScript.currentHealth >= playerScript.maxHealth && !hpFaded)
        {
            FadeHealthOut();
        }

        else if (playerScript.currentHealth < playerScript.maxHealth && hpFaded)
        {
            FadeHealthIn();
        }

        if (playerScript.currentStamina >= playerScript.maxStamina && !staminaFaded)
        {
            FadeStaminaOut();
        }

        else if (playerScript.currentStamina < playerScript.maxStamina && staminaFaded)
        {
            FadeStaminaIn();
        }

        if (cooldownScript.currentCooldown >= cooldownScript.cooldownDuration && !cooldownFaded)
        {
            FadeCooldownOut();
        }

        else if (cooldownScript.currentCooldown < cooldownScript.cooldownDuration && cooldownFaded)
        {
            FadeCooldownIn();
        }

    }

    public void FadeHealthIn()
    {
        hpFaded = false;
        StartCoroutine(FadeCanvasGroup(hpBar, hpBar.alpha, 1));
    }

    public void FadeHealthOut()
    {
        StartCoroutine(FadeCanvasGroup(hpBar, hpBar.alpha, 0));
        hpFaded = true;
    }

    public void FadeStaminaIn()
    {
        staminaFaded = false;
        StartCoroutine(FadeCanvasGroup(staminaBar, staminaBar.alpha, 1));
    }

    public void FadeStaminaOut()
    {
        StartCoroutine(FadeCanvasGroup(staminaBar, staminaBar.alpha, 0));
        staminaFaded = true;
    }
    public void FadeCooldownIn()
    {
        cooldownFaded = false;
        StartCoroutine(FadeCanvasGroup(itemCooldown, itemCooldown.alpha, 1));
    }

    public void FadeCooldownOut()
    {
        StartCoroutine(FadeCanvasGroup(itemCooldown, itemCooldown.alpha, 0));
        cooldownFaded = true;
    }

    public IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startValue, float endValue, float lerpTime = 0.5f)
    {
        float timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while (true)
        {
            timeSinceStarted = Time.time - timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(startValue, endValue, percentageComplete);

            canvasGroup.alpha = currentValue;

            if (percentageComplete >= 1) break;

            yield return new WaitForEndOfFrame();
        }
    }
}
