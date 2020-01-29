using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCooldown : MonoBehaviour
{
    public List<Item> item;

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (item[0].currentCooldown >= item[0].cooldown)
            {
                item[0].currentCooldown = 0;
            }
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (item[1].currentCooldown >= item[1].cooldown)
            {
                item[1].currentCooldown = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (item[2].currentCooldown >= item[2].cooldown)
            {
                item[2].currentCooldown = 0;
            }
        }
    }

    void Update()
    {
        foreach(Item i in item)
        {
            if (i.currentCooldown < i.cooldown)
            {
                i.currentCooldown += Time.deltaTime;
                i.itemIcon.fillAmount = i.currentCooldown / i.cooldown;
                
            }
        }
    }

}

[System.Serializable]
public class Item
{
    public float cooldown;
    public Image itemIcon;
    [HideInInspector]
    public float currentCooldown;
}