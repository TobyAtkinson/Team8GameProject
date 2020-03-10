using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCooldown : MonoBehaviour
{
    public float cooldownDuration;
    public Image itemIcon;
    //[HideInInspector]
    public float currentCooldown;


    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentCooldown >= cooldownDuration)
            {
                currentCooldown = 0;
            }
        }
    }

    void Update()
    {

        if (currentCooldown < cooldownDuration)
        {
            currentCooldown += Time.deltaTime;
            itemIcon.fillAmount = 1 - currentCooldown / cooldownDuration;              
        }
    }

}
